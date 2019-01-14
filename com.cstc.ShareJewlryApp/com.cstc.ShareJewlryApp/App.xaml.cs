using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace com.cstc.ShareJewlryApp
{
    public partial class App : Application
    {
        //bool load = false;
        //public static int ScreenWidth;
        //public static int ScreenHeight;
        //public App()
        //{
        //    InitializeComponent();

        //    //Views.MainPage page = new Views.MainPage();
        //    //NavigationPage navipage = new NavigationPage(page);
        //    //page.getUserData();
        //    //this.MainPage = navipage;  


        //    Views.HomePage.homePage page = new Views.HomePage.homePage();
        //    NavigationPage navipage = new NavigationPage(page);
        //    this.MainPage = navipage;

        //    // MainPage = new Views.HomePage.ProductDetails.ProductSoldOutPage();
        //}


        public App(NavigationPage navigationPage, string flag)
        {
            InitializeComponent();
            

            if (flag == "comdty")
            {

                this.MainPage = navigationPage;
            }
            else
            {
                //Views.HomePage.homePage page = new Views.HomePage.homePage();
                NavigationPage navipage = new NavigationPage(Helpers.MConfig.homePage);
                this.MainPage = navipage;
            }

            // The root page of your application

        }

      

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
