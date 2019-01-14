using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using BigTed;
using Xamarin.Forms;
using System.Net;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(com.cstc.ShareJewlryApp.iOS.Tools.Copy))]
namespace com.cstc.ShareJewlryApp.iOS.Tools
{
    public class Copy : com.cstc.ShareJewlryApp.Tools.ICopy
    {

        public void SaveNetImage(byte[] imgStream)
        {
 
            if (imgStream==null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Show_Error("图片加载失败，请稍后再试");
                });
                return;
            }

            try
            { 
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string LocalFilename = "dowm.png";
                string localPath = Path.Combine(documentsPath, LocalFilename);

                ///转换成图片
                NSData nsdata = NSData.FromArray(imgStream);
                UIImage uimage = new UIImage(nsdata);



                File.WriteAllBytes(localPath, imgStream);


                //请求更改照片库     
                Photos.PHPhotoLibrary.SharedPhotoLibrary.PerformChanges(() =>
                {
                    Photos.PHAssetChangeRequest pHAssetChangeRequest = Photos.PHAssetChangeRequest.FromImage(uimage);
                },
                (t, p) =>
                {
                    if (t)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Show_Success("保存成功");
                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Show_Error("保存失败");
                        });
                    }
                });
            }
            catch
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Show_Error("图片加载失败，请稍后再试");
                });
            }


        }

        #region 消息提示
        /// <summary>
        /// 一个成功的图像显示一个消息，有一个明确的背景，并自动排除后2秒
        /// </summary>
        /// <param name="Message"></param>
        public void Show_Success(string Message)
        {
            BTProgressHUD.ForceiOS6LookAndFeel = true;
            BTProgressHUD.ShowSuccessWithStatus(Message, 3000);
        }
        /// <summary>
        /// 显示一个错误图像与一个模糊的背景的消息，并自动排除后2秒
        /// </summary>
        /// <param name="Message"></param>
        public void Show_Error(string Message)
        {
            BTProgressHUD.ForceiOS6LookAndFeel = true;
            BTProgressHUD.ShowErrorWithStatus(Message, 3000);
        }


        public void Show_StatusMessage(string StatusMessage)
        {
            BTProgressHUD.ForceiOS6LookAndFeel = true;
            BTProgressHUD.Show(StatusMessage);
        }

        #endregion




        public void SavePictureToDisk(string filename, byte[] imageData)
        {
            try
            {
                var chartImage = new UIImage(NSData.FromArray(imageData));
                chartImage.SaveToPhotosAlbum((image, error) =>
                {
                    //you can retrieve the saved UI Image as well if needed using
                    //var i = image as UIImage;
                    com.cstc.ShareJewlryApp.Tools.Ihud iHUD = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.Ihud>();
                    //if (error != null)
                    //{
                    //    iHUD.Show_Toast("保存失败");
                    //}
                    //else
                    //{
                    //    iHUD.Show_Toast("保存成功");
                    //}
                });
            }
            catch
            {
            }

        }

        public void SaveTest(string test)
        {
            try
            {
                UIPasteboard uIPasteboard = UIPasteboard.General;
                uIPasteboard.String = test;
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    Show_Success("复制成功");
                });
            }
            catch
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    Show_Error("复制失败");
                });
            }


        }
    }
}