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
using Android.Content.PM;

namespace com.cstc.ShareJewlryApp.Droid
{
    [Activity(Label = "新贵佳人", Icon = "@drawable/icon", Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DelayedNaviagition();
            // Create your application here
        }

        private void DelayedNaviagition()
        {

            //System.Threading.Tasks.Task.Delay(2000);

            var intent = new Intent(this, typeof(MainActivity));
            
            StartActivity(intent);
            Finish();
        }
    }
}