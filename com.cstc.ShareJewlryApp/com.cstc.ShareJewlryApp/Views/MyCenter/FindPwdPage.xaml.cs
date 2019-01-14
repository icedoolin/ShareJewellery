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
    public partial class FindPwdPage: BasePage 
    {
 
  
        public FindPwdPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }

        /// <summary>
        /// "下一步"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_findPWD(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                string 手机 = ety_Phone.Text.Trim();

                string 验证码 = ety_DynamicCode.Text.Trim();

                if (手机 == "")
                {
                    hud.Show_Toast("请输入手机号码");

                    return;
                }

                if (验证码 == "")
                {

                    return;
                }

                Tools.AsyncMsg am_验证手机 = new Tools.AsyncMsg();

                string para = "Tel=" + 手机 + "&DynamicPassword=" + 验证码;

                am_验证手机.Completion += (object obj, string ex) =>
                {
                    string returnJson = obj.ToString();
                    string ErrMsg = "";

                #region 转换json
                Newtonsoft.Json.Linq.JArray jar = null;

                    if (returnJson == "[]" || returnJson == "")
                    {
 
                        return;
                    }

             
                #endregion


                if (returnJson.Contains("验证码无效"))
                    {

                        hud.Show_Toast("验证码无效");
                        return;
                    }

                    if (returnJson.Contains("成功"))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
 
                        ///进入另一个页面
                        MyMsgPage page = new MyMsgPage();
                        //page.phoneNum = 手机;
                        //Helpers.NavagationPageMgr.Open(Navigation, Helpers.Settings.myCenterPage);
                        Navigation.PushAsync(page, true);

                        });
                    }

                };
 
                Tools.NetClass.GetStringByUrl("QueryData", "App\\VerificationDynamicPassword_1_0_0_1", para, am_验证手机);
            }
        }


        /// <summary>
        /// 获得验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_getAuthCode_clicked(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                string 电话 = ety_Phone.Text.Trim();

                if (电话 == "")
                {
                    hud.Show_Toast("请输入手机号码");
                    return;
                }


                string URL = "https://test.gxzb168.com:8443/Plugin_Text?ClassName=Plugin_SMS.VerificationCode&PhoneNumber=" + 电话;

                Tools.AsyncMsg am_发送验证码 = new Tools.AsyncMsg();

                am_发送验证码.Completion += (object obje, string exc) =>
                {
                #region UI呈现
                int i = 60;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                    //  btn_getAuthCode.IsEnabled = false;

                    btn_getAuthCode.TextColor = Color.Silver;
                        fr_getACodeBtn.BackgroundColor = Color.Silver;
                    });

                    Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                    {

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            btn_getAuthCode.Text = i.ToString() + "s";
                            i--;
                        });

                        if (i == 0)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                btn_getAuthCode.Text = "获取";
   
                                btn_getAuthCode.TextColor = Color.Black;
                                fr_getACodeBtn.BackgroundColor = Color.Black;
                            });
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    });
                #endregion
            };


                Tools.NetClass.创建网络Get请求(URL, am_发送验证码);
            }
        }
    }
}