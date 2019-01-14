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
using Com.Tencent.MM.Sdk.Openapi;
using Com.Tencent.MM.Sdk.Modelpay;

namespace com.cstc.ShareJewlryApp.Droid
{
    [Activity(Label = "WXPayActivity")]
    public class WXPayActivity : Activity
    {
        public static string APP_ID = "wxb86a48efa41420e8";
        
        public static IWXAPI WXapi;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.NoTitle);//无标题栏
            string payid = Intent.GetStringExtra("payid");
            string noncestr = Intent.GetStringExtra("noncestr");
            string sign = Intent.GetStringExtra("sign");
            string timestamp = Intent.GetStringExtra("timestamp");

            WXapi = MainActivity.wxApi;
            if (WXapi == null)
            {
                WXapi = WXAPIFactory.CreateWXAPI(this, APP_ID);
            }
            WXapi.RegisterApp(APP_ID);
            PayReq payReq = new PayReq();
            payReq.AppId = APP_ID;
            payReq.PartnerId = com.cstc.ShareJewlryApp.Tools.CommonClass.MCH_ID;  //商户号
            payReq.PrepayId = payid;
            payReq.PackageValue = "Sign=WXPay";
            payReq.NonceStr = noncestr;//Helpers.Common.GenerateRandomNumber(32);// resultNoncestr;
            payReq.TimeStamp = timestamp;
            payReq.Sign = sign;//signResult;
            bool isflag = WXapi.SendReq(payReq);
            Finish();
        }
    }
}