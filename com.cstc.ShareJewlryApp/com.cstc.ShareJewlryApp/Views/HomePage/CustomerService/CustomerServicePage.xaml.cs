using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.HomePage.CustomerService
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerServicePage : BasePage
    {
        com.cstc.ShareJewlryApp.Tools.ICopy copy = DependencyService.Get<Tools.ICopy>();
 
        public string shopGUID { get; set; } = "";
        public CustomerServicePage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            gd_myCenterHomePageBg.HeightRequest = 308 * Helpers.MConfig.screenRatios;
        }

        protected override void OnAppearing()
        {
            getData();
        }

        /// <summary>
        /// 获取客服信息
        /// </summary>
        public void getData()
        {
            Helpers.AsyncMsg am_获取客服信息 = new Helpers.AsyncMsg();
            am_获取客服信息.Completion += (object obj, string e) =>
            {
                try
                {
                    var templist = (List<Data.ShopData>)obj;
                    Data.ShopData item = templist[0];
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        lb_shopName.Text = item.ShopName;
                        lb_shopAddress.LeftText = item.ShopAdrress;
                        lb_wetChat.LeftText = "客服号：" + item.ServiceWechat;
                        lb_tencent.LeftText = "客服号：" + item.ServiceQQ;
                        lb_tel.LeftText = "电话号：" + item.Tel;
                    });
                }
                catch (Exception ex)
                { }

            };

            Data.ShopDataMgr.GetShopInfo(am_获取客服信息, shopGUID);
        }


        private void tapped_closePage(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                Navigation.PopAsync(true);
            }
        }

        /// <summary>
        /// 复制微信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_copy(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                string s = lb_wetChat.LeftText;

                copy.SaveTest(s);
            }
        }

        /// <summary>
        /// 复制QQ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_copyTencent(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                string s = lb_tencent.LeftText;

                copy.SaveTest(s);
            }
        }

        /// <summary>
        /// 拨打电话
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_callTel(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                string phpne = lb_tel.LeftText;

                string telPhone = "tel:" + phpne.Replace("电话号：", "").Trim();
                Uri uri = new Uri(telPhone);
                Device.OpenUri(uri);
            }
        }
    }
}