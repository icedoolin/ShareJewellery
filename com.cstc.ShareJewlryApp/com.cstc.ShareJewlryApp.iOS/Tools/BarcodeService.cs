using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using System.IO;
using CoreImage;
using CoreGraphics;
using System.Net;
using Xamarin.Forms;
using System.Drawing;

[assembly: Xamarin.Forms.Dependency(typeof(com.cstc.ShareJewlryApp.iOS.Tools.BarcodeService))]
namespace com.cstc.ShareJewlryApp.iOS.Tools
{
    class BarcodeService :com.cstc.ShareJewlryApp.Tools.IBarcodeService
    {
        com.cstc.ShareJewlryApp.Tools.Ihud iHUD = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.Ihud>();

        public Stream compoundPhoto(string url, string templetGuid, float CoordinateX, float CoordinateY, int Length)
        {
            var webClient = new WebClient();

            if (string.IsNullOrEmpty(url))
            {
                Device.BeginInvokeOnMainThread(() =>
                {

                    iHUD.Show_Toast("图片加载失败，请稍后重试");
                });
                return null;
            }
            var URL = new Uri(url);
            //  webClient.DownloadDataAsync(URL);
            byte[] bytes = webClient.DownloadData(URL);

            UIImage BackImg = ImageFromByteArray(bytes);    //背景图

            Stream QRstream = ConvertImageStream("http://test.gxzb168.com:8811/APP/invite/invite.html?templetGuid=" + templetGuid + "&parentGuid=" + Data.UserInfoCache.UserGUID, Length, Length);

            byte[] QRbuffer = new byte[QRstream.Length];
            QRstream.Read(QRbuffer, 0, QRbuffer.Length);

            UIImage QRImg = ImageFromByteArray(QRbuffer);   //二维码图

            UIGraphics.BeginImageContext(BackImg.Size);  //开始绘画

            BackImg.Draw(new RectangleF(0, 0, (float)BackImg.Size.Width, (float)BackImg.Size.Height));

            QRImg.Draw(new RectangleF((float)BackImg.Size.Width-120, (float)BackImg.Size.Height - 120, (float)QRImg.Size.Width, (float)QRImg.Size.Height));

            var resultImage = UIGraphics.GetImageFromCurrentImageContext();  //获取到绘画画出的图片
            UIGraphics.EndImageContext();  //结束绘画

            var newImgBytes =  resultImage.AsPNG().ToArray();   //转成byte数组

            Stream newImgStream = new MemoryStream(newImgBytes);
            newImgStream.Seek(0, SeekOrigin.Begin);

            return newImgStream;

        }


        public static UIKit.UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("图片加载失败: " + e.Message);
                return null;
            }
            return image;
        }


        public Stream ConvertImageStream(string text, int Width, int Height)
        {
            //1. 实例化二维码滤镜
            CIFilter filter = CIFilter.FromName("CIQRCodeGenerator");
            // 2. 恢复滤镜的默认属性
            filter.SetDefaults();
            // 3. 将字符串转换成NSData


            byte[] byteArray = Encoding.GetEncoding("utf-8").GetBytes(text);

            NSData adata = NSData.FromArray(byteArray);
            // NSData adata = new NSData(text, NSDataBase64DecodingOptions.None);
            // 4. 通过KVO设置滤镜inputMessage数据
            filter.SetValueForKey(adata, (NSString)"inputMessage");
            // 5. 获得滤镜输出的图像
            CIImage outputImage = filter.OutputImage;

            UIImage reimg = createNonInterpolatedUIImageFormCIImage(outputImage, Width, Width, Height);

            Stream s = null;

            try
            {
                s = reimg.AsPNG().AsStream();
            }
            catch (Exception e)
            {
                System.Console.WriteLine("错误：" + e.Message);
            }
            return s;
        }

        /**
        * 根据CIImage生成指定大小的UIImage
        *
        * @param image CIImage
        * @param size 图片宽度
        */
        UIImage createNonInterpolatedUIImageFormCIImage(CIImage image, double size, int width, int height)
        {

            CGRect ctr = image.Extent;
            float scale = (float)Math.Min(size / ctr.Width, size / ctr.Height);
            //float scale = (float)Math.Min(size / width, size / height);
            double s_width = ctr.Width * scale;
            double s_height = ctr.Height * scale;

            CGColorSpace color_space = CGColorSpace.CreateDeviceGray();
            CGContext cg_context = new CGBitmapContext(null, (int)s_width, (int)s_height, 8, 0, color_space, CGImageAlphaInfo.None);
            //CGContext cg_context = new CGBitmapContext(null, width, height, 8, 0, color_space, CGImageAlphaInfo.None);
            CIContext context = CIContext.FromOptions(null);

            CGImage bitmap_imge = context.CreateCGImage(image, ctr);

            cg_context.InterpolationQuality = CGInterpolationQuality.None;
            cg_context.ScaleCTM(scale, scale);
            cg_context.DrawImage(ctr, bitmap_imge);
            CGImage scaleImg = ((CGBitmapContext)cg_context).ToImage();

            return UIImage.FromImage(scaleImg);
        }
    }
}