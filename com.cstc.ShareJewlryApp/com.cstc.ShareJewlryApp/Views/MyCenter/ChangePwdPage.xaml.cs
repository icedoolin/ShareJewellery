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
    public partial class ChangePwdPage: BasePage 
    {

        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
        bool 是否初次设置密码 = false;
        bool 按钮防呆 = false;
        public ChangePwdPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            UI呈现();

        }

        void UI呈现()
        {
            if (Data.UserInfoCache.Password == "")
            {
                st_oldPwd.IsVisible = false;
                是否初次设置密码 = true;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void save_Changepwd(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;

            按钮防呆 = true;

            string 旧密码 = "";
            string 新密码 = "";
            string 确认新密码 = "";

            try
            {
                旧密码 = ety_oldPWD.Text;
                新密码 = ety_newPWD.Text;
                确认新密码 = ety_newPWDcheck.Text;
            }
            catch (Exception ex)
            {
                按钮防呆 = false;
                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    DisplayAlert("提示", "转换旧密码出错：" + ex.Message, "知道了");
                //});
                return;
            }

            if (!是否初次设置密码)
            {

                if (旧密码 == "")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Show_Toast("请输入旧密码"); 
                    });
                    按钮防呆 = false;
                    return;
                }

                if (旧密码 != Data.UserInfoCache.Password)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Show_Toast("旧密码输入不正确"); 
                    });
                    按钮防呆 = false;
                    return;
                }

                if (Tools.CommonClass.判断是否有特殊字符(旧密码))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Show_Toast("旧密码存在特殊字符"); 
                    });
                    按钮防呆 = false;
                    return;
                }
            }

            if (新密码 == "")
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    hud.Show_Toast("请输入新密码"); 
                });
                按钮防呆 = false;
                return;
            }

            if (新密码 != 确认新密码)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    hud.Show_Toast("新密码两次输入不一致"); 
                });
                按钮防呆 = false;
                return;
            }

            if (Tools.CommonClass.判断是否有特殊字符(新密码))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    hud.Show_Toast("新密码存在特殊字符"); 
                });
                按钮防呆 = false;
                return;
            }

            Tools.AsyncMsg am_修改密码 = new Tools.AsyncMsg();
            string para = "";

            if (是否初次设置密码)
            {
                para = "UserGUID=" + Data.UserInfoCache.UserGUID + "&NewPassword=" + 新密码;

            }
            else
            {
                para = "UserGUID=" + Data.UserInfoCache.UserGUID + "&OldPassword=" + 旧密码 + "&NewPassword=" + 新密码;
            }
            am_修改密码.Completion += (object obj, string ex) =>
            {
                Data.UserInfoCache.Password = 新密码;

                按钮防呆 = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    hud.Show_Toast("修改成功");
                    Navigation.PopAsync(true);
                });
            };

            am_修改密码.Cancel += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //DisplayAlert("提示", "修改密码失败:" + ex.ToString(), "知道了");
                });
                按钮防呆 = false;
                return;
            };

            if (是否初次设置密码)
            {
                Tools.NetClass.GetStringByUrl("ExSql", "App\\UpdatePasswordFromUserGUID_1_0_0_1", para, am_修改密码);

            }
            else
            {
                Tools.NetClass.GetStringByUrl("ExSql", "App\\UpdatePassword_1_0_0_1", para, am_修改密码);
            }

           

        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_forgetPWD(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;

            按钮防呆 = true;

            FindPwdPage page = new FindPwdPage();
            Navigation.PushAsync(page, true);

            按钮防呆 = false;
        }
    }
}