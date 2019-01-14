using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
 


namespace com.cstc.ShareJewlryApp.Views.MyCenter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuyVipPage:BasePage
    {
        bool 按钮防呆 = false;
        com.cstc.ShareJewlryApp.Tools.IWeChat WXDependency = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.IWeChat>();
        com.cstc.ShareJewlryApp.Tools.IAli aliPay = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.IAli>();
        //com.cstc.ShareJewlryApp.Tools.Ihud hud = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.Ihud>();
 

        public BuyVipPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            //getMembershipPrice();
            //VipBenefits_List.HeightRequest = ScreenWidth * 0.5314;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Data.UserInfoCache.UserGUID=="")
            {
                st_LoginBox.IsVisible = true;
            }
 
        }

        protected override bool OnBackButtonPressed()
        {
            bool re = false;
            if (Device.RuntimePlatform.ToString() == Device.Android)
            {
                if (BuyVip_PayBox.IsVisible)
                {
                    BuyVip_PayBox.IsVisible = false;
                    return true;
                }
            }
            return re;
        }
 

 
        /// <summary>
        /// 服务条款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_service(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(new Views.MyCenter.ServiceTermsPage(), true);
            });
            按钮防呆 = false;
        }


        /// <summary>
        /// 点击立即开通，弹出支付界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TapBuy_Tapped(object sender, EventArgs e)
        {
            BuyVip_PayBox.ShowMembershipPrice();
            BuyVip_PayBox.IsVisible = true;

        }

        /// <summary>
        /// 点击同意条款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TapAgreeTerm_Tapped(object sender, EventArgs e)
        {
            if (lbl_Agree.TextColor == Color.Black)
            {
                lbl_Agree.TextColor = Color.Gray;
                SelectBox.Source = "SelectBox.png";
                btn_buyvip.IsEnabled = false;
            }
            else
            {
                lbl_Agree.TextColor = Color.Black;
                SelectBox.Source = "SelectedBox.png";
                btn_buyvip.IsEnabled = true;
            }

          }
 

        /// <summary>
        /// 点击vip权益列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VipBenefits_List_SelectedTitleChanged(object sender, EventArgs e)
        {
            Helpers.AsyncMsg am = new Helpers.AsyncMsg();
            am.Completion += (object obje, string exc) =>
            {
                string returnJson = obje.ToString();
                if (returnJson == "[]" || returnJson == "")
                    return;
                try
                {
                    Data.MessageBoxElement item = new Data.MessageBoxElement();
                    List<Data.MessageBoxElement> lists= Helpers.HttpHelper.GetItemList<Data.MessageBoxElement>(returnJson);
                    item = lists[0];
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        VipBenefitsDetail_Show.ImageBackGroundSource = item.TitleImage.ToLower().TrimEnd();
                        VipBenefitsDetail_Show.Body = item.Content;
                        VipBenefitsDetail_Show.IsVisible = true;
                    });

                }
                catch (Exception exce)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "获取会员权益内容出错！" + exce.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }
            };
            //am.Cancel += (object obj, string ex) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        DisplayAlert("提示", "获取数据失败:" + ex.ToString(), "知道了");
            //    });
            //    return;
            //};
            
            //根据标题获取文本
            int id = VipBenefits_List.SelectedCellNO;
            string para = "&Type=会员权益" + "&Sequence=" + id;
            Helpers.HttpHelper.GetStringByUrl( @"APP\GetMemberAdvantagesAndEquity_1_0_0_1", para, am);


        }


        /// <summary>
        /// 付款成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuyVip_PayBox_PaySuccess(object sender, EventArgs e)
        {
            //如果购买成功，转到会员中心页面
 
            Device.BeginInvokeOnMainThread(() =>
            {
                BuyVipSuccess_Show.IsVisible = true;
                //Navigation.PopAsync();
                //Helpers.NavagationPageMgr.Open(Navigation, Helpers.MConfig.vipInfoPage);
            });
        }

        private void St_LoginBox_LoginSuccess(object sender, EventArgs e)
        {

        }

        private void St_LoginBox_LoginCancel(object sender, EventArgs e)
        {

        }

        private void BuyVipSuccess_Show_ButtonLongClicked(object sender, EventArgs e)
        {
 
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(new Views.HomePage.homePage(), true);
            });
 
        }
    }
}