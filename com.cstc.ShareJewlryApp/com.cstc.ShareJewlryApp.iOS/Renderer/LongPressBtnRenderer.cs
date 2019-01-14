using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(com.cstc.ShareJewlryApp.Mycontrols.LongPressBtn), typeof(com.cstc.ShareJewlryApp.iOS.Renderer.LongPressBtnRenderer))]
namespace com.cstc.ShareJewlryApp.iOS.Renderer
{
    public class LongPressBtnRenderer
    {
        public class LongPressGestureRecognizerButtonRenderer : ButtonRenderer
        {
            com.cstc.ShareJewlryApp.Mycontrols.LongPressBtn  view;

            public LongPressGestureRecognizerButtonRenderer()
            {
                
                this.AddGestureRecognizer(new UILongPressGestureRecognizer((longPress) => {
                    if (longPress.State == UIGestureRecognizerState.Began)
                    {
                        view.HandleLongPress(view, new EventArgs());
                    }
                }));
            }

            protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
            {

                base.OnElementChanged(e);
                try
                {
                    if (e.NewElement != null)
                        view = e.NewElement as com.cstc.ShareJewlryApp.Mycontrols.LongPressBtn;
                }
                catch (Exception ex) { }
            

            }
        }
    }
}