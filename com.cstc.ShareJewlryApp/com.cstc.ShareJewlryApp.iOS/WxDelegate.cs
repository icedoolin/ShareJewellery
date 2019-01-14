using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using WeChatBinding;
using BigTed;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.iOS
{
    public class WxDelegate : WXApiDelegate
    {
        //微信密钥
        private string WX_APP_SECRET = "3d960bc08bbe09cd0a24ac70f6d6fb93";
 
        public override void OnReq(BaseReq req)
        {
            //base.OnReq(req); 
        }

        public override void OnResp(BaseResp resp)
        {
 
            if (resp is PayResp)
            {
                #region 微信支付回调
                PayResp response = (PayResp)resp;
                switch (response.ErrCode)
                {
                    case 0:

                        Show_Success("支付成功");

                        //  System.Threading.Tasks.Task.Delay(1000);

                        if (com.cstc.ShareJewlryApp.Tools.CommonClass.支付页面标志 == "还珠宝")
                        {
                            com.cstc.ShareJewlryApp.Tools.CommonClass.am_还珠宝支付.OnCompletion(null, "");
                        }

                        if (com.cstc.ShareJewlryApp.Tools.CommonClass.支付页面标志 == "卖家报损赔付")
                        {
                            com.cstc.ShareJewlryApp.Tools.CommonClass.am_卖家报损支付.OnCompletion(null, "");
                        }

                        if (com.cstc.ShareJewlryApp.Tools.CommonClass.支付页面标志 == "全额赔偿")
                        {
                            com.cstc.ShareJewlryApp.Tools.CommonClass.am_全额赔偿支付.OnCompletion(null, "");
                        }

                        if (com.cstc.ShareJewlryApp.Tools.CommonClass.支付页面标志 == "购买会员")
                        {
                            com.cstc.ShareJewlryApp.Tools.CommonClass.am_微信购买会员.OnCompletion(null, "");
                        }
                        #region 作废省略
                        //string url = Helpers.Common.appDomain + "order/weixinpay";
                        //Helpers.NetHelper.Post_FormData(url, "OrderNumber=" + Models.LoginClass.payOrderNo + "&totalPay=" + Models.LoginClass.payOrderPrice + "").ContinueWith(t =>
                        //{
                        //    if (t.Exception != null)
                        //    {
                        //        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        //        {
                        //            Show_Error("修改订单状态异常" + t.Exception.Message);
                        //        });
                        //    }
                        //    else if (t.Status == TaskStatus.RanToCompletion)
                        //    {
                        //        string result = t.Result;
                        //        if (result.ToUpper().Contains("OK"))
                        //        {
                        //            if (Models.LoginClass.payOrderNo.Contains("T"))
                        //            {
                        //                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        //                {
                        //                    Show_Success("支付成功");
                        //                    com.zph.mall.Helpers.ViewHelper.ClosePage();
                        //                    Models.LoginClass.Login_isWDStore = 1;
                        //                    com.zph.mall.Views.Category.VipListPage shoping = new com.zph.mall.Views.Category.VipListPage();
                        //                    Helpers.ViewHelper.OpenPage(shoping, false, false);
                        //                });
                        //            }
                        //            else if (Models.LoginClass.payOrderNo.Contains("P"))
                        //            {
                        //                Show_Success("支付成功");
                        //                if ((Models.LoginClass.Number - 1) == 0)
                        //                {
                        //                    com.zph.mall.Helpers.ViewHelper.ClosePage();
                        //                    Views.PinTuan.PinTuanLiji pinTuanLiji = new Views.PinTuan.PinTuanLiji();
                        //                    pinTuanLiji.ProductGuid = Models.LoginClass.PintanGuid;
                        //                    pinTuanLiji.JoinNumber = 0;
                        //                    pinTuanLiji.tid = Models.LoginClass.tid;
                        //                    pinTuanLiji.State = 1;
                        //                    Helpers.ViewHelper.OpenPage(pinTuanLiji, false, false);
                        //                }
                        //                else
                        //                {
                        //                    com.zph.mall.Helpers.ViewHelper.ClosePage();
                        //                    com.zph.mall.Views.PinTuan.pintuanfengxian shoping = new com.zph.mall.Views.PinTuan.pintuanfengxian();
                        //                    Helpers.ViewHelper.OpenPage(shoping, false, false);
                        //                }
                        //            }
                        //            else
                        //            {
                        //                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        //                {
                        //                    Show_Success("支付成功");
                        //                    com.zph.mall.Helpers.ViewHelper.ClosePage();
                        //                    com.zph.mall.Views.PersonalCenter.Order.PendingShipmentPage shoping = new com.zph.mall.Views.PersonalCenter.Order.PendingShipmentPage();
                        //                    shoping.memLoginID = Models.LoginClass.Login_MemLoginID;//"ceshi-liu";           
                        //                    Helpers.ViewHelper.OpenPage(shoping, false, false);
                        //                });
                        //            }
                        //        }
                        //        else
                        //        {
                        //            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        //            {
                        //                Show_Error("支付成功，但修改订单状态失败");
                        //                com.zph.mall.Helpers.ViewHelper.ClosePage();
                        //                com.zph.mall.Views.PersonalCenter.Order.PendingShipmentPage shoping = new com.zph.mall.Views.PersonalCenter.Order.PendingShipmentPage();
                        //                shoping.memLoginID = Models.LoginClass.Login_MemLoginID;//"ceshi-liu";           
                        //                Helpers.ViewHelper.OpenPage(shoping, false, false);
                        //            });
                        //        }
                        //    }
                        //}); 
                        #endregion
                        break;
                    default:
                        //Show_Error("支付失败：" + resp.ErrCode);
                        Show_Error("支付失败" );
                        break;
                }
                #endregion
            }
            else if (resp is SendAuthResp)
            {
 
                #region 微信登陆回调
                switch (resp.ErrCode)
                {
                    case 0:

                        string code = ((WeChatBinding.SendAuthResp)resp).Code;
                        string AccessToken = GetToken(code);
                        Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.Linq.JObject.Parse(AccessToken);
                        if (!string.IsNullOrEmpty(obj.ToString()))
                        {
                            if (AccessToken.Contains("errcode"))
                            {
 
                                Show_Error("登录错误" + obj["errmsg"].ToString());
                            }
                            else
                            {                               
                                string access_token = obj["access_token"].ToString();
                                string openid = obj["openid"].ToString();
 
                                string userInfo = GetUserInfo(access_token, openid);//获取用户个人信息（微信）       
 
                                Newtonsoft.Json.Linq.JObject UserInfo_obj = Newtonsoft.Json.Linq.JObject.Parse(userInfo);
                                if (string.IsNullOrEmpty(UserInfo_obj.ToString()) && userInfo.Contains("errcode"))
                                {
 
                                    Show_Error("登录错误" + UserInfo_obj["errmsg"].ToString());
                                }
                                else
                                {
                                    string OpenID = UserInfo_obj["openid"].ToString();
                                    string NickName = UserInfo_obj["nickname"].ToString();
                                    string HeadImgUrl = UserInfo_obj["headimgurl"].ToString();
                                    string Sex = UserInfo_obj["sex"].ToString();
 
                                    com.cstc.ShareJewlryApp.Tools.CommonClass.微信OpenID = OpenID;
                                    com.cstc.ShareJewlryApp.Tools.CommonClass.微信昵称 = NickName;
                                    if (com.cstc.ShareJewlryApp.Tools.CommonClass.微信授权标志 == "绑定")
                                    {
                                        com.cstc.ShareJewlryApp.Tools.CommonClass.am_微信绑定.OnCompletion(null, "");
                                    }
                                    else if (com.cstc.ShareJewlryApp.Tools.CommonClass.微信授权标志 == "登录")
                                    {
                                        com.cstc.ShareJewlryApp.Tools.CommonClass.am_微信登录.OnCompletion(null, "");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                            {
                                Show_Error("登录错误");
                            });
                        }
                        break;
                    case -4:
                        break;
                    case -2:
                        break;
                }
                #endregion
            }
            else
            {
                #region 微信分享回调
                switch (resp.ErrCode)
                {
                    case 0:
                        Show_Success("分享成功");
                        break;
                    case -4:
                        Show_Error("分享失败");
                        break;
                    case -2:
                        Show_Success("取消分享");
                        break;
                }
                #endregion
            }
        }



        #region 通过code获取access_token
        /// <summary>
        /// 通过code获取access_token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string GetToken(string code)
        {
            System.Net.WebClient webClient = new System.Net.WebClient();
            string result = string.Empty;
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + Helpers.MConfig.APP_ID + "&secret=" + WX_APP_SECRET + "&code=" + code + "&grant_type=authorization_code";
            result = webClient.DownloadString(url);
            return result;
        }
        #endregion

        #region 通过access_token获取微信用户信息
        /// <summary>
        /// 通过access_token获取微信用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        private string GetUserInfo(string token, string openid)
        {
            System.Net.WebClient webClient = new System.Net.WebClient();
            string result = string.Empty;
            string url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + token + "&openid=" + openid + "";
            result = webClient.DownloadString(url);
            return result;
        }
        #endregion

        #region 消息提示
        /// <summary>
        /// 一个成功的图像显示一个消息，有一个明确的背景，并自动排除后2秒
        /// </summary>
        /// <param name="Message"></param>
        public void Show_Success(string Message)
        {
            BTProgressHUD.ForceiOS6LookAndFeel = true;
            BTProgressHUD.ShowSuccessWithStatus(Message, 3000);
        }
        /// <summary>
        /// 显示一个错误图像与一个模糊的背景的消息，并自动排除后2秒
        /// </summary>
        /// <param name="Message"></param>
        public void Show_Error(string Message)
        {
            BTProgressHUD.ForceiOS6LookAndFeel = true;
            BTProgressHUD.ShowErrorWithStatus(Message, 3000);
        }
        #endregion

    }
}