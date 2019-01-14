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
    public partial class MainTooBarView : ContentView
    {
        public MainTooBarView()
        {
            InitializeComponent();

        }

        public string PageTitle
        {
            get
            {
                if (Home_Icon.TextColor == com.cstc.ShareJewlryApp.Style.Color.Color.redFont)
                {
                    return "首页";
                }
                if (Classification_Icon.TextColor == com.cstc.ShareJewlryApp.Style.Color.Color.redFont)
                {
                    return "分类";
                }
                if (VipCenter_Icon.TextColor == com.cstc.ShareJewlryApp.Style.Color.Color.redFont)
                {
                    return "会员中心";
                }
                else
                    return "个人中心";
            }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (value == "首页")
                    {
                        Home_Icon.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                        Home_Text.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                    }
                    else if (value == "分类")
                    {
                        Classification_Icon.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                        Classification_Text.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                    }
                    if (value == "会员中心")
                    {
                        VipCenter_Icon.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                        VipCenter_Text.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                    }
                    if (value == "个人中心")
                    {
                        UserCenter_Icon.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                        UserCenter_Text.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                    }
                });
            }
        }

        private void tapped_turnClassificationPage(object sender, EventArgs e)
        {
            if (PageTitle == "分类") return;

            Device.BeginInvokeOnMainThread(() =>
            {

                Helpers.NavagationPageMgr.Open(Navigation, Helpers.MConfig.classificationPage);

            });

        }

        private void tapped_turnMyCenterPage(object sender, EventArgs e)
        {
            if (PageTitle == "个人中心") return;

            Device.BeginInvokeOnMainThread(() =>
            {

                Helpers.NavagationPageMgr.Open(Navigation, Helpers.MConfig.myCenterPage);

            });

        }

        private void tapped_rutnShoppingCartMainPage(object sender, EventArgs e)
        {
            if (PageTitle == "会员中心") return;

            Device.BeginInvokeOnMainThread(() =>
            {

                if (Data.UserInfoCache.userInfo.RemainderDays > 0)
                    Helpers.NavagationPageMgr.Open(Navigation, Helpers.MConfig.vipInfoPage);
                else
                {
                    Helpers.NavagationPageMgr.Open(Navigation, Helpers.MConfig.buyVipPage);
                }

            });

        }

        private void tapped_turnHomePage(object sender, EventArgs e)
        {
            if (PageTitle == "首页") return;

            Device.BeginInvokeOnMainThread(() =>
            {

                Navigation.PopToRootAsync();

            });

        }
    }
}