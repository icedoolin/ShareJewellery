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
    public partial class ResetPWDPage: BasePage 
    {
        bool 按钮防呆 = false;
        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();

        public string phoneNum { get; set; } = "";//手机号

        public ResetPWDPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }
        /// <summary>
        /// “确定”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_savePWD(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            string 新密码 = "";
            string 确认新密码 = "";

            try
            { 
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

            string para = "Phone=" + phoneNum + "&NewPassword=" + 新密码;

            am_修改密码.Completion += (object obj, string ex) =>
            {
                Data.UserInfoCache.Password = 新密码;

                按钮防呆 = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    hud.Show_Toast("修改成功");
                    Navigation.PopAsync(false);
                    Navigation.PopAsync(true);
                });
            };

            am_修改密码.Cancel += (object obj, string ex) =>
            {
                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    DisplayAlert("提示", "重设密码失败:" + ex.ToString(), "知道了");
                //});
                按钮防呆 = false; 
            };


            Tools.NetClass.GetStringByUrl("ExSql", "App\\UpdatePasswordFromPhone_1_0_0_1", para, am_修改密码);
        }
    }
}