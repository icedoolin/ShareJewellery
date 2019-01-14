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


[assembly: Xamarin.Forms.Dependency(typeof(com.cstc.ShareJewlryApp.Droid.Tools.Ali))]
namespace com.cstc.ShareJewlryApp.Droid.Tools
{
    public class Ali : com.cstc.ShareJewlryApp.Tools.IAli
    {

        public void AliPay(string orderInfo)
        {
            Intent intent = new Intent(MainActivity.PA, typeof(AliPayActivity));
            intent.PutExtra("orderInfo", orderInfo);

            try
            {
                MainActivity.PA.StartActivity(intent);
            }
            catch (Exception ex)
            {
                Android.Widget.Toast.MakeText(MainActivity.PA, "支付异常:" + ex.Message, ToastLength.Long).Show();
            }
        }
    }
}