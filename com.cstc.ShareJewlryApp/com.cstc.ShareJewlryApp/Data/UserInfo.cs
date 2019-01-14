using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;

namespace com.cstc.ShareJewlryApp.Data
{
    public static class UserInfoCache
    {

        //使用isetting接口可以将数据缓存到手机，即使程序退出仍然可以使用，相当于pc的ini文件
        //应当是个singleton类
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }
        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;


        public static int OpenCount //软件打开次数
        {
            get
            {
                //string s = AppSettings.GetValueOrDefault("UserGUID", SettingsDefault);
                return AppSettings.GetValueOrDefault("OpenCount", 0);
            }
            set
            {
                AppSettings.AddOrUpdateValue("OpenCount", value);
            }
        }

        public static string UserGUID //用户GUID
        {
            get
            {
                //string s = AppSettings.GetValueOrDefault("UserGUID", SettingsDefault);
                return AppSettings.GetValueOrDefault("UserGUID", SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue("UserGUID", value);
            }
        }
        public static string UserName
        {
            get
            {
                return AppSettings.GetValueOrDefault("UserName", SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue("UserName", value);
            }
        }

        public static string Password
        {
            get
            {
                return AppSettings.GetValueOrDefault("Password", SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue("Password", value);
            }
        }

        public static string SearchHistory1
        {
            get
            {
                return AppSettings.GetValueOrDefault("SearchHistory1", SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue("SearchHistory1", value);
            }
        }

        public static string SearchHistory2
        {
            get
            {
                return AppSettings.GetValueOrDefault("SearchHistory2", SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue("SearchHistory2", value);
            }
        }

        public static string SearchHistory3
        {
            get
            {
                return AppSettings.GetValueOrDefault("SearchHistory3", SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue("SearchHistory3", value);
            }
        }
        public static string Logs
        {
            get
            {
                return AppSettings.GetValueOrDefault("Logs", SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue("Logs", value);
            }
        }
        public static void 日志(string msg)
        {
            if (Logs.Length > 300000)
            {
                Logs.Substring(0, 200000);
            }

            Logs = System.DateTime.Now.ToString("HH:mm:ss,fff") + ">>> " + msg + "\r\n" + Logs;
        }
        public static UserInfo userInfo { get; set; } = new UserInfo();
    }
    public class UserInfo:BindableObject
    {
        public string UserGUID { get; set; } = "";
        public string Nickname { get; set; } = "";    //昵称
        public string Password { get; set; } = "";
        public string Photo { get; set; } = "";  // /头像
        public string RealName { get; set; } = "";  // 真实姓名
        public string IDCardNo { get; set; } = "";  // 身份证
        public string Tel { get; set; } = "";          //绑定电话    
        public string WeChatOpenId { get; set; } = "";      //微信OpenId
        public string WeChatName { get; set; } = "";     //微信支付昵称
        public string AlipayUserID { get; set; } = "";    //支付宝用户ID
        public string AlipayName { get; set; } = "";   //支付宝昵称
        public string ShareJewelleryGUID { get; set; } = "";    //朋友分享的商品guid
        public int RemainderDays { get; set; } = 0; //会员剩余日期（大于0表示开通了会员）
        public int RealNameAuthentication { get; set; } = 0; //实名认证与否（1：认证，0：没有认证2:认证中,3:认证失败）
        public string Arrears { get; set; } = "";    //欠费
        public  int Team { get; set; } = 0; // 团队人数
        public string AllowRent { get; set; } = ""; //是否允许租借（冻结/激活）
        public string LockReasons { get; set; } = "";       //冻结理由
        public  int OrderNumber { get; set; } = 0; //历史订单数
        public  decimal Balance { get; set; } = 0;  //余额
        public  int CollectionNumber { get; set; } = 0;  //首饰盒珠宝数量
        public  int InvitationNumber { get; set; } = 0; //邀请好友人数

        public  string platformHotKeyword { get; set; } = ""; //平台搜索关键词
        public int UserCrystal { get; set; } = 0; // 水晶
        public decimal Reward { get; set; } = 0;  //邀请收益
        public string IDCardPhoto1 { get; set; } = "";//身份证照片正面
        public string IDCardPhoto2 { get; set; } = "";//身份证照片背面
        public string IDCardPhoto3 { get; set; } = "";//身份证照片正面
        public string level { get; set; } = "注册用户";
        public string MemberExpiryDate { get; set; } = "2018-01-01";
        public string FatherNickName { get; set; } = "";   //邀请人昵称
        public int NoticeNumber { get; set; } = 0;    //新消息数量
        public string AndroidVersion { get; set; } = "";    //软件版本号
        public string IOSVersion { get; set; } = "";    //软件版本号
        public string PhotoForShow
        {
            get
            {
                if (Data.UserInfoCache.userInfo.Photo != "")
                {
                    return  Helpers.MConfig.picUrl + "Pic/yonghutouxiang/" + Data.UserInfoCache.userInfo.Photo.Substring(0, 2) + "/" + Data.UserInfoCache.userInfo.Photo;
                }
                else
                {
                    return "unLogin_headImg.png";
                }
            }
        }

    }



    public class UserinfoMgr
    {

        /// <summary>
        /// 登录获取用户信息
        /// </summary>
        /// <param name="Sign"></param>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="am"></param>
        public static void Login(string Sign , string UserName , string Password ,Helpers.AsyncMsg am)
        {
            Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
            Data.UserInfoCache.UserName = UserName;
            //Data.UserInfoCache.Password = Password;
            Helpers.AsyncMsg am_获取用户数据 = new Helpers.AsyncMsg();
            am_获取用户数据.Completion += (object obj, string ex) =>
            {
                string returnJson = obj.ToString();
                if (returnJson == "[]" || returnJson == "")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Show_Toast("密码或验证码错误");
                    });
                    ClearUserInfoCache();

                    am.OnCancel();
                    return;
                }
                try
                {
                    List<Data.UserInfo> lists = Helpers.HttpHelper.GetItemList<Data.UserInfo>(returnJson);
                    Data.UserInfoCache.userInfo = lists[0];
                    Data.UserInfoCache.UserGUID = Data.UserInfoCache.userInfo.UserGUID;
                    Data.UserInfoCache.Password = Data.UserInfoCache.userInfo.Password;
                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Show_Toast("密码或验证码错误");
                    });
                    am.OnCancel();
                }
                am.OnCompletion();
            };

            am_获取用户数据.Cancel += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    hud.Show_Toast("密码或验证码错误");
                });
                am.OnCancel();
            };

