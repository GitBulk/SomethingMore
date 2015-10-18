using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tam.QRCode;

namespace ConsumerQRCode
{
    public partial class Form1 : Form
    {
        const int MAX_LENGTH_PER_PART = 500;
        public Form1()
        {
            InitializeComponent();
            CenterToScreen();
            pictureBoxQRCode.BackgroundImageLayout = ImageLayout.Zoom;
        }

        private void RenderQRCode()
        {
            string path = @"E:\Project2\Blog2\trunk\Tam.Framework\Core\Tam.Util\StringHelper.cs";
            string content = File.ReadAllText(path);
            //string content = "http://en.code-bude.net/2013/10/17/qrcoder-an-open-source-qr-code-generator-implementation-in-csharp/";
            var lines = File.ReadAllLines(path);
            var builder = new StringBuilder();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) == false)
                {
                    builder.Append(line.Trim());
                }
            }
            //this.Text = content.Length.ToString() + "-" + builder.ToString().Length.ToString();
            string trimContent = builder.ToString();
            int lengthOfText = trimContent.Length;
            int part = (lengthOfText / MAX_LENGTH_PER_PART) + 1;
            var listText = new List<string>();
            string folderPathQRCode = "E:\\usb";
            string temp = string.Empty;
            for (int i = 0; i < part; i++)
            {
                if (i == part - 1)
                {
                    temp = trimContent.Substring(i * MAX_LENGTH_PER_PART);
                    listText.Add(temp);
                }
                else
                {
                    temp = trimContent.Substring(i * MAX_LENGTH_PER_PART, MAX_LENGTH_PER_PART);
                    listText.Add(temp);
                }
                CreateQRCodeBitmap(temp).Save(Path.Combine(folderPathQRCode, string.Format("qr{0}.bmp", i)));
            }
            this.Text = string.Format("{0} - {1}", lengthOfText, listText.Sum(l => l.Length));
            //pictureBoxQRCode.BackgroundImage = CreateQRCodeBitmap(listText[0]);
            MessageBox.Show(listText[0]);
        }

        private Bitmap CreateQRCodeBitmap(string content)
        {
            QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.Q;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, eccLevel);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap bitmap = qrCode.GetGraphic(20);
            return bitmap;
        }

        private void btnGenerateQRCode_Click(object sender, EventArgs e)
        {
            try
            {
                RenderQRCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
