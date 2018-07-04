//Copyright 2018 josephwalden
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using jLib;
using Microsoft.VisualBasic.FileIO;
using SevenZipExtractor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinSCP;

namespace Tweak_Installer
{
    public partial class Main : Form
    {
        static bool verbose = false, update = true, enabled = false;
        //md5 function from https://blogs.msdn.microsoft.com/csharpfaq/2006/10/09/how-do-i-calculate-a-md5-hash-from-a-string/
        public string CalculateMD5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();

        }
        public Main()
        {
            InitializeComponent();
        }

        private void select_Click(object sender, EventArgs e)
        {
            openFileDialog.Multiselect = true; //allow file dialog to select multiple files
            openFileDialog.Filter = "Tweaks|*.deb;*.zip;*.ipa"; //set valid files to deb, zip or ipa
            tweaks.Clear(); //clear current list of debs
            var f = openFileDialog.ShowDialog(); //open the file dialog
            switch (f)
            {
                case DialogResult.OK:
                    {
                        enabled = true; //enable install / uninstall functions
                        foreach (string i in openFileDialog.FileNames)
                        {
                            tweaks.Add(i); //add each file to the deb list
                        }
                        break;
                    }
                default:
                    enabled = false;
                    break;
            }
        }

        private void install_Click(object sender, EventArgs e)
        {
            emptyDir("files"); //empty temporary directory
            if (!enabled)
            {
                MessageBox.Show("Please select a deb, ipa or zip first");
                return;
            }
            clean(); //clear temporary files / directories
            getOptions(); //get options 
            session = getSession(host.Text, "root", pass.Text, int.Parse(port.Text)); //open an ssh session
            getJailbreakSpecificOptions(session); //get options specific to certain jailbreaks - e.g. sign binaries with platform-binary on Libre / Electra
            foreach (string tweak in tweaks)
            {
                clean(); //clear temp files from previous packages / apps
                if (tweak.Contains(".deb")) //if file is a deb
                {
                    if (rc) //if full jailbreak
                    {
                        string file = CalculateMD5Hash(tweak) + ".deb"; //calculate hash to use as filename
                        if (!session.FileExists("/tweakinstaller")) session.CreateDirectory("/tweakinstaller"); //create folder to store debs
                        session.PutFiles(tweak, "/tweakinstaller/" + file); //move deb file to tweak installer directory
                        var i = session.ExecuteCommand("dpkg -i /tweakinstaller/" + file); //install using dpkg
                        session.RemoveFiles("/tweakinstaller/" + file); //delete deb
                        if (i.IsSuccess)
                        {
                            log("Installed " + tweak + " with dpkg"); //log sucess
                        }
                        else
                        {
                            log(i.ErrorOutput); //log failure
                        }
                    }
                    else
                    {
                        extractDeb(tweak); //extract deb for manual installation
                    }
                }
                else if (tweak.Contains(".ipa")) //if file is an ipa
                {
                    if (rc) //if full jailbreak
                    {
                        log("Installing IPA with AppSync");
                        if (!session.FileExists("/tweakinstaller")) session.CreateDirectory("/tweakinstaller"); //create tweakinstaller directory if it doesn't exist
                        if (session.FileExists("/usr/bin/appinst")) //check for appinst
                        {
                            string file = CalculateMD5Hash(tweak) + ".ipa"; //calculate hash to use as filename
                            session.PutFiles(tweak, "/tweakinstaller/" + file); //move ipa to tweakinstaller dir
                            var i = session.ExecuteCommand("appinst /tweakinstaller/" + file); //use appinst to install ipa
                            session.RemoveFiles("/tweakinstaller/" + file); //remove ipa
                            if (i.IsSuccess)
                            {
                                log("Installed IPA"); //log success
                            }
                            else
                            {
                                log(i.ErrorOutput); //log failure
                            }
                        }
                        else
                        {
                            log("Couldn't install ipa. Please install appinst and AppSync from Karen's repo (https://cydia.angelxwind.net/)");
                        }
                    }
                    else
                    {
                        extractIPA(tweak); //extract ipa for manual installation
                    }
                }
                else if (tweak.Contains(".zip")) //if file is a zip
                {
                    extractZip(tweak); //try to extract zip file
                }
            }
            if (convert) convertTweaks(); //if tweaks need converting to a different format (e.g. for electra 0.X) convert them
            getFiles(); //get list of files in tweaks
            installFiles(session); //install files over ssh
            log(""); //log newline
        }

