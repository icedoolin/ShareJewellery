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

[assembly: ExportRenderer(typeof(com.cstc.ShareJewlryApp.Mycontrols.MyScrollView), typeof(com.cstc.ShareJewlryApp.Droid.Renderer.MyScrollViewRenderer))]
namespace com.cstc.ShareJewlryApp.Droid.Renderer
{
    public class MyScrollViewRenderer : ScrollViewRenderer
    {
        private GestureDetector mGestureDetector;

       

    }
}