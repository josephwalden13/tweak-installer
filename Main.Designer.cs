namespace Tweak_Installer
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.installbtn = new System.Windows.Forms.Button();
            this.uninstallbtn = new System.Windows.Forms.Button();
            this.host = new System.Windows.Forms.TextBox();
            this.pass = new System.Windows.Forms.TextBox();
            this.iplabel = new System.Windows.Forms.Label();
            this.passwordlabel = new System.Windows.Forms.Label();
            this.select = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.respringbtn = new System.Windows.Forms.Button();
            this.uicachebtn = new System.Windows.Forms.Button();
            this.auto = new System.Windows.Forms.CheckBox();
            this.error = new System.Windows.Forms.LinkLabel();
            this.version = new System.Windows.Forms.Label();
            this.twitter = new System.Windows.Forms.LinkLabel();
            this.reddit = new System.Windows.Forms.LinkLabel();
            this.creator = new System.Windows.Forms.LinkLabel();
            this.ui = new System.Windows.Forms.LinkLabel();
            this.github = new System.Windows.Forms.LinkLabel();
            this.autolabel = new System.Windows.Forms.Label();
            this.paypal = new System.Windows.Forms.LinkLabel();
            this.portlbl = new System.Windows.Forms.Label();
            this.port = new System.Windows.Forms.TextBox();
            this.terminal = new System.Windows.Forms.Button();
            this.output = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectDebsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractEntitlementsFromLocalFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useCustomEntitlementsFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useDefaultEntitlementsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dontSignApplicationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verboseModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // installbtn
            // 
            this.installbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(27)))), ((int)(((byte)(27)))));
            this.installbtn.FlatAppearance.BorderSize = 0;
            this.installbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.installbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 35F, System.Drawing.FontStyle.Bold);
            this.installbtn.ForeColor = System.Drawing.Color.Gainsboro;
            this.installbtn.Location = new System.Drawing.Point(9, 283);
            this.installbtn.Name = "installbtn";
            this.installbtn.Size = new System.Drawing.Size(220, 75);
            this.installbtn.TabIndex = 5;
            this.installbtn.Text = "Install";
            this.installbtn.UseVisualStyleBackColor = false;
            this.installbtn.Click += new System.EventHandler(this.install_Click);
            // 
            // uninstallbtn
            // 
            this.uninstallbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(27)))), ((int)(((byte)(27)))));
            this.uninstallbtn.FlatAppearance.BorderSize = 0;
            this.uninstallbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.uninstallbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold);
            this.uninstallbtn.ForeColor = System.Drawing.Color.Gainsboro;
            this.uninstallbtn.Location = new System.Drawing.Point(235, 283);
            this.uninstallbtn.Name = "uninstallbtn";
            this.uninstallbtn.Size = new System.Drawing.Size(220, 75);
            this.uninstallbtn.TabIndex = 6;
            this.uninstallbtn.Text = "Uninstall";
            this.uninstallbtn.UseVisualStyleBackColor = false;
            this.uninstallbtn.Click += new System.EventHandler(this.Uninstall_Click);
            // 
            // host
            // 
            this.host.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.host.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.host.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.host.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.host.ForeColor = System.Drawing.Color.White;
            this.host.Location = new System.Drawing.Point(238, 35);
            this.host.Name = "host";
            this.host.Size = new System.Drawing.Size(200, 27);
            this.host.TabIndex = 0;
            this.host.TextChanged += new System.EventHandler(this.host_TextChanged);
            // 
            // pass
            // 
            this.pass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.pass.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pass.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pass.ForeColor = System.Drawing.Color.White;
            this.pass.Location = new System.Drawing.Point(238, 72);
            this.pass.Name = "pass";
            this.pass.PasswordChar = '*';
            this.pass.Size = new System.Drawing.Size(200, 27);
            this.pass.TabIndex = 1;
            this.pass.TextChanged += new System.EventHandler(this.pass_TextChanged);
            // 
            // iplabel
            // 
            this.iplabel.AutoSize = true;
            this.iplabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Bold);
            this.iplabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.iplabel.Location = new System.Drawing.Point(106, 33);
            this.iplabel.Name = "iplabel";
            this.iplabel.Size = new System.Drawing.Size(126, 29);
            this.iplabel.TabIndex = 17;
            this.iplabel.Text = "device ip:";
            // 
            // passwordlabel
            // 
            this.passwordlabel.AutoSize = true;
            this.passwordlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Bold);
            this.passwordlabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.passwordlabel.Location = new System.Drawing.Point(46, 72);
            this.passwordlabel.Name = "passwordlabel";
            this.passwordlabel.Size = new System.Drawing.Size(186, 29);
            this.passwordlabel.TabIndex = 18;
            this.passwordlabel.Text = "root password:";
            // 
            // select
            // 
            this.select.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(27)))), ((int)(((byte)(27)))));
            this.select.FlatAppearance.BorderSize = 0;
            this.select.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.select.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.select.ForeColor = System.Drawing.Color.DimGray;
            this.select.Location = new System.Drawing.Point(9, 226);
            this.select.Name = "select";
            this.select.Size = new System.Drawing.Size(446, 51);
            this.select.TabIndex = 4;
            this.select.Text = "Select Debs, Zips and IPAs";
            this.select.UseVisualStyleBackColor = false;
            this.select.Click += new System.EventHandler(this.select_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "example.deb";
            this.openFileDialog.Multiselect = true;
            // 
            // respringbtn
            // 
            this.respringbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(27)))), ((int)(((byte)(27)))));
            this.respringbtn.FlatAppearance.BorderSize = 0;
            this.respringbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.respringbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 23F, System.Drawing.FontStyle.Bold);
            this.respringbtn.ForeColor = System.Drawing.Color.White;
            this.respringbtn.Location = new System.Drawing.Point(9, 364);
            this.respringbtn.Name = "respringbtn";
            this.respringbtn.Size = new System.Drawing.Size(220, 49);
            this.respringbtn.TabIndex = 7;
            this.respringbtn.Text = "Respring";
            this.respringbtn.UseVisualStyleBackColor = false;
            this.respringbtn.Click += new System.EventHandler(this.respring_Click);
            // 
            // uicachebtn
            // 
            this.uicachebtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(27)))), ((int)(((byte)(27)))));
            this.uicachebtn.FlatAppearance.BorderSize = 0;
            this.uicachebtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.uicachebtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 23F, System.Drawing.FontStyle.Bold);
            this.uicachebtn.ForeColor = System.Drawing.Color.White;
            this.uicachebtn.Location = new System.Drawing.Point(235, 364);
            this.uicachebtn.Name = "uicachebtn";
            this.uicachebtn.Size = new System.Drawing.Size(220, 49);
            this.uicachebtn.TabIndex = 8;
            this.uicachebtn.Text = "Uicache";
            this.uicachebtn.UseVisualStyleBackColor = false;
            this.uicachebtn.Click += new System.EventHandler(this.uicache_Click);
            // 
            // auto
            // 
            this.auto.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.auto.AutoSize = true;
            this.auto.Checked = true;
            this.auto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.auto.FlatAppearance.BorderSize = 0;
            this.auto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.auto.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Bold);
            this.auto.ForeColor = System.Drawing.Color.White;
            this.auto.Location = new System.Drawing.Point(56, 423);
            this.auto.Name = "auto";
            this.auto.Size = new System.Drawing.Size(12, 11);
            this.auto.TabIndex = 9;
            this.auto.UseVisualStyleBackColor = true;
            // 
            // error
            // 
            this.error.AutoSize = true;
            this.error.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.error.LinkColor = System.Drawing.Color.Red;
            this.error.Location = new System.Drawing.Point(4, 451);
            this.error.Name = "error";
            this.error.Size = new System.Drawing.Size(467, 29);
            this.error.TabIndex = 10;
            this.error.TabStop = true;
            this.error.Text = "report errors / bugs / general problems";
            this.error.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.error_LinkClicked);
            // 
            // version
            // 
            this.version.AutoSize = true;
            this.version.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold);
            this.version.ForeColor = System.Drawing.Color.DimGray;
            this.version.Location = new System.Drawing.Point(12, 33);
            this.version.Name = "version";
            this.version.Size = new System.Drawing.Size(36, 13);
            this.version.TabIndex = 16;
            this.version.Text = "2.2.1";
            this.version.Click += new System.EventHandler(this.version_Click);
            // 
            // twitter
            // 
            this.twitter.AutoSize = true;
            this.twitter.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.twitter.LinkColor = System.Drawing.Color.Red;
            this.twitter.Location = new System.Drawing.Point(9, 491);
            this.twitter.Name = "twitter";
            this.twitter.Size = new System.Drawing.Size(61, 13);
            this.twitter.TabIndex = 11;
            this.twitter.TabStop = true;
            this.twitter.Text = "my twitter";
            this.twitter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.twitter_LinkClicked);
            // 
            // reddit
            // 
            this.reddit.AutoSize = true;
            this.reddit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reddit.LinkColor = System.Drawing.Color.Red;
            this.reddit.Location = new System.Drawing.Point(76, 491);
            this.reddit.Name = "reddit";
            this.reddit.Size = new System.Drawing.Size(39, 13);
            this.reddit.TabIndex = 12;
            this.reddit.TabStop = true;
            this.reddit.Text = "reddit";
            this.reddit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.reddit_LinkClicked);
            // 
            // creator
            // 
            this.creator.AutoSize = true;
            this.creator.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.creator.LinkColor = System.Drawing.Color.Red;
            this.creator.Location = new System.Drawing.Point(281, 491);
            this.creator.Name = "creator";
            this.creator.Size = new System.Drawing.Size(101, 13);
            this.creator.TabIndex = 15;
            this.creator.TabStop = true;
            this.creator.Text = "by josephwalden";
            this.creator.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.creator_LinkClicked);
            // 
            // ui
            // 
            this.ui.AutoSize = true;
            this.ui.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ui.LinkColor = System.Drawing.Color.Red;
            this.ui.Location = new System.Drawing.Point(388, 491);
            this.ui.Name = "ui";
            this.ui.Size = new System.Drawing.Size(81, 13);
            this.ui.TabIndex = 16;
            this.ui.TabStop = true;
            this.ui.Text = "(ui by brnnkr)";
            this.ui.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ui_LinkClicked);
            // 
            // github
            // 
            this.github.AutoSize = true;
            this.github.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.github.LinkColor = System.Drawing.Color.Red;
            this.github.Location = new System.Drawing.Point(121, 491);
            this.github.Name = "github";
            this.github.Size = new System.Drawing.Size(42, 13);
            this.github.TabIndex = 13;
            this.github.TabStop = true;
            this.github.Text = "github";
            this.github.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.github_LinkClicked);
            // 
            // autolabel
            // 
            this.autolabel.AutoSize = true;
            this.autolabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autolabel.ForeColor = System.Drawing.Color.DimGray;
            this.autolabel.Location = new System.Drawing.Point(74, 414);
            this.autolabel.Name = "autolabel";
            this.autolabel.Size = new System.Drawing.Size(312, 25);
            this.autolabel.TabIndex = 19;
            this.autolabel.Text = "automatic respring and uicache";
            this.autolabel.Click += new System.EventHandler(this.autolabel_Click);
            // 
            // paypal
            // 
            this.paypal.AutoSize = true;
            this.paypal.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paypal.LinkColor = System.Drawing.Color.Red;
            this.paypal.Location = new System.Drawing.Point(169, 491);
            this.paypal.Name = "paypal";
            this.paypal.Size = new System.Drawing.Size(44, 13);
            this.paypal.TabIndex = 14;
            this.paypal.TabStop = true;
            this.paypal.Text = "paypal";
            this.paypal.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.paypal_LinkClicked);
            // 
            // portlbl
            // 
            this.portlbl.AutoSize = true;
            this.portlbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Bold);
            this.portlbl.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.portlbl.Location = new System.Drawing.Point(163, 102);
            this.portlbl.Name = "portlbl";
            this.portlbl.Size = new System.Drawing.Size(66, 29);
            this.portlbl.TabIndex = 21;
            this.portlbl.Text = "port:";
            // 
            // port
            // 
            this.port.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.port.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.port.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.port.ForeColor = System.Drawing.Color.White;
            this.port.Location = new System.Drawing.Point(238, 105);
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(200, 27);
            this.port.TabIndex = 20;
            this.port.TextChanged += new System.EventHandler(this.port_TextChanged);
            // 
            // terminal
            // 
            this.terminal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(27)))), ((int)(((byte)(27)))));
            this.terminal.FlatAppearance.BorderSize = 0;
            this.terminal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.terminal.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.terminal.ForeColor = System.Drawing.Color.DimGray;
            this.terminal.Location = new System.Drawing.Point(9, 169);
            this.terminal.Name = "terminal";
            this.terminal.Size = new System.Drawing.Size(446, 51);
            this.terminal.TabIndex = 22;
            this.terminal.Text = "Terminal";
            this.terminal.UseVisualStyleBackColor = false;
            this.terminal.Click += new System.EventHandler(this.terminal_Click);
            // 
            // output
            // 
            this.output.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.output.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.output.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.output.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.output.ForeColor = System.Drawing.Color.White;
            this.output.Location = new System.Drawing.Point(476, 35);
            this.output.Multiline = true;
            this.output.Name = "output";
            this.output.ReadOnly = true;
            this.output.Size = new System.Drawing.Size(306, 469);
            this.output.TabIndex = 23;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.advancedToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(794, 24);
            this.menuStrip1.TabIndex = 24;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectDebsToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // selectDebsToolStripMenuItem
            // 
            this.selectDebsToolStripMenuItem.Name = "selectDebsToolStripMenuItem";
            this.selectDebsToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.selectDebsToolStripMenuItem.Text = "Select Files";
            this.selectDebsToolStripMenuItem.Click += new System.EventHandler(this.selectDebsToolStripMenuItem_Click);
            // 
            // advancedToolStripMenuItem
            // 
            this.advancedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractEntitlementsFromLocalFileToolStripMenuItem,
            this.useCustomEntitlementsFileToolStripMenuItem,
            this.useDefaultEntitlementsToolStripMenuItem,
            this.dontSignApplicationsToolStripMenuItem,
            this.verboseModeToolStripMenuItem});
            this.advancedToolStripMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.advancedToolStripMenuItem.Name = "advancedToolStripMenuItem";
            this.advancedToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.advancedToolStripMenuItem.Text = "Advanced";
            // 
            // extractEntitlementsFromLocalFileToolStripMenuItem
            // 
            this.extractEntitlementsFromLocalFileToolStripMenuItem.Name = "extractEntitlementsFromLocalFileToolStripMenuItem";
            this.extractEntitlementsFromLocalFileToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.extractEntitlementsFromLocalFileToolStripMenuItem.Text = "Extract entitlements from local file";
            this.extractEntitlementsFromLocalFileToolStripMenuItem.Click += new System.EventHandler(this.extractEntitlementsFromLocalFileToolStripMenuItem_Click);
            // 
            // useCustomEntitlementsFileToolStripMenuItem
            // 
            this.useCustomEntitlementsFileToolStripMenuItem.Name = "useCustomEntitlementsFileToolStripMenuItem";
            this.useCustomEntitlementsFileToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.useCustomEntitlementsFileToolStripMenuItem.Text = "Use custom entitlements file";
            this.useCustomEntitlementsFileToolStripMenuItem.Click += new System.EventHandler(this.useCustomEntitlementsFileToolStripMenuItem_Click);
            // 
            // useDefaultEntitlementsToolStripMenuItem
            // 
            this.useDefaultEntitlementsToolStripMenuItem.Name = "useDefaultEntitlementsToolStripMenuItem";
            this.useDefaultEntitlementsToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.useDefaultEntitlementsToolStripMenuItem.Text = "Use default entitlements";
            this.useDefaultEntitlementsToolStripMenuItem.Click += new System.EventHandler(this.useDefaultEntitlementsToolStripMenuItem_Click);
            // 
            // dontSignApplicationsToolStripMenuItem
            // 
            this.dontSignApplicationsToolStripMenuItem.Name = "dontSignApplicationsToolStripMenuItem";
            this.dontSignApplicationsToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.dontSignApplicationsToolStripMenuItem.Text = "Don\'t sign applications";
            this.dontSignApplicationsToolStripMenuItem.Click += new System.EventHandler(this.dontSignApplicationsToolStripMenuItem_Click);
            // 
            // verboseModeToolStripMenuItem
            // 
            this.verboseModeToolStripMenuItem.Name = "verboseModeToolStripMenuItem";
            this.verboseModeToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.verboseModeToolStripMenuItem.Text = "Verbose mode";
            this.verboseModeToolStripMenuItem.Click += new System.EventHandler(this.verboseModeToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(794, 516);
            this.Controls.Add(this.output);
            this.Controls.Add(this.terminal);
            this.Controls.Add(this.portlbl);
            this.Controls.Add(this.port);
            this.Controls.Add(this.paypal);
            this.Controls.Add(this.autolabel);
            this.Controls.Add(this.github);
            this.Controls.Add(this.ui);
            this.Controls.Add(this.creator);
            this.Controls.Add(this.reddit);
            this.Controls.Add(this.twitter);
            this.Controls.Add(this.version);
            this.Controls.Add(this.error);
            this.Controls.Add(this.auto);
            this.Controls.Add(this.uicachebtn);
            this.Controls.Add(this.respringbtn);
            this.Controls.Add(this.select);
            this.Controls.Add(this.passwordlabel);
            this.Controls.Add(this.iplabel);
            this.Controls.Add(this.pass);
            this.Controls.Add(this.host);
            this.Controls.Add(this.uninstallbtn);
            this.Controls.Add(this.installbtn);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.ShowIcon = false;
            this.Text = "Tweak Installer";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button installbtn;
        private System.Windows.Forms.Button uninstallbtn;
        private System.Windows.Forms.TextBox host;
        private System.Windows.Forms.TextBox pass;
        private System.Windows.Forms.Label iplabel;
        private System.Windows.Forms.Label passwordlabel;
        private System.Windows.Forms.Button select;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button respringbtn;
        private System.Windows.Forms.Button uicachebtn;
        private System.Windows.Forms.CheckBox auto;
        private System.Windows.Forms.LinkLabel error;
        private System.Windows.Forms.Label version;
        private System.Windows.Forms.LinkLabel twitter;
        private System.Windows.Forms.LinkLabel reddit;
        private System.Windows.Forms.LinkLabel creator;
        private System.Windows.Forms.LinkLabel ui;
        private System.Windows.Forms.LinkLabel github;
        private System.Windows.Forms.Label autolabel;
        private System.Windows.Forms.LinkLabel paypal;
        private System.Windows.Forms.Label portlbl;
        private System.Windows.Forms.TextBox port;
        private System.Windows.Forms.Button terminal;
        private System.Windows.Forms.TextBox output;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem advancedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectDebsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractEntitlementsFromLocalFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verboseModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dontSignApplicationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useCustomEntitlementsFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useDefaultEntitlementsToolStripMenuItem;
    }
}

