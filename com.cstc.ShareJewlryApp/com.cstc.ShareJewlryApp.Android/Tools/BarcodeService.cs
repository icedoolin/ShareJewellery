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
using System.IO;
using Android.Graphics;
using System.Net;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(com.cstc.ShareJewlryApp.Droid.Tools.BarcodeService))]
namespace com.cstc.ShareJewlryApp.Droid.Tools
{
    class BarcodeService : com.cstc.ShareJewlryApp.Tools.IBarcodeService
    {
        com.cstc.ShareJewlryApp.Tools.Ihud iHUD = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.Ihud>();

        public Stream compoundPhoto(string url, string templetGuid,float CoordinateX,float CoordinateY,int Length)
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

            //背景图片
            Android.Graphics.Bitmap BackBmp = Android.Graphics.BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);

            BarcodeService bs = new BarcodeService();

            Stream codeStream = bs.ConvertImageStream("http://test.gxzb168.com:8811/APP/invite/invite.html?templetGuid=" + templetGuid + "&parentGuid=" + Data.UserInfoCache.UserGUID, Length, Length);   //获得二维码

            byte[] CodeBytes = new byte[codeStream.Length];
            codeStream.Read(CodeBytes, 0, CodeBytes.Length);
            //// 设置当前流的位置为流的开始
            codeStream.Seek(0, SeekOrigin.Begin);
            //二维码图片
            Android.Graphics.Bitmap CodeBmp = Android.Graphics.BitmapFactory.DecodeByteArray(CodeBytes, 0, CodeBytes.Length);
            //最后的图片
            Bitmap bitmap = Bitmap.CreateBitmap(BackBmp.Width, BackBmp.Height, BackBmp.GetConfig());
            Canvas canvas = new Canvas(bitmap);
            canvas.DrawBitmap(BackBmp, new Matrix(), null);
            canvas.DrawBitmap(CodeBmp, BackBmp.Width-120, BackBmp.Height-120, null);   //绘制二维码


            Stream baos = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, baos);
            baos.Position = 0;    //合成后的图片流



            return baos;



        }


        public Stream ConvertImageStream(string text, int Width, int Height)
        {
            var barcodeWriter = new ZXing.Mobile.BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = Width,
                    Height = Height,
                    Margin = 0
                }
            };

            barcodeWriter.Renderer = new ZXing.Mobile.BitmapRenderer();
            var bitmap = barcodeWriter.Write(text);
            var stream = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);  // this is the diff between iOS and Android
            stream.Position = 0;
            return stream;
        }
    }
}