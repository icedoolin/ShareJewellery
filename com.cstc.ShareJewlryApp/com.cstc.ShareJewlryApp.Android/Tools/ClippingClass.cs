using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(com.cstc.ShareJewlryApp.Droid.Tools.ClippingClass))]
namespace com.cstc.ShareJewlryApp.Droid.Tools
{
    class ClippingClass : com.cstc.ShareJewlryApp.Tools.IClippingClass
    {
        public byte[] ResizeImage(byte[] imageData, float maxLength)
        {
            return ResizeImageAndroid(imageData, maxLength);
        }

        public static byte[] ResizeImageAndroid(byte[] imageData, float maxLength)
        {
            //Load the bitmap
            Android.Graphics.Bitmap originalImage = Android.Graphics.BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);

            if (originalImage.Width <= maxLength && originalImage.Height <= maxLength)
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    originalImage.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, ms);
                    return ms.ToArray();
                }
            }
            int NewHeight = 0;
            int NewWidth = 0;
            if (originalImage.Width > originalImage.Height)
            {
                NewWidth = (int)maxLength;
                NewHeight = Convert.ToInt32(originalImage.Height * (maxLength / originalImage.Width));
            }
            else
            {
                NewHeight = (int)maxLength;
                NewWidth = Convert.ToInt32(originalImage.Width * (maxLength / originalImage.Height));
            }

            Android.Graphics.Bitmap resizedImage = Android.Graphics.Bitmap.CreateScaledBitmap(originalImage, (int)NewWidth, (int)NewHeight, false);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                resizedImage.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
    }
}