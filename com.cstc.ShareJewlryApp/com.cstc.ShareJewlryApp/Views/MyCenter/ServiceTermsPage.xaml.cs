using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.MyCenter
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ServiceTermsPage: BasePage 
	{
		public ServiceTermsPage ()
		{
			InitializeComponent ();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }
	}
}