using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.QRCode
{
    public class BootQR
    {
        private const int MAX_LENGTH_PER_PART = 800;
        private static Object LockFile = new Object();

        public string ContainerQrFolder
        {
            get;
            set;
        }

        public string ContainerTextFolder
        {
            get;
            set;
        }

        public BootQR(string containerQrFolder, string containerTextFolder)
        {
            if (string.IsNullOrWhiteSpace(containerQrFolder))
            {
                throw new ArgumentNullException(containerQrFolder);
            }
            if (string.IsNullOrWhiteSpace(containerTextFolder))
            {
                throw new ArgumentNullException(containerTextFolder);
            }
            this.ContainerQrFolder = containerQrFolder;
            this.ContainerTextFolder = containerTextFolder;
        }

        private static string ReadFileAndRemoveWhiteSpaceLine(string filePath)
        {
            string content = File.ReadAllText(filePath);
            var lines = File.ReadAllLines(filePath);
            var builderContent = new StringBuilder();
            builderContent.AppendLine(string.Format(@"\\{0}", filePath));
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

        private static string[] GetAllFilesOfFolder(string folder) // no recusive
        {
            return Directory.GetFiles(folder);
        }

        private static string CreateFileName(string filePath)
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

        private static Bitmap CreateQrBitmap(string content)
        {
            QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.Q;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, eccLevel);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap bitmap = qrCode.GetGraphic(20);
            return bitmap;
        }

        private static void SaveQrBitmap(Bitmap bitmap, string fullFilePath)
        {
            bitmap.Save(fullFilePath);
        }

        public void Done(string folder)
        {
            try
            {
                string filePath = Path.Combine(folder, "result.txt");
                lock (LockFile)
                {
                    StreamWriter writer = new StreamWriter(filePath, true);
                    writer.WriteLine("Date/Time: " + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString());
                    writer.WriteLine("Done");
                    writer.WriteLine("================================================================");
                    writer.WriteLine();
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DoProcess()
        {
            string[] files = GetAllFilesOfFolder(ContainerTextFolder);
            string contentOfFile = string.Empty;
            foreach (var item in files)
            {
                string fileName = CreateFileName(item);
                contentOfFile = ReadFileAndRemoveWhiteSpaceLine(item);
                int lengthOfText = contentOfFile.Length;
                int part = (lengthOfText / MAX_LENGTH_PER_PART) + 1;

                // substring of file source code (500 characters)
                string temp = string.Empty;

                string fileNameOfQrPicture = string.Empty;

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
            Done(this.ContainerQrFolder);
        }
    }
}
