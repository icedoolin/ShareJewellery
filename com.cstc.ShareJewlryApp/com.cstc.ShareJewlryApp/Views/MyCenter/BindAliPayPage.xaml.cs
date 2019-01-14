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
    public partial class BindAliPayPage: BasePage 
    {
        public Tools.AsyncMsg am_支付宝页面 = new Tools.AsyncMsg();
        bool 按钮防呆 = false;
        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
        public BindAliPayPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }

        /// <summary>
        /// 弹出确认保存框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_BindAliPay(object sender, EventArgs e)
        {
            block.IsVisible = true;
            st_checkBox.IsVisible = true;

        }


        /// <summary>
        /// 关闭确认弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_cancle(object sender, EventArgs e)
        {
            block.IsVisible = false;
            st_checkBox.IsVisible = false;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_confirmed(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;


            string 支付宝账号 = ety_AliPayNum.Text.Trim();
            string 真名 = ety_realName.Text.Trim();

            if (支付宝账号 == "")
            {
                按钮防呆 = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    hud.Show_Toast("支付宝账号为空");
                });

                return;
            }

            if (真名 == "")
            {
                按钮防呆 = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    hud.Show_Toast("支付宝拥有人为空");
                });

                return;
            }
            Tools.AsyncMsg am_绑定支付宝 = new Tools.AsyncMsg();

            string para = "UserGUID=" + Data.UserInfoCache.UserGUID + "&AlipayUserID=" + 支付宝账号 + "&AlipayName=" + 真名;

            am_绑定支付宝.Completion += (object obj, string ex) =>
            {

              

                Device.BeginInvokeOnMainThread(() =>
                {
                    Data.UserInfoCache.userInfo.AlipayName = 真名;

                    hud.Show_Toast("绑定成功");

                    Navigation.PopAsync(true);
                    
                    按钮防呆 = false;
                });

                am_支付宝页面.OnCompletion(null, "");
                return;
            };


            am_绑定支付宝.Cancel += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //DisplayAlert("提示", "绑定支付宝失败:" + ex.ToString(), "知道了");
                    按钮防呆 = false;
                });
                return;
            };

            Tools.NetClass.GetStringByUrl("ExSql", "App\\BindingUserAlipay_1_0_0_1", para, am_绑定支付宝);
        }
    }
}