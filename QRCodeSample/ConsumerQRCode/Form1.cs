﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tam.QRCode;

namespace ConsumerQRCode
{
    public partial class Form1 : Form
    {
        const int MAX_LENGTH_PER_PART = 500;

        public string ContainerQrFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["ContainerQrFolder"].ToString();
            }
        }

        public string ContainerTextFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["ContainerTextFolder"].ToString();
            }
        }

        public Form1()
        {
            InitializeComponent();
            CenterToScreen();
            pictureBoxQRCode.BackgroundImageLayout = ImageLayout.Zoom;
        }

        private string ReadFileAndRemoveWhiteSpaceLine(string filePath)
        {
            string content = File.ReadAllText(filePath);
            var lines = File.ReadAllLines(filePath);
            var builderContent = new StringBuilder();
            foreach (var line in lines)
            {
                // content of file (we don't need white space line)
                if (string.IsNullOrWhiteSpace(line) == false)
                {
                    builderContent.Append(line.Trim());
                }
            }
            return builderContent.ToString();
        }

        private string[] GetAllFilesOfFolder(string folder) // no recusive
        {
            return Directory.GetFiles(folder);
        }

        private string CreateFileName(string filePath)
        {
            string fileName = filePath.Replace(":", "");
            fileName = fileName.Replace(@"\", "@");
            return fileName;
        }

        private void RenderQRCode(string filePath)
        {
            var listText = new List<string>();
            // content of file (we don't need white space)
            string trimContent = ReadFileAndRemoveWhiteSpaceLine(filePath);
            int lengthOfText = trimContent.Length;
            int part = (lengthOfText / MAX_LENGTH_PER_PART) + 1;

            // substring of file source code (500 characters)
            string temp = string.Empty;

            string fileNameOfQrPicture = string.Empty;
            string fileName = CreateFileName(filePath);

            for (int i = 0; i < part; i++)
            {
                if (i == part - 1)
                {
                    temp = trimContent.Substring(i * MAX_LENGTH_PER_PART);
                }
                else
                {
                    temp = trimContent.Substring(i * MAX_LENGTH_PER_PART, MAX_LENGTH_PER_PART);
                }
                //listText.Add(temp);
                fileNameOfQrPicture = Path.Combine(ContainerQrFolder, string.Format("{0}.part{1}.bmp", fileName, i));
                SaveQrBitmap(CreateQrBitmap(temp), fileNameOfQrPicture);
            }
            //this.Text = string.Format("{0} - {1}", lengthOfText, listText.Sum(l => l.Length));
            //pictureBoxQRCode.BackgroundImage = CreateQRCodeBitmap(listText[0]);
            //MessageBox.Show(listText[0]);
        }

        private void RenderQRCode()
        {
            // sample
            RenderQRCode(@"E:\Project2\Blog2\trunk\Tam.Framework\Core\Tam.Util\StringHelper.cs");
        }

        private Bitmap CreateQrBitmap(string content)
        {
            QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.Q;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, eccLevel);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap bitmap = qrCode.GetGraphic(20);
            return bitmap;
        }

        private void SaveQrBitmap(Bitmap bitmap, string fullFilePath)
        {
            bitmap.Save(fullFilePath);
        }

        private void DoProcess()
        {
            string[] files = GetAllFilesOfFolder(ContainerTextFolder);
            string contentOfFile = string.Empty;
            foreach (var item in files)
            {
                contentOfFile = ReadFileAndRemoveWhiteSpaceLine(item);
                int lengthOfText = contentOfFile.Length;
                int part = (lengthOfText / MAX_LENGTH_PER_PART) + 1;

                // substring of file source code (500 characters)
                string temp = string.Empty;

                string fileNameOfQrPicture = string.Empty;
                string fileName = CreateFileName(item);

                for (int i = 0; i < part; i++)
                {
                    if (i == part - 1)
                    {
                        temp = contentOfFile.Substring(i * MAX_LENGTH_PER_PART);
                    }
                    else
                    {
                        temp = contentOfFile.Substring(i * MAX_LENGTH_PER_PART, MAX_LENGTH_PER_PART);
                    }
                    //listText.Add(temp);
                    fileNameOfQrPicture = Path.Combine(ContainerQrFolder, string.Format("{0}.part{1}.bmp", fileName, i));
                    SaveQrBitmap(CreateQrBitmap(temp), fileNameOfQrPicture);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnGenerateQRCode_Click(object sender, EventArgs e)
        {
            try
            {
                //RenderQRCode();
                //DoProcess();
                var qr = new BootQR(this.ContainerQrFolder, this.ContainerTextFolder);
                qr.DoProcess();
                Thread thread = new Thread(new ThreadStart(qr.DoProcess));
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
