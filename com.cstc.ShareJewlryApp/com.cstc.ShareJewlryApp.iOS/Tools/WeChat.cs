using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using WeChatBinding;

[assembly: Xamarin.Forms.Dependency(typeof(com.cstc.ShareJewlryApp.iOS.Tools.WeChat))]
namespace com.cstc.ShareJewlryApp.iOS.Tools
{
    public class WeChat : com.cstc.ShareJewlryApp.Tools.IWeChat
    {
 
        public static WXApi WXapi;

        public void WXLogin()
        {
            Data.UserInfoCache.日志("开始微信登录授权！");
            SendAuthReq req = new SendAuthReq();
            req.Scope = @"snsapi_userinfo";
            req.State = @"com_cstc_ShareJewlryApp_wx_login";
            WXApi.SendReq(req);
            Data.UserInfoCache.日志("完成微信登录授权！");
        }

        public void shareWebPage(string url, string webPageTitle, string webPageDesc, string tag, string imgUrl)
        {
            WXMediaMessage message = new WXMediaMessage();
            message.Title = webPageTitle;
            message.Description = webPageDesc;
            WXWebpageObject webpageobject = new WXWebpageObject();
            webpageobject.WebpageUrl = url;
            message.MediaObject = webpageobject;
            if (string.IsNullOrEmpty(imgUrl))
            {
                var img = new UIImage();
                img = UIImage.FromBundle("Icon");
                message.ThumbData = NSData.FromData(img.AsPNG());
            }
            else
            {
                var imgurl = new NSUrl(imgUrl);
                message.ThumbData = NSData.FromData(NSData.FromUrl(imgurl));
            }

            SendMessageToWXReq req = new SendMessageToWXReq();
            req.BText = false;
            req.Message = message;

            if (tag == "Friends")
                req.Scene = Convert.ToInt32(WXScene.Timeline);
            if (tag == "Chat")
                req.Scene = Convert.ToInt32(WXScene.Session);

            WXApi.SendReq(req);
        }

       

        public void WXPay(string resultPayid, string resultNoncestr, string resultSign, string timestamp)
        {
            PayReq request = new PayReq();
            request.PartnerId = com.cstc.ShareJewlryApp.Tools.CommonClass.MCH_ID;
            request.PrepayId = resultPayid;
            request.Package = @"Sign=WXPay";
            request.NonceStr = resultNoncestr;
            request.TimeStamp = Convert.ToUInt32(timestamp);
            request.Sign = resultSign;
            WXApi.SendReq(request);
        }
    }
}