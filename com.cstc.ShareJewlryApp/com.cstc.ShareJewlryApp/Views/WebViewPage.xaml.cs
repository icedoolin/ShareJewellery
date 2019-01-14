using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebViewPage: BasePage 
    {
        public WebViewPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }

        public string Title
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    PageTitle.Text = value;
                });
            }
        }

        public string Url
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    webView.Source = value;
                });
            }
        }

    }
}