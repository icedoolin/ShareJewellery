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
    public partial class LoginView : ContentView
    {
        public LoginView()
        {
            InitializeComponent();
            Layout_Show.BackgroundColor = Color.FromRgba(0, 0, 0, 0.5);

            Layout_Show.HeightRequest = 450 * Helpers.MConfig.screenRatios;
            Main_Layout.WidthRequest = 300 * Helpers.MConfig.screenRatios;

            Tools.CommonClass.am_微信登录.Completion += (object obj, string e) =>
            {
                // hud.Show_Toast("回调成功");
                sign = "3";
                string UserName = Tools.CommonClass.微信OpenID;
                string Password = Tools.CommonClass.微信昵称;
                Login(UserName, Password);  
                // 按钮防呆 = false;
            };
 
        }
        string sign = "1";  //登录类型  sign=1代表正常注册登登录，sign=2代表验证码注册登录，sign=3代表微信注册登录
        bool 按钮防呆 = false;
        bool 验证码防呆 = false;
        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
        Tools.IWeChat wechat = DependencyService.Get<Tools.IWeChat>();

        //声明委托事件
        public delegate void myevent(object sender, EventArgs e);
        //命名委托
        public event myevent LoginSuccess;
        public event myevent LoginCancel;
        /// <summary>
        /// 登录成功
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLoginSuccess(EventArgs e)
        {
            ety_userName.Text = "";
            ety_pwd.Text = "";
            按钮防呆 = false;
            if (LoginSuccess != null)
                LoginSuccess(this, e);
        }

        /// <summary>
        /// 取消登录
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLoginCancel(EventArgs e)
        {
            按钮防呆 = false;
            //ety_userName.Text = "";
            ety_pwd.Text = "";
            if (LoginCancel != null)
                LoginCancel(this, e);
        }



        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btn_getAuthCode_clicked(object sender, EventArgs e)
        {
            if (验证码防呆)
                return;

            验证码防呆 = true;

            string 电话 = ety_userName.Text.Trim();

            if (电话 == "")
            {
                验证码防呆 = false;
                return;
            }

            if (!Tools.CommonClass.是否手机(电话))
            {
                hud.Show_Toast("请输入正确的手机号码");
                验证码防呆 = false;
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

                    fr_getACodeBtn.TextColor = Color.Silver;
                    //fr_getACodeBtn.BackgroundColor = Color.Silver;
                });

                Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        fr_getACodeBtn.Text = i.ToString() + "s";
                        i--;
                    });

                    if (i == 0)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            fr_getACodeBtn.Text = "获取";
                            // btn_getAuthCode.IsEnabled = true;
                            验证码防呆 = false;
                            fr_getACodeBtn.TextColor = Color.Black;
                            //fr_getACodeBtn.BackgroundColor = Color.Black;
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
                    //DisplayAlert("提示", "发送验证码失败:" + exc.ToString(), "知道了");
                    验证码防呆 = false;
                });
                return;
            };

            Tools.NetClass.创建网络Get请求(URL, am_发送验证码);
        }


        /// <summary>
        /// 登录
        /// </summary>
        private void tapped_login(object sender, EventArgs e)
        {
            if (按钮防呆)
            {
                return;
            }
            按钮防呆 = true;
            string UserName = ety_userName.Text;
            string Password = ety_pwd.Text;
            if (UserName == "" || Password == "")
            {
                hud.Show_Toast("账号或密码不能为空");
                按钮防呆 = false;
                return;
            }

            if (!Tools.CommonClass.是否手机(UserName))
            {
                hud.Show_Toast("请输入正确的手机号码");
                按钮防呆 = false;
                return;
            }

            Login(UserName, Password);

        }



        /// <summary>
        /// 登录提交数据库并获取信息
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        private void Login( string UserName,string Password )
        {

            Data.UserInfoCache.UserName = UserName;
            //Data.UserInfoCache.Password = Password;
            Helpers.AsyncMsg am = new Helpers.AsyncMsg();
            am.Completion += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.IsVisible = false;
                    ety_userName.Unfocus();
                    ety_pwd.Unfocus();
                    OnLoginSuccess(new EventArgs());
                });
            };
            am.Cancel += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    按钮防呆 = false;
                    OnLoginCancel(new EventArgs());
                    //return;
                });
            };

            Data.UserinfoMgr.Login(sign, UserName, Password, am);
        }


        /// <summary>
        /// “切换登录方式”按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_changeLoginWay(object sender, EventArgs e)
        {
            #region UI呈现
            Mycontrols.MyCenterItem btn = (Mycontrols.MyCenterItem)sender;

            Device.BeginInvokeOnMainThread(() =>
            {

                if (按钮防呆)
                    return;
                按钮防呆 = true;

                ety_pwd.Text = "";

                if (btn.LeftText == "切换验证码登录")
                {
                    fr_getACodeBtn.IsVisible = true;
                    ety_pwd.IsPassWord = false;
                    ety_pwd.keyBorad = Keyboard.Telephone;
                    ety_pwd.etyPlaceholder = "输入验证码";
                    ety_pwd.ico = "login_scanf.png";
                    myitem_changeLoginWay.LeftText = "切换至密码登录";
                    myitem_changeLoginWay.LeftIco = "login_lock.png";
                    sign = "2";
                }
                else if (btn.LeftText == "切换至密码登录")
                {
                    fr_getACodeBtn.IsVisible = false;
                    ety_pwd.IsPassWord = true;
                    ety_pwd.keyBorad = Keyboard.Default;
                    ety_pwd.etyPlaceholder = "输入您的密码";
                    ety_pwd.ico = "login_lock.png";
                    myitem_changeLoginWay.LeftText = "切换验证码登录";
                    myitem_changeLoginWay.LeftIco = "login_scanf.png";
                    sign ="1";
                }

                按钮防呆 = false;
            });
            #endregion
        }
        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_WeichatLogin(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;
            Tools.CommonClass.微信授权标志 = "登录";
            sign = "3";
            wechat.WXLogin();
            按钮防呆 = false;
        }
        private void tapped_closeLoginBox(object sender, EventArgs e)
        {
            ety_userName.Unfocus();
            ety_pwd.Unfocus();
            OnLoginCancel(new EventArgs());
            this.IsVisible = false;

        }

 
    }
}