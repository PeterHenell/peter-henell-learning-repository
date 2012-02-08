using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SlowFileMover
{
    public partial class SlowFileMover : Form
    {
        public SlowFileMover()
        {
            InitializeComponent();
        }

        private void btnSource_Click(object sender, EventArgs e)
        {
            FileDialogSource.ShowDialog();

            lblSource.Text = FileDialogSource.FileName.ToString();
        }

        private void btnDestination_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();

            lblDestination.Text = folderBrowserDialog1.SelectedPath.ToString();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            try
            {

                lblStatus.Text = "Starting.";

                FileStream fs = File.OpenRead(FileDialogSource.FileName.ToString());
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);


                string filename = FileDialogSource.FileName.Substring(FileDialogSource.FileName.LastIndexOf(@"\") + 1);
                FileInfo fi = new FileInfo(folderBrowserDialog1.SelectedPath.ToString() + "\\" + filename);

                BinaryWriter bw = new BinaryWriter(fi.OpenWrite());

                for (int i = 0; i < data.Length; i++)
                {
                    bw.Write(data[i]);
                    lblStatus.Text = "Status: Copying section " + i + " / " + data.Length;

                    Application.DoEvents();

                    if (radioButton1.Checked == true)
                    {

                    }
                    else if (radioButton2.Checked == true)
                    {
                        System.Threading.Thread.Sleep(1);
                    }
                    else if (radioButton3.Checked == true)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }

                }

                lblStatus.Text = "Finished.";

                bw.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Please select both source and destination");
            }
        }
    }
}
