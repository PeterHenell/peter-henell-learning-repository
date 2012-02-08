using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FileTaggerMinimums
{
    public partial class Form1 : Form
    {
        public Form1(string[] args)
        {
            InitializeComponent();

            textBox1.Text = Properties.Settings.Default.LastTag;
            

            listBox1.Items.AddRange(args);
            
           
           
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            textBox1.Select();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            foreach (string item in listBox1.Items)
            {
                try
                {
                    Tagga(item, textBox1.Text);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString());
                }

            }
            Properties.Settings.Default.LastTag = textBox1.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void Tagga(string filePathAndName, string tag)
        {
            if (File.Exists(filePathAndName))
            {
                string oldFileName = Path.GetFileName(filePathAndName);
                string oldFilePath = Path.GetDirectoryName(filePathAndName);

                string taggar = "";
                foreach (var item in tag.Split(' '))
                {
                    taggar += string.Format("[{0}]", item);
                }

                string newFileName = string.Format(@"{0}\{1}{2}", oldFilePath, taggar, oldFileName);


                File.Move(filePathAndName, newFileName);

               
            }
            else if (Directory.Exists(filePathAndName))
            {
                string oldFileName = Path.GetFileName(filePathAndName);
                string oldFilePath = Path.GetDirectoryName(filePathAndName);

                string taggar = "";
                foreach (var item in tag.Split(' '))
                {
                    taggar += string.Format("[{0}]", item);
                }

                string newFileName = string.Format(@"{0}\{1}{2}", oldFilePath, taggar, oldFileName);

                 Directory.Move(filePathAndName, newFileName);
            }
        }
    }
}
