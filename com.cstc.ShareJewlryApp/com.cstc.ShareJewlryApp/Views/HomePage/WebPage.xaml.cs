using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.HomePage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebPage: BasePage 
    {
        public string webUrl { get; set; } = "";

        public WebPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }

        public void UI呈现()
        {
            web.Source = webUrl;
        }

        private void tapped_closePage(object sender, EventArgs e)
        {
            Navigation.PopAsync(true);
        }

        private void tapped_goBackPage(object sender, EventArgs e)
        {
            if (web.CanGoBack)
                web.GoBack();
        }
    }
}