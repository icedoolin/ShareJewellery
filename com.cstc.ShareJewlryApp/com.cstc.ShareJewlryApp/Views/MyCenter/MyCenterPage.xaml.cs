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
    public partial class MyCenterPage: BasePage 
    {

        bool 按钮防呆 = false;
        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
        int 登录标记 = 1;
        Tools.IWeChat wechat = DependencyService.Get<Tools.IWeChat>();
 
        bool 验证码防呆 = false;
        Tools.AsyncMsg am_更新完成 = new Tools.AsyncMsg();
        public MyCenterPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            gd_myCenterHomePageBg.HeightRequest = 181 * Helpers.MConfig.screenRatios;
            //img_myheadImg.HeightRequest=80  *Helpers.MConfig.screenRatios;
            //img_myheadImg.WidthRequest = 80  *Helpers.MConfig.screenRatios;
        }
 
        protected override void OnAppearing()
        {
            base.OnAppearing();
            bool iss = st_LoginBox.IsVisible;
            显示用户数据();           
        }

        public void 显示用户数据()
        {
            st_LoginBox.IsVisible = false;
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    if (Data.UserInfoCache.UserGUID == "")
                    {
                        lb_nickName.Text = "登录/注册";
                        img_myheadImg.Source = "unLogin_headImg.png";
                        //img_vip.Source = "";
                        frm_vip.IsVisible = false;
                        //lb_Vip.RightText = "";
                        lb_invite.RightText = "";
                        //lb_Check.RightText = "";
                        lb_Myorder.Text = "0";
                        lb_blance.Text = "0";
                        lb_myjewelry.Text = "0";
                        lb_team.RightText = "团队人数：0人";
                        st_LoginBox.IsVisible = true;
                        //frm_quite.IsVisible = false;
                    }
                    else
                    {
                        //st_LoginBox.IsVisible = false;
                        //frm_quite.IsVisible = true;
                        if (Data.UserInfoCache.userInfo.RemainderDays > 0)
                        {
                            //lb_Vip.RightText = "会员还有" +Data.UserInfoCache.userInfo.RemainderDays.ToString() + "天到期";
                            frm_vip.IsVisible = true;
                           // img_vip.Source = "vipstar.png";
                        }
                        else
                        {
                            //lb_Vip.RightText = "未开通会员";
                            frm_vip.IsVisible = false;
                            //img_vip.Source = "";
                        }

                        if (Data.UserInfoCache.userInfo.InvitationNumber > 0)
                        {
                            lb_invite.RightText = "已邀请注册用户" +Data.UserInfoCache.userInfo.InvitationNumber.ToString() + "人";
                        }
                        else
                        {
                            lb_invite.RightText = "未邀请过用户";
                        }

                        lb_team.RightText = "团队人数：" + Data.UserInfoCache.userInfo.Team + "人";
                        lb_Myorder.Text =Data.UserInfoCache.userInfo.UserCrystal.ToString();
                        lb_blance.Text =Data.UserInfoCache.userInfo.Balance.ToString();
                        lb_myjewelry.Text =Data.UserInfoCache.userInfo.CollectionNumber.ToString();
                        lb_nickName.Text =Data.UserInfoCache.userInfo.Nickname;
                        VipClass.Text = Data.UserInfoCache.userInfo.level;

                        img_myheadImg.Source = Data.UserInfoCache.userInfo.PhotoForShow;
                    }
                }
                catch (Exception ex)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "显示用户数据错误！" + ex.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }
            });
        }

        /// <summary>
        /// 购买会员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_BuyVip(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (按钮防呆)
                    return;
                按钮防呆 = true;


                if (Data.UserInfoCache.UserGUID == "")
                {
                    hud.Show_Toast("请先登录");
                    按钮防呆 = false;
                    return;
                }

                Navigation.PushAsync(new Views.MyCenter.BuyVipPage(), true);

                按钮防呆 = false;
            });

        }

        /// <summary>
        /// 邀请好友
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_Invite(object sender, EventArgs e)
        {
            //hud.Show_Toast("即将开通");
            //return;
            if (Data.UserInfoCache.UserGUID == "")
            {
                hud.Show_Toast("请先登录");
                按钮防呆 = false;
                return;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                if (按钮防呆)
                    return;
                按钮防呆 = true;

                

                Navigation.PushAsync(new Views.MyCenter.InvitePage(), true);

                按钮防呆 = false;

            });
        }

 

        /// <summary>
        /// 身份认证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_CheckIdentify(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;

            按钮防呆 = true;

            if (Data.UserInfoCache.UserGUID == "")
            {
                hud.Show_Toast("请先登录");
                按钮防呆 = false;
                return;
            }

            Navigation.PushAsync(new Views.MyCenter.CheckIdentify(), true);

            按钮防呆 = false;

        }

        /// <summary>
        /// 资料设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_MyMsg(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;
 
            if (Data.UserInfoCache.UserGUID == "")
            {
                
                hud.Show_Toast("请先登录");
                按钮防呆 = false;
                return;
            }

            Views.MyCenter.MyMsgPage page = new MyMsgPage();
            //page.呈现用户数据();

            page.am_更新数据.Completion += (object obj, string Phone) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    显示用户数据();
                });
            };

            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(page, true);
                按钮防呆 = false;
            });
        }

        /// <summary>
        /// 我的订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_Myorder(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (按钮防呆)
                    return;
                按钮防呆 = true;

                if (Data.UserInfoCache.UserGUID == "")
                {
                    hud.Show_Toast("请先登录");
                    按钮防呆 = false;
                    return;
                }

                Views.MyCenter.MyOrderPage page = new MyOrderPage();
                page.state = 1;
                page.getAllOrderData();

                Navigation.PushAsync(page, true);

                按钮防呆 = false;
            });
        }

        /// <summary>
        /// 显示登录弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_ShowLoginBox(object sender, EventArgs e)
        {
            if (Data.UserInfoCache.UserGUID != "")
            {
                Views.MyCenter.MyMsgPage page = new MyMsgPage();
                //page.呈现用户数据();

                page.am_更新数据.Completion += (object obj, string Phone) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        显示用户数据();
                    });
                };

                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushAsync(page, true);

                });
            }
            else
            {
                //block.IsVisible = true;
                st_LoginBox.IsVisible = true;
            }
        }


        /// <summary>
        /// 首饰盒
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_MyjewelryBox(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            if (Data.UserInfoCache.UserGUID == "")
            {
                hud.Show_Toast("请先登录");
                按钮防呆 = false;
                return;
            }

            JewlryBox.MyJewBoxPage page = new JewlryBox.MyJewBoxPage();
            page.跳转标识 = "首饰盒";
            page.GetCollectionjewelryData();

            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(page, true);
                按钮防呆 = false;
            });
        }


        /// <summary>
        /// 显示退出登录确认弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_quite(object sender, EventArgs e)
        {
            if (Data.UserInfoCache.UserGUID == "")
            {
                return;
            }

            st_checkBox.IsVisible = true;
            block.IsVisible = true;

        }

        /// <summary>
        /// 确认退出登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_yes(object sender, EventArgs e)
        {
            st_checkBox.IsVisible = false;
            block.IsVisible = false;

            hud.Show_Toast("退出成功");
            Data.UserInfoCache.UserGUID = "";
            Data.UserInfoCache.UserName = "";
            Data.UserInfoCache.Password = "";
            显示用户数据();
        }

        /// <summary>
        /// 关闭遮罩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_cancle(object sender, EventArgs e)
        {
            st_checkBox.IsVisible = false;
            block.IsVisible = false;
        }
 
    
        /// <summary>
        /// 打开商品明细
        /// </summary>
        /// <param name="jewelleryGUID"></param>
        private void ShowProductDetail(string jewelleryGUID)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Views.HomePage.ProductDetails.ProductDetailsPage page = new Views.HomePage.ProductDetails.ProductDetailsPage();
                    page.JewelleryGUID = jewelleryGUID;
                    Helpers.NavagationPageMgr.Open(Navigation, page);

                    //Navigation.PushAsync(page, true);
                });
            }
            catch (Exception ex)
            {

            }

            按钮防呆 = false;
        }
 

         
       

        /// <summary>
        /// 余额
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_OpenMyPack(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            if (Data.UserInfoCache.UserGUID == "")
            {
                hud.Show_Toast("请先登录");
                按钮防呆 = false;
                return;
            }

            Views.MyCenter.MyPackage.MypackagePage page = new MyPackage.MypackagePage();

            Navigation.PushAsync(page, true);

            按钮防呆 = false;
        }



        /// <summary>
        /// 我的团队
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_openMyTeamPage(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            if (Data.UserInfoCache.UserGUID == "")
            {
                hud.Show_Toast("请先登录");
                按钮防呆 = false;
                return;
            }

            Views.MyCenter.Myteam.MyTeamPage page = new Myteam.MyTeamPage();
            Navigation.PushAsync(page, true);

            按钮防呆 = false;
        }


        ///// <summary>
        ///// 微信登录
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void tapped_WeichatLogin(object sender, EventArgs e)
        //{
        //    if (按钮防呆)
        //        return;
        //    按钮防呆 = true;
        //    Tools.CommonClass.微信授权标志 = "登录";
        //    wechat.WXLogin();

        //    按钮防呆 = false;
        //}

        /// <summary>
        /// 绑定手机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_bindingTel(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            string 电话 = ety_tel.Text;

            string 验证码 = ety_checkCode.Text;

            if (电话 == "")
            {
                按钮防呆 = false;
                return;
            }


            if (验证码 == "")
            {
                按钮防呆 = false;
                return;
            }


            Tools.AsyncMsg am_绑定 = new Tools.AsyncMsg();

            string para = "Tel=" + 电话 + "&UserGUID=" + Data.UserInfoCache.UserGUID + "&DynamicPassword=" + 验证码;

            am_绑定.Completion += (object obj, string ex) =>
            {
                string returnJson = obj.ToString();
                string ErrMsg = "";

               // Newtonsoft.Json.Linq.JArray jar = null;

                if (returnJson == "[]" || returnJson == "")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Show_Toast("手机或验证码错误");
                        按钮防呆 = false;
                    });

                    return;
                }

             
                if (returnJson.Contains("已被注册"))
                {
                    按钮防呆 = false;
                    hud.Show_Toast("该手机号码已被注册，无法使用");
                    return;
                }

                if (returnJson.Contains("已过期") || returnJson.Contains("无效"))
                {
                    按钮防呆 = false;
                    hud.Show_Toast("验证码错误");
                    return;
                }

                if (returnJson.Contains("成功"))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        按钮防呆 = false;
                        hud.Show_Toast("绑定成功");
                        
                        //Tools.CommonClass.获取用户数据();
                        //tapped_closeLoginBox(null, null);
                    });
                }

                按钮防呆 = false;
            };

            am_绑定.Cancel += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //DisplayAlert("提示", "绑定手机失败:" + ex.ToString(), "知道了");
                    按钮防呆 = false;
                });
                return;
            };

       
            Tools.NetClass.GetStringByUrl("QueryData", "App\\BindingPhone_1_0_0_1", para, am_绑定);
        }

        private void tapped_AboutUs(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            //if (Data.UserInfoCache.UserGUID == "")
            //{
            //    hud.Show_Toast("请先登录");
            //    按钮防呆 = false;
            //    return;
            //}

            Views.AboutUsPage page = new Views.AboutUsPage();
            Navigation.PushAsync(page, true);

            按钮防呆 = false;

        }

        private void btn_getCheckCode_clicked(object sender, EventArgs e)
        {

        }

        private void tapped_closeLoginBox(object sender, EventArgs e)
        {

        }

        private void St_LoginBox_LoginSuccess(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                //block.IsVisible = false;

                st_LoginBox.IsVisible = false;
                
                显示用户数据();

                //如果有朋友分享的guid就跳转到详情页
                if (Data.UserInfoCache.userInfo.ShareJewelleryGUID.Trim() != "")
                {
                    ShowProductDetail(Data.UserInfoCache.userInfo.ShareJewelleryGUID);
                }
            });
        }

        private void St_LoginBox_LoginCancel(object sender, EventArgs e)
        {
            //block.IsVisible = false;
            //st_LoginBox.IsVisible = false;
        }
    }
}