using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using WeChatBinding;
using AlipaySDKBinding;
using Xamarin.Forms;
using BigTed;

namespace com.cstc.ShareJewlryApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //public static readonly string APP_ID = "wxb86a48efa41420e8";

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            Helpers.MConfig.screenWidth = UIScreen.MainScreen.Bounds.Width; //屏幕宽度
            UITabBar.Appearance.SelectedImageTintColor = UIColor.FromRGB(254,142,197);
            //自动登录
            if (Data.UserInfoCache.UserGUID != "")
            {
                Helpers.AsyncMsg am = new Helpers.AsyncMsg();
                Data.UserinfoMgr.LoginWithoutAlert("4", Data.UserInfoCache.UserGUID, "", am);
            }

            LoadApplication(new App(null, "homepage"));
            WXApi.RegisterApp(com.cstc.ShareJewlryApp.Helpers.MConfig.APP_ID);
            return base.FinishedLaunching(app, options);
        }

        //public override bool HandleOpenURL(UIApplication application, NSUrl url)
        //{
        //    Data.UserInfoCache.日志("开始HandleOpenURL！");
        //    WxDelegate d = new WxDelegate();
        //    return WXApi.HandleOpenURL(url, d);
        //}

        private bool AliPayResult(NSUrl url)
        {
            if (url.Host.Equals("safepay"))
            {
                string resultStatus = "";
                try
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        AlipaySDK.DefaultService.ProcessOrderWithPaymentResult(url,
                        new CompletionBlock(aa =>
                        {
                            try
                            {
                                string result = System.Web.HttpUtility.UrlDecode(url.ToString());
                                string[] lists = result.Split("?"); //分解获取?后面的内容
                                Data.AliResponse user = (Data.AliResponse)Newtonsoft.Json.JsonConvert.DeserializeObject(lists[1], typeof(Data.AliResponse));
                                resultStatus = user.memo.ResultStatus;
                            }
                            catch (Exception ex)
                            {
                                BTProgressHUD.ShowToast("支付失败", true, 2000);
                            }


                        }));
                        ////如果支付失败
                        if (!resultStatus.Equals("9000"))
                        {
                            BTProgressHUD.ShowToast("支付失败", true, 2000);
                        }
                        else
                        {
                            BTProgressHUD.ShowToast("支付成功", true, 2000);
                            //如果支付成功
                            if (com.cstc.ShareJewlryApp.Tools.CommonClass.支付页面标志 == "还珠宝")
                            {
                                com.cstc.ShareJewlryApp.Tools.CommonClass.am_还珠宝支付.OnCompletion(null, "");
                            }

                            if (com.cstc.ShareJewlryApp.Tools.CommonClass.支付页面标志 == "卖家报损赔付")
                            {
                                com.cstc.ShareJewlryApp.Tools.CommonClass.am_卖家报损支付.OnCompletion(null, "");
                            }

                            if (com.cstc.ShareJewlryApp.Tools.CommonClass.支付页面标志 == "全额赔偿")
                            {
                                com.cstc.ShareJewlryApp.Tools.CommonClass.am_全额赔偿支付.OnCompletion(null, "");
                            }

                            if (com.cstc.ShareJewlryApp.Tools.CommonClass.支付页面标志 == "购买会员")
                            {
                                com.cstc.ShareJewlryApp.Tools.CommonClass.am_支付宝购买会员.OnCompletion(null, "");
                                //com.cstc.ShareJewlryApp.Views.MyCenter.BuyVipPage.am_支付宝付款.OnCompletion(null, "");
                            }
                        }
                    });
                }
                catch (Exception ex)
                {

                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 支付宝支付回调, IOS9.3以上
        /// </summary>
        /// <param name="app"></param>
        /// <param name="url"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {

            if (url.Scheme.Equals(com.cstc.ShareJewlryApp.Helpers.MConfig.APP_ID))
            {
 
                WxDelegate d = new WxDelegate();
                bool b = WXApi.HandleOpenURL(url, d);
                return b;
            }
            else if (url.Host.Equals("safepay"))
            {
                return AliPayResult(url);
            }
            else
                return true;
        }


        /// <summary>
        /// 微信回调，IOS9.3以下支付宝支付回调 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="url"></param>
        /// <param name="sourceApplication"></param>
        /// <param name="annotation"></param>
        /// <returns></returns>
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
 
            if (url.Scheme.Equals(com.cstc.ShareJewlryApp.Helpers.MConfig.APP_ID))
            {
                WxDelegate d = new WxDelegate();
                bool b = WXApi.HandleOpenURL(url, d);
                return b;
            }
            else if(url.Host.Equals("safepay"))
            {
                return AliPayResult(url);
            }
            else
            {
                return true;
            }
        }

       
    }
}
