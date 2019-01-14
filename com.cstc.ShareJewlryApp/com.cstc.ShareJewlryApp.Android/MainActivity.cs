using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using Com.Tencent.MM.Sdk.Openapi;
using Xamarin.Forms;

namespace com.cstc.ShareJewlryApp.Droid
{
    [Activity(Label = "com.cstc.ShareJewlryApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
    Categories = new[] { Android.Content.Intent.CategoryDefault,
                         Android.Content.Intent.CategoryBrowsable },
    DataScheme = "sharejewlryapp",
    DataHost = "productdetails")]   ///urlscheme过滤器  自定义scheme必须转化为小写 即TestApp => testapp

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity PA;
        public static IWXAPI wxApi;
        //public static readonly string APP_ID = "wxb86a48efa41420e8";  //开放平台上的AppID
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //com.cstc.ShareJewlryApp.App.ScreenWidth= Resources.DisplayMetrics.WidthPixels;
            //com.cstc.ShareJewlryApp.App.ScreenHeight = Resources.DisplayMetrics.HeightPixels;
            global::Xamarin.Forms.Forms.Init(this, bundle);
            if (Data.UserInfoCache.UserGUID != "")
            {
                Helpers.AsyncMsg am = new Helpers.AsyncMsg();
                Data.UserinfoMgr.LoginWithoutAlert("4", Data.UserInfoCache.UserGUID, "", am);
            }
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;//只允许竖屏
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            var width = Resources.DisplayMetrics.WidthPixels;
            var density = Resources.DisplayMetrics.Density; //屏幕密度
            Helpers.MConfig.screenWidth= width / density; //屏幕宽度
            HideToolBar();

            PA = this;
            wxApi = WXAPIFactory.CreateWXAPI(this, com.cstc.ShareJewlryApp.Helpers.MConfig.APP_ID, true); //向微信注册该APP
            wxApi.RegisterApp(com.cstc.ShareJewlryApp.Helpers.MConfig.APP_ID);
            Tools.WeChat.PA = this;
 

            Android.Net.Uri uri = Intent.Data;
 
            if (uri != null)
            {
                if (uri.Scheme == "sharejewlryapp")
                {
                    string guid = uri.GetQueryParameter("JewelleryGUID");

                    var mainPage = new NavigationPage(new Views.HomePage.homePage());
                    var cmtyPage = new Views.HomePage.ProductDetails.ProductDetailsPage();
                    cmtyPage.JewelleryGUID = guid;
                    //cmtyPage.getCommodityDetailsData();
 
                    mainPage.PushAsync(cmtyPage);
                    LoadApplication(new App(mainPage, "comdty"));
                }
            }
            else
            {
                LoadApplication(new App(null, "homepage"));
            }

        }

        /// <summary>
        /// 隐藏工具栏
        /// </summary>
        public void HideToolBar()
        {
            var uiOpts = SystemUiFlags.LightStatusBar    
                //SystemUiFlags.LayoutStable
                 //| SystemUiFlags.LayoutHideNavigation
                 | SystemUiFlags.LayoutFullscreen
                 //| SystemUiFlags.Fullscreen
                 //| SystemUiFlags.HideNavigation
                 //| SystemUiFlags.LightStatusBar
                 ;

            Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            Window.SetNavigationBarColor(Android.Graphics.Color.Transparent);
            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOpts;
 

        }

    }
}

