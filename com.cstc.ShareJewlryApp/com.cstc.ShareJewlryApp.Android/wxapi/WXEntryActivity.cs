using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Tencent.MM.Sdk.Modelbase;
using Com.Tencent.MM.Sdk.Modelmsg;
using Com.Tencent.MM.Sdk.Openapi;
using Xamarin.Forms;


namespace com.cstc.ShareJewlryApp.Droid.wxapi
{
    [Activity(Label = "WXEntryActivity", Name = "com.cstc.ShareJewlryApp.Android.wxapi.WXEntryActivity", Theme = "@android:style/Theme.Translucent", LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Exported = true)]

    public class WXEntryActivity : Activity, IWXAPIEventHandler
    {
        private IWXAPI api;
        private string WX_APP_SECRET = "3d960bc08bbe09cd0a24ac70f6d6fb93";
        private string APP_ID = "wxb86a48efa41420e8";
        System.Net.WebClient webClient = new System.Net.WebClient();

        public void OnReq(BaseReq req)
        {

        }

        public void OnResp(BaseResp resp)
        {
            try
            {
                if (resp.Type == 2)
                {
                    switch (resp.MyErrCode)
                    {
                        case BaseResp.ErrCode.ErrOk:


                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Toast.MakeText(this, "分享成功", ToastLength.Long).Show();
                            });

                            Finish();
                            break;
                        case BaseResp.ErrCode.ErrAuthDenied:
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Toast.MakeText(this, "分享失败", ToastLength.Long).Show();
                            });
                            Finish();
                            break;
                        case BaseResp.ErrCode.ErrUserCancel:
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Toast.MakeText(this, "取消分享", ToastLength.Long).Show();
                            });
                            Finish();
                            break;
                    }
                }
                else
                {
                  
                    #region 微信登录回调
                    switch (resp.MyErrCode)
                    {
                        case BaseResp.ErrCode.ErrOk:
                            string code = ((SendAuth.Resp)resp).Code;
                            string AccessToken = GetToken(code);
                            Newtonsoft.Json.Linq.JObject obj = com.cstc.ShareJewlryApp.Tools.CommonClass.LoadTryTwo(AccessToken);
                            if (!string.IsNullOrEmpty(obj.ToString()) && AccessToken.Contains("errcode"))
                            {
                                Toast.MakeText(Xamarin.Forms.Forms.Context, "登录错误:" + obj["errmsg"].ToString(), ToastLength.Short).Show();
                                Finish();
                            }
                            else
                            {
                                string access_token = obj["access_token"].ToString();
                                string openid = obj["openid"].ToString();
                                string userInfo = GetUserInfo(access_token, openid);//获取用户个人信息（微信）       
                                Newtonsoft.Json.Linq.JObject UserInfo_obj = com.cstc.ShareJewlryApp.Tools.CommonClass.LoadTryTwo(userInfo);
                                if (!string.IsNullOrEmpty(UserInfo_obj.ToString()))
                                {
                                    if (userInfo.Contains("errcode"))
                                    {
                                        Toast.MakeText(Xamarin.Forms.Forms.Context, "登录错误:" + UserInfo_obj["errmsg"].ToString(), ToastLength.Short).Show();
                                        Finish();
                                    }
                                    else
                                    {
                                      //string url = Helpers.Common.appDomain + "account/WXRegist";
                                        string OpenID = UserInfo_obj["openid"].ToString();
                                        string NickName = UserInfo_obj["nickname"].ToString();
                                        string HeadImgUrl = UserInfo_obj["headimgurl"].ToString();
                                        string Sex = UserInfo_obj["sex"].ToString();
                                        #region APP端实现注册登录过程


                                        com.cstc.ShareJewlryApp.Tools.CommonClass.微信OpenID = OpenID;
                                        com.cstc.ShareJewlryApp.Tools.CommonClass.微信昵称 = NickName;
                                        if (com.cstc.ShareJewlryApp.Tools.CommonClass.微信授权标志 == "绑定")
                                        {
                                            com.cstc.ShareJewlryApp.Tools.CommonClass.am_微信绑定.OnCompletion(null, "");
                                        }
                                        else if(com.cstc.ShareJewlryApp.Tools.CommonClass.微信授权标志 == "登录")
                                        {
                                            com.cstc.ShareJewlryApp.Tools.CommonClass.am_微信登录.OnCompletion(null, "");
                                        }
                                        else if (com.cstc.ShareJewlryApp.Tools.CommonClass.微信授权标志 == "登录_首饰盒")
                                        {
                                            com.cstc.ShareJewlryApp.Tools.CommonClass.am_微信登录_首饰盒.OnCompletion(null, "");
                                        }
                                        else if (com.cstc.ShareJewlryApp.Tools.CommonClass.微信授权标志 == "登录_商品详情")
                                        {
                                            com.cstc.ShareJewlryApp.Tools.CommonClass.am_微信登录_商品详情页.OnCompletion(null, "");
                                        }

                                        Finish();
                                        #endregion
                                    }
                                }
                                else
                                {
                                    Toast.MakeText(Xamarin.Forms.Forms.Context, "登录出错:" + UserInfo_obj["errmsg"].ToString(), ToastLength.Short).Show();
                                    Finish();
                                }
                            }
                            break;
                        case BaseResp.ErrCode.ErrAuthDenied:
                            Finish();
                            break;
                        case BaseResp.ErrCode.ErrUserCancel:
                            if (com.cstc.ShareJewlryApp.Tools.CommonClass.微信授权标志 == "绑定")
                            {
                                com.cstc.ShareJewlryApp.Tools.CommonClass.am_微信绑定.OnCancel(null, "");
                            }
                            else if (com.cstc.ShareJewlryApp.Tools.CommonClass.微信授权标志 == "登录")
                            {
                                com.cstc.ShareJewlryApp.Tools.CommonClass.am_微信登录.OnCancel(null, "");
                            }
                            else if (com.cstc.ShareJewlryApp.Tools.CommonClass.微信授权标志 == "登录_首饰盒")
                            {
                                com.cstc.ShareJewlryApp.Tools.CommonClass.am_微信登录_首饰盒.OnCancel(null, "");
                            }
                            else if (com.cstc.ShareJewlryApp.Tools.CommonClass.微信授权标志 == "登录_商品详情")
                            {
                                com.cstc.ShareJewlryApp.Tools.CommonClass.am_微信登录_商品详情页.OnCancel(null, "");
                            }
                            Finish();
                            break;
                    }
                    #endregion
                }
            }
            catch
            { }

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);//无标题栏
            api = WXAPIFactory.CreateWXAPI(this, APP_ID, false);
            api.HandleIntent(Intent, this);
            // Create your application here
        }

         protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            Intent = intent;
            api.HandleIntent(intent, this);
        }

        #region 通过code获取access_token
        /// <summary>
        /// 通过code获取access_token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string GetToken(string code)
        {
            string result = string.Empty;
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + APP_ID + "&secret=" + WX_APP_SECRET + "&code=" + code + "&grant_type=authorization_code";
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
            string result = string.Empty;
            string url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + token + "&openid=" + openid + "";
            result = webClient.DownloadString(url);
            return result;
        }
        #endregion

       
    }
}