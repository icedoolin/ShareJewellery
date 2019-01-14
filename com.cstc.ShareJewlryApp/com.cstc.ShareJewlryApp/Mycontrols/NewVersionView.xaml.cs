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
    public partial class NewVersionView : ContentView
    {
        public NewVersionView()
        {
            InitializeComponent();
            Layout_Show.BackgroundColor = Color.FromRgba(0, 0, 0, 0.5);
        }


        
        /// <summary>
        /// 下载更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TapBtn1_Tapped(object sender, EventArgs e)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Device.OpenUri(new Uri(Helpers.MConfig.UpdUrl_Android));
            }
            if (Device.RuntimePlatform == Device.iOS)
            {
                Device.OpenUri(new Uri(Helpers.MConfig.UpdUrl_IOS));
            }

            this.IsVisible = false;
        }

        /// <summary>
        /// 忽略
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TapBtn2_Tapped(object sender, EventArgs e)
        {
            this.IsVisible = false;
        }

        private void TapClose_Tapped(object sender, EventArgs e)
        {
            this.IsVisible = false;
        }
    }
}