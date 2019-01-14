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
    public partial class AlertBoxView : ContentView
    {
        public AlertBoxView()
        {
            InitializeComponent();
            BackgroundColor = Color.FromRgba(0, 0, 0, 0.5);
 
            Main_Layout.HeightRequest = 220 * Helpers.MConfig.screenRatios;
            Main_Layout.WidthRequest = 300 * Helpers.MConfig.screenRatios;
 
        }
        public string Text
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    BodyText.Text = value;
                });
            }
        }
        public string ImgSource
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    BgImage.Source = value;
                });
            }
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private void TapOK_Tapped(object sender, EventArgs e)
        {
            this.IsVisible = false;
        }

 





    }
}