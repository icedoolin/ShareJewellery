using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            
        }

 
 
        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            string title = this.CurrentPage.Title;
            

            if(title=="我的")
            {
                //this.CurrentPage = this.Children[0];
                
                //  if (Helpers.Settings.UserNum == "")
                //     return;
                var page = (Views.MyCenter.MyCenterPage)this.CurrentPage;
                page.显示用户数据();
            }


        }

        protected override void OnPagesChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnPagesChanged(e);
           
        }

        




    }
}