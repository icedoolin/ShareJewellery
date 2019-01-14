using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(com.cstc.ShareJewlryApp.Mycontrols.MyEditor), typeof(com.cstc.ShareJewlryApp.iOS.Renderer.MyEditorRender))]
namespace com.cstc.ShareJewlryApp.iOS.Renderer
{
    public class MyEditorRender : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                // do whatever you want to the UITextField here!

                Control.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);
            }
        }
    }
}