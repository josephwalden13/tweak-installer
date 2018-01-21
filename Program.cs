using jLib;
using SevenZipExtractor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinSCP;

namespace Unjailbreaker
{
    class Program
    {
        static void createDirIfDoesntExist(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }
        static void deleteIfExists(string path)
        {
            if (File.Exists(path)) File.Delete(path);
        }
        static void emptyDir(string path)
        {
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);
        }
        static void moveDirIfPresent(string source, string dest, string parent = null)
        {
            if (Directory.Exists(source))
            {
                if (parent != null)
                {
                    Directory.CreateDirectory(parent);
                }
                Directory.Move(source, dest);
            }
        }
        static void Main(string[] args)
        {
            bool install = false, uninstall = false, convert = false, manual = false;

            string[] data = File.ReadAllLines("settings"); //get ssh settings
            for (int i = 0; i != data.Length; i++)
            {
                data[i] = data[i].Split('#')[0];
            }
            string ip = data[0];
            string user = data[1];
            string password = data[2];

            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = ip,
                UserName = user,
                Password = password,
                GiveUpSecurityAndAcceptAnySshHostKey = true
            };
            Session session = new Session();
            session.Open(sessionOptions);

            if (session.FileExists("/usr/lib/SBInject"))
            {
                convert = true;
            }

            if (args.Contains("convert")) convert = true;
            if (args.Contains("uninstall")) uninstall = true;
            if (args.Contains("install")) install = true;

            if (manual)
            {
                //this is mostly pointless
                createDirIfDoesntExist("files");
                Console.WriteLine("Please extract the deb file's data.tar to 'files' and press any key to " + (convert ? "begin conversion and " : "") + (install ? "install" : uninstall ? "uninstall" : ""));
                Console.ReadLine();
            }
            else
            {
                emptyDir("files");
                emptyDir("temp");
                deleteIfExists("data.tar");
                string deb = "";
                foreach (string i in args)
                {
                    if (i.Contains(".deb"))
                    {
                        deb = i;
                    }
                }
                Console.WriteLine("Extracting Deb");
                using (ArchiveFile archiveFile = new ArchiveFile(deb))
                {
                    archiveFile.Extract("temp");
                }
                var p = Process.Start(@"7z.exe", "e temp\\data.tar." + (File.Exists("temp\\data.tar.lzma") ? "lzma" : "gz") + " -o.");
                p.WaitForExit();
                using (ArchiveFile archiveFile = new ArchiveFile("data.tar"))
                {
                    archiveFile.Extract("files");
                }
            }
            string name = "script";
            if (args.Length > 0) //contains options?
            {
                if (convert) //convert to electra format
                {
                    createDirIfDoesntExist("files\\bootstrap");
                    createDirIfDoesntExist("files\\bootstrap\\Library");
                    if (Directory.Exists("files\\Library\\MobileSubstrate\\"))
                    {
                        createDirIfDoesntExist("files\\usr\\lib\\SBInject");
                        foreach (string file in Directory.GetFiles("files\\Library\\MobileSubstrate\\DynamicLibraries\\"))
                        {
                            File.Move(file, "files\\usr\\lib\\SBInject\\" + new FileInfo(file).Name);
                        }
                        foreach (string file in Directory.GetDirectories("files\\Library\\MobileSubstrate\\DynamicLibraries\\"))
                        {
                            Directory.Move(file, "files\\usr\\lib\\SBInject\\" + new DirectoryInfo(file).Name);
                        }
                        Directory.Delete("files\\Library\\MobileSubstrate", true);
                    }
                    moveDirIfPresent("files\\Library\\Themes\\", "files\\System\\Library\\Themes\\", "files\\System\\Library\\");
                    moveDirIfPresent("files\\Library\\PreferenceBundles\\", "files\\bootstrap\\Library\\PreferenceBundles\\");
                    moveDirIfPresent("files\\Library\\PreferenceLoader\\", "files\\bootstrap\\Library\\PreferenceLoader\\");
                }
            }
            Crawler c = new Crawler("files", true); //gets all files in the tweak
            c.Remove("DS_STORE");
            string s = "";
            c.Files.ForEach(i =>
                       {
                           s += ("rm " + i.Replace("\\", "/").Replace(" ", "\\ ").Replace("(", "\\(").Replace(")", "\\)") + "\n").Replace("'", "\\'").Replace("@", "\\@"); //creates uninstall script for tweak (used if uninstall == true)
                       });
            File.WriteAllText("files\\" + name + ".sh", s); //add uninstall script to install folder
            if (args.Length > 0)
            {
                if (install)
                {
                    foreach (string dir in Directory.GetDirectories("files"))
                    {
                        session.PutFiles(dir, "/"); //put directories
                    }
                    foreach (string file in Directory.GetFiles("files"))
                    {
                        session.PutFiles(file, "/"); //put files
                    }
                    session.ExecuteCommand("killall -9 SpringBoard"); //respring
                }
                else if (uninstall)
                {
                    session.PutFiles("files\\script.sh", "/");
                    session.ExecuteCommand("sh /script.sh"); //if uninstall == true then run uninstall script
                    session.ExecuteCommand("killall -9 SpringBoard"); //respring
                }
            }
        }
    }
}
