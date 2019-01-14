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
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(com.cstc.ShareJewlryApp.Mycontrols.noBorderEntry), typeof(com.cstc.ShareJewlryApp.Droid.Renderer.MyEntryRender))]
namespace com.cstc.ShareJewlryApp.Droid.Renderer
{
    public class MyEntryRender: EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.Transparent);
                
                
            }


        }
    }
}