using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(com.cstc.ShareJewlryApp.Mycontrols.autoHeightWebview), typeof(com.cstc.ShareJewlryApp.iOS.Renderer.ExtendedWebViewRenderer))]
namespace com.cstc.ShareJewlryApp.iOS.Renderer
{
    public class ExtendedWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            Delegate = new ExtendedUIWebViewDelegate(this);
        }
    }
}