        private void Uninstall_Click(object sender, EventArgs e)
        {
            emptyDir("files"); //remove temporary files
            if (!enabled)
            {
                MessageBox.Show("Please select a deb, ipa or zip first");
                return;
            }
            getOptions(); //get options
            session = getSession(host.Text, "root", pass.Text, int.Parse(port.Text)); //open ssh session
            getJailbreakSpecificOptions(session); //get tool specific options (e.g. whether or not files should be signed differently)
            foreach (string tweak in tweaks)
            {
                clean(); //remove temporary files & directories
                if (tweak.Contains(".deb"))
                {
                    if (rc) //if full jailbreak
                    {
                        var i = session.ExecuteCommand("dpkg -r " + getPkgID(tweak)); //get package id and remove using dpkg
                        log("Removed " + tweak + " with dpkg"); //log success
                    }
                    else
                    {
                        extractDeb(tweak); //extract files for uninstallation
                    }
                }
                else if (tweak.Contains(".ipa")) //if an ipa
                {
                    if (rc)
                    {
                        log("Can't remove IPA on this version of Electra, please uninstall via SpringBoard"); //can't remove apps via ssh
                    }
                    else
                    {
                        extractIPA(tweak); //extract apps files for uninstallation
                    }
                }
                else if (tweak.Contains(".zip")) //if a zip
                {
                    extractZip(tweak); //try to extract zip
                }
            }
            if (convert) convertTweaks(); //if files need to be converted for new format convert them
            if (File.Exists("prerm")) //check for prerm script
            {
                if (verbose) log("Running prerm script");
                session.PutFiles("prerm", "script");
                session.ExecuteCommand("./script && rm script");
                //run prerm script
            }
            getFiles(); //get list of files to remove
            uninstallFiles(session); //uninstall files over ssh
            log(""); //log newline
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (Environment.GetCommandLineArgs().Contains("dont-update")) update = false; //if dont-update in command line args set update to false
            
            //check for updates
            if (update)
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        string current = File.ReadAllText("version.txt"); //read version.txt for current version
                        string version = client.DownloadString("https://raw.githubusercontent.com/josephwalden13/tweak-installer/master/bin/Debug/version.txt"); //get latest version from my repo
                        if (current != version) //if they don't match show update prompt
                        {
                            var f = MessageBox.Show(caption: "Update Available!", text: ($"Version {version.Replace("\n", "")} released. Please download it from https://github.com/josephwalden13/tweak-installer/releases\nWould you like to update?"), buttons: MessageBoxButtons.YesNo);
                            if (f == DialogResult.Yes)
                            {
                                Process.Start("https://github.com/josephwalden13/tweak-installer/releases");
                            }
                        }
                    }
                }
                catch { }
            }
            if (!File.Exists("settings")) //if settings file doesn't exist create a default one
            {
                string[] def = new string[] { "192.168.1.1", "22", "" };
                File.WriteAllLines("settings", def);
            }

            string[] data = File.ReadAllLines("settings"); //get ssh settings
            for (int i = 0; i != data.Length; i++)
            {
                data[i] = data[i].Split('#')[0];
            }
            host.Text = data[0];
            port.Text = data[1];
            pass.Text = data[2];
            if (port.Text == "" || port.Text == "root" /*(port is the same line as user used to be so if port==root reset it)*/)
            {
                port.Text = "22";
            }
        }
        
        private void host_TextChanged(object sender, EventArgs e)
        {
            string[] data = { host.Text, port.Text, pass.Text };
            File.WriteAllLines("settings", data);
        }

        private void pass_TextChanged(object sender, EventArgs e)
        {
            string[] data = { host.Text, port.Text, pass.Text };
            File.WriteAllLines("settings", data);
        }

        void respring_Click(object sender, EventArgs e)
        {
            session = getSession(host.Text, "root", pass.Text, int.Parse(port.Text)); //get ssh session
            log("Respringing");
            session.ExecuteCommand("killall -9 SpringBoard"); //kill springboard
            log("Done");
            log("");
            session.Close();
        }

        private void uicache_Click(object sender, EventArgs e)
        {
            session = getSession(host.Text, "root", pass.Text, int.Parse(port.Text)); //get ssh session
            log("Running uicache");
            session.ExecuteCommand("uicache"); //run uicache
            session.ExecuteCommand("Done");
            log("");
            session.Close();
        }

        private void error_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/josephwalden13/tweak-installer/issues");
        }

        private void debslnk_Click(object sender, EventArgs e)
        {
            Process.Start("http://s0n1c.org/cydia/");
        }

        private void reddit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.reddit.com/user/josephwalden/");
        }

        private void creator_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.reddit.com/user/josephwalden/");
        }

        private void ui_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.reddit.com/user/brnnkr/");
        }

        private void twitter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://twitter.com/jmw_2468");
        }

        private void github_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/josephwalden13/");
        }

        private void paypal_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://paypal.me/JosephWalden");
        }

        private void autolabel_Click(object sender, EventArgs e)
        {
            auto.Checked = !auto.Checked;
        }

        private void port_TextChanged(object sender, EventArgs e)
        {
            string[] data = { host.Text, port.Text, pass.Text };
            File.WriteAllLines("settings", data);
        }

        private void version_Click(object sender, EventArgs e)
        {
            verbose = true;
        }

        private void terminal_Click(object sender, EventArgs e)
        {
            Process.Start("putty.exe", host.Text + ":" + port.Text + " -l root -pw " + pass.Text);
        }

        //tweak installer library

        public List<string> tweaks = new List<string>(), skip = new List<string>();
        public string[] data;
        public bool uicache = false, jtool = false, convert = false, dont_sign = false, dont_del_empty_dirs = false, rc = false;
        public Crawler crawler;
        public string user;
        public Session session;

        public string convert_path(string i, bool unix = false)
        {
            if (!unix)
            {
                return i.Replace("\\", "/");
            }
            else
            {
                return i.Replace("\\", "/").Replace(" ", "\\ ").Replace("(", "\\(").Replace(")", "\\)").Replace("'", "\\'").Replace("@", "\\@");
            }
        }
        public void createDirIfDoesntExist(string path, bool verbose = false)
        {
            if (!Directory.Exists(path))
            {
                if (verbose) log("Creating directory " + path);
                Directory.CreateDirectory(path);
                if (verbose) log("Created directory " + path);
            }
            else
            {
                if (verbose) log("No need to create " + path + " as it already exists");
            }
        }
        public void deleteIfExists(string path, bool verbose = false)
        {
            if (verbose) log("Searching for " + path);
            if (File.Exists(path))
            {
                if (verbose) log("Deleting " + path);
                File.Delete(path);
                if (verbose) log("Deleted " + path);
            }
        }
        public void emptyDir(string path, bool verbose = false)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                if (verbose) log("Deleted " + path);
            }
            Directory.CreateDirectory(path);
            if (verbose) log("Created directory " + path);
        }
        public void moveDirIfPresent(string source, string dest, string parent = null, bool verbose = false)
        {
            if (Directory.Exists(source))
            {
                if (verbose) log("Found " + source);
                if (parent != null)
                {
                    createDirIfDoesntExist(parent);
                    if (verbose) log("Created " + parent);
                }
                FileSystem.MoveDirectory(source, dest, true);
                if (verbose) log("Moved " + source + " to " + dest);
            }
        }
        public void log(string s)
        {
            if (!File.Exists("log.txt")) File.Create("log.txt").Close();
            try
            {
                File.AppendAllText("log.txt", s + Environment.NewLine);
                Console.WriteLine(s);
                output.Text += s + Environment.NewLine;
                output.SelectionStart = output.Text.Length;
                output.ScrollToCaret();
            }
            catch
            {
                Thread.Sleep(1000);
                log(s);
            }
        }

        public void getOptions()
        {
            skip = File.Exists("skip.list") ? File.ReadAllLines("skip.list").ToList() : new List<string>(); //get files to skip
        }

        public void extractZip(string path)
        {
            log("Extracting Zip " + path);
            try
            {
                using (ArchiveFile archiveFile = new ArchiveFile(path))
                {
                    if (verbose) log("Extracting zip");
                    archiveFile.Extract("temp");
                    if (verbose) log("Extracted zip");
                }
            }
            catch (Exception e)
            {
                log("Not a valid ZIP archive / Access Denied");
                throw e;
            }
            if (Directory.Exists("temp\\bootstrap\\"))
            {
                log("Found bootstrap");
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
                log("Unrecognised format. Determining ability to install");
                List<string> exts = new List<string>();
                List<string> directories = new List<string>();
                foreach (string dir in Directory.GetDirectories("temp", "*", System.IO.SearchOption.AllDirectories))
                {
                    directories.Add(new DirectoryInfo(dir).Name);
                }
                if (directories.Contains("bootstrap"))
                {
                    log("Found bootstrap");
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
                        log("Substrate Addon. Installing");
                        createDirIfDoesntExist("files\\usr\\lib\\SBInject");
                        foreach (string i in Directory.GetFiles("temp"))
                        {
                            File.Copy(i, "files\\usr\\lib\\SBInject\\" + new FileInfo(i).Name, true);
                        }
                        moveDirIfPresent("files\\Library\\PreferenceBundles\\", "files\\bootstrap\\Library\\PreferenceBundles\\");
                        moveDirIfPresent("files\\Library\\PreferenceLoader\\", "files\\bootstrap\\Library\\PreferenceLoader\\");
                        moveDirIfPresent("files\\Library\\LaunchDaemons\\", "files\\bootstrap\\Library\\LaunchDaemons\\");
                    }
                    else
                    {
                        MessageBox.Show("Unsafe to install. To install this tweak you must do so manually. Press enter to continue...");
                        Environment.Exit(0);
                    }
                }
            }
        }

        public void extractIPA(string path)
        {
            clean();
            log("Extracting IPA " + path);
            try
            {
                using (ArchiveFile archiveFile = new ArchiveFile(path))
                {
                    if (verbose) log("Extracting payload");
                    archiveFile.Extract("temp");
                }
                createDirIfDoesntExist("files\\Applications");
                foreach (string app in Directory.GetDirectories("temp\\Payload\\"))
                {
                    if (verbose) log("Moving payload");
                    Directory.Move(app, "files\\Applications\\" + new DirectoryInfo(app).Name);
                    if (verbose) log("Moved payload");
                }
            }
            catch (Exception e)
            {
                log("Not a valid IPA / Access Denied");
                throw e;
            }
        }

        public void extractDeb(string path)
        {
            clean();
            log("Extracting " + path);
            try
            {
                using (ArchiveFile archiveFile = new ArchiveFile(path))
                {
                    if (verbose) log("Extracting data.tar.lzma || data.tar.gz");
                    archiveFile.Extract("temp");
                    if (verbose) log("Extracted");
                }
                if (verbose) log("Extracting data.tar");
                var p = Process.Start(@"7z.exe", "e " + "temp\\data.tar." + (File.Exists("temp\\data.tar.lzma") ? "lzma" : "gz") + " -o.");
                if (verbose) log("Waiting for subprocess to complete");
                p.WaitForExit();
                if (verbose) log("Extracting control file");
                p = Process.Start(@"7z.exe", "e " + "temp\\control.tar.gz -o.");
                p.WaitForExit();
                if (verbose) log("Successfully extracted data.tar");
                using (ArchiveFile archiveFile = new ArchiveFile("data.tar"))
                {
                    if (verbose) log("Extracting deb files");
                    archiveFile.Extract("files");
                    if (verbose) log("Extracted");
                }
                using (ArchiveFile archiveFile = new ArchiveFile("control.tar"))
                {
                    archiveFile.Extract(".");
                }
                Dictionary<string, string> control = new Dictionary<string, string>();
                foreach (string i in File.ReadAllLines("control"))
                {
                    var j = i.Split(':');
                    if (j.Length < 2) continue;
                    control.Add(j[0].ToLower().Replace(" ", ""), j[1]);
                }
                if (Directory.Exists("files\\Applications") && control.ContainsKey("skipsigning"))
                {
                    using (ArchiveFile archiveFile = new ArchiveFile("data.tar"))
                    {
                        archiveFile.Extract("temp");
                    }
                    foreach (string app in Directory.GetDirectories("temp\\Applications\\"))
                    {
                        File.Create(app.Replace("temp\\", "files\\") + "\\skip-signing").Close();
                    }
                }
                clean();
            }
            catch (Exception e)
            {
                log("Not a valid deb file / Access Denied");
                throw e;
            };
        }

        string getPkgID(string path)
        {
            clean();
            try
            {
                using (ArchiveFile archiveFile = new ArchiveFile(path))
                {
                    archiveFile.Extract("temp");
                }
                Process p = Process.Start(@"7z.exe", "e " + "temp\\control.tar.gz -o.");
                p.WaitForExit();
                using (ArchiveFile archiveFile = new ArchiveFile("control.tar"))
                {
                    archiveFile.Extract(".");
                }
                Dictionary<string, string> control = new Dictionary<string, string>();
                foreach (string i in File.ReadAllLines("control"))
                {
                    var j = i.Split(':');
                    if (j.Length < 2) continue;
                    control.Add(j[0].ToLower().Replace(" ", ""), j[1]);
                }
                clean();
                return control["package"];
            }
            catch (Exception e)
            {
                log("Not a valid deb file / Access Denied");
                throw e;
            };
        }

        public void clean()
        {
            deleteIfExists("JMWCrypto.dll");
            emptyDir("temp");
            deleteIfExists("data.tar");
            deleteIfExists("control.tar");
            deleteIfExists("control");
            deleteIfExists("postinst");
            deleteIfExists("prerm");
            deleteIfExists("postrm");
        }

        public void getJailbreakSpecificOptions(Session session)
        {
            if (session.FileExists("/usr/lib/SBInject") && !session.FileExists("/electra"))
            {
                if (verbose) log("You're running Electa (non RC build). I'll convert tweaks to that format & add entitlements to applications");
                convert = true;
                if (!session.FileExists("/bootstrap/Library/Themes"))
                {
                    session.CreateDirectory("/bootstrap/Library/Themes");
                    session.ExecuteCommand("touch /bootstrap/Library/Themes/dont-delete");
                    log("Themes folder missing. Touching /bootstrap/Library/Themes/dont-delete to prevent this in future");
                }
                jtool = true;
            }
            if (session.FileExists("/usr/bin/dpkg"))
            {
                if (!session.FileExists("/var/lib/dpkg/updates"))
                {
                    session.CreateDirectory("/var/lib/dpkg/updates");
                    log("Fixed dpkg");
                }
                dont_del_empty_dirs = true;
                rc = true;
            }
            if (session.FileExists("/jb/"))
            {
                if (verbose) log("You're running LibreiOS. I'll add entitlements to applications");
                jtool = true;
            }
            /*
             * check if jakeshacks jailbreak being used
             * set convert to true
             */
        }

        public Session getSession(string ip, string user, string password, int port)
        {
            log("Connecting");
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = ip,
                UserName = user,
                Password = password,
                PortNumber = port,
                GiveUpSecurityAndAcceptAnySshHostKey = true
            };
            Session session = new Session();
            try
            {
                session.Open(sessionOptions);
            }
            catch (SessionRemoteException e)
            {
                if (e.ToString().Contains("refused")) MessageBox.Show("Error: SSH Connection Refused\nAre you jailbroken?\nHave you entered your devices IP and port correctly?");
                else if (e.ToString().Contains("Access denied")) MessageBox.Show("Error: SSH Connection Refused due to incorrect credentials. Are you sure you typed your password correctly?");
                else if (e.ToString().Contains("Cannot initialize SFTP protocol")) MessageBox.Show("Error: SFTP not available. Make sure you have sftp installed by default. For Yalu or Meridian, please install \"SCP and SFTP for dropbear\" by coolstar. For LibreIOS, make sure SFTP is moved to /usr/bin/.");
                else
                {
                    Clipboard.SetText(e.ToString());
                    MessageBox.Show("Unknown Error. Please use the big red bug report link and include some form of crash report. Error report copying to clipboard.");
                    throw e;
                }
                Environment.Exit(0);
            }
            log("Connected to SSH");
            return session;
        }

        public void finish(Session session)
        {
            if (uicache && auto.Checked)
            {
                log("Running uicache (may take up to 30 seconds)");
                session.ExecuteCommand("uicache"); //respring
            }
            if (auto.Checked)
            {
                log("Respringing...");
                session.ExecuteCommand("killall -9 SpringBoard"); //respring
            }
            session.Close();
        }

        private void selectDebsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tweaks.Clear();
            var f = openFileDialog.ShowDialog();
            switch (f)
            {
                case DialogResult.OK:
                    {
                        enabled = true;
                        foreach (string i in openFileDialog.FileNames)
                        {
                            tweaks.Add(i);
                        }
                        break;
                    }
                default:
                    enabled = false;
                    break;
            }
        }

        private void extractEntitlementsFromLocalFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "";
            var i = openFileDialog.ShowDialog();
            switch (i)
            {
                case DialogResult.OK:
                    session = getSession(host.Text, "root", pass.Text, int.Parse(port.Text));
                    session.PutFiles(openFileDialog.FileName, "file");
                    session.ExecuteCommand("jtool -e arch -arch arm64 file && mv file.arch_arm64 file && jtool --ent file >> new.ent");
                    session.GetFiles("new.ent", new FileInfo(openFileDialog.FileName).Name + ".ent");
                    session.ExecuteCommand("rm new.ent file");
                    session.Close();
                    output.Text += "Extracted entitlements to Tweak Installer directory" + Environment.NewLine;
                    break;
            }
        }

        private void dontSignApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dont_sign = true;
            output.Text += "Won't sign applications" + Environment.NewLine;
        }

        private void useCustomEntitlementsFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Entitlements|*.ent|All Files|*.*";
            var i = openFileDialog.ShowDialog();
            switch (i)
            {
                case DialogResult.OK:
                    if (File.Exists(openFileDialog.FileName))
                    {
                        deleteIfExists("entitlements.ent");
                        File.Copy(openFileDialog.FileName, "entitlements.ent");
                        output.Text += "Using custom entitlements" + Environment.NewLine;
                    }
                    break;
            }
        }

        private void useDefaultEntitlementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists("platform-binary.ent"))
            {
                deleteIfExists("entitlements.ent");
                File.Copy("platform-binary.ent", "entitlements.ent");
                output.Text += "Using default entitlements" + Environment.NewLine;
            }
            else
            {
                using (WebClient c = new WebClient())
                {
                    c.DownloadFile("https://raw.githubusercontent.com/josephwalden13/tweak-installer/master/bin/Debug/platform-binary.ent", "platform-binary.ent");
                    deleteIfExists("entitlements.ent");
                    File.Copy("platform-binary.ent", "entitlements.ent");
                    output.Text += "Using default entitlements" + Environment.NewLine;
                }
            }
        }

        private void verboseModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            verbose = true;
            output.Text += "Verbose mode" + Environment.NewLine;
        }

        public void convertTweaks()
        {
            /*
             * add code to check if jakeshacks jailbreak
             * patch tweaks for that
             */
            log("Converting to electra tweak format");
            createDirIfDoesntExist("files\\bootstrap");
            createDirIfDoesntExist("files\\bootstrap\\Library");
            if (Directory.Exists("files\\Library\\MobileSubstrate\\"))
            {
                if (verbose) log("Found MobileSubstrate");
                createDirIfDoesntExist("files\\usr\\lib\\SBInject");
                foreach (string file in Directory.GetFiles("files\\Library\\MobileSubstrate\\DynamicLibraries\\"))
                {
                    if (verbose) log("Moving Substrate file " + file + " to SBInject");
                    File.Move(file, "files\\usr\\lib\\SBInject\\" + new FileInfo(file).Name);
                }
                foreach (string file in Directory.GetDirectories("files\\Library\\MobileSubstrate\\DynamicLibraries\\"))
                {
                    if (verbose) log("Moving Substrate dir " + file + " to SBInject");
                    Directory.Move(file, "files\\usr\\lib\\SBInject\\" + new DirectoryInfo(file).Name);
                }
                Directory.Delete("files\\Library\\MobileSubstrate", true);
                if (verbose) log("Deleted MobileSubstrate folder");
            }
            moveDirIfPresent("files\\Library\\Themes\\", "files\\bootstrap\\Library\\Themes\\");
            moveDirIfPresent("files\\Library\\LaunchDaemons\\", "files\\bootstrap\\Library\\LaunchDaemons\\");
            moveDirIfPresent("files\\Library\\PreferenceBundles\\", "files\\bootstrap\\Library\\PreferenceBundles\\");
            moveDirIfPresent("files\\Library\\PreferenceLoader\\", "files\\bootstrap\\Library\\PreferenceLoader\\");
        }

        public void getFiles()
        {
            if (verbose) log("Getting all files");
            crawler = new Crawler(Environment.CurrentDirectory + "\\files", true); //gets all files in the tweak
            crawler.Remove("DS_STORE");
        }

        public void installFiles(Session session)
        {
            if (session.FileExists("/entitlements.ent"))
            {
                session.RemoveFiles("/entitlements.ent");
                if (verbose) log("Removed old entitlements file from the device");
            }
            createDirIfDoesntExist("backup");
            if (Directory.Exists("files\\Applications") && jtool)
            {
                File.Copy("entitlements.ent", "files\\entitlements.ent", true);
                if (verbose) log("Entitlements needed. Copying entitlements file");
            }
            if (Directory.Exists("files\\Applications\\electra.app"))
            {
                if (verbose) log("please no");
                var f = MessageBox.Show("Please do not try this");
                Environment.Exit(0);
            }
            if (verbose) log("Creating directory list");
            string[] directories = Directory.GetDirectories("files", "*", searchOption: System.IO.SearchOption.AllDirectories);
            if (verbose) log("Got list. Creating backup folders");
            foreach (string dir in directories)
            {
                if (!Directory.Exists("backup\\" + dir.Replace("files\\", "\\")))
                {
                    Directory.CreateDirectory("backup\\" + dir.Replace("files\\", "\\"));
                }
            }
            log("Preparing to install");

            if (verbose) log("Creating local file list");
            List<string> local = new List<string>();
            crawler.Files.ForEach(i => local.Add(convert_path(i)));

            if (verbose) log("Creating remote file list");
            List<string> remote = new List<string>();
            foreach (string i in Directory.GetDirectories("files"))
            {
                string dir = new DirectoryInfo(i).Name;
                if (dir == "System")
                {
                    log("This tweak may take longer than usual to process (45 second max)");
                }
                session.ExecuteCommand("find /" + dir + " > ~/files.list");
                session.GetFiles("/var/root/files.list", "files.list");
                foreach (string file in File.ReadAllLines("files.list"))
                {
                    remote.Add(file);
                }
                File.Delete("files.list");
            }

            List<string> duplicates = new List<string>();
            foreach (string i in local)
            {
                if (remote.Contains(i))
                {
                    duplicates.Add(i);
                }
            }
            bool overwrite = false;
            foreach (var i in duplicates)
            {
                bool go = false, action = false;
                if (!overwrite)
                {
                    if (verbose) log(convert_path(i) + " already exists");
                    var dialog = new YNAD("Do you want to backup and overwrite " + convert_path(i) + "? (y/n/a)");
                    dialog.ShowDialog();
                    while (true)
                    {
                        switch (dialog.result)
                        {
                            case 1:
                                go = true;
                                action = true;
                                break;
                            case 3:
                                go = true;
                                action = true;
                                overwrite = true;
                                break;
                            case 2:
                                action = false;
                                go = true;
                                break;
                        }
                        log("\n");
                        if (go) break;
                    }
                }
                else
                {
                    action = true;
                }
                if (!action)
                {
                    if (verbose) log("Skipping file " + i);
                    File.Delete("files\\" + i);
                    if (!skip.Contains(i))
                    {
                        skip.Add(i);
                    }
                }
                session.GetFiles(convert_path(i), @"backup\" + i.Replace("/", "\\"));
            }
            log("Installing");
            foreach (string dir in Directory.GetDirectories("files"))
            {
                if (verbose) log("Installing directory " + dir);
                session.PutFiles(dir, "/"); //put directories
            }
            foreach (string file in Directory.GetFiles("files"))
            {
                if (verbose) log("Installing file " + file);
                session.PutFiles(file, "/"); //put files
            }
            File.WriteAllLines("skip.list", skip);
            if (Directory.Exists("files\\Applications") && jtool)
            {
                if (verbose) log("Entitlements needed");
                session.PutFiles("entitlements.ent", "/");
                if (verbose) log("Sending entitlements");
                log("Signing applications");
                foreach (var app in Directory.GetDirectories("files\\Applications\\"))
                {
                    uicache = true;
                    if (verbose) log("Signing " + convert_path(app.Replace("files\\", "\\")));
                    crawler = new Crawler(app, true);
                    crawler.Files.ForEach(i =>
                    {
                        bool sign = false;
                        if (new FileInfo(i).Name.Split('.').Length < 2) sign = true;
                        if (!sign)
                        {
                            if (i.Split('.').Last() == "dylib") sign = true;
                        }
                        i = convert_path(i);
                        if (File.Exists(app + "\\skip-signing"))
                        {
                            sign = false;
                            if (verbose) log("Skipped Signing " + i);
                        }
                        if (sign && !dont_sign)
                        {
                            session.ExecuteCommand("jtool -e arch -arch arm64 " + convert_path(app.Replace("files\\", "\\")) + i);
                            session.ExecuteCommand("mv " + convert_path(app.Replace("files\\", "\\")) + i + ".arch_arm64 " + convert_path(app.Replace("files\\", "\\")) + i);
                            session.ExecuteCommand("jtool --sign --ent /entitlements.ent --inplace " + convert_path(app.Replace("files\\", "\\")) + i);
                            if (verbose) log("Signed " + convert_path(app.Replace("files\\", "\\")) + i);
                        }
                    });
                    crawler = new Crawler("files");
                    crawler.Files.ForEach(i =>
                    {
                        session.ExecuteCommand("chmod 777 " + convert_path(i.Replace("\\files", "")));
                    });
                }
            }
            if (File.Exists("postinst"))
            {
                if (verbose) log("Running postinst script");
                session.PutFiles("postinst", "script");
                session.ExecuteCommand("./script && rm script");
            }
            clean();
            finish(session);
            log("Done");
        }

        public void uninstallFiles(Session session)
        {
            log("Preparing to uninstall");
            bool overwrite = false;
            List<string> remove = new List<string>();
            crawler.Files.ForEach(i =>
            {
                if (!skip.Contains(i))
                {
                    bool go = false, action = false;
                    if (File.Exists("backup" + i) && !overwrite)
                    {
                        if (verbose) log("You have a backup of this file");
                        var dialog = new YNAD("Do you want to restore " + convert_path(i) + " from your backup? (y/n/a)");
                        dialog.ShowDialog();
                        while (true)
                        {
                            switch (dialog.result)
                            {
                                case 1:
                                    go = true;
                                    action = true;
                                    break;
                                case 3:
                                    go = true;
                                    action = true;
                                    overwrite = true;
                                    break;
                                case 2:
                                    action = false;
                                    go = true;
                                    break;
                            }
                            log("\n");
                            if (go) break;
                        }
                    }
                    if (action || overwrite)
                    {
                        string path = i.Replace(i.Substring(i.LastIndexOf('\\')), "");
                        session.PutFiles(new FileInfo("backup" + convert_path(i)).ToString().Replace("/", "\\"), convert_path(path) + "/" + new FileInfo(i).Name);
                        if (verbose) log("Reinstalled " + i);
                    }
                    else
                    {
                        remove.Add(convert_path(i, true));
                    }
                }
            });
            log("Uninstalling");
            string script = "";
            foreach (string i in remove)
            {
                script += "rm " + i + "\n";
            }
            File.WriteAllText("script.sh", script);
            session.PutFiles("script.sh", "script.sh");
            session.ExecuteCommand("sh script.sh");
            if (Directory.Exists("files\\Applications"))
            {
                if (verbose) log("uicache refresh required");
                uicache = true;
            }
            if (!dont_del_empty_dirs)
            {
                log("Locating and removing *some* empty folders");
                session.ExecuteCommand("find /System/Library/Themes/ -type d -empty -delete");
                session.ExecuteCommand("find /usr/ -type d -empty -delete");
                session.ExecuteCommand("find /Applications/ -type d -empty -delete");
                session.ExecuteCommand("find /Library/ -type d -empty -delete");
                session.ExecuteCommand("find /bootstrap/Library/Themes/* -type d -empty -delete");
                session.ExecuteCommand("find /bootstrap/Library/PreferenceLoader/* -type d -empty -delete");
                session.ExecuteCommand("find /bootstrap/Library/PreferenceBundles/* -type d -empty -delete");
                session.ExecuteCommand("find /bootstrap/Library/SBInject/* -type d -empty -delete");
            }
            if (File.Exists("postrm"))
            {
                if (verbose) log("Running postrm script");
                session.PutFiles("postrm", "script");
                session.ExecuteCommand("./script && rm script");
            }
            clean();
            finish(session);
            log("Done");
        }
    }
}
