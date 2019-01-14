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
using Xamarin.Forms;
using Android.Support.V4.Content;
using Android;
using System.Net;
using Android.Content.PM;
using System.IO;
using Android.Graphics;
using Java.Nio;
using Java.IO;

[assembly: Xamarin.Forms.Dependency(typeof(com.cstc.ShareJewlryApp.Droid.Tools.Copy))]
namespace com.cstc.ShareJewlryApp.Droid.Tools
{
    public class Copy : com.cstc.ShareJewlryApp.Tools.ICopy
    {
        public static MainActivity PA = MainActivity.PA;
        com.cstc.ShareJewlryApp.Tools.Ihud iHUD = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.Ihud>();

        public void SaveNetImage(byte[] imgbtyes)
        {
            if (ContextCompat.CheckSelfPermission(PA, Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    iHUD.Show_StatusMessage("");
                });
            

                if (imgbtyes == null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        iHUD.Dismiss();
                        iHUD.Show_Toast("图片加载失败，请稍后重试");
                    });
                    return;
                }
                // byte[] NewByteArray = new byte[imgStream.Length];    //获得图片的byte数组
                //imgStream.Read(NewByteArray, 0, NewByteArray.Length);
                //imgStream.Seek(0, SeekOrigin.Begin);

                Java.IO.File dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
                string DcimAbsolutePath = dir.AbsolutePath;
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string localFilename = "downloaded.png";
                string localPath = System.IO.Path.Combine(documentsPath, localFilename);
                System.IO.File.WriteAllBytes(localPath, imgbtyes); // writes to local storage
                                                                   //adding a time stamp time file name to allow saving more than one image... otherwise it overwrites the previous saved image of the same name
                string name = "Share" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                string filePath = System.IO.Path.Combine(DcimAbsolutePath, name);
                try
                {
                    System.IO.File.WriteAllBytes(filePath, imgbtyes);
                    //mediascan adds the saved image into the gallery   存进相册
                    var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                    mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(filePath)));
                    Xamarin.Forms.Forms.Context.SendBroadcast(mediaScanIntent);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        iHUD.Dismiss();
                        iHUD.Show_Toast("保存成功");
                    });
                }
                catch
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        iHUD.Dismiss();
                        iHUD.Show_Toast("保存失败,请检查系统授权");
                    });

                }
            }
            else
            {

                PA.RequestPermissions(new string[] { Manifest.Permission.WriteExternalStorage }, (int)Android.Content.PM.RequestedPermission.Granted);
            }
        }

        public void SavePictureToDisk(string filename, byte[] imageData)
        {
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
            var pictures = dir.AbsolutePath;
            //adding a time stamp time file name to allow saving more than one image... otherwise it overwrites the previous saved image of the same name
            string name = filename + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
            string filePath = System.IO.Path.Combine(pictures, name);
            try
            {
                System.IO.File.WriteAllBytes(filePath, imageData);
                //mediascan adds the saved image into the gallery
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(filePath)));
                Xamarin.Forms.Forms.Context.SendBroadcast(mediaScanIntent);
                iHUD.Show_Toast("保存成功");
            }
            catch
            {
                iHUD.Show_Toast("保存失败");
            }
        }

        public void SaveTest(string test)
        {
            try
            {

                var s = Context.ClipboardService;
                ClipboardManager cm = (ClipboardManager)PA.GetSystemService(s);
                cm.Text = test;
                Device.BeginInvokeOnMainThread(() =>
                {
                    iHUD.Show_Toast("复制成功");
                });
            }
            catch
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    iHUD.Show_Toast("复制失败");
                });
            }
        }
    }
}