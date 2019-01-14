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
	public partial class ChageNickNamePage: BasePage 
	{

        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
        string _NickName = "";
        public string NickName 
        {
            get
            {
                return _NickName;
            }
            set
            {
                _NickName = value;
                Device.BeginInvokeOnMainThread(() =>
                {
                    ety_nickName.Text = value;
                });
            }
        }

        public Tools.AsyncMsg am_保存昵称 = new Tools.AsyncMsg();
        bool 按钮防呆 = false;

		public ChageNickNamePage ()
		{
			InitializeComponent ();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_SaveNickName(object sender,EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            string 昵称 = ety_nickName.Text.Trim();

            if (昵称 == "")
            {
                hud.Show_Toast("请输入昵称");
                按钮防呆 = false;
                return;
            }

            if(Tools.CommonClass.判断是否有特殊字符(昵称))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
     
                    DisplayAlert("提示", "您输入的昵称存在特殊字符", "知道了");
                    按钮防呆 = false;
                });
                return;
            }

            if (昵称.Length >10)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("提示", "您输入的昵称过长", "知道了");
                    按钮防呆 = false;
                });
                return;
            }

            Tools.AsyncMsg am_修改昵称 = new Tools.AsyncMsg();

            string para = "UserGUID=" + Data.UserInfoCache.UserGUID + "&Nickname=" + 昵称;

            am_修改昵称.Completion += (object obj, string ex) =>
            {
                Data.UserInfoCache.userInfo.Nickname = 昵称;

                am_保存昵称.OnCompletion(null, "");

                Device.BeginInvokeOnMainThread(() =>
                {
                    hud.Show_Toast("修改成功");
                    Navigation.PopAsync(true);
                    按钮防呆 = false;
                });
            };

            am_修改昵称.Cancel += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //DisplayAlert("提示", "修改昵称失败", "知道了");
                    按钮防呆 = false;
                });
                return;
            };

            Tools.NetClass.GetStringByUrl("ExSql", "App\\UpdateNickname_1_0_0_1", para, am_修改昵称);
        }


    }
}