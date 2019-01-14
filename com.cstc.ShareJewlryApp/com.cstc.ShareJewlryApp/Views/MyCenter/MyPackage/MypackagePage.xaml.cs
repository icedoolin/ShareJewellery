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
    public partial class MypackagePage: BasePage 
    {
 
 
        public MypackagePage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            //grid_head.HeightRequest = 0.32 * Tools.CommonClass.屏幕宽度 + 20;
           // 获取用户余额();
        }

        void 获取用户余额()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                lb_balance.Text = Data.UserInfoCache.userInfo.Balance.ToString();
                lb_UserCrystal.Text = "水晶 "+ Data.UserInfoCache.userInfo.UserCrystal.ToString()+" 颗";
            });
        }

        protected override void OnAppearing()
        {
            获取用户余额();
        }

        private void tapped_closePage(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                Navigation.PopAsync(true);
            }
        }


        /// <summary>
        /// 提示框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_showTips(object sender, EventArgs e)
        {
            block.IsVisible = true;
            frm_tips.IsVisible = true;
        }

        /// <summary>
        /// 关闭弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_closeBlock(object sender, EventArgs e)
        {
            block.IsVisible = false;
            frm_tips.IsVisible = false;
        }

        /// <summary>
        /// 打开提现页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_OpenDeposit(object sender, EventArgs e)
        {

            if (Helpers.MConfig.isNormalClick)
            {
                DepositPage page = new DepositPage();

                Navigation.PushAsync(page, true);

            }
          
        }

        /// <summary>
        /// 打开资金明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_openFundsDetail(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                FundsDetailPage page = new FundsDetailPage();

                Navigation.PushAsync(page, true);
            }

         
        }

        private void tapped_tips_ok(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                tapped_closeBlock(null, null);
            }
        }
    }
}