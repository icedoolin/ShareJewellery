using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.VIPCenter
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VipInfoPage: BasePage 
	{
		public VipInfoPage ()
		{
			InitializeComponent ();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            grid_head.HeightRequest = 158 * Helpers.MConfig.screenRatios;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshVipInfo();
        }

        /// <summary>
        /// 刷新会员信息
        /// </summary>
        private void RefreshVipInfo()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                NickName.Text = Data.UserInfoCache.userInfo.Nickname;
                VipClass.Text = Data.UserInfoCache.userInfo.level;
                VipValidityDate.Text =  Data.UserInfoCache.userInfo.MemberExpiryDate + " 到期";

                if (Data.UserInfoCache.userInfo.Photo != "")
                {
                    img_myheadImg.Source = Helpers.MConfig.picUrl + "Pic/yonghutouxiang/" 
                    + Data.UserInfoCache.userInfo.Photo.Substring(0, 2) + "/" + Data.UserInfoCache.userInfo.Photo;
                }
                else
                {
                    img_myheadImg.Source = "unLogin_headImg.png";
                }
            });
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

        private void TapMessageLogo_Tapped(object sender, EventArgs e)
        {

        }

        private void tapped_ShowLoginBox(object sender, EventArgs e)
        {

        }

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
                    List<Data.MessageBoxElement> lists = Helpers.HttpHelper.GetItemList<Data.MessageBoxElement>(returnJson);
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
                        //await DisplayAlert("错误", "解析微信下单数据包错误！" + exce.Message, "知道了");
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
            Helpers.HttpHelper.GetStringByUrl(@"APP\GetMemberAdvantagesAndEquity_1_0_0_1", para ,am);

 
        }

 
        private void TapBuyMoreVip_Tapped(object sender, EventArgs e)
        {
            //充值
            BuyVip_PayBox.ShowMembershipPrice();
            BuyVip_PayBox.IsVisible = true;          
        }

        private void BuyVip_PayBox_PaySuccess(object sender, EventArgs e)
        {
            //如果购买成功了 刷新会员等级等信息
            //Data.UserinfoMgr.LoginWithoutAlert()
            RefreshVipInfo();
        }
    }


}