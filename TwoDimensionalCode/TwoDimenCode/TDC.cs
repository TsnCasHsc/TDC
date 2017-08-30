using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing.QrCode.Internal;

namespace TwoDimensionalCode.TwoDimenCode
{
  public  static  class TDC
    {
        /// <summary>
        /// 根据输入创建二维码,并用bitmap保存
        /// </summary>
        /// <param name="sourceStr"></param>
        /// <returns></returns>
        public static Bitmap create2DC(string sourceStr)
        {
            BarcodeWriter writer = null;
            EncodingOptions options = null;
            options = new QrCodeEncodingOptions
            {
                CharacterSet = "utf-8",//设置内容编码
                Width = 400,//二维码高和宽
                Height = 400,
                Margin = 1
            };
            writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;//格式决定了 生成的是啥类型码
            writer.Options = options;


            Bitmap bitmap = writer.Write(sourceStr);
            
            return bitmap;

        }
        /// <summary>
        /// 生成条形码 只支持偶数 和数字 
        /// </summary>
        /// <param name="strVal"></param>
        /// <returns></returns>
        public static  Bitmap create1DC(string strVal)
        {
            BarcodeWriter writer = null;
            EncodingOptions options = null;
            options = new EncodingOptions
            {
                Width = 300,
                Height = 400

            };
            writer = new BarcodeWriter();
            writer.Options = options;
            writer.Format = BarcodeFormat.ITF;//CODE_128

            Bitmap bitmap = writer.Write(strVal);

            return bitmap;

        }


        public  static  Bitmap create2DCWithLogo(string logoFileName,string strText)
        {
            BarcodeWriter writer = null;
            Bitmap logo = new Bitmap(logoFileName);

            MultiFormatWriter mfWriter = new MultiFormatWriter();
            Dictionary<EncodeHintType, object> hintDic = new Dictionary<EncodeHintType, object>();
            hintDic.Add(EncodeHintType.CHARACTER_SET, "utf-8");
            hintDic.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);

            //
            BitMatrix bitMatrix = mfWriter.encode(strText, BarcodeFormat.QR_CODE, 300, 300, hintDic);
            writer = new BarcodeWriter();
            Bitmap bitmap = writer.Write(bitMatrix);

            int[] rectangle = bitMatrix.getEnclosingRectangle();

            //计算插入图片大小
            int middleW = Math.Min((int)(rectangle[2] / 3.5), logo.Width);
            int middleH = Math.Min((int)(rectangle[3] / 3.5), logo.Height);

            int middleL = (bitmap.Width - middleW) / 2;
            int middleT = (bitmap.Height - middleH) / 2;

            Bitmap bitmapImg = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppRgb);
            var gph = Graphics.FromImage(bitmapImg);
            gph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            gph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            gph.DrawImage(bitmap, 0, 0);

            var myGraphics = Graphics.FromImage(bitmapImg);
            myGraphics.FillRectangle(Brushes.White, middleL, middleT, middleW, middleH);
            myGraphics.DrawImage(logo, middleL, middleT, middleW, middleH);

            //保存图片 
            // string filename = @"C:\Users\hsc\Desktop\TDC\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            //bitmapImg.Save(filename, ImageFormat.Png);

            return bitmapImg;
        }
        /// <summary>
        /// Bitmap转BitmapImage 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bitmap.Save(ms,ImageFormat.Png);//注意设置图片保存的格式
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }
        /// <summary>
        /// 解析二维码图片
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string decode2DC(string filename)
        {
            BarcodeReader reader = null;
            reader = new BarcodeReader();
            reader.Options.CharacterSet = "utf-8";
            Bitmap bitmap = new Bitmap(filename);
            Result result = reader.Decode(bitmap);

            return result == null ? "解析失败请确认解析的是否是二维码" : result.Text;
        }
    }
}
