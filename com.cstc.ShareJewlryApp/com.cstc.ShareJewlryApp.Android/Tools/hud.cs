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
using AndroidHUD;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(com.cstc.ShareJewlryApp.Droid.Tools.hud))]
namespace com.cstc.ShareJewlryApp.Droid.Tools
{
    public class hud : com.cstc.ShareJewlryApp.Tools.Ihud
    {
        /// <summary>
        /// 显示旋转 + 文本
        /// </summary>
        /// <param name="StatusMessage"></param>
        public void Show_StatusMessage(string StatusMessage)
        {
            AndHUD.Shared.Show(Forms.Context, StatusMessage, -1, MaskType.Clear);
        }
        /// <summary>
        /// 关闭
        /// </summary>
        public void Dismiss()
        {
            AndHUD.Shared.Dismiss();
        }
        /// <summary>
        /// 一个成功的图像显示一个消息，有一个明确的背景，并自动排除后2秒
        /// </summary>
        /// <param name="Message"></param>
        public void Show_Success(string Message)
        {
            AndHUD.Shared.ShowSuccess(Forms.Context, Message, MaskType.Clear, TimeSpan.FromSeconds(2));
        }
        /// <summary>
        /// 显示一个错误图像与一个模糊的背景的消息，并自动排除后2秒
        /// </summary>
        /// <param name="Message"></param>
        public void Show_Error(string Message)
        {
            AndHUD.Shared.ShowError(Forms.Context, Message, MaskType.Clear, TimeSpan.FromSeconds(2));
        }
        /// <summary>
        /// 显示一个安卓风格的吐司
        /// </summary>
        /// <param name="Message"></param>
        public void Show_Toast(string Message)
        {
            AndHUD.Shared.ShowToast(Forms.Context, Message, MaskType.Clear, TimeSpan.FromSeconds(2));
        }
    }
}