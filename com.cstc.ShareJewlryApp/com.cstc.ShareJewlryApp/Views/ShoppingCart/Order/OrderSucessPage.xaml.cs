using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.ShoppingCart.Order
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OrderSucessPage: BasePage 
	{
 
        public decimal price { get; set; } = 0;
        public string flag { get; set; } = "";
        public OrderSucessPage ()
		{
			InitializeComponent ();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            img_sucess.HeightRequest = Helpers.MConfig.screenWidth * 0.6076;
        }

       public void UI呈现()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                lb_price.Text = price.ToString();
                if(flag=="购买")
                {
                    st_price.IsVisible = true;
                    st_PayType.IsVisible = true;
                }
                else if(flag=="免费戴")
                {
                    st_price.IsVisible = false;
                    st_PayType.IsVisible = false;

                }
            });
        }

        void tapped_backHomePage(object sender,EventArgs e)
        {
            Navigation.PopToRootAsync(true);
        }

    }
}