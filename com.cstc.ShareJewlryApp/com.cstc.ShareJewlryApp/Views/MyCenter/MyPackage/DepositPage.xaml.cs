using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.MyCenter.MyPackage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DepositPage: BasePage 
    {
        bool 按钮防呆 = false;
        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();

        public DepositPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            //Tools.CommonClass.获取用户数据();
            UI呈现();
        }

        void UI呈现()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                lb_balance.Text = "可提现金额 ¥" + Data.UserInfoCache.userInfo.Balance.ToString();
                if (Data.UserInfoCache.userInfo.AlipayName != "")
                {
                    lb_type.RightText = "昵称：" + Data.UserInfoCache.userInfo.AlipayName;
                    btn_BindAiPay.IsVisible = false;
                    btn_deposit.IsVisible = true;
                }
                else
                {
                    btn_BindAiPay.IsVisible = true;
                    btn_deposit.IsVisible = false;
                }

            });
        }


        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_deposit(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            decimal 金额 = 0;

            if (ety_depositePrice.Text.Trim() == "")
            {

                按钮防呆 = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("提示", "提现失败，请输入提现金额。", "知道了");
                });
                return;
            }




            try
            {
                金额 = Convert.ToDecimal(ety_depositePrice.Text);
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    hud.Show_Toast("请输入正确的金额");
                });
                按钮防呆 = false;
            }


            if (金额 == 0)
            {
                按钮防呆 = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("提示", "提现失败，只支持2位小数，小数点前最大支持13位，金额必须大于0", "知道了");
                });
                return;
            }

            if (金额 > Data.UserInfoCache.userInfo.Balance)
            {
                按钮防呆 = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    hud.Show_Toast("超过可提现金额");
                });
                return;
            }


            string URL = "https://test.gxzb168.com:8443/Plugin_Text?ClassName=Plugin_AL.Withdraw.User_DoByAlUsername&UserGUID=" + Data.UserInfoCache.UserGUID + "&Amount=" + 金额;

            Tools.AsyncMsg am_提交订单 = new Tools.AsyncMsg();

            am_提交订单.Completion += (object obje, string exc) =>
            {
                string returnJson = obje.ToString();
                string ErrMsg = "";

                Device.BeginInvokeOnMainThread(() =>
                {
                    hud.Show_Toast("提现成功");
                    Data.UserInfoCache.userInfo.Balance = Data.UserInfoCache.userInfo.Balance - 金额;
                    //Tools.CommonClass.获取用户数据();
                    Navigation.PopAsync(true);
                });


                按钮防呆 = false;
            };

            am_提交订单.Cancel += (object obje, string exc) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("提示", "提现失败"  , "知道了");
                });
                按钮防呆 = false;
                return;
            };

            Tools.NetClass.创建网络Get请求(URL, am_提交订单);

        }

        /// <summary>
        /// 全部提现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_depositAll(object sender, EventArgs e)
        {
            ety_depositePrice.Text = Data.UserInfoCache.userInfo.Balance.ToString();
        }

        /// <summary>
        /// 绑定支付宝
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_OpenBindAlipay(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            BindAliPayPage page = new BindAliPayPage();

            page.am_支付宝页面.Completion += (object obj, string NickName) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //Tools.CommonClass.获取用户数据();
                    UI呈现();
                });
            };

            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(page, true);
                按钮防呆 = false;
            });

        }
    }
}