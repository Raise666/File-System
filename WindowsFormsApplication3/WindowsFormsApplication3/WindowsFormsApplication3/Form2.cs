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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = "0个字符";
            toolStripStatusLabel3.Text = "第1行，第1列";
        }
        public int lengthini;
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            lengthini = richTextBox1.Text.Length;
            toolStripStatusLabel1.Text = richTextBox1.Text.Length.ToString()+"个字符";
            int totalline = richTextBox1.GetLineFromCharIndex(richTextBox1.Text.Length) + 1;//得到总行数
            int index = richTextBox1.GetFirstCharIndexOfCurrentLine();//得到当前行第一个字符的索引
            int line = richTextBox1.GetLineFromCharIndex(index) + 1;//得到当前行的行号
            int col = richTextBox1.SelectionStart - index + 1;//.SelectionStart得到光标所在位置的索引 - 当前行第一个字符的索引 = 光标所在的列数
            toolStripStatusLabel3.Text = "第" + line + "行，第" + col + "列";
        }
        string addtext;
        public string Addtext
        {
            get { return addtext; }
            set { addtext = value; }
        }

        static private int flag;

        public static int Flag
        {
            get { return Form2.flag; }
            set { Form2.flag = value; }
        }
        static private string modifytime;

        public static string Modifytime
        {
            get { return Form2.modifytime; }
            set { Form2.modifytime = value; }
        }
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addtext = richTextBox1.Text;
            richTextBox1.Enabled = true;
            保存ToolStripMenuItem.Enabled = true;
            Form1.Iswrite = 0;
            Form1.Isdu = 0;
            flag = 1;
            modifytime = DateTime.Now.ToString();
            this.Close();
        }
      

        private void Form2_Load(object sender, EventArgs e)
        {
         
            richTextBox1.Text = Form1.Du;
            if (Form1.Iswrite == 1)
                richTextBox1.Text = Form1.Du;
                //richTextBox1.Text = "";
            if (Form1.Isdu == 1)
            {
                richTextBox1.Text = Form1.Du;
                richTextBox1.Enabled = false;
                保存ToolStripMenuItem.Enabled = false;
            }
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Enabled = true;
            保存ToolStripMenuItem.Enabled = true;
            flag = 0;
            Form1.Isdu = 0;
            Form1.Iswrite = 0;
            this.Close();
        }
    }
}
