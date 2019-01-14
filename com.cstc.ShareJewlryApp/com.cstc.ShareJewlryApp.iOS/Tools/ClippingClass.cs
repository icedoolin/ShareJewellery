using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using UIKit;
using System.Drawing;
using CoreGraphics;


[assembly: Xamarin.Forms.Dependency(typeof(com.cstc.ShareJewlryApp.iOS.Tools.ClippingClass))]
namespace com.cstc.ShareJewlryApp.iOS.Tools
{
    class ClippingClass : com.cstc.ShareJewlryApp.Tools.IClippingClass
    {
        public byte[] ResizeImage(byte[] imageData, float maxLength)
        {
            return ResizeImageIOS(imageData, maxLength);
        }

        public static byte[] ResizeImageIOS(byte[] imageData, float maxLength)
        {
            UIKit.UIImage originalImage = ImageFromByteArray(imageData);
            UIKit.UIImageOrientation orientation = originalImage.Orientation;
            //create a 24bit RGB image

            if (originalImage.Orientation != UIImageOrientation.Up)   //翻转照片
            {
                UIGraphics.BeginImageContextWithOptions(originalImage.Size, false, originalImage.CurrentScale);
                originalImage.Draw(new RectangleF(0, 0, (float)originalImage.Size.Width, (float)originalImage.Size.Height));
                var resultImage = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();

                originalImage = resultImage;
                orientation = originalImage.Orientation;
            }


            if (originalImage.Size.Width <= maxLength && originalImage.Size.Height <= maxLength)
            {
                return originalImage.AsJPEG().ToArray();
            }

            int NewHeight = 0;
            int NewWidth = 0;

            if (originalImage.Size.Width > originalImage.Size.Height)
            {
                NewWidth = (int)maxLength;
                NewHeight = Convert.ToInt32(originalImage.Size.Height * (maxLength / originalImage.Size.Width));
            }
            else
            {
                NewHeight = (int)maxLength;
                NewWidth = Convert.ToInt32(originalImage.Size.Width * (maxLength / originalImage.Size.Height));
            }

            using (CoreGraphics.CGBitmapContext context = new CoreGraphics.CGBitmapContext(IntPtr.Zero,
                                                 (int)NewWidth, (int)NewHeight, 8,
                                                 (int)(4 * NewWidth), CoreGraphics.CGColorSpace.CreateDeviceRGB(),
                                                 CoreGraphics.CGImageAlphaInfo.PremultipliedFirst))
            {
                System.Drawing.RectangleF imageRect = new System.Drawing.RectangleF(0, 0, NewWidth, NewHeight);
                // Draw the image
                context.DrawImage(imageRect, originalImage.CGImage);
                UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 0, orientation);
                // save the image as a jpeg
                return resizedImage.AsJPEG().ToArray();
            }
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
    }
}