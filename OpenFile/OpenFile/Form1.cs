using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;

namespace OpenFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            ProcessStartInfo l_st_info = new ProcessStartInfo();
            l_st_info.FileName = openFileDialog1.FileName;
            l_st_info.Verb = "open";
            l_st_info.WindowStyle = ProcessWindowStyle.Maximized;
            Process.Start(l_st_info);
        }
    }
}
