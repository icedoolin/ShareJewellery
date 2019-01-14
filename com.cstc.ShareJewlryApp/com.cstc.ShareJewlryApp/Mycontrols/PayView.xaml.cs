using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Mycontrols
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PayView : ContentView
	{
		public PayView ()
		{
			InitializeComponent ();
            //IOS的按钮弧度半径为android的一半
            if (Device.RuntimePlatform == Device.iOS)
            {
                btn_Pay.CornerRadius = 10;
            }


        }
        com.cstc.ShareJewlryApp.Tools.IWeChat wxPay = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.IWeChat>();
        com.cstc.ShareJewlryApp.Tools.IAli aliPay = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.IAli>();
        com.cstc.ShareJewlryApp.Tools.Ihud hud = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.Ihud>();
        bool 按钮防呆 = false;
        bool IsUseCrystal = false;
        string PayWay = "Weixin";
        double PayMoney = 0;

        Data.VipData vipData = new Data.VipData();
 
        /// <summary>
        /// 使用水晶数量
        /// </summary>
        public int UseCrystalAmount
        {
            get
            {
                int d = 0;
                try
                {
                    d = Convert.ToInt32(txt_CrystalAmount.Text);
                }
                catch (Exception ex)
                {
                    d = 0;
                }
                return d;
            }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    txt_CrystalAmount.Text =value.ToString();
                });

            }
        }


        public delegate void myevent(object sender, EventArgs e);
        //命名委托
        public event myevent PaySuccess;
        public event myevent PayCancel;
        /// <summary>
        /// 支付成功
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPaySuccess(EventArgs e)
        {
            if (PaySuccess != null)
                PaySuccess(this, e);
        }
        /// <summary>
        /// 取消支付
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPayCancel(EventArgs e)
        {
            if (PayCancel != null)
                PayCancel(this, e);
        }
        private void TapUseCrystal_Tapped(object sender, EventArgs e)
        {
            if (!IsUseCrystal)
            {
                IsUseCrystal = true;
                img_UseCrystal.Source = "PayWaySelected.png";
                st_CrystalInput.IsEnabled = true;
                UseCrystalAmount = Convert.ToInt32(vipData.Price * Helpers.MConfig.CrystalLimit*10);
            }
            else
            {
                IsUseCrystal = false;
                img_UseCrystal.Source = "PayWayUnSelected.png";
                st_CrystalInput.IsEnabled = false;
                txt_CrystalAmount.Text = "";
                UseCrystalAmount = 0;
            }
 
        }
 
        private void TapWeixinPay_Tapped(object sender, EventArgs e)
        {
            if (PayWay == "Weixin")
            {
                return;
            }
            else
            {
                PayWay = "Weixin";
                st_Weixin.RightIco = "PayWaySelected.png";
                st_Ali.RightIco = "PayWayUnSelected.png";
            }
        }

        private void TapAliPay_Tapped(object sender, EventArgs e)
        {
            if (PayWay == "Ali")
            {
                return;
            }
            else
            {
                PayWay = "Ali";
                st_Ali.RightIco = "PayWaySelected.png";
                st_Weixin.RightIco = "PayWayUnSelected.png";
            }
        }
 
        /// <summary>
        /// 支付成功后，需要更新静态用户信息，会员信息本身会在显示时获取刷新。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PayBtn_Clicked(object sender, EventArgs e)
        {
            if (按钮防呆) return;
            按钮防呆 = true;
            //调取对应的付款
            RequestPay();

            按钮防呆 = false;
        }
 
        private void TapClose_Tapped(object sender, EventArgs e)
        {
            this.IsVisible = false;
        }


        /// <summary>
        /// 获取会员价格信息
        /// </summary>
        public void  ShowMembershipPrice()
        {
            Helpers.AsyncMsg am_获取会员价格 = new Helpers.AsyncMsg();

            am_获取会员价格.Completion += (object obj, string ex) =>
            {
                vipData = (Data.VipData)obj;
                Device.BeginInvokeOnMainThread(() =>
                {
                    lbl_TotalCrystal.Text = Data.UserInfoCache.userInfo.UserCrystal.ToString();
                    lbl_Total.Text = vipData.Price.ToString();
                    PayMoney = vipData.Price;
                });
            };

            //am_获取会员价格.Cancel += (object obj, string ex) =>
            //{
            //    return;
            //};
            Data.VipDataMgr.GetVipData(am_获取会员价格);

        }


        /// <summary>
        /// 支付
        /// </summary>
        private void RequestPay()
        {
            string 会员订单GUID = Guid.NewGuid().ToString();
            Tools.CommonClass.支付页面标志 = "购买会员";
            //支付流程：先向服务器保存订单，成功后向服务器发起支付订单（服务器会向微信创建支付订单），成功后前端向微信调起支付。
            Helpers.AsyncMsg am_提交订单 = new Helpers.AsyncMsg();
            am_提交订单.Completion += (object obj, string ex) =>
            {
                if (PayWay == "Weixin")
                {
                    Helpers.AsyncMsg am_微信支付下单 = new Helpers.AsyncMsg();
                    am_微信支付下单.Completion += (object obje, string exc) =>
                    {
                    //下单失败
                    if (obje == null)
                        {
                            OnPayCancel(new EventArgs());
                            return;
                        }
                        //支付成功后更新界面信息，此回调由微信支付成功后自动发起
                        Tools.CommonClass.am_微信购买会员.Completion += (object s, string a) =>
                         {
                             Device.BeginInvokeOnMainThread(() =>
                             {
                                 //更新用户信息
                                 Helpers.AsyncMsg am_更新用户信息 = new Helpers.AsyncMsg();
                                 am_更新用户信息.Completion += (object obss, string bbb) =>
                                 {
                                     Data.UserInfoCache.userInfo.UserCrystal = Data.UserInfoCache.userInfo.UserCrystal - UseCrystalAmount;
                                     OnPaySuccess(new EventArgs());
                                     return;
                                 };
                                 Data.UserinfoMgr.RefreshUserInfoCache("会员信息", am_更新用户信息);
                                 this.IsVisible = false;
                             });

                         };

                    //调用微信支付
                    Data.PaymentWeixin item = (Data.PaymentWeixin)obje;
                        string timestamp = Tools.CommonClass.getTimestamp();
                        string sign = Tools.CommonClass.SecSign(item.prepay_id, item.nonceStr);
                        wxPay.WXPay(item.prepay_id, item.nonceStr, sign, timestamp);
                    };

                    am_微信支付下单.Cancel += (object obje, string exc) =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            hud.Show_Toast("提交微信支付订单失败");
                        });
                        return;
                    };
                    //向服务器请求微信支付    
                    string bodydesc = "超值会员:" + vipData.detail;
                    if (Data.UserInfoCache.userInfo.Tel == "18950102821" || Data.UserInfoCache.userInfo.Tel == "18900000000")
                        PayMoney = 0.01;
                    Data.PaymentMgr.GetPaymentWeixin(PayMoney, bodydesc, 会员订单GUID, am_微信支付下单);
                }
                else if (PayWay == "Ali")
                {
                    //if (Device.RuntimePlatform == Device.iOS)
                    //{
                    //    hud.Show_Toast("正在建设中！");
                    //    return;
                    //}

                    Helpers.AsyncMsg am_支付宝支付下单 = new Helpers.AsyncMsg();
                    am_支付宝支付下单.Completion += (object obje, string exc) =>
                    {
                        //下单失败
                        if (obje == null)
                        {
                            OnPayCancel(new EventArgs());
                            return;
                        }
                        //支付成功后更新界面信息，此回调由支付宝支付成功后自动发起
                        Tools.CommonClass.am_支付宝购买会员.Completion += (object s, string a) =>
                        {

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                //更新用户信息
                                Helpers.AsyncMsg am_更新用户信息 = new Helpers.AsyncMsg();
                                am_更新用户信息.Completion += (object obss, string bbb) =>
                                 {
                                     Data.UserInfoCache.userInfo.UserCrystal = Data.UserInfoCache.userInfo.UserCrystal - UseCrystalAmount;
                                     OnPaySuccess(new EventArgs());
                                     return;
                                 };
                                Data.UserinfoMgr.RefreshUserInfoCache("会员信息", am_更新用户信息);
                                this.IsVisible = false;
                            });
                        };

                        //调用支付宝支付
                        Data.PaymentAli item = (Data.PaymentAli)obje;
                        aliPay.AliPay(item.sign);
                    };

                    am_支付宝支付下单.Cancel += (object obje, string exc) =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            hud.Show_Toast("提交支付订单失败");
                        });
                        return;
                    };
                    //向服务器请求支付    
                    string bodydesc = "超值会员:" + vipData.detail;
                    if (Data.UserInfoCache.userInfo.Tel == "18950102821" || Data.UserInfoCache.userInfo.Tel == "18900000000") 
                        PayMoney = 0.01;
                    Data.PaymentMgr.GetPaymentAli(PayMoney, bodydesc, 会员订单GUID, am_支付宝支付下单);
                }
            };

            am_提交订单.Cancel += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    hud.Show_Toast("提交支付失败");
                });
                return;
            };

            //向服务器保存订单
            string 支付方式 = "";
            if (PayWay == "Ali") 支付方式 = "支付宝";
            if (PayWay == "Weixin") 支付方式 = "微信支付";
            string para = "&MemberGUID=" + 会员订单GUID + "&UserGUID=" + Data.UserInfoCache.UserGUID
                + "&MembershipPriceGUID=" + vipData.MembershipPriceGUID
                + "&PaymentMethod=" + 支付方式 
                + " &CrystalNum=" + UseCrystalAmount;
            string sqlname = Helpers.AppApi.SetBuyVipOrder + para;
            Helpers.HttpHelper.PostDataByUrl(sqlname, "", am_提交订单);

        }

        private void Txt_CrystalAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int MaxCrystalAmount = Convert.ToInt32(vipData.Price * Helpers.MConfig.CrystalLimit * 10);
                if (MaxCrystalAmount > Data.UserInfoCache.userInfo.UserCrystal)
                {
                    MaxCrystalAmount = Data.UserInfoCache.userInfo.UserCrystal;
                }
                if (UseCrystalAmount > MaxCrystalAmount)
                {
                    UseCrystalAmount = MaxCrystalAmount;
                }
                PayMoney = vipData.Price - UseCrystalAmount / 10;
            }
            catch(Exception ex)
            {

            }
        }
    }
}