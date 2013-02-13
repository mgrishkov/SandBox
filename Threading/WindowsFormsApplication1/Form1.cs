using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        public delegate void SetDataHandle(object value);
        public SetDataHandle OnSetValue;

        public Form1()
        {
            InitializeComponent();
            OnSetValue = new SetDataHandle(SetData);
        }

        private void SetData(object value)
        {
            dateEdit1.EditValue = DateTime.Now;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(get_date);
            th.Start();
        }

        private void get_date()
        {
            raise_set_date();
        }

        private void raise_set_date()
        {
            Invoke(OnSetValue, DateTime.Now);
        }
    }
}
