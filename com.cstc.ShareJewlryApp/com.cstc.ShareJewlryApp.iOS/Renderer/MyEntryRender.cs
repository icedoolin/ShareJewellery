using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(com.cstc.ShareJewlryApp.Mycontrols.noBorderEntry), typeof(com.cstc.ShareJewlryApp.iOS.Renderer.MyEntryRender))]
namespace com.cstc.ShareJewlryApp.iOS.Renderer
{
    public class MyEntryRender : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                // do whatever you want to the UITextField here!
                Control.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);
                Control.BorderStyle = UITextBorderStyle.None;

            }
        }
    }
}