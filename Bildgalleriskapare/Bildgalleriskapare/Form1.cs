using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace Bildgalleriskapare
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult res;
            res = folderBrowserDialog1.ShowDialog();
            FileList.Items.Clear();
            toolStripProgressBar1.Value = 0;

            Image.GetThumbnailImageAbort tni = new Image.GetThumbnailImageAbort(ThumbnailCallback);

            if (res.ToString() == "OK")
            {
                string[] files = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.JPG");
                foreach (string file in files)
                {
                    FileList.Items.Add(file.ToString());
                    Match m = Regex.Match(file.ToString(), @"([\w\W]+.JPG)");
                    //label1.Text = m.ToString();
                    //string bildnamn;

                    Bitmap bild = new Bitmap(file.ToString());
                    Bitmap bild2 = new Bitmap((Bitmap)bild.GetThumbnailImage(160, 120, tni, IntPtr.Zero));
                    // spara tummnageln med prefix
                    bild2.Save(folderBrowserDialog1.SelectedPath + "/TN_" + m.ToString());
                    bild2.Dispose();
                    bild.Dispose();
                }
                label1.Text = "Resize done. Thumbnails save in same directory as original files.";
                toolStripProgressBar1.Value = 1;
                // någon koll om de gick bra?
                // Skapa fil som använder masterpage
                int antal = FileList.Items.Count;
                int i, x, y, sum;
                string folderName = folderBrowserDialog1.SelectedPath.Substring(folderBrowserDialog1.SelectedPath.LastIndexOf(@"\") + 1);
                string imageName, prevImage, nextImage, lastImage, firstImage;
                label1.Text = folderName;

                Match lm = Regex.Match(FileList.Items[antal - 1].ToString(), @"(\w+.JPG)");
                Match fm = Regex.Match(FileList.Items[0].ToString(), @"(\w+.JPG)");

                lastImage = lm.ToString();
                firstImage = fm.ToString();

                FileStream fs = new FileStream(folderBrowserDialog1.SelectedPath + "/" + folderName + ".aspx", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("<%@ page language='VB' autoeventwireup='false' masterpagefile='~/Main.master' %>");
                sw.WriteLine("<asp:content id='Content1' contentplaceholderid='bread' runat='server'>");
                sw.WriteLine("<span style='font-weight:bold; font-size:2.4em; padding-left:50px;'>- " + folderName + " -</span>");
                sw.WriteLine("<table>");

                y = (int)(antal + 4) / 4;
                sum = 0;

                prevImage = "";
                for (i = 0; i < y; i++)
                {
                    sw.WriteLine("<tr>");
                    for (x = 0; x < 4; x++)
                    {
                        if (sum < antal)
                        {

                            sw.WriteLine("  <td>");
                            Match m = Regex.Match(FileList.Items[sum].ToString(), @"(\w+.JPG)");
                            imageName = m.ToString();
                            if (sum < antal - 1)
                            {
                                Match m2 = Regex.Match(FileList.Items[sum + 1].ToString(), @"(\w+.JPG)");
                                nextImage = m2.ToString();
                            }
                            else
                            {
                                nextImage = "";
                            }

                            sw.WriteLine("    <a href='" + folderName + "/" + imageName + ".aspx'><img src='../../bilder/" + folderName + "/TN_" + imageName + "' width='160' height='120' /></a>");
                            sw.WriteLine("  </td>");

                            // skapa aspx-sida för bilden
                            Directory.CreateDirectory(folderBrowserDialog1.SelectedPath + "/" + folderName);
                            FileStream fsBildSida = new FileStream(folderBrowserDialog1.SelectedPath + "/" + folderName + "/" + imageName + ".aspx", FileMode.OpenOrCreate, FileAccess.Write);
                            StreamWriter swBildSida = new StreamWriter(fsBildSida);
                            swBildSida.WriteLine("<%@ page language='VB' autoeventwireup='false' masterpagefile='~/Main.master' %>");
                            swBildSida.WriteLine("<asp:content id='Content1' contentplaceholderid='bread' runat='server'>");



                            if (prevImage != "")
                            {
                                // Bagåt och första
                                swBildSida.WriteLine(" <a title='F&ouml;rsta' style='display:inline; border:none;padding:0;margin:0' href='" + firstImage + ".aspx" + "'><img style='border:none;' src='../../images/first.gif' /></a>");
                                swBildSida.WriteLine(" <a title='F&ouml;reg&aring;ende' style='display:inline; border:none;padding:0;margin:0' href='" + prevImage + ".aspx" + "'><img style='border:none;' src='../../images/previous.gif' /></a>");
                            }

                            // Home-länk
                            swBildSida.WriteLine(" <a title='Till gallerian' style='display:inline; border:none;padding:0;margin:0' href='/2.1/bilder.aspx'><img style='border:none;' src='../../images/home.gif' /></a>");
                            // index-länk
                            swBildSida.WriteLine(" <a title='" + folderName + "-Index' style='display:inline; border:none;padding:0;margin:0' href='../" + folderName + ".aspx" + "'><img style='border:none;' src='../../images/index.gif' /></a>");

                            if (nextImage != "")
                            {
                                // Gå till sista, och gå till nästa
                                swBildSida.WriteLine(" <a title='N&auml;sta' style='display:inline; border:none;padding:0;margin:0' href='" + nextImage + ".aspx" + "'><img style='border:none;' src='../../images/next.gif' /></a>");
                                swBildSida.WriteLine(" <a title='Sista' style='display:inline; border:none;padding:0;margin:0' href='" + lastImage + ".aspx" + "'><img style='border:none;' src='../../images/last.gif' /></a>");
                            }

                            // Gallerirubrik
                            swBildSida.WriteLine(" <span style='font-weight:bold; font-size:2.4em; padding-left:50px;'>- " + folderName + " " + (sum + 1) + "/" + antal + "-</span>");

                            swBildSida.WriteLine(" <br />");
                            // Själva bilden i litet format länkad till den stora bilden
                            swBildSida.WriteLine(" <a href='../../../bilder/" + folderName + "/" + imageName + "'><img src='../../../bilder/" + "/" + folderName + "/" + imageName + "' height='600' width='800' /></a>");


                            swBildSida.WriteLine("</asp:content>");
                            swBildSida.Close();
                            fsBildSida.Close();

                            swBildSida.Dispose();
                            fsBildSida.Dispose();

                            sum++;

                            prevImage = imageName;

                            if (sum == antal / 2)
                                toolStripProgressBar1.Value = 2;

                        }
                    }
                    sw.WriteLine("</tr>");
                }
                toolStripProgressBar1.Value = 3;
                sw.WriteLine("</table>");
                sw.WriteLine("</asp:content>");
                sw.Close();
                fs.Close();
                // toolStripProgressBar1.Value = 0;
            }
        }
        public bool ThumbnailCallback() { return false; }

    }
}