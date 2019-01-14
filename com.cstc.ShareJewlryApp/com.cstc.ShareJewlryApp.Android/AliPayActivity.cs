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
using Com.Alipay;
using Com.Alipay.Sdk.App;
using Xamarin.Forms;
namespace com.cstc.ShareJewlryApp.Droid
{
    [Activity(Label = "AliPayActivity")]
    public class AliPayActivity : Activity
    {
        //int SDK_PAY_FLAG = 1;
        //string orderInfo = "";
        //Handler handler = new Handler((Message msg)=>
        //{ 
        //    ///回调方法
        //    string s = (string)msg.Obj;
          

        //});
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);//无标题栏
            string orderInfo = Intent.GetStringExtra("orderInfo");


            // Create your application here
            System.Threading.Thread thread = new System.Threading.Thread(Pay);
            thread.Start(orderInfo);
            
            //Pay(orderInfo);
        }

        void Pay(object orderObj)
        {
            // string orderInfo = "app_id=2015052600090779&biz_content=%7B%22timeout_express%22%3A%2230m%22%2C%22seller_id%22%3A%22%22%2C%22product_code%22%3A%22QUICK_MSECURITY_PAY%22%2C%22total_amount%22%3A%220.02%22%2C%22subject%22%3A%221%22%2C%22body%22%3A%22%E6%88%91%E6%98%AF%E6%B5%8B%E8%AF%95%E6%95%B0%E6%8D%AE%22%2C%22out_trade_no%22%3A%22314VYGIAGG7ZOYY%22%7D&charset=utf-8&method=alipay.trade.app.pay&sign_type=RSA2&timestamp=2016-08-15%2012%3A12%3A15&version=1.0&sign=MsbylYkCzlfYLy9PeRwUUIg9nZPeN9SfXPNavUCroGKR5Kqvx0nEnd3eRmKxJuthNUx4ERCXe552EV9PfwexqW%2B1wbKOdYtDIb4%2B7PL3Pc94RZL0zKaWcaY3tSL89%2FuAVUsQuFqEJdhIukuKygrXucvejOUgTCfoUdwTi7z%2BZzQ%3D";


            string orderInfo = orderObj.ToString();
            Com.Alipay.Sdk.App.PayTask pa = new Com.Alipay.Sdk.App.PayTask(this);
           // com.cstc.ShareJewlryApp.Helpers.Settings.日志("调用支付功能");
            string result = pa.Pay(orderInfo);

            

            if (result.Contains("6001"))
            {
                Device.BeginInvokeOnMainThread( () =>
                {
                    Toast.MakeText(Xamarin.Forms.Forms.Context, "取消支付", ToastLength.Short).Show();
                });
               
            }
            else if (result.Contains("9000"))
            {
                Device.BeginInvokeOnMainThread( () =>
                {
                    Toast.MakeText(Xamarin.Forms.Forms.Context, "支付成功", ToastLength.Short).Show();
                });

                

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
                    com.cstc.ShareJewlryApp.Tools.CommonClass.am_支付宝购买会员.OnCompletion(null, result);
                    //com.cstc.ShareJewlryApp.Views.MyCenter.BuyVipPage.am_支付宝付款.OnCompletion(null, result);
                }

            }
            else if (result.Contains("4000"))
            {
                Device.BeginInvokeOnMainThread( () =>
                {
                     Toast.MakeText(Xamarin.Forms.Forms.Context, "支付失败", ToastLength.Short).Show();
                });
               
            }


            Finish();


            // com.cstc.ShareJewlryApp.Helpers.Settings.日志("支付返回result:" + result);



            //Message msg = new Message();
            //msg.What = SDK_PAY_FLAG;
            //msg.Obj = result;

            //handler.SendMessage(msg);

            // Finish();
        }

        
      

    }


}