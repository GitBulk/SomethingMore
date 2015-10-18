using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.QRCode
{
    public abstract class AbstractQRCode<T>
    {
        protected QRCodeData qrCodeData;

        protected AbstractQRCode(QRCodeData data)
        {
            qrCodeData = data;
        }

        public abstract T GetGraphic(int pixelsPerModule);
    }
}
