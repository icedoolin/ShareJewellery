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
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(com.cstc.ShareJewlryApp.Mycontrols.MyEditor), typeof(com.cstc.ShareJewlryApp.Droid.Renderer.MyEditorRender))]
namespace com.cstc.ShareJewlryApp.Droid.Renderer
{
    public class MyEditorRender : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.Transparent);
            }
        }
    }
}