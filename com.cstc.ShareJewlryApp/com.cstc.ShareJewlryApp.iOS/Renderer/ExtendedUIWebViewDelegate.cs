using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
 

namespace com.cstc.ShareJewlryApp.iOS.Renderer
{
    public class ExtendedUIWebViewDelegate : UIWebViewDelegate
    {
        ExtendedWebViewRenderer webViewRenderer;

        public ExtendedUIWebViewDelegate(ExtendedWebViewRenderer wKWebView =null)
        {
            webViewRenderer = wKWebView ?? new ExtendedWebViewRenderer();
        }

        public override async void LoadingFinished(UIWebView webView)
        {
            try
            {
                var wv = webViewRenderer.Element as com.cstc.ShareJewlryApp.Mycontrols.autoHeightWebview;
                if (wv != null)
                {
                    int i = 20;
                    while (i-- > 0) // wait here till content is rendered
                    {
                        await System.Threading.Tasks.Task.Delay(1000);
                        if (wv.HeightRequest != (double)webView.ScrollView.ContentSize.Height)
                            wv.HeightRequest = (double)webView.ScrollView.ContentSize.Height;
                    }
                }
            }
            catch (Exception ex)  { }
        }
    }
}