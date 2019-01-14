using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Mycontrols
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdView : ContentView
    {
        public AdView()
        {
            InitializeComponent();
            BackgroundColor = Color.FromRgba(0, 0, 0, 0.5);
        }

        bool 按钮防呆 = false;
        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
        public string Title {
            get { return AdView_Title.Text; }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    AdView_Title.Text = value;
                });
            }
        }
        public string Text
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    AdView_Text.Text = value;
                });
            }
        }
        public string ImgSource
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    AdView_Image.Source = value;
                });
            }
        }
 
        private void TapAdClose_Tapped(object sender, EventArgs e)
        {
            this.IsVisible = false;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private void TapAdView_Tapped(object sender, EventArgs e)
        {
            this.IsVisible = false;
        }

        private void TapAdBeVip_Tapped(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            if (Data.UserInfoCache.UserGUID == "")
            {
                hud.Show_Toast("请先登录");
                按钮防呆 = false;
                return;
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(new Views.MyCenter.BuyVipPage(), true);
            });
            按钮防呆 = false;
        }

        private void TapAdInvite_Tapped(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;


            if (Data.UserInfoCache.UserGUID == "")
            {
                hud.Show_Toast("请先登录");
                按钮防呆 = false;
                return;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(new Views.MyCenter.InvitePage(), true);
            });
            按钮防呆 = false;
        }
    }
}