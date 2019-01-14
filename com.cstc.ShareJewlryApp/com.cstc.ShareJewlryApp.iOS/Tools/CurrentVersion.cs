using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
[assembly: Xamarin.Forms.Dependency(typeof(com.cstc.ShareJewlryApp.iOS.Tools.GetCurrentVersion))]
namespace com.cstc.ShareJewlryApp.iOS.Tools
{
    class GetCurrentVersion : com.cstc.ShareJewlryApp.Tools.IGetVersion
    {
        public string CurrentVersion
        {
            get
            {
                string buildName = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
                string buildCode = NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
                return buildName;
            }

        }
    }
}