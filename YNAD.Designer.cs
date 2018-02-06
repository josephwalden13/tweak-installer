namespace Tweak_Installer
{
    partial class YNAD
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
            this.display = new System.Windows.Forms.TextBox();
            this.y = new System.Windows.Forms.Button();
            this.n = new System.Windows.Forms.Button();
            this.a = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // display
            // 
            this.display.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.display.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.display.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.display.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.display.ForeColor = System.Drawing.Color.White;
            this.display.Location = new System.Drawing.Point(12, 12);
            this.display.Multiline = true;
            this.display.Name = "display";
            this.display.ReadOnly = true;
            this.display.Size = new System.Drawing.Size(984, 80);
            this.display.TabIndex = 1;
            // 
            // y
            // 
            this.y.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(27)))), ((int)(((byte)(27)))));
            this.y.FlatAppearance.BorderSize = 0;
            this.y.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.y.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold);
            this.y.ForeColor = System.Drawing.Color.Gainsboro;
            this.y.Location = new System.Drawing.Point(12, 98);
            this.y.Name = "y";
            this.y.Size = new System.Drawing.Size(172, 51);
            this.y.TabIndex = 6;
            this.y.Text = "Yes";
            this.y.UseVisualStyleBackColor = false;
            this.y.Click += new System.EventHandler(this.y_Click);
            // 
            // n
            // 
            this.n.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(27)))), ((int)(((byte)(27)))));
            this.n.FlatAppearance.BorderSize = 0;
            this.n.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.n.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold);
            this.n.ForeColor = System.Drawing.Color.Gainsboro;
            this.n.Location = new System.Drawing.Point(190, 98);
            this.n.Name = "n";
            this.n.Size = new System.Drawing.Size(172, 51);
            this.n.TabIndex = 7;
            this.n.Text = "No";
            this.n.UseVisualStyleBackColor = false;
            this.n.Click += new System.EventHandler(this.n_Click);
            // 
            // a
            // 
            this.a.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(27)))), ((int)(((byte)(27)))));
            this.a.FlatAppearance.BorderSize = 0;
            this.a.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.a.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold);
            this.a.ForeColor = System.Drawing.Color.Gainsboro;
            this.a.Location = new System.Drawing.Point(738, 98);
            this.a.Name = "a";
            this.a.Size = new System.Drawing.Size(258, 51);
            this.a.TabIndex = 8;
            this.a.Text = "Yes For All";
            this.a.UseVisualStyleBackColor = false;
            this.a.Click += new System.EventHandler(this.a_Click);
            // 
            // YNAD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.ClientSize = new System.Drawing.Size(1008, 161);
            this.Controls.Add(this.a);
            this.Controls.Add(this.n);
            this.Controls.Add(this.y);
            this.Controls.Add(this.display);
            this.Name = "YNAD";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.YNAD_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox display;
        private System.Windows.Forms.Button y;
        private System.Windows.Forms.Button n;
        private System.Windows.Forms.Button a;
    }
}