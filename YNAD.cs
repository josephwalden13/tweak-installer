using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tweak_Installer
{
    public partial class YNAD : Form
    {
        public int result = 0;
        public YNAD(string question)
        {
            InitializeComponent();
            display.Text = question;
            display.SelectionStart = display.Text.Length;
        }

        private void YNAD_Load(object sender, EventArgs e)
        {

        }

        private void y_Click(object sender, EventArgs e)
        {
            result = 1;
            Close();
        }

        private void n_Click(object sender, EventArgs e)
        {
            result = 2;
            Close();
        }

        private void a_Click(object sender, EventArgs e)
        {
            result = 3;
            Close();
        }
    }
}
