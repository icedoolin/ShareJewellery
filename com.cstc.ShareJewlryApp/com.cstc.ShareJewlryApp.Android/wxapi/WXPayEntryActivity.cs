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
using Com.Tencent.MM.Sdk.Modelbase;
using Com.Tencent.MM.Sdk.Openapi;
using Xamarin.Forms;

namespace com.cstc.ShareJewlryApp.Droid.wxapi
{
    [Activity(Label = "WXPayEntryActivity", Name = "com.cstc.ShareJewlryApp.Android.wxapi.WXPayEntryActivity", Theme = "@android:style/Theme.Translucent", LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Exported = true)]

    public class WXPayEntryActivity : Activity, IWXAPIEventHandler
    {
        private IWXAPI api;
        private string WX_APP_ID = Helpers.MConfig.APP_ID;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            api = WXAPIFactory.CreateWXAPI(this, WX_APP_ID);
            api.HandleIntent(Intent, this);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            Intent = intent;
            api.HandleIntent(intent, this);
        }

        public void OnReq(BaseReq req)
        {
            //  throw new NotImplementedException();
        }

        public void OnResp(BaseResp resp)
        {
            switch (resp.MyErrCode)
            {
                case BaseResp.ErrCode.ErrOk:

                    Device.BeginInvokeOnMainThread( () =>
                    {
                        Toast.MakeText(Xamarin.Forms.Forms.Context, "支付成功", ToastLength.Short).Show();
                    });
                   
                   

                    //System.Threading.Tasks.Task.Delay(1000);

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
                        com.cstc.ShareJewlryApp.Tools.CommonClass.am_微信购买会员.OnCompletion(null, "");
                    }

                    Finish();
                    break;
                case BaseResp.ErrCode.ErrAuthDenied:
                    //AndHUD.Shared.ShowError(this, "支付失败", MaskType.Clear, TimeSpan.FromSeconds(3));
                    Device.BeginInvokeOnMainThread( () =>
                    {
                        Toast.MakeText(Xamarin.Forms.Forms.Context, "支付失败", ToastLength.Short).Show();
                    });
                    
                    Finish();
                    break;
                case BaseResp.ErrCode.ErrUserCancel:
                    //AndHUD.Shared.ShowSuccess(this, "您已取消支付", MaskType.Clear, TimeSpan.FromSeconds(3));
                    
                    Device.BeginInvokeOnMainThread( () =>
                    {
                        Toast.MakeText(Xamarin.Forms.Forms.Context, "取消支付", ToastLength.Short).Show();
                    });

                    Finish();
                    break;
            }
        }


    }
}