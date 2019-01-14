using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using com.cstc.ShareJewlryApp.iOS.Renderer;
using com.cstc.ShareJewlryApp.Mycontrols;

[assembly: ExportRenderer(typeof(MultiLineLabel), typeof(CustomMultiLineLabelRenderer))]
namespace com.cstc.ShareJewlryApp.iOS.Renderer
{
    public class CustomMultiLineLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            MultiLineLabel multiLineLabel = (MultiLineLabel)Element;

            if (multiLineLabel != null && multiLineLabel.Lines != -1)
                Control.Lines = multiLineLabel.Lines;
        }
    }
}