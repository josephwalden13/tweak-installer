//Copyright 2018 josephwalden
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


using jLib;
using SevenZipExtractor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using WinSCP;

namespace Unjailbreaker
{
    class Program
    {
        static bool install = false, uninstall = false, convert = false, manual = false, jtool = false, update = true, uicache = false, respring_override = false, uicache_override = false, onlyPerformSSHActions = false;
        static string convert_path(string i, bool unix = false)
        {
            if (!unix)
            {
                return i.Replace("\\", "/");//.Replace(" ", "\\ ").Replace("(", "\\(").Replace(")", "\\)").Replace("'", "\\'").Replace("@", "\\@");
            }
            else
            {
                return i.Replace("\\", "/").Replace(" ", "\\ ").Replace("(", "\\(").Replace(")", "\\)").Replace("'", "\\'").Replace("@", "\\@");
            }
        }
        static void finish(Session session)
        {
            if (uicache && !uicache_override)
            {
                Console.WriteLine("Running uicache (may take up to 30 seconds)");
                session.ExecuteCommand("uicache"); //respring
            }
            if (!respring_override)
            {
                Console.WriteLine("Respringing...");
                session.ExecuteCommand("killall -9 SpringBoard"); //respring
            }
            session.Close();
        }
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
                FileSystem.MoveDirectory(source, dest, true);
            }
        }
        [STAThread]
        static void Main(string[] args)
        {
            List<string> skip = File.Exists("skip.list") ? File.ReadAllLines("skip.list").ToList() : new List<string>();
            if (!File.Exists("settings"))
            {
                string[] def = new string[3];
                File.WriteAllLines("settings", def);
            }
            if (args.Contains("convert")) convert = true;
            if (args.Contains("uninstall")) uninstall = true;
            if (args.Contains("install")) install = true;
            if (args.Contains("manual")) manual = true;
            if (args.Contains("dont-update")) update = false;
            if (args.Contains("dont-refresh")) uicache_override = true;
            if (args.Contains("dont-respring")) respring_override = true;
            if (args.Contains("no-install")) onlyPerformSSHActions = true;

            createDirIfDoesntExist(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\tweak-installer\\");

            if (update)
            {
                //check for updates
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        string version = client.DownloadString("https://raw.githubusercontent.com/josephwalden13/tweak-installer/master/bin/Debug/version.txt");
                        string current = File.ReadAllText("version.txt");
                        if (current != version)
                        {
                            Console.WriteLine($"Version {version.Replace("\n", "")} released. Please download it from https://github.com/josephwalden13/tweak-installer/releases\nPress enter to continue...");
                            Console.ReadLine();
                        }
                    }
                }
                catch { }
            }

            string[] data = File.ReadAllLines("settings"); //get ssh settings
            for (int i = 0; i != data.Length; i++)
            {
                data[i] = data[i].Split('#')[0];
            }
            string ip = data[0];
            string user = "root"; //data[1];
            string password = data[2];

            Console.WriteLine("Connecting");
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = ip,
                UserName = user,
                Password = password,
                GiveUpSecurityAndAcceptAnySshHostKey = true
            };
            Session session = new Session();
            try
            {
                session.Open(sessionOptions);
            }
            catch (SessionRemoteException e)
            {
                if (e.ToString().Contains("refused")) Console.WriteLine("Error: SSH Connection Refused\nAre you jailbroken?\nHave you entered your devices IP correctly?");
                else if (e.ToString().Contains("Access denied")) Console.WriteLine("Error: SSH Connection Refused due to incorrect credentials. Are you sure you typed your password correctly?");
                else if (e.ToString().Contains("Cannot initialize SFTP protocol")) Console.WriteLine("Error: SFTP not available. Make sure you have sftp installed by default. For Yalu or Meridian, please install \"SCP and SFTP for dropbear\" by coolstar. For LibreIOS, make sure SFTP is moved to /usr/bin/.");
                else
                {
                    Console.WriteLine("Unknown Error. Please use the big red bug report link and include some form of crash report. Error report copying to clipboard.");
                    Thread.Sleep(2000);
                    Clipboard.SetText(e.ToString());
                    throw e;
                }
                Console.ReadLine();
                Environment.Exit(0);
            }

            if (onlyPerformSSHActions)
            {
                uicache = true;
                finish(session);
                return;
            }

            if (session.FileExists("/usr/lib/SBInject"))
            {
                convert = true;
                if (!session.FileExists("/bootstrap/Library/Themes"))
                {
                    session.CreateDirectory("/bootstrap/Library/Themes");
                    session.ExecuteCommand("touch /bootstrap/Library/Themes/dont-delete");
                    Console.WriteLine("Themes folder missing. Touching /bootstrap/Library/Themes/dont-delete to prevent this in future");
                }
                jtool = true;
            }
            if (session.FileExists("/jb/"))
            {
                jtool = true;
            }

            if (manual)
            {
                createDirIfDoesntExist("files");
                Console.WriteLine("Please move rootfs file into 'files' and press enter to continue");
                Console.ReadLine();
            }
            else
            {
                emptyDir("files");
                emptyDir("temp");
                deleteIfExists("data.tar");
                List<string> debs = new List<string>();
                foreach (string i in args)
                {
                    if (i.Contains(".deb"))
                    {
                        debs.Add(i);
                    }
                    if (i.Contains(".ipa"))
                    {
                        debs.Add(i);
                    }
                    if (i.Contains(".zip"))
                    {
                        debs.Add(i);
                    }
                }
                foreach (string deb in debs)
                {
                    if (deb.Contains(".deb"))
                    {
                        emptyDir("temp");
                        deleteIfExists("data.tar");
                        Console.WriteLine("Extracting " + deb);
                        try
                        {
                            using (ArchiveFile archiveFile = new ArchiveFile(deb))
                            {
                                archiveFile.Extract("temp");
                            }
                            var p = Process.Start(@"7z.exe", "e " + "temp\\data.tar." + (File.Exists("temp\\data.tar.lzma") ? "lzma" : "gz") + " -o.");
                            p.WaitForExit();
                            using (ArchiveFile archiveFile = new ArchiveFile("data.tar"))
                            {
                                archiveFile.Extract("files");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Not a valid deb file / Write Access Denied");
                            throw e;
                        };
                    }
                    else if (deb.Contains(".ipa"))
                    {
                        emptyDir("temp");
                        Console.WriteLine("Extracting IPA " + deb);
                        try
                        {
                            using (ArchiveFile archiveFile = new ArchiveFile(deb))
                            {
                                archiveFile.Extract("temp");
                            }
                            createDirIfDoesntExist("files\\Applications");
                            foreach (string app in Directory.GetDirectories("temp\\Payload\\"))
                            {
                                Directory.Move(app, "files\\Applications\\" + new DirectoryInfo(app).Name);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Not a valid IPA / Write Access Denied");
                            throw e;
                        }
                    }
                    else
                    {
                        emptyDir("temp");
                        Console.WriteLine("Extracting Zip " + deb);
                        try
                        {
                            using (ArchiveFile archiveFile = new ArchiveFile(deb))
                            {
                                archiveFile.Extract("temp");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Not a valid ZIP archive / Write Access Denied");
                            throw e;
                        }
                        if (Directory.Exists("temp\\bootstrap\\"))
                        {
                            Console.WriteLine("Found bootstrap");
                            if (Directory.Exists("temp\\bootstrap\\Library\\SBInject\\"))
                            {
                                createDirIfDoesntExist("files\\usr\\lib\\SBInject");
                                foreach (string file in Directory.GetFiles("temp\\bootstrap\\Library\\SBInject\\"))
                                {
                                    File.Move(file, "files\\usr\\lib\\SBInject\\" + new FileInfo(file).Name);
                                }
                                foreach (string file in Directory.GetDirectories("temp\\bootstrap\\Library\\SBInject\\"))
                                {
                                    Directory.Move(file, "files\\usr\\lib\\SBInject\\" + new DirectoryInfo(file).Name);
                                }
                                Directory.Delete("temp\\bootstrap\\Library\\SBInject", true);
                            }
                            moveDirIfPresent("temp\\bootstrap\\Library\\Themes\\", "files\\bootstrap\\Library\\Themes\\");
                            foreach (string dir in Directory.GetDirectories("temp"))
                            {
                                FileSystem.MoveDirectory(dir, "files\\" + new DirectoryInfo(dir).Name, true);
                            }
                            foreach (string file in Directory.GetFiles("temp"))
                            {
                                File.Copy(file, "files\\" + new FileInfo(file).Name, true);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Unrecognised format. Determining ability to install");
                            List<string> exts = new List<string>();
                            List<string> directories = new List<string>();
                            foreach (string dir in Directory.GetDirectories("temp", "*", System.IO.SearchOption.AllDirectories))
                            {
                                directories.Add(new DirectoryInfo(dir).Name);
                            }
                            if (directories.Contains("bootstrap"))
                            {
                                Console.WriteLine("Found bootstrap");
                                foreach (string dir in Directory.GetDirectories("temp", "*", System.IO.SearchOption.AllDirectories))
                                {
                                    if (new DirectoryInfo(dir).Name == "bootstrap")
                                    {
                                        createDirIfDoesntExist("files\\bootstrap\\");
                                        FileSystem.CopyDirectory(dir, "files\\bootstrap");
                                        moveDirIfPresent("files\\bootstrap\\SBInject", "files\\bootstrap\\Library\\SBInject", "files\\bootstrap\\Library\\SBInject");
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                foreach (string i in Directory.GetFiles("temp"))
                                {
                                    string ext = new FileInfo(i).Extension;
                                    if (!exts.Contains(ext)) exts.Add(ext);
                                }
                                if (exts.Count == 2 && exts.Contains(".dylib") && exts.Contains(".plist"))
                                {
                                    Console.WriteLine("Substrate Addon. Installing");
                                    createDirIfDoesntExist("files\\usr\\lib\\SBInject");
                                    foreach (string i in Directory.GetFiles("temp"))
                                    {
                                        File.Copy(i, "files\\usr\\lib\\SBInject\\" + new FileInfo(i).Name, true);
                                    }
                                    moveDirIfPresent("files\\Library\\PreferenceBundles\\", "files\\bootstrap\\Library\\PreferenceBundles\\");
                                    moveDirIfPresent("files\\Library\\PreferenceLoader\\", "files\\bootstrap\\Library\\PreferenceLoader\\");
                                }
                                else
                                {
                                    Console.WriteLine("Unsafe to install. To install this tweak you must do so manually. Press enter to continue...");
                                    Console.ReadLine();
                                }
                            }
                        }
                    }
                }
            }
            if (convert) //convert to electra format
            {
                Console.WriteLine("Converting to electra tweak format");
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
                moveDirIfPresent("files\\Library\\Themes\\", "files\\bootstrap\\Library\\Themes\\");
                moveDirIfPresent("files\\Library\\PreferenceBundles\\", "files\\bootstrap\\Library\\PreferenceBundles\\");
                moveDirIfPresent("files\\Library\\PreferenceLoader\\", "files\\bootstrap\\Library\\PreferenceLoader\\");
            }
            Crawler c = new Crawler("files", true); //gets all files in the tweak
            c.Remove("DS_STORE");
            string s = "";
            c.Files.ForEach(i =>
                                               {
                                                   s += ("rm " + convert_path(i) + "\n"); //creates uninstall script for tweak (used if uninstall == true)
                                               });
            //File.WriteAllText("files\\" + name + ".sh", s); //add uninstall script to install folder
            if (args.Length > 0)
            {
                if (install)
                {
                    Console.WriteLine("Preparing to install");
                    if (session.FileExists("/plat.ent"))
                    {
                        session.RemoveFiles("/plat.ent");
                    }
                    createDirIfDoesntExist("backup");
                    if (Directory.Exists("files\\Applications") && jtool)
                    {
                        File.Copy("plat.ent", "files\\plat.ent", true);
                    }
                    if (Directory.Exists("files\\Applications\\electra.app"))
                    {
                        var f = MessageBox.Show("Please do not try this");
                        Environment.Exit(0);
                    }
                    bool overwrite = false;
                    c = new Crawler("files", true); //gets all files in the tweak
                    c.Remove("DS_STORE");
                    string[] directories = Directory.GetDirectories("files", "*", searchOption: System.IO.SearchOption.AllDirectories);
                    foreach (string dir in directories)
                    {
                        if (!session.FileExists(convert_path(dir.Replace("files", ""))))
                        {
                            //crappy method - will change
                            string pathstr = "/";
                            foreach (string sub in convert_path(dir.Replace("files", "")).Split('/'))
                            {
                                pathstr += sub + '/';
                                if (session.FileExists(pathstr)) continue;
                                //Console.WriteLine("Creating " + pathstr);
                                session.CreateDirectory(pathstr);
                            }
                        }
                    }
                    long size = 0;
                    long done = 0;
                    foreach (string file in Directory.GetFiles("files", "*", System.IO.SearchOption.AllDirectories))
                    {
                        size += new FileInfo(file).Length;
                    }
                    int percentage = -1;
                    Console.WriteLine("Installing");
                    Console.Write("0%");
                    c.Files.ForEach(i =>
                    {
                        done += new FileInfo("files\\" + i).Length;
                        if (Math.Floor((done * 100) / (double)size) != percentage)
                        {
                            percentage = (int)Math.Floor((done * 100) / (double)size);
                            Console.Write("\b\b\b   \b\b\b");
                            Console.Write(percentage + "%");
                        }
                        bool go = false, action = false;
                        if (session.FileExists(convert_path(i)) && !overwrite)
                        {
                            Console.WriteLine("Do you want to backup and overwrite " + convert_path(i) + "? (y/n/a)");
                            while (true)
                            {
                                switch (Console.ReadKey().Key)
                                {
                                    case ConsoleKey.Y:
                                        go = true;
                                        action = true;
                                        break;
                                    case ConsoleKey.A:
                                        go = true;
                                        action = true;
                                        overwrite = true;
                                        break;
                                    case ConsoleKey.N:
                                        action = false;
                                        go = true;
                                        break;
                                }
                                Console.WriteLine();
                                if (go) break;
                            }
                        }
                        else
                        {
                            action = true;
                        }
                        if (!action)
                        {
                            if (!skip.Contains(i))
                            {
                                skip.Add(i);
                            }
                        }
                        string path = i.Replace(i.Substring(i.LastIndexOf('\\')), "");
                        //Console.WriteLine("Backing up " + path);
                        createDirIfDoesntExist("backup\\" + path);
                        session.GetFiles(convert_path(i), "backup\\" + path + "\\" + new FileInfo(i).Name);
                        if (action || overwrite)
                        {
                            if (skip.Contains(i)) skip.Remove(i);
                            //Console.WriteLine("Uploading " + convert_path(i));
                            //Console.WriteLine("File exists? " + File.Exists("files\\" + i));
                            session.PutFiles("files\\" + i, convert_path(i));
                        }
                    });
                    Console.Write("\b\b\b\b    \b\b\b\bDone\n");
                    File.WriteAllLines("skip.list", skip);
                    if (Directory.Exists("files\\Applications") && jtool)
                    {
                        Console.WriteLine("Signing applications");
                        foreach (var app in Directory.GetDirectories("files\\Applications\\"))
                        {
                            //Crawler crawler = new Crawler(app);
                            //Dictionary<string, object> dict = (Dictionary<string, object>)Plist.readPlist(app + "\\Info.plist");
                            //string bin = dict["CFBundleExecutable"].ToString();
                            c.Files.ForEach(i =>
                            {
                                if (i.Contains("\\Applications\\"))
                                {
                                    uicache = true;
                                    bool sign = false;
                                    //if (new FileInfo(i).Name == bin)
                                    //{
                                    //    i = convert_path(i);
                                    //    session.ExecuteCommand("jtool -e arch -arch arm64 " + i);
                                    //    session.ExecuteCommand("mv " + i + ".arch_arm64 " + i);
                                    //    session.ExecuteCommand("jtool --ent " + i + " > orig.ent");
                                    //    session.GetFiles("orig.ent", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\tweak-installer\\orig.ent");
                                    //    string ent = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\tweak-installer\\orig.ent").Replace("</dict>", "").Replace("</plist>", "");
                                    //    string[] plat = File.ReadAllLines("plat.ent");
                                    //    for (int j = 3; j != plat.Length; j++)
                                    //    {
                                    //        ent += plat[j];
                                    //    }
                                    //    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\tweak-installer\\orig.ent", ent);
                                    //    session.PutFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\tweak-installer\\orig.ent", "orig.ent");
                                    //    session.ExecuteCommand("jtool --sign --ent orig.ent --inplace " + i);
                                    //}
                                    //else
                                    {
                                        if (new FileInfo(i).Name.Split('.').Length < 2) sign = true;
                                        if (!sign)
                                        {
                                            if (i.Split('.').Last() == "dylib") sign = true;
                                        }
                                        i = convert_path(i);
                                        if (sign)
                                        {
                                            session.ExecuteCommand("jtool -e arch -arch arm64 " + i);
                                            session.ExecuteCommand("mv " + i + ".arch_arm64 " + i);
                                            session.ExecuteCommand("jtool --sign --ent /plat.ent --inplace " + i);
                                        }
                                    }
                                }
                            });
                        }
                    }
                    finish(session);
                }
                else if (uninstall)
                {
                    Console.WriteLine("Uninstalling");
                    bool overwrite = false;
                    c = new Crawler("files", true); //gets all files in the tweak
                    c.Remove("DS_STORE");
                    c.Files.ForEach(i =>
                    {
                        if (!skip.Contains(i))
                        {
                            //Console.WriteLine(convert_path(i));
                            bool go = false, action = false;
                            if (File.Exists("backup\\" + convert_path(i)) && !overwrite)
                            {
                                Console.WriteLine("Do you want to restore " + convert_path(i) + " from your backup? (y/n/a)");
                                while (true)
                                {
                                    switch (Console.ReadKey().Key)
                                    {
                                        case ConsoleKey.Y:
                                            go = true;
                                            action = true;
                                            break;
                                        case ConsoleKey.A:
                                            go = true;
                                            action = true;
                                            overwrite = true;
                                            break;
                                        case ConsoleKey.N:
                                            go = true;
                                            break;
                                    }
                                    Console.WriteLine();
                                    if (go) break;
                                }
                            }
                            session.ExecuteCommand("rm " + convert_path(i, true));
                            if (action || overwrite)
                            {
                                string path = i.Replace(i.Substring(i.LastIndexOf('\\')), "");
                                session.PutFiles(new FileInfo("backup" + convert_path(i)).ToString().Replace("/", "\\"), convert_path(path) + "/" + new FileInfo(i).Name);
                            }
                        }
                    });
                    if (Directory.Exists("files\\Applications"))
                    {
                        uicache = true;
                    }
                    Console.WriteLine("Locating and removing *some* empty folders");
                    session.ExecuteCommand("find /System/Library/Themes/ -type d -empty -delete");
                    session.ExecuteCommand("find /usr/ -type d -empty -delete");
                    session.ExecuteCommand("find /Applications/ -type d -empty -delete");
                    session.ExecuteCommand("find /Library/ -type d -empty -delete");
                    session.ExecuteCommand("find /bootstrap/Library/Themes/ -type d -empty -delete");
                    session.ExecuteCommand("find /bootstrap/Library/SBInject/ -type d -empty -delete");
                    finish(session);
                }
            }
        }
    }
}