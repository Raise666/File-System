using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 文件结构
        /// </summary>
        public class content
        {
            public char[] name;      //文件或目录名
            public char[] type;       //文件类型名
            public int size=0;
            public string date;
            public string store="0";
            public string id;
            public int address;       //文件或目录的起始盘块号
            public int length; 
        //文件长度，以盘块为单位                   //////共20个字节长度
        } ;
        public class pointer
        {
            public int dnum;          //磁盘盘块号
            public int bnum;           //磁盘盘块内第几个字节
        } ;            //已打开文件表中读写指针的结构

        public class OFILE
        {
            public char[] name;     //文件绝对路径名
            public int address;         //文件起始盘块号
            public int length;         //文件长度，文件占用的字节数
            public pointer read;       //读文件的位置，文件刚打开时dnum为文件起始盘
            public pointer write;      //写文件的位置，文件刚建立时dnum为文件起始盘
        } ;
        public OFILE xy = new OFILE();

        /// <summary>
        /// 已打开文件表结构
        /// </summary>
        public class openfile
        {
            public OFILE[] file;    //已打开文件表
            public int length;       //已打开文件表中登记的文件数量
        } ;
        public byte[] buffer1 = new byte[120];     //模拟缓冲区


        public void addtoList(ListView L, int arr)
        {
            string s1 = new string(fileInfo[arr].name);
            string s2 = new string(fileInfo[arr].type);
            ListViewItem list = new ListViewItem(s1);
            list.SubItems.Add(fileInfo[arr].date);
            list.SubItems.Add(s2);
            list.SubItems.Add(fileInfo[arr].size.ToString()+"KB");
            L.Items.Add(list);
        }

        public void showInList(TreeNode selNode)
        {
            for (int j = 0; j < fileCount; j++)
            {
                if (fileInfo[j].id == selNode.Name && fileInfo[j].store == "1")
                {
                    addtoList(listView1, j);
                }
            }
        }

         /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
     static  public  openfile openfile1 = new openfile();

        static public openfile Openfile1
        {
            get { return openfile1; }
            set { openfile1 = value; }
        }
      
     public  void Form1_Load(object sender, EventArgs e)
        {
           FileStream fs = new FileStream("D:\\test.txt", FileMode.Create, FileAccess.ReadWrite);
           BinaryWriter m_streamwriter = new BinaryWriter (fs);//向空文件流写数据
           int i;//byte字节型，int是整型，byte是8bit，int是32bit,无符号的 8 位整数,0~255
           byte b = 1;
           byte c = 0;
           m_streamwriter.BaseStream.Seek(0, SeekOrigin.Begin);
               for (i = 1; i <= 4; i++)
               {
                  m_streamwriter.Write(b);
                  m_streamwriter.Flush();
               }
               for (i = 5; i <=120 ; i++)
               {
                   m_streamwriter.Write(c);
                   m_streamwriter.Flush();
               }
               m_streamwriter.Write(b);
               for (i = 122; i <= 240; i++)
               {
                   m_streamwriter.Write(c);
                   m_streamwriter.Flush();
               }

          openfile1.file = new OFILE[5];
          openfile1.length = 0;
          m_streamwriter.BaseStream.Seek (240,SeekOrigin .Begin);         
          content newcontent = new content();
          newcontent.name = new char[3];
          newcontent.type = new char[3];
          newcontent.name[0] = 'C';
          newcontent.name[1] = ' ';
          newcontent.name[2] = ' ';
          newcontent.type[0] = ' ';
          newcontent.type[1] = ' ';
          newcontent.type[2] = ' ';
          m_streamwriter.Write(newcontent.name);
          m_streamwriter.Write(newcontent.type);
          Int32 l = 3;
          m_streamwriter.Write((Int32)l);
          m_streamwriter.Write(newcontent.length);//盘块
          m_streamwriter.Flush();
          for (i = 0; i < 6; i++)
          {
              m_streamwriter.BaseStream.Seek(360 + i * 20, SeekOrigin.Begin);
              content newcontent3 = new content();
              newcontent3.name = new char[3];
              newcontent3.type = new char[3];
              newcontent3.name[0] = '$';
              newcontent3.name[1] = ' ';
              newcontent3.name[2] = ' ';
              newcontent3.type[0] = ' ';
              newcontent3.type[1] = ' ';
              newcontent3.type[2] = ' ';

              m_streamwriter.Write(newcontent3.name);
              m_streamwriter.Write(newcontent3.type);
              int p = 0;
              newcontent3.address = p;
              newcontent3.length = p;
              m_streamwriter.Write(newcontent3.address);
              m_streamwriter.Write(newcontent3.length);
              m_streamwriter.Flush();
          }
          // m_streamwriter.BaseStream.Seek(239 * 120, SeekOrigin.Begin);
          m_streamwriter.BaseStream.Seek(119 * 120, SeekOrigin.Begin);
          m_streamwriter.Write((byte)'#');
          m_streamwriter.Close();
          fs.Close();

        }
          /// <summary>
        ///             建立新文件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        static  public int isempty;
        public int create(char[] name)
        {

              add = 0;//int add
              int  j, k, s=0, t, b, dd, dn, bn;
              int d;
             char [] dname=new char [3];
             char [] tname=new char [3];
             char [] pathname=new char [17];
             OFILE x = new OFILE();
             if (openfile1.length == 5)
             {
                 add = 1;
                 MessageBox.Show("已打开表已满，无法建立！@_@");
                 return 0;
             }
             for (j = 0; name[j] != '\0'; j++)
                 if (name[j] == '\\') 
                     s = j;
             for (j = 0; j < s; j++)
                 pathname[j] = name[j];
             pathname[j] = '\0';
             for (k = 0, j = s + 1; name[j] != '\0' && k < 3 && name[j] != '.'; j++, k ++)
                 dname[k] = name[j];
             if (k == 0)
             {
                 add = 1;
                 MessageBox.Show("错误的文件名或目录名！");
                 return 0;
             }
             for (; k < 3; k++)
                 dname[k] = ' ';
             k = 0;
             if (name[j++] == '.')
             {
                 for (; name[j] != '\0' && k < 3 && name[j] != '.'; j++, k++)
                     tname[k] = name[j];
             }
             for (; k < 3; k++)
                 tname[k] = ' ';
             if ((d =search(pathname, out  dn,out  bn)) == 0)
             {
                 add =1;
                 MessageBox.Show("目录不存在，不能建立!");
                 return 0;
             }                   
             b = -1;
             FileStream ss = new FileStream("D:\\test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
             BinaryReader m_streamreader = new BinaryReader (ss);
             m_streamreader.BaseStream.Seek(d * 120, SeekOrigin.Begin);
             char a;
             for (t = 0; t < 6; t++)
             {
                 m_streamreader.BaseStream.Seek(d * 120+t*20, SeekOrigin.Begin);
                 if ((a=m_streamreader.ReadChar()) == dname[0] && m_streamreader.ReadChar() == dname[1] && m_streamreader.ReadChar() == dname[2] && m_streamreader.ReadChar() == tname[0] && m_streamreader.ReadChar() == tname[1] && m_streamreader.ReadChar() == tname[2])
                 {
                     add = 1;
                     MessageBox.Show("文件已存在，不能建立！！@_@");
                     ss.Close();
                     return 0;
                 }
                 if (a == '$' && b == -1)
                     b = t;
             }
             if (b == -1)
             {
                 add = 1;
                 MessageBox.Show("目录无空间");

                 ss.Close();
                 return 0;
             }
             m_streamreader.Close();
             ss.Close();
             int allocatecd = 1;

             if ((dd = allocate(allocatecd )) == 0)
             {
                 add = 1;
                 MessageBox.Show("建立文件失败");
                 ss.Close();
                 return 0;
             }                                                     
             isempty++; 
             FileStream sss = new FileStream("D:\\test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryWriter writer = new BinaryWriter (sss);
            writer .BaseStream .Seek (d*120+b*20,SeekOrigin.Begin );
             writer.Write(dname);
             writer.Flush();
             writer.Write(tname);
             writer.Flush();
             writer.Write(dd);
             writer.Flush();
             int assd = 0;
             writer.Write(assd);
             writer.Flush();
             m_streamreader.Close();
             writer.Close();
             sss.Close();
             x.name = new char[20];
             x.name = name;
             x.address = dd;
             x.length = 0;
             pointer po = new pointer();
             po.dnum = dd;
             po.bnum = 0;
             x.read = po;
             pointer po1 = new pointer();
             po1.dnum = dd;
             po1.bnum = 0;
             x.write = po1;
             iopen(x);
             for (int i = 0; i < x.name.Length; i++)
             {                
                     textBox1.Text += x.name[i];
               
             }
            textBox1.Text += "\r\n";
            return 1;
        }


        /// <summary>
        /// 在已打开表中查找
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns> 
        bool test;
        bool isfirst(int i,char [] newname)
        {
             test = false;
            for (int j = 0; j < 20; j++)
            {
                if (openfile1.file[i].name[j] != newname[j])
                {
                    test = true;
                    break;
                }
            }
           return test;
        }


        public int sopen(char[] name)
        {
            int i;  
            int retur = -1;
            char [] newname=new char [20];
            for (i = 0; i < name .Length ; i++)
            {               
                   newname[i] = name[i];
            }
        
                i = 0;    //依次查找已打开文件表
          
                while  ((i < openfile1.length) && isfirst (i, newname ) )
                {
                     ++i;
                     if (i >= openfile1.length)
                         return retur;
                   
                }
                
                if (i >= openfile1.length)
                    return retur;
            return i;
        }
        public int iopen(OFILE x)
        {
            int i;
            i = sopen(x.name);
            if (i != -1)
            {
                MessageBox.Show("文件已打开！");
                return 0;
            }
            else if (openfile1.length == 5)
            {
                MessageBox.Show("已打开文件表已满");
                return 0;
            }
            else
            {
                openfile1.file[openfile1.length] = new OFILE();
                copen(openfile1 .file [openfile1.length], x);
                openfile1.length++;
                return 1;
            }
          
        }
        public void  copen(OFILE x1, OFILE x2)
        {
           
            x1.name =new char[20];
            x1.name= x2.name;
            x1.address  = x2.address;
            x1.length = x2.length;
            x1.read = new pointer();
            x1.read = x2.read;
            x1.write = new pointer();
            x1.write = x2.write;      
        }
     




        /// <summary>
        /// 分配新磁盘块
        /// </summary>
        /// <param name="allocatecd"></param>
        /// <returns></returns>
        public int allocate(int allocatecd)

        {
            int i;
            
            FileStream sss = new FileStream("D:\\test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryReader m_streamreader = new BinaryReader (sss);            //将模拟磁盘的文件指针移至模拟磁盘FAT表中
            BinaryWriter writer = new BinaryWriter (sss);
            if (allocatecd == 1)
            {
                m_streamreader.BaseStream.Seek(0, SeekOrigin.Begin);
                buffer1= m_streamreader.ReadBytes(120);
                 
                for (i = 3; i < 120; i++)
                    if (buffer1[i] == 0)  //如果FAT中的第i项为0，分配第i块磁盘块，修改FAT表，并且写回到磁盘
                    {
                        buffer1[i] = 254;
                        
                        writer.BaseStream.Seek(0, SeekOrigin.Begin);
                        writer.Write(buffer1);
                        writer.Flush();
                        writer.Close();
                        sss.Close();
                        return i;		//返回磁盘号
                    }
            }
            MessageBox.Show("已经没有磁盘空间!");
            return 0;
        }

        /// <summary>
        /// 查找文件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dnum"></param>
        /// <param name="bnum"></param>
        /// <returns></returns>
        public char[] pna = new char[3];
        public char[] type = new char[3];
        public int search(char[] newname, out int dnum, out int bnum)
        {
            int k, s, J, last = 0;
            Int32 i;

            if (((newname.ToString() == "") == true) || ((newname.ToString() == "\\") == true))
            {
                dnum = 0;
                bnum = 0;
                return 2;
            }
            k = 0;
            if (newname[0] == '\\')
                k = 1;
            i = 2;
            while (last != 1)
            {
                for (s = 0; (newname[k] != '.') && (newname[k] != '\0') && (s < 3) && (newname[k] != '\\'); s++, k++)
                    pna[s] = newname[k];
                for (; s < 3; s++)
                    pna[s] = ' ';
                while ((newname[k] != '.') && (newname[k] != '\0') && (newname[k] != '\\'))
                    k++;
                type[0] = type[1] = type[2] = ' ';
                if (newname[k] == '.')
                {
                    k++;
                    if (newname[k] != '\0')
                        type[0] = newname[k];
                    k++;
                    if (newname[k] != '\0') type[1] = newname[k];
                    k++;
                    if (newname[k] != '\0') type[2] = newname[k];
                    if (newname[k] != '\0' && newname[k + 1] != '\0')
                    {
                        MessageBox.Show("文件名错误！");
                        dnum = 0;
                        bnum = 0;
                        return 0;
                    }
                    last = 1;
                }
                else
                    if (newname[k] != '\0') k++;
                if (newname[k] == '\0')
                    last = 1;
                FileStream fss = new FileStream("D:\\test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                BinaryReader m_streamreader = new BinaryReader(fss);
                m_streamreader.BaseStream.Seek(i * 120, SeekOrigin.Begin);
                J = 0;

                if (last == 1)
                    while (((J < 6) && (m_streamreader.ReadChar() == pna[0]) && (m_streamreader.ReadChar() == pna[1]) && (m_streamreader.ReadChar() == pna[2]) && (m_streamreader.ReadChar() == type[0]) && (m_streamreader.ReadChar() == type[1]) && (m_streamreader.ReadChar() == type[2])) == false)
                    {
                        J++;
                        m_streamreader.BaseStream.Seek(i * 120 + J * 20, SeekOrigin.Begin);
                    }
                else
                    while (((J < 6) && (m_streamreader.ReadChar() == pna[0]) && (m_streamreader.ReadChar() == pna[1]) && (m_streamreader.ReadChar() == pna[2])) == false)
                    {
                        J++;
                        m_streamreader.BaseStream.Seek(i * 120 + J * 20, SeekOrigin.Begin);
                    }
                if (J < 6)
                    if (last == 1)
                    {
                        dnum = i;
                        bnum = J;
                        int jk = m_streamreader.ReadInt32();
                        m_streamreader.Close();
                        fss.Close();
                        return jk;
                    }
                    else
                    {
                        m_streamreader.ReadChar();
                        m_streamreader.ReadChar();
                        m_streamreader.ReadChar();
                        int i1 = m_streamreader.ReadInt32();
                        i = i1;
                    }
                else
                {
                    MessageBox.Show("路径错误！");
                    dnum = 0;
                    bnum = 0;
                    fss.Close();
                    return 0;
                }

                fss.Close();

            }//while 结束
            dnum = 0;
            bnum = 0;

            return 1;
        }

        /// <summary>
        /// 关闭文件函数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int closee = 0;
        public int close_file(char[] name)
        {
            int i, dnum, bnum;
            if ((i = sopen(name)) == -1)
            {
                closee = 1;
                MessageBox.Show("打开的文件中没有该文件，关闭失败");
                return 0;
            }
            FileStream closefile = new FileStream("D:\\test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryReader reader = new BinaryReader(closefile);
            BinaryWriter writer = new BinaryWriter(closefile);
            reader.BaseStream.Seek(openfile1.file[i].write.dnum * 120, SeekOrigin.Begin);
            buffer1 = reader.ReadBytes(120);
            //displaymemory();
            buffer1[openfile1.file[i].write.bnum] = (byte)'#';
            writer.BaseStream.Seek(openfile1.file[i].write.dnum * 120, SeekOrigin.Begin);
            writer.Write(buffer1);
            closefile.Close();
            search(name, out dnum, out bnum);
            FileStream closefile1 = new FileStream("D:\\test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryReader reader1 = new BinaryReader(closefile1);
            BinaryWriter writer1 = new BinaryWriter(closefile1);
            writer1.BaseStream.Seek(dnum * 120 + bnum * 20 + 16, SeekOrigin.Begin);
            writer1.Write(openfile1.file[i].length);
            if ((openfile1.length > 1) && (i != openfile1.length - 1))
                copen(openfile1.file[i], openfile1.file[openfile1.length - 1]);
            openfile1.length--;
            writer1.Close();
            reader1.Close();
            closefile1.Close();
            return 1;
        }

        /// <summary>
        /// 删除文件函数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int deleteeee;
        public int delete(char[] name)
        {
            deleteeee = 0;
            int dnum, bnum;
            byte t;
            if ((t = Convert.ToByte(search(name, out dnum, out bnum))) == (byte)'0')
            {
                deleteeee = 1;
                MessageBox.Show("文件不存在");

                return 0;
            }
            if (sopen(name) != -1)
            {
                deleteeee = 1;
                MessageBox.Show("该文件打开，不能删除");

                return 0;
            }
            isempty--;
            FileStream fss = new FileStream("D:\\test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryReader m_streamreader = new BinaryReader(fss);
            BinaryWriter writer = new BinaryWriter(fss);
            m_streamreader.BaseStream.Seek(dnum * 120, SeekOrigin.Begin);
            buffer1 = m_streamreader.ReadBytes(120);
            //displaymemory();
            buffer1[bnum * 20] = (byte)'$';
            writer.BaseStream.Seek(dnum * 120, SeekOrigin.Begin);
            writer.Write(buffer1);

            while (t != 254)
            {
                dnum = Convert.ToInt32(t);
                m_streamreader.BaseStream.Seek(dnum / 120 * 120, SeekOrigin.Begin);
                // displaymemory();
                buffer1 = m_streamreader.ReadBytes(120);
                t = buffer1[dnum % 120];
                int recovery = dnum / 120;
                buffer1[dnum % 120] = 0;
                //displayrecovery(dnum % 120, recovery);
                writer.BaseStream.Seek(dnum / 120 * 120, SeekOrigin.Begin);
                writer.Write(buffer1);

            }
            writer.Close();
            m_streamreader.Close();
            fss.Close();
            return 1;
        }//


        /// <summary>
        /// 在文件打开表中删除
        /// </summary>
        /// <param name="name"></param>
        void dopen(char[] name)
        {
            int i;
            i = sopen(name);

            if (i == 0 || (i == openfile1.length - 1))
                openfile1.length--;
            else
            {
                copen(openfile1.file[i], openfile1.file[openfile1.length - 1]);
                openfile1.length--;
            }
        }//删除函数结束

        /// <summary>
        /// 新建目录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        int md(char[] name)
        {
            int i, j, k, s = 0, d, t, b, dd, dn, bn;
            char[] dname = new char[3];
            char[] pathname = new char[20];
            i = 2;
            for (j = 0; name[j] != '\0'; j++)
                if (name[j] == '\\') s = j;
            for (j = 0; j < s; j++)
                pathname[j] = name[j];
            pathname[j] = '\0';
            for (k = 0, j = s + 1; name[j] != '\0' && k < 3 && name[j] != '.'; j++, k++)
                dname[k] = name[j];
            if (k == 0)
            {
                MessageBox.Show("错误文件名或目录名");

                return 0;
            }
            for (; k < 3; k++)
                dname[k] = ' ';
            if ((d = search(pathname, out dn, out bn)) == 0)
            {
                MessageBox.Show("目录不存在，不能建立");
                return 0;
            }
            b = -1;
            FileStream fss = new FileStream("D:\\test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryReader m_streamreader = new BinaryReader(fss);
            BinaryWriter writer = new BinaryWriter(fss);
            char temp;
            for (t = 0; t < 6; t++)
            {
                m_streamreader.BaseStream.Seek(d * 120 + t * 20, SeekOrigin.Begin);

                if ((temp = m_streamreader.ReadChar()) == dname[0] && m_streamreader.ReadChar() == dname[1] && m_streamreader.ReadChar() == dname[2])
                {
                    MessageBox.Show("目录已存在，不能建立");

                    return 0;
                }
                if (temp == '$' && b == -1)
                    b = t;
            }
            if (b == -1)
            {
                MessageBox.Show("目录无空间");
                return 0;
            }
            //int alloc = 0;
            //if (name[0] == 'C')
            int alloc = 1;
            fss.Close();

            if ((dd = allocate(alloc)) == 0)
            {
                MessageBox.Show("没有磁盘空间");
                return 0;
            }
            FileStream fs = new FileStream("D:\\test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryReader reader = new BinaryReader(fs);
            BinaryWriter writer1 = new BinaryWriter(fs);
            writer1.BaseStream.Seek(d * 120 + b * 20, SeekOrigin.Begin);
            writer1.Write(dname);
            writer1.Flush();

            for (i = 0; i < 3; i++)
                writer1.Write(' ');

            writer1.Write(dd);
            writer1.Write(0);
            for (t = 0; t < 6; t++)
            {
                writer1.BaseStream.Seek(dd * 120 + t * 20, SeekOrigin.Begin);
                writer1.Write('$');
            }
            fs.Close();
            return 1;
        }//



        /// <summary>
        /// 删除目录函数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int rd(char[] name)
        {
            int dnum, bnum, t, i, flag;
            if ((t = search(name, out dnum, out bnum)) == 0)
            {
                MessageBox.Show("目录不存在");
                return 0;
            }
            if (name.Length <= 3)
            {
                MessageBox.Show("该目录为根目录，不能删除");
                return 0;
            }
            FileStream fs = new FileStream("D:\\test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryReader reader = new BinaryReader(fs);
            BinaryWriter writer = new BinaryWriter(fs);

            flag = 1;

            for (i = 0; i < 6; i++)
            {
                reader.BaseStream.Seek(t * 120 + i * 20, SeekOrigin.Begin);
                if (reader.ReadChar() != '$') flag = 0;
            }
            if (flag == 0)
            {
                MessageBox.Show("该目录为非空目录，不能删除");
                fs.Close();
                return 0;
            }
            writer.BaseStream.Seek(dnum * 120 + bnum * 20, SeekOrigin.Begin);
            writer.Write('$');
            reader.BaseStream.Seek(t / 120 * 120, SeekOrigin.Begin);
            buffer1 = reader.ReadBytes(120);

            buffer1[t % 120] = (byte)'0';

            writer.BaseStream.Seek(t / 120 * 120, SeekOrigin.Begin);
            writer.Write(buffer1);
            fs.Close();
            return 1;
        }//

        /// <summary>
        /// 写文件函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="buff"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public int write_file(char[] name, char[] buff, Int32 length)
        {
            int i, t, dd, allocated = 0;
            if ((i = sopen(name)) == -1)
            {
                MessageBox.Show("文件没有打开或不存在");
                return 0;
            }

            t = 0;
            FileStream fs = new FileStream("D:\\test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryWriter writer = new BinaryWriter(fs);
            BinaryReader reader = new BinaryReader(fs);
            writer.BaseStream.Seek(openfile1.file[i].write.dnum * 120 + openfile1.file[i].write.bnum, SeekOrigin.Begin);
            int isover = 0;
            while (t < length)
            {


                if (openfile1.file[i].write.bnum < 120)
                {
                    if (isover == 0)//文件打开了
                    {
                        writer.Write((byte)buff[t]);
                        openfile1.file[i].write.bnum++;
                        openfile1.file[i].length++;
                    }
                    else//文件没打开
                    {
                        FileStream fsa = new FileStream("D:\\test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        BinaryWriter writer2 = new BinaryWriter(fsa);
                        BinaryReader reader2 = new BinaryReader(fsa);
                        writer2.BaseStream.Seek(openfile1.file[i].write.dnum * 120 + openfile1.file[i].write.bnum, SeekOrigin.Begin);

                        writer2.Write((byte)buff[t]);
                        openfile1.file[i].write.bnum++;
                        openfile1.file[i].length++;
                        fsa.Close();
                    }

                }


                else
                {
                    fs.Close();
                    isover = 1;
                    if (name[0] == 'C')
                        allocated = 1;
                    if ((dd = allocate(allocated)) == 0)
                    {
                        MessageBox.Show("无磁盘空间，部分信息丢失，写失败");
                        writer.Close();
                        reader.Close();
                        return 0;
                    }
                    FileStream fsa = new FileStream("D:\\test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    BinaryWriter writer2 = new BinaryWriter(fsa);
                    BinaryReader reader2 = new BinaryReader(fsa);
                    reader2.BaseStream.Seek(openfile1.file[i].write.dnum / 120 * 120, SeekOrigin.Begin);// /120得到盘块号
                    buffer1 = reader2.ReadBytes(120);
                    //displaymemory();
                    buffer1[openfile1.file[i].write.dnum % 120] = (byte)dd;//%120得到物理块的偏移量
                    writer2.BaseStream.Seek(openfile1.file[i].write.dnum / 120 * 120, SeekOrigin.Begin);
                    writer2.Write(buffer1);
                    openfile1.file[i].write.dnum = dd;
                    openfile1.file[i].write.bnum = 0;
                    writer2.Seek(openfile1.file[i].write.dnum * 120 + openfile1.file[i].write.bnum, SeekOrigin.Begin);
                    writer2.Write((byte)buff[t]);
                    openfile1.file[i].write.bnum++;
                    openfile1.file[i].length++;
                    fsa.Close();
                }
                t++;
            }
            fs.Close();
            writer.Close();

            int dunm, bunm;
            int find = search(name, out  dunm, out bunm);
            FileStream fss = new FileStream("D:\\test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryWriter writer1 = new BinaryWriter(fss);
            BinaryReader reader1 = new BinaryReader(fss);
            reader1.BaseStream.Seek(dunm * 120 + bunm * 20 + 16, SeekOrigin.Begin);
            int temp = reader1.ReadInt32();
            writer1.BaseStream.Seek(dunm * 120 + bunm * 20 + 16, SeekOrigin.Begin);

            writer1.Write(length + temp);
            writer1.Flush();
            writer1.Close();
            fss.Close();
            reader.Close();
            return 1;
        }//写函数结束








        ///
        /// 读文件函数
        ///
        static private string result;

        public static string Result
        {
            get { return Form1.result; }
            set { Form1.result = value; }
        }
        public string read_file(char[] name, int length)
        {
            int i, t;
            result = "";
            if ((i = sopen(name)) == -1)
            {
                MessageBox.Show("文件没有打开或不存在");
                return result;
            }

            t = 0;
            FileStream fs = new FileStream("D:\\test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryWriter writer = new BinaryWriter(fs);
            BinaryReader reader = new BinaryReader(fs);
            reader.BaseStream.Seek(openfile1.file[i].read.dnum * 120, SeekOrigin.Begin);
            for (int j = 0; j < openfile1.file[i].read.bnum; j++)
                reader.ReadChar();

            while (t < length && (reader.ReadByte() != (byte)'#'))
            {
                reader.BaseStream.Seek(-1, SeekOrigin.Current);
                result += reader.ReadChar();
                if ((t + 1) % 60 == 0)
                    result += '\n';
                openfile1.file[i].read.bnum++;
                if (openfile1.file[i].read.bnum >= 60)
                {
                    reader.BaseStream.Seek(openfile1.file[i].read.dnum / 120 * 120, SeekOrigin.Begin);
                    buffer1 = reader.ReadBytes(120);
                    //displaymemory();
                    openfile1.file[i].read.dnum = Convert.ToInt32(buffer1[openfile1.file[i].read.dnum % 120]);
                    openfile1.file[i].read.bnum = 0;
                    reader.BaseStream.Seek(openfile1.file[i].read.dnum * 120, SeekOrigin.Begin);
                }
                t++;
            }
            reader.Close();
            writer.Close();
            fs.Close();
            return result;
        }//读函数结束







        /// <summary>
        /// 打开文件函数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int open_file(char[] name)
        {
            int isdir = 0;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == '.')
                    isdir = 1;
            }
            if (isdir == 1)
            {
                OFILE x = new OFILE();
                int dnum, bnum, d;
                if ((d = search(name, out dnum, out bnum)) == 0)
                {
                    MessageBox.Show("文件不存在，打开操作失败");
                    return 0;
                }
                FileStream fs = new FileStream("D:\\test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                BinaryWriter writer = new BinaryWriter(fs);
                BinaryReader reader = new BinaryReader(fs);
                reader.BaseStream.Seek(dnum * 120, SeekOrigin.Begin);
                buffer1 = reader.ReadBytes(120);
                //displaymemory();
                char[] temp = new char[20];
                for (int tempint = 0; tempint < 20; tempint++)
                {
                    //if (name[tempint] == '\\')
                    //    temp[tempint] = '/';
                    //else 
                    temp[tempint] = name[tempint];
                }
                x.name = temp;
                reader.BaseStream.Seek(dnum * 120 + bnum * 20 + 12, SeekOrigin.Begin);
                x.address = reader.ReadInt32();
                pointer newpointer = new pointer();
                newpointer.dnum = x.address;
                newpointer.bnum = 0;
                x.read = x.write = newpointer;
                int temp1 = iopen(x);
                if (temp1 == 0)
                {
                    return 0;
                }
                fs.Close();
            }
            else
            {

                MessageBox.Show("目录不能打开！@_@");
                return 0;
            }
            return 1;
        }




        /// <summary>
        /// 显示文件内容函数
        /// </summary>
        /// 


        static public string typefil;

        public static string Typefil
        {
            get { return Form1.typefil; }
            set { Form1.typefil = value; }
        }

        public string typefile(char[] name)
        {
            typefil = "";
            int dnum, dn, bn, t;
            //	int i;
            if ((dnum = search(name, out dn, out bn)) == 0)
            {
                MessageBox.Show("文件不存在");
                return typefil;
            }
            if (dnum <= 2)
            {
                MessageBox.Show("路径错误！！");
                return typefil;
            }
            FileStream fs = new FileStream("D:\\test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryReader reader = new BinaryReader(fs);
            if (dnum < 119)
            {
                while (dnum != 254)
                {
                    reader.BaseStream.Seek(dnum * 120, SeekOrigin.Begin);
                    for (t = 0; t < 120 && (reader.ReadByte() != (byte)'#'); t++)
                    {
                        reader.BaseStream.Seek(-1, SeekOrigin.Current);
                        typefil += Convert.ToChar(reader.ReadByte());
                    }
                    reader.BaseStream.Seek(dnum / 120 * 120, SeekOrigin.Begin);
                    buffer1 = reader.ReadBytes(120);
                    //displaymemory();
                    dnum = Convert.ToInt32(buffer1[dnum % 120]);
                }
            }
            else
            {
                while (dnum != 254)
                {
                    reader.BaseStream.Seek((dnum) * 120, SeekOrigin.Begin);
                    for (t = 0; t < 120 && (reader.ReadByte() != (byte)'#'); t++)
                    {
                        reader.BaseStream.Seek(-1, SeekOrigin.Current);
                        typefil += Convert.ToChar(reader.ReadByte());
                    }
                    reader.BaseStream.Seek(dnum / 120 * 120, SeekOrigin.Begin);
                    buffer1 = reader.ReadBytes(120);
                    //displaymemory();
                    dnum = Convert.ToInt32(buffer1[dnum % 120]);
                }
            }
            reader.Close();
            fs.Close();
            return typefil;
        }//

        static private int iswrite;

        public static int Iswrite
        {
            get { return Form1.iswrite; }
            set { Form1.iswrite = value; }
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            textBox4.Text = treeView1.SelectedNode.FullPath;
            for (int i = 0; i < fileCount; i++)
            {
                string s1 = new string(fileInfo[i].name);
                string s2 = new string(fileInfo[i].type);
                if(treeView1.SelectedNode.Name==s1 && s2=="文件夹")
                {
                    if (treeView1.SelectedNode.Nodes.Count != 0)
                    {
                        int sum = 0;
                        for (int k = 0; k < fileCount; k++)
                        {
                            if (fileInfo[k].id == treeView1.SelectedNode.Name)
                            {                                
                                sum = sum + fileInfo[k].size;                                                               
                            }
                        }
                        fileInfo[i].size = sum;
                        listView1.Items.Clear();
                        showInList(treeView1.SelectedNode);
                    }
                }
            }
            listView1.Items.Clear();
            showInList(treeView1.SelectedNode);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        

        private void 位示图ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string storeInfo = "";
            for (int m = fileCount; m < 120; m++)
            {
                fileInfo[m] = new content();
            }
            for (int i = 0; i < 120; i++)
            {
                if (i % 10 == 0 && i != 0)
                {
                    storeInfo = storeInfo + "\n" + "\n";
                }
                storeInfo = storeInfo + "        " + fileInfo[i].store;
            }
            string aaa = "采用位视图管理文件系统的分配与回收";
            string bb = "位视图信息：";
            MessageBox.Show(aaa + "\n\n" + bb + "\n" + storeInfo, "位视图");
        }

        static int fileCount = 0;
        content[] fileInfo = new content[120];
        int add;
        private void 文本文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ismulu = 1;
            char[] kind = new char[3];
            if (textBox4.Text.Length <= 3)
                ismulu = 1;
            else
            {
                for (int tem = textBox4.Text.Length - 3, k = 0; tem < textBox4.Text.Length; tem++, k++)
                    kind[k] = textBox4.Text[tem];
                if (kind[0] == 't' && kind[1] == 'x' && kind[2] == 't')
                    ismulu = 0;



            }
            if (ismulu == 0)
            {
                MessageBox.Show("sorry！文本文件不可新建文件！");
                return;
            }


            char[] fullname = new char[20];
            string full;

            full = textBox4.Text + '\\' + textBox2.Text + ".txt";
            int i;
            for (i = 0; i < full.Length; i++)
            {
                fullname[i] = full[i];
            }
            fullname[i] = '\0';
            create(fullname);
            if (add != 1)
            {
                treeView1.SelectedNode.Expand();
                treeView1.SelectedNode.Nodes.Add(textBox2.Text + ".txt");
                fileInfo[fileCount] = new content();
                fileInfo[fileCount].name = textBox2.Text.ToCharArray();
                fileInfo[fileCount].size = 0;
                fileInfo[fileCount].type = "文本文件".ToCharArray();
                fileInfo[fileCount].date = DateTime.Now.ToString();
                listView1.Items.Clear();
                showInList(treeView1.SelectedNode);
                if (treeView1.SelectedNode.Nodes.Count == 0)
                {
                    treeView1.SelectedNode.Nodes[0].Name = textBox2.Text;
                    fileInfo[fileCount].id = treeView1.SelectedNode.Name;
                    fileInfo[fileCount].store = "1";
                    fileInfo[fileCount].type = "文本文件".ToCharArray();
                    addtoList(listView1, fileCount);
                    fileCount++;
                }
                else
                {
                    treeView1.SelectedNode.Nodes[treeView1.SelectedNode.Nodes.Count - 1].Name = textBox2.Text;
                    fileInfo[fileCount].id = treeView1.SelectedNode.Name;
                    fileInfo[fileCount].store = "1";
                    fileInfo[fileCount].type = "文本文件".ToCharArray();
                    addtoList(listView1, fileCount);
                    fileCount++;
                }
            }
        }

        private void 文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ismulu = 1;
            char[] kind = new char[3];
            if (textBox4.Text.Length <= 3)
                ismulu = 1;
            else
            {
                for (int tem = textBox4.Text.Length - 3, k = 0; tem < textBox4.Text.Length; tem++, k++)
                    kind[k] = textBox4.Text[tem];
                if (kind[0] == 't' && kind[1] == 'x' && kind[2] == 't')
                    ismulu = 0;
            }
            if (ismulu == 0)
            {
                MessageBox.Show("sorry！文本文件不可新建文件！");
                return;
            }

            char[] fullname = new char[20];
            string full;

            full = textBox4.Text + '\\' + textBox2.Text;
            if (full.Length >= 20)
            {
                MessageBox.Show("路径深度超出！请在其它路径建立文件或目录！");
                return;
            }
            else
            {
                int i;
                for (i = 0; i < full.Length; i++)
                {
                    fullname[i] = full[i];
                }
                fullname[i] = '\0';
                int temp = md(fullname);
                if (temp == 1)
                    treeView1.SelectedNode.Expand();
                treeView1.SelectedNode.Nodes.Add(textBox2.Text);
                fileInfo[fileCount] = new content();
                fileInfo[fileCount].name = textBox2.Text.ToCharArray();
                fileInfo[fileCount].size = 0;
                fileInfo[fileCount].type = "文件夹".ToCharArray();
                fileInfo[fileCount].date = DateTime.Now.ToString();
                listView1.Items.Clear();
                showInList(treeView1.SelectedNode);
                if (treeView1.SelectedNode.Nodes.Count == 0)
                {
                    treeView1.SelectedNode.Nodes[0].Name = textBox2.Text;
                    fileInfo[fileCount].id = treeView1.SelectedNode.Name;
                    fileInfo[fileCount].store = "1";
                    fileInfo[fileCount].type = "文件夹".ToCharArray();
                    addtoList(listView1, fileCount);
                    fileCount++;
                }
                else
                {
                    treeView1.SelectedNode.Nodes[treeView1.SelectedNode.Nodes.Count - 1].Name = textBox2.Text;
                    fileInfo[fileCount].id = treeView1.SelectedNode.Name;
                    fileInfo[fileCount].store = "1";
                    fileInfo[fileCount].type = "文件夹".ToCharArray();
                    addtoList(listView1, fileCount);
                    fileCount++;
                }
            }
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i;
            char[] open = new char[20];

            for (i = 0; i < textBox4.Text.Length; i++)
                open[i] = textBox4.Text[i];
            open[i] = '\0';
            int temp = open_file(open);

            if (temp == 1)
            {
                if (openfile1.length < 5)
                {
                    string s = treeView1.SelectedNode.Text;
                    MessageBox.Show(s + "打开成功！");
                }
                textBox1.Text += treeView1.SelectedNode.FullPath + "\r\n";
            }
        }
        

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i;

            char[] deletee = new char[20];
            for (i = 0; i < textBox4.Text.Length; i++)
            {
                deletee[i] = textBox4.Text[i];
            }
            int isfolder = 0;
            for (i = 0; i < textBox4.Text.Length; i++)
            {
                if (deletee[i] == '.')
                    isfolder = 1;
            }
            if (isfolder == 1)
            {
                delete(deletee);
                if (deleteeee == 0)
                {
                    for (int m = 0; m < fileCount; m++)
                    {
                        string s1 = new string(fileInfo[m].name);
                        if (treeView1.SelectedNode.Name == s1&&fileInfo[m].id==treeView1.SelectedNode.Parent.Name)
                        {
                            for (int k = 0; k < fileCount; k++)
                            {
                                if (fileInfo[k].name==treeView1.SelectedNode.Parent.Name.ToCharArray()&&fileInfo[k].type=="文件夹".ToCharArray())
                                {
                                    fileInfo[k].size = fileInfo[k].size - fileInfo[m].size;
                                    listView1.Items.Clear();
                                    showInList(treeView1.SelectedNode);

                                }
                            }
                            fileInfo[m] = new content();
                        }
                    }
                    treeView1.Nodes.Remove(treeView1.SelectedNode);

                }
            }
            else
            {
                int temp = rd(deletee);
                if (temp == 1)
                    for (int m = 0; m < fileCount; m++)
                    {
                        string s1 = new string(fileInfo[m].name);
                        if (treeView1.SelectedNode.Name == s1)
                        {
                            fileInfo[m] = new content();
                        }
                    }
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string a = textBox4.Text;

            if (a.Length > 20)
            {
                MessageBox.Show("路径深度已超出处理范围！请在其它目录下建立文件@_@");
            }
        }

        public int i;

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            char[] closeee = new char[20];
            for (i = 0; i < textBox4.Text.Length; i++)
            {
                closeee[i] = textBox4.Text[i];
            }
            close_file(closeee);
            if (closee == 0)
            {
                int index = textBox1.Text.IndexOf(treeView1.SelectedNode.FullPath);
                textBox1.Text = textBox1.Text.Remove(index, treeView1.SelectedNode.FullPath.Length + 2);

            }
        }


        static private string pathpath;

        public static string Pathpath
        {
            get { return Form1.pathpath; }
            set { Form1.pathpath = value; }
        }
        static int lengthlength;

        static public int Lengthlength
        {
            get { return lengthlength; }
            set { lengthlength = value; }
        }


        static private int kindoffile;

        public static int Kindoffile
        {
            get { return Form1.kindoffile; }
            set { Form1.kindoffile = value; }
        }
        static private string filename;

        public static string Filename
        {
            get { return Form1.filename; }
            set { Form1.filename = value; }
        }


        private void 查看属性ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox4.Text.Length <= 3)
            {
                MessageBox.Show("sorry！只有文本文件可查看属性！");
                return;
            }
            char[] kind = new char[3];
            for (int tem = textBox4.Text.Length - 3, k = 0; tem < textBox4.Text.Length; tem++, k++)
                kind[k] = textBox4.Text[tem];
            if (kind[0] == 't' && kind[1] == 'x' && kind[2] == 't')
                kindoffile = 0;

            else
            {
                MessageBox.Show("sorry！只有文本文件可查看属性！");
                return;
            }
            int ddnum, bbnum;
            pathpath = treeView1.SelectedNode.FullPath;
            filename = treeView1.SelectedNode.Text;
            char[] newchar = new char[20];
            for (i = 0; i < textBox4.Text.Length; i++)
            {
                newchar[i] = textBox4.Text[i];
            }
            search(newchar, out ddnum, out bbnum);
            FileStream fs = new FileStream("D:\\test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);

            BinaryReader reader = new BinaryReader(fs);
            reader.BaseStream.Seek(ddnum * 120 + bbnum * 20 + 16, SeekOrigin.Begin);
            lengthlength = reader.ReadInt32();

            Form3 fm3 = new Form3();
            fm3.Show();
            reader.Close();
            fs.Close();
        }
        static public string du;

        public static string Du
        {
            get { return du; }
            set { du = value; }
        }
        static private int isdu;

        public static int Isdu
        {
            get { return Form1.isdu; }
            set { Form1.isdu = value; }
        }

        static public char[] dayin;

        public static char[] Dayin
        {
            get { return Form1.dayin; }
            set { Form1.dayin = value; }
        }

        private void 读文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isempty == 0)
            {
                MessageBox.Show("没有文件可读！请新建文件！");
                return;
            }
            dayin = new char[20];
            for (int i = 0; i < textBox4.Text.Length; i++)
            {
                dayin[i] = textBox4.Text[i];
            }
            du = typefile(dayin);
            isdu = 1;
           
            Form2 fm2 = new Form2();
            fm2.Show();
        }

        private void 写文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iswrite = 1;
            int isfolder = 0;
            dayin = new char[20];
            string tempname = treeView1.SelectedNode.Name;           
            string parentname = treeView1.SelectedNode.Parent.Name;           
            for (int i = 0; i < textBox4.Text.Length; i++)
            {
                dayin[i] = textBox4.Text[i];
                if (textBox4.Text[i] == '.')
                    isfolder = 1;
            }
            if (isfolder == 0)
            {
                MessageBox.Show("只有文本文件可写！请检查当前路径！");
                return;
            }
            typefile(dayin);
            Form2 fm2 = new Form2();
            fm2.ShowDialog();
            if (Form2.Flag==1)
            {
                char[] add = new char[160];
                add = fm2.Addtext.ToCharArray();
                Int32 len = Convert.ToInt32(add.Length);              
                write_file(dayin, add, len);
                for (int j = 0; j < fileCount; j++)
                {
                    string s1 = new string(fileInfo[j].type);                   
                    if (s1 == "文本文件")
                    {
                        string s2 = new string(fileInfo[j].name);                      
                        if (s2 == tempname && fileInfo[j].id == parentname)
                        {
                            //fileInfo[j].name = s2.ToCharArray();
                            //fileInfo[j].type = "文本文件".ToCharArray();
                            fileInfo[j].size = len;
                            fileInfo[j].date = DateTime.Now.ToString();
                            listView1.Items.Clear();
                            showInList(treeView1.SelectedNode.Parent);
                            
                        }
                    }                   
                }
            }
        }




    }
}
