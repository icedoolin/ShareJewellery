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

[assembly: Xamarin.Forms.Dependency(typeof(com.cstc.ShareJewlryApp.Droid.Tools.GetCurrentVersion))]
namespace com.cstc.ShareJewlryApp.Droid.Tools
{
    class GetCurrentVersion: com.cstc.ShareJewlryApp.Tools.IGetVersion
    {
        public string CurrentVersion
        {
            get
            {
                Context context = Application.Context;
                string name = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
                string code = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionCode.ToString();
                return name;
            }

        }
    }
}