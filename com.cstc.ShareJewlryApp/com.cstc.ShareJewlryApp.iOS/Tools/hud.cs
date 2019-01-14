using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using BigTed;

[assembly: Xamarin.Forms.Dependency(typeof(com.cstc.ShareJewlryApp.iOS.Tools.hud))]
namespace com.cstc.ShareJewlryApp.iOS.Tools
{
    public class hud :com.cstc.ShareJewlryApp.Tools.Ihud
    {
        /// <summary>
        /// 显示旋转 + 文本
        /// </summary>
        /// <param name="StatusMessage"></param>
        public void Show_StatusMessage(string StatusMessage)
        {
            BTProgressHUD.ForceiOS6LookAndFeel = true;
            BTProgressHUD.Show(StatusMessage);
        }
        /// <summary>
        /// 关闭
        /// </summary>
        public void Dismiss()
        {
            BTProgressHUD.Dismiss();
        }
        /// <summary>
        /// 一个成功的图像显示一个消息，有一个明确的背景，并自动排除后2秒
        /// </summary>
        /// <param name="Message"></param>
        public void Show_Success(string Message)
        {
            BTProgressHUD.ForceiOS6LookAndFeel = true;
            BTProgressHUD.ShowSuccessWithStatus(Message, 2000);
        }
        /// <summary>
        /// 显示一个错误图像与一个模糊的背景的消息，并自动排除后2秒
        /// </summary>
        /// <param name="Message"></param>
        public void Show_Error(string Message)
        {
            BTProgressHUD.ForceiOS6LookAndFeel = true;
            BTProgressHUD.ShowErrorWithStatus(Message, 2000);
        }
        /// <summary>
        /// 显示一个IOS风格的吐司
        /// </summary>
        /// <param name="Message"></param>
        public void Show_Toast(string Message)
        {
            BTProgressHUD.ForceiOS6LookAndFeel = true;
            BTProgressHUD.ShowToast(Message, false, 2000);
        }
    }
}