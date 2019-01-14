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


[assembly: ExportRenderer(typeof(com.cstc.ShareJewlryApp.Mycontrols.LongPressBtn), typeof(com.cstc.ShareJewlryApp.Droid.Renderer.LongPressBtnRenderer))]
namespace com.cstc.ShareJewlryApp.Droid.Renderer
{
    public class LongPressBtnRenderer :ButtonRenderer
    {
        public LongPressBtnRenderer(Context c):base(c)
        { }
        protected override void OnElementChanged(ElementChangedEventArgs<global::Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
          // if (e.OldElement == null)
          //  {

                var nativeButton = Control;
                nativeButton.LongClick += delegate
                {
                    //Do something
                     
                    if (e.NewElement != null && e.NewElement is com.cstc.ShareJewlryApp.Mycontrols.LongPressBtn)
                    {
                        ((com.cstc.ShareJewlryApp.Mycontrols.LongPressBtn)e.NewElement).OnLongClicked(e.NewElement, e);
                    }
                    
                };
          //  }
        }
    }
}