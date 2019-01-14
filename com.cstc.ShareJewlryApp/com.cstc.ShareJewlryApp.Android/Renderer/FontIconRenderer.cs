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
using Android.Graphics;

[assembly: ExportRenderer(typeof(com.cstc.ShareJewlryApp.Mycontrols.FontIcon), typeof(com.cstc.ShareJewlryApp.Droid.Renderer.FontIconRenderer))]
namespace com.cstc.ShareJewlryApp.Droid.Renderer
{
    public class FontIconRenderer :LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {

            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Typeface = Typeface.CreateFromAsset(Forms.Context.Assets, "iconfont.ttf");
            }
        }
    }
}