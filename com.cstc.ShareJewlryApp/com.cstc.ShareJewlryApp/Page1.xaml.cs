using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1: BasePage 
    {
        public Page1()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}