using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            label9.Text = Form1.Pathpath;
            label8.Text = Form1.Lengthlength.ToString();
            label10.Text = Form2.Modifytime;
            label11.Text = "文本文件";
            label12.Text = "可读写";
            label7.Text = Form1.Filename;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