            string para = "&Sign=" + Sign + "&UserName=" + UserName + "&Password=" + Password;
            Helpers.HttpHelper.GetStringByUrl(Helpers.AppApi.LoginAndGetUserInfo, para, am_获取用户数据);
 
        }

        /// <summary>
        /// 登录获取用户信息
        /// </summary>
        /// <param name="Sign"></param>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="am"></param>
        public static void LoginWithoutAlert(string Sign, string UserName, string Password, Helpers.AsyncMsg am)
        {
 
            Data.UserInfoCache.UserName = UserName;
            //Data.UserInfoCache.Password = Password;
            Helpers.AsyncMsg am_获取用户数据 = new Helpers.AsyncMsg();
            am_获取用户数据.Completion += (object obj, string ex) =>
            {
                string returnJson = obj.ToString();
                if (returnJson == "[]" || returnJson == "")
                {
                    ClearUserInfoCache();

                    am.OnCancel();
                    return;
                }
                try
                {
                    List<Data.UserInfo> lists = Helpers.HttpHelper.GetItemList<Data.UserInfo>(returnJson);
                    Data.UserInfoCache.userInfo = lists[0];
                    Data.UserInfoCache.UserGUID = Data.UserInfoCache.userInfo.UserGUID;
                    Data.UserInfoCache.Password = Data.UserInfoCache.userInfo.Password;
                }
                catch (Exception exc)
                {
                    am.OnCancel();
                }
                am.OnCompletion();
            };

            am_获取用户数据.Cancel += (object obj, string ex) =>
            {
                am.OnCancel();
            };

            string para = "&Sign=" + Sign + "&UserName=" + UserName + "&Password=" + Password;
            Helpers.HttpHelper.GetStringByUrl(Helpers.AppApi.LoginAndGetUserInfo, para, am_获取用户数据);

        }

        /// <summary>
        /// 根据类型获取用户信息
        /// </summary>
        /// <param name="Sign">用户信息类型</param>
        public static void RefreshUserInfoCache(string Sign , Helpers.AsyncMsg am)
        {
            Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
            Helpers.AsyncMsg am_获取数据 = new Helpers.AsyncMsg();
            Data.UserInfo item = new UserInfo();
            am_获取数据.Completion += (object obj, string ex) =>
            {
                string returnJson = obj.ToString();
                if (returnJson == "[]" || returnJson == "")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Show_Toast("获取会员价格失败！");
                    });
                    am.OnCancel();

                }
                try
                {
                    List<Data.UserInfo> lists = Helpers.HttpHelper.GetItemList<Data.UserInfo>(returnJson);
                    item = lists[0];
                    if (Sign == "会员信息")
                    {
                        UserInfoCache.userInfo.MemberExpiryDate = item.MemberExpiryDate;
                        UserInfoCache.userInfo.RemainderDays = item.RemainderDays;
                        UserInfoCache.userInfo.AllowRent = item.AllowRent ;
                        UserInfoCache.userInfo.LockReasons = item.LockReasons;
                    }
                    if (Sign == "未读公告")
                    {
                        UserInfoCache.userInfo.NoticeNumber = item.NoticeNumber;
                    }
                    if (Sign == "微信绑定")
                    {
                        UserInfoCache.userInfo.WeChatName = item.WeChatName;
                    }
                    if (Sign == "获取头像")
                    {
                        UserInfoCache.userInfo.Photo = item.Photo;
                    }
                    if(Sign =="个人信息")
                    {
                        UserInfoCache.userInfo.CollectionNumber = item.CollectionNumber;
                    }

                }
                catch (Exception exc)
                {
                    am.OnCancel();
                    return;
                }
                am.OnCompletion(item, "");
            };

            am_获取数据.Cancel += (object obj, string ex) =>
            {
                am.OnCancel();
            };
            string para = "&UserGUID=" + UserInfoCache.UserGUID + "&Sign=" +Sign;            
            Helpers.HttpHelper.GetStringByUrl(Helpers.AppApi.GetUserInfo, para, am_获取数据);
        }

        /// <summary>
        /// 清理手机缓存中的用户信息，通常用于退出登录或者登录失败
        /// </summary>
        public static void ClearUserInfoCache()
        {
            Data.UserInfoCache.UserGUID = "";
            Data.UserInfoCache.UserName = "";
            Data.UserInfoCache.Password = "";
            Data.UserInfoCache.userInfo = new Data.UserInfo();
        }
    }
}
