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


using WebView = Android.Webkit.WebView;
[assembly: ExportRenderer(typeof(com.cstc.ShareJewlryApp.Mycontrols.autoHeightWebview), typeof(com.cstc.ShareJewlryApp.Droid.Renderer.ExtendedWebViewRenderer))]
namespace com.cstc.ShareJewlryApp.Droid.Renderer
{
    public class ExtendedWebViewRenderer: WebViewRenderer
    {
        //static com.cstc.ShareJewlryApp.Mycontrols.autoHeightWebview _xwebView = null;
        //WebView _webView;

        class ExtendedWebViewClient : Android.Webkit.WebViewClient
        {

            public com.cstc.ShareJewlryApp.Mycontrols.autoHeightWebview _xwebView { get; set; } = null;

            public override async void OnPageFinished(WebView view, string url)
            {
                try
                {
                    if (_xwebView != null)
                    {
                        int i = 20;
                        while ( i-- > 0) // wait here till content is rendered
                        {
                            await System.Threading.Tasks.Task.Delay(1000);
                            if(_xwebView.HeightRequest != view.ContentHeight)
                                _xwebView.HeightRequest = view.ContentHeight;
                        }
 
                    }
                }
                catch(Exception ex)
                {

                }
 
            }
        }

        public  ExtendedWebViewRenderer(Context c): base(c)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            try
            {
                base.OnElementChanged(e);
                var ewv = new ExtendedWebViewClient();
                ewv._xwebView = e.NewElement as com.cstc.ShareJewlryApp.Mycontrols.autoHeightWebview;

                //if (e.OldElement == null)
                //{
                Control.SetWebViewClient(ewv);
                //}
            }
            catch (Exception ex)
            {

            }
        }

    }
}