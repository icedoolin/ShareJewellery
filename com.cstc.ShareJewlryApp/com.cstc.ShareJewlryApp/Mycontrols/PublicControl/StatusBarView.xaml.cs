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
    public partial class StatusBarView : ContentView
    {
        public StatusBarView()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
                main.HeightRequest =40;

        }

        public Color bgColor
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    statusbar.BackgroundColor = value;
                });
            }
        }

        public int TotalHeight
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    statusbar.HeightRequest = value;
                    if (Device.RuntimePlatform == Device.iOS)
                        statusbar.HeightRequest = value*1.5;
                });
            }
        }
 
    }
}