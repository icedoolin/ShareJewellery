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
    public partial class MessageCenter: BasePage   
    {
        public MessageCenter()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Web_MessageCenter.Source =Helpers.MConfig.MessageUrl + "userGuid=" + Data.UserInfoCache.UserGUID;
        }

        bool isfirstpage = true;

        protected override bool OnBackButtonPressed()
        {
            bool re = false;
            if (Device.RuntimePlatform.ToString() == Device.Android)
            {
                if (!isfirstpage)
                {
                    Web_MessageCenter.GoBack();
                    return true;
                }
                else
                    Navigation.PopAsync(false);
            }
            return re;


        }

        private void Web_MessageCenter_Navigating(object sender, WebNavigatingEventArgs e)
        {

            string identify = "detail"; //自定义协议关键字:二级页面包含NoticeGUID
            string url = e.Url; //href信息
            if (url.ToLower().Contains(identify)) //是自定义的xaml:协议，执行事件
            {
                isfirstpage = false;
                lbl_Title.IsFirstPage = false;
                //e.Cancel = true;

            }
            else
            {
                isfirstpage = true;
            }


        }

        private void Lbl_Title_BtnBackClick(object sender, EventArgs e)
        {
            if (!isfirstpage)
            {
                Web_MessageCenter.GoBack();
            }
            else
                Navigation.PopAsync(false);
        }
    }
}