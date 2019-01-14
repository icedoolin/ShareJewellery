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
	public partial class jerBoxBtn : ContentView
	{
		public jerBoxBtn ()
		{
			InitializeComponent ();
		}

        public string Text
        {
            get
            {
                return lb_title.Text;
            }
            set
            {
                lb_title.Text = value;
            }
        }
    }
}