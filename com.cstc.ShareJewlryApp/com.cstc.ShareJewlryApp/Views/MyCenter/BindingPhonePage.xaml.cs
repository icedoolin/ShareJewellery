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
    public partial class BindingPhonePage: BasePage 
    {

 
        bool 验证码防呆 = false;
        bool 按钮防呆 = false;

        string _phone = "";
        public string Phone   //绑定手机
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
                Device.BeginInvokeOnMainThread(() =>
                {
                    ety_BindingPhone.Text = value;
                });
            }
        }



        public Tools.AsyncMsg am_绑定手机 = new Tools.AsyncMsg();

        public BindingPhonePage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

        }


        void btn_getAuthCode_clicked(object sender, EventArgs e)  //获取验证码
        {
            if (验证码防呆)
                return;

            验证码防呆 = true;

            string 电话 = ety_BindingPhone.Text.Trim();

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
                            // btn_getAuthCode.IsEnabled = true;
                            验证码防呆 = false;
                            btn_getAuthCode.TextColor = Color.Black ;
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

            am_发送验证码.Cancel += (object obje, string exc) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("提示", "发送验证码失败" , "知道了");
                    验证码防呆 = false;
                });
                return;
            };

            Tools.NetClass.创建网络Get请求(URL, am_发送验证码);


        }


        void tapped_BindingPhone(object sender, EventArgs e)  //绑定手机
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

             
            string 手机 = ety_BindingPhone.Text.Trim();

            string 验证码 = ety_DynamicCode.Text.Trim();

            if (手机 == "")
            {
                hud.Show_Toast("请输入手机号码");
                按钮防呆 = false;
                return;
            }

            if (验证码 == "")
            {
               
                按钮防呆 = false;
                return;
            }

            
            Tools.AsyncMsg am_绑定 = new Tools.AsyncMsg();

            string para = "Tel=" + 手机 + "&UserGUID=" + Data.UserInfoCache.UserGUID + "&DynamicPassword=" + 验证码;

            am_绑定.Completion += (object obj, string ex) =>
            {
                string returnJson = obj.ToString();
                string ErrMsg = "";

                Newtonsoft.Json.Linq.JArray jar = null;

                if (returnJson == "[]" || returnJson == "")
                {

                    按钮防呆 = false;

                    return;
                }

                //try
                //{
                //    returnJson = Tools.CommonClass.GetJsonByTip(returnJson, ref ErrMsg);
                //}
                //catch (Exception exc)
                //{
                //    Device.BeginInvokeOnMainThread(async () =>
                //    {
                //        //await DisplayAlert("错误", "解析用户数据错误！" + exc.Message, "知道了");
                //        // Navigation.PopAsync(true);
                //        按钮防呆 = false;
                //    });
                //    return;
                //}

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
                        hud.Show_Toast("绑定成功");
                        按钮防呆 = false;
                        //Tools.CommonClass.获取用户数据();
                        am_绑定手机.OnCompletion(null, 手机);
                        Navigation.PopAsync(true);

                    });
                }
            };

            am_绑定.Cancel += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("提示", "绑定手机失败",  "知道了");

                });
                return;
            };
 
            Tools.NetClass.GetStringByUrl("QueryData", "App\\BindingPhone_1_0_0_1", para, am_绑定);

        }

    }
}