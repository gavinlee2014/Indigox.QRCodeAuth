using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace Indigox.QRCodeAuth.Application.Biz
{
    class QRCodeUtil
    {
        public static Bitmap GenerateQRCode(string text)
        {
            var QCwriter = new BarcodeWriter();
            QCwriter.Format = BarcodeFormat.QR_CODE;
            QCwriter.Options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                ErrorCorrection = ErrorCorrectionLevel.H,
                CharacterSet = "UTF-8",
                Width = 250,
                Height = 250,
            };
            return QCwriter.Write(text);

        }

        public static byte[] GenerateQRCodeByte(string text)
        {
            Bitmap code = GenerateQRCode(text);
            using (MemoryStream ms = new MemoryStream())
            {
                code.Save(ms, ImageFormat.Bmp);
                return ms.ToArray();
            }
        }
    }
}
