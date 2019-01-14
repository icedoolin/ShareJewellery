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
using com.cstc.ShareJewlryApp.Mycontrols;
using com.cstc.ShareJewlryApp.Droid.Renderer;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MultiLineLabel), typeof(CustomMultiLineLabelRenderer))]
namespace com.cstc.ShareJewlryApp.Droid.Renderer
{
    public class CustomMultiLineLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            MultiLineLabel multiLineLabel = (MultiLineLabel)Element;

            if (multiLineLabel != null && multiLineLabel.Lines != -1)
            {
                Control.SetSingleLine(false);
                Control.SetLines(multiLineLabel.Lines);
            }
        }
    }
}