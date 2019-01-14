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
    public partial class AboutUsPage: BasePage 
    {
        public AboutUsPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            lbl_VersionNumber.RightText = Helpers.MConfig.AppCurrentVersion;
        }
 
 
        bool 按钮防呆 = false;
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private void TapCompanyDesc_Tapped(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (按钮防呆)
                    return;
                按钮防呆 = true;
                Views.WebViewPage page = new Views.WebViewPage();
                page.Title = "公司介绍";
                page.Url = Helpers.MConfig.CompanyDescUrl;
                Navigation.PushAsync(page, true);
 
                按钮防呆 = false;
            });
        }

        private void TapTermsOfService_Tapped(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (按钮防呆)
                    return;
                按钮防呆 = true;
                Views.WebViewPage page = new Views.WebViewPage();
                page.Title = "用户协议";
                page.Url = Helpers.MConfig.TermsOfServiceUrl;
                Navigation.PushAsync(page, true);

                按钮防呆 = false;
            });
        }

        private void TapCustomerService_Tapped(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (Helpers.MConfig.isNormalClick)
                {
                    Views.HomePage.CustomerService.CustomerServicePage page = new Views.HomePage.CustomerService.CustomerServicePage();
                    //page.getData();
                    Navigation.PushAsync(page, true);
                }
            });
        }
 
        private void TapNewVersion_Tapped(object sender, EventArgs e)
        {
            string newversion = "";
            if (Device.RuntimePlatform == Device.Android)
            {
                newversion = Data.UserInfoCache.userInfo.AndroidVersion;
            }
            if (Device.RuntimePlatform == Device.iOS)
            {
                newversion = Data.UserInfoCache.userInfo.IOSVersion;
            }
            if (Helpers.MConfig.AppCurrentVersion != newversion)
            {
                NewVersionBox.IsVisible = true;
            }
            else
            {
                hud.Show_Toast("当前已是最新的版本了！");
            }
 
        }

 
    }
}