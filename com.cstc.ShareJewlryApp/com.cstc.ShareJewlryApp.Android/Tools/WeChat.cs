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
using Com.Tencent.MM.Sdk.Modelpay;
using Com.Tencent.MM.Sdk.Openapi;
using Xamarin.Forms;
using Com.Tencent.MM.Sdk.Modelmsg;
using Android.Graphics;
using System.IO;
using AndroidHUD;

[assembly: Xamarin.Forms.Dependency(typeof(com.cstc.ShareJewlryApp.Droid.Tools.WeChat))]
namespace com.cstc.ShareJewlryApp.Droid.Tools
{
    class WeChat : com.cstc.ShareJewlryApp.Tools.IWeChat
    {
        public static MainActivity PA = null;
        public static IWXAPI wxapi;

        /// <summary>
        /// 微信登录
        /// </summary>
        public void WXLogin()
        {
            wxapi = MainActivity.wxApi;
            if (wxapi == null)
            {
                wxapi = WXAPIFactory.CreateWXAPI(Forms.Context, Helpers.MConfig.APP_ID, true);
            }
            if (!wxapi.IsWXAppInstalled)
            {
                Toast.MakeText(Forms.Context, "请先安装微信应用", ToastLength.Long).Show();
                AndHUD.Shared.Dismiss();
                return;
            }
            if (!wxapi.IsWXAppSupportAPI)
            {
                Toast.MakeText(Forms.Context, "请先更新微信应用", ToastLength.Long).Show();
                AndHUD.Shared.Dismiss();
                return;
            }
            wxapi.RegisterApp(Helpers.MConfig.APP_ID);
            SendAuth.Req req = new SendAuth.Req();

            //授权用户信息
            req.Scope = "snsapi_userinfo";
            //自定义信息
            req.State = "com_cstc_ShareJewlryApp_Droid_wx_login";
            wxapi.SendReq(req);

           
        }

        public void shareWebPage(string url, string webPageTitle, string webPageDesc, string tag,string imgUrl)
        {
            wxapi = MainActivity.wxApi;
            if (wxapi == null)
            {
                wxapi = WXAPIFactory.CreateWXAPI(Forms.Context, Helpers.MConfig.APP_ID, true);
            }
            if (!wxapi.IsWXAppInstalled)
            {
                Toast.MakeText(Forms.Context, "请先安装微信应用", ToastLength.Long).Show();
                return;
            }
            if (!wxapi.IsWXAppSupportAPI)
            {
                Toast.MakeText(Forms.Context, "请先更新微信应用", ToastLength.Long).Show();
                return;
            }
            wxapi.RegisterApp(Helpers.MConfig.APP_ID);

            WXWebpageObject webPage = new WXWebpageObject();
            webPage.WebpageUrl = url;
            WXMediaMessage msg = new WXMediaMessage(webPage);
            msg.Title = webPageTitle;
            msg.Description = webPageDesc;
            //注意，微信分享的时候图片不能大于32K，否则无法分享
            msg.ThumbData = GetImage(imgUrl);

            

            SendMessageToWX.Req req = new SendMessageToWX.Req();
            req.Transaction = buildTransaction("webpage");
            req.Message = msg;
            if(tag=="Friends")
            {
                req.Scene = SendMessageToWX.Req.WXSceneTimeline;
            }
            if(tag=="Chat")
            {
                req.Scene = SendMessageToWX.Req.WXSceneSession;
            }

           bool a=  wxapi.SendReq(req);
        }

      

        public void WXPay(string resultPayid, string resultNoncestr, string resultSign, string timestamp)
        {
            Intent intent = new Intent(PA, typeof(WXPayActivity));
            intent.PutExtra("payid", resultPayid);
            intent.PutExtra("noncestr", resultNoncestr);
            intent.PutExtra("sign", resultSign);
            intent.PutExtra("timestamp", timestamp);

            try
            {
                PA.StartActivity(intent);
            }
            catch(Exception ex)
            {
                Android.Widget.Toast.MakeText(PA, "支付异常:" + ex.Message, ToastLength.Long).Show();
            }
        }

        private string buildTransaction(string type)
        {
            return (type == null) ? Convert.ToString(com.cstc.ShareJewlryApp.Tools.CommonClass.currentTimeMillis()) : type + com.cstc.ShareJewlryApp.Tools.CommonClass.currentTimeMillis();
        }

        public byte[] GetImage(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                Bitmap thumb = BitmapFactory.DecodeResource(Forms.Context.Resources, Resource.Drawable.icon);
                MemoryStream stream = new MemoryStream();
                thumb.Compress(Bitmap.CompressFormat.Png, 0, stream);
                byte[] bitmapData = stream.ToArray();
                return bitmapData;
            }
            else
            {
                Bitmap thumb = getBitmap(url);
                MemoryStream stream = new MemoryStream();
                thumb.Compress(Bitmap.CompressFormat.Png, 0, stream);
                byte[] bitmapData = stream.ToArray();
                return bitmapData;
            }
        }

        public static Bitmap getBitmap(string imagePath)
        {
            byte[] imgByte = null;
            System.Net.WebClient webClient = new System.Net.WebClient();
            imgByte = webClient.DownloadData(imagePath);
            Bitmap bmp = BitmapFactory.DecodeByteArray(imgByte, 0, imgByte.Length);
            return bmp;
        }
    }
}