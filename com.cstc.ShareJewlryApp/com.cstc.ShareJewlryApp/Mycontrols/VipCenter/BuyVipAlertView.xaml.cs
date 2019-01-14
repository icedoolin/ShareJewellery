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
	public partial class BuyVipAlertView : ContentView
	{
		public BuyVipAlertView()
		{
			InitializeComponent ();
            BackgroundColor = Color.FromRgba(0, 0, 0, 0.5);
            //IOS的按钮弧度半径为android的一半
            if (Device.RuntimePlatform == Device.iOS)
            {
                BuyVipSuccess_Btn.CornerRadius = 10;
            }
            Main_Layout.HeightRequest = 390 * Helpers.MConfig.screenRatios;
            Main_Layout.WidthRequest = 260 * Helpers.MConfig.screenRatios;
        }

        bool 按钮防呆 = false;
        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();

        private void TapClose_Tapped(object sender, EventArgs e)
        {
            this.IsVisible = false;
        }
 

        private void TapKnowMore_Tapped(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(new Views.MyCenter.BuyVipPage(), true);
            });
            按钮防呆 = false;
        }
    }
}