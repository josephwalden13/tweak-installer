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
        static void Main(string[] args)
        {
            if (!File.Exists(@"C:\Program Files\7-Zip\7z.exe"))
            {
                Console.WriteLine("This program requires 7zip to be installed. Please install it to continue :(");
                Console.ReadLine();
                return;
            }
            bool install = false, uninstall = false, convert = false, manual = false /* for now you'll have to manually extract the files in 'files'. The program will still convert and install automatically though */;

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
                if (!Directory.Exists("files")) Directory.CreateDirectory("files");
                Console.WriteLine("Please extract the deb file's data.tar to 'files' and press any key to " + (convert ? "begin conversion and " : "") + (install ? "install" : uninstall ? "uninstall" : ""));
                Console.ReadLine();
            }
            else
            {
                if (Directory.Exists("files")) Directory.Delete("files", true); Directory.CreateDirectory("files");
                if (Directory.Exists("temp")) Directory.Delete("temp", true); Directory.CreateDirectory("temp");
                if (File.Exists("data.tar")) File.Delete("data.tar");
                string deb = "";
                foreach (string i in args)
                {
                    if (i.Contains(".deb"))
                    {
                        deb = i;
                    }
                }
                using (ArchiveFile archiveFile = new ArchiveFile(deb))
                {
                    archiveFile.Extract("temp");
                }
                var p = Process.Start(@"C:\Program Files\7-Zip\7z.exe", "e temp\\data.tar." + (File.Exists("temp\\data.tar.lzma") ? "lzma" : "gz") + " -o.");
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
                    if (!Directory.Exists("files\\bootstrap\\Library\\"))
                    {
                        Directory.CreateDirectory("files\\bootstrap\\Library\\");
                    }
                    if (!Directory.Exists("files\\bootstrap\\"))
                    {
                        Directory.CreateDirectory("files\\bootstrap");
                    }
                    if (Directory.Exists("files\\Library\\MobileSubstrate\\"))
                    {
                        Directory.CreateDirectory("files\\usr\\lib\\SBInject");
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
                    if (Directory.Exists("files\\Library\\Themes\\"))
                    {
                        Directory.CreateDirectory("files\\System\\Library\\");
                        Directory.Move("files\\Library\\Themes\\", "files\\System\\Library\\Themes\\");
                    }
                    if (Directory.Exists("files\\Library\\PreferenceBundles\\"))
                    {
                        Directory.Move("files\\Library\\PreferenceBundles\\", "files\\bootstrap\\Library\\PreferenceBundles\\");
                    }
                    if (Directory.Exists("files\\Library\\PreferenceLoader\\"))
                    {
                        Directory.Move("files\\Library\\PreferenceLoader\\", "files\\bootstrap\\Library\\PreferenceLoader\\");
                    }
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
