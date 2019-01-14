using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using System.Text.RegularExpressions;

namespace com.cstc.ShareJewlryApp.Tools
{
  
    //通用类
    public class CommonClass
    {
        public static string 微信授权标志 { get; set; } = "";

        public static string 支付页面标志 { get; set; } = "";  //支付宝和微信通用

        public static string 微信OpenID { get; set; }

        public static string 微信昵称 { get; set; }

        public static Tools.AsyncMsg am_微信登录 = new AsyncMsg();

        public static Tools.AsyncMsg am_微信登录_首饰盒 = new AsyncMsg();

        public static Tools.AsyncMsg am_微信登录_商品详情页 = new AsyncMsg();

        public static Tools.AsyncMsg am_微信绑定 = new AsyncMsg();

        public static Tools.AsyncMsg am_还珠宝支付 = new AsyncMsg();

        public static Tools.AsyncMsg am_卖家报损支付 = new AsyncMsg();

        public static Tools.AsyncMsg am_全额赔偿支付 = new AsyncMsg();

        public static Tools.AsyncMsg am_微信购买会员 = new AsyncMsg();

        public static Tools.AsyncMsg am_支付宝购买会员 = new Tools.AsyncMsg();

        public static string APP_ID = "wxb86a48efa41420e8";

        public static string MCH_ID = "1511180761";

        public static string outkey = "gongxiangzhenpin1234567887654321";  //商户平台API密钥
 
        public static string GetJsonByTag(string tag, ref string ErrMsg)
        {
            #region 防呆
            if (tag == null || tag.Trim() == "")
            {
                return "";
            }
            #endregion

            Newtonsoft.Json.Linq.JArray jar = null;
            try
            {
                jar = Newtonsoft.Json.Linq.JArray.Parse(tag);
            }
            catch (Exception ex)
            {
                ErrMsg = "公共GetJsonByTag方法中，jarry转换错误！" + ex.Message;
                return "";
            }
            if (jar.ToString() == "[]")
            {
                ErrMsg = "公共GetJsonByTag方法中，返回值为空！";
                return "";
            }
            else
            {
                try
                {
                    return jar[0]["Table"].ToString();
                }
                catch (Exception ex)
                {
                    ErrMsg = "公共GetJsonByTag方法中，list转换错误！" + ex.Message;
                    return "";
                }
            }
        }

        public static string GetJsonByTip(string tag, ref string ErrMsg)
        {
            #region 防呆
            if (tag == null || tag.Trim() == "")
            {
                return "";
            }
            #endregion

            Newtonsoft.Json.Linq.JArray jar = null;
            try
            {
                jar = Newtonsoft.Json.Linq.JArray.Parse(tag);
            }
            catch (Exception ex)
            {
                ErrMsg = "公共GetJsonByTag方法中，jarry转换错误！" + ex.Message;
                return "";
            }
            if (jar.ToString() == "[]")
            {
                ErrMsg = "公共GetJsonByTag方法中，返回值为空！";
                return "";
            }
            else
            {
                try
                {
                    return jar[0]["Tip"].ToString();
                }
                catch (Exception ex)
                {
                    ErrMsg = "公共GetJsonByTag方法中，list转换错误！" + ex.Message;
                    return "";
                }
            }
        }

        public static bool 判断是否有特殊字符(string judgeString)
        {
            string str = "+~#%^&='\"?`|·ε￣☆╰╮▄︻┻┳═∵=≡Σつω☆.≧▽≦☆ﾉڡ◑︿•￣εಥ▕:▒▏✿ヽﾟノ๑˙❥´‸◔`乀ˉ╯‵□′︵━┻╰｜ﾟｰﾟヾ^ˉ￣・⌒┳°٩༥و＂→ヽ｀⌒´メ•ˇ‸≧≦」∠❀ツ┏┓╭•′╭☞ヾ∀Őﾉωっ´`ง⁽'-⁾";
            str = str.Replace(".", "").Replace("-", "").Replace("o", "").Replace("一", "").Replace("。", "").Replace("；", "").Replace("，", "").Replace("#", "").Replace("$", "").Replace("：", "").Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "");
            foreach (var c in str)
                if (judgeString.Contains(c.ToString()))
                    return true;
            return false;
        }

        public static void AddPostJsonString(ref string json, Dictionary<string, string> values)
        {
            if (values == null || values.Count == 0)
                return;

            string line = "";
            foreach (var value in values)
            {
                if (value.Key == null || value.Key.Trim() == "")
                    continue;

                string 键值配对 = @"""" + value.Key + @"""" + ":" + @"""" + (value.Value == null ? "" : value.Value) + @"""";
                if (line == "")
                {
                    line += 键值配对;
                }
                else
                {
                    line += "," + 键值配对;
                }
            }

            line = "{" + line + "}";

            if (json == "")
            {
                json = line;
            }
            else
            {
                json = json + "," + line;
            }
        }

        public static long currentTimeMillis()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        /// <summary>
        /// 将字符串转换成JOBJECT对象
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static JObject LoadTryTwo(string result)
        {
            try
            {
                JObject Login_Obj = Newtonsoft.Json.Linq.JObject.Parse(result);
                return Login_Obj;
            }
            catch
            {
                return new JObject();
            }
        }

        private static char[] constant =
        {
        '0','1','2','3','4','5','6','7','8','9',
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
      };

        /// <summary>
        /// 生成指定长度的随机数
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string GenerateRandomNumber(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 微信支付签名算法
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string GetSignContent(IDictionary<string, string> parameters)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();
            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append("=").Append(value).Append("&");
                }
            }
            string content = query.ToString().Substring(0, query.Length - 1);
            return content;
        }


        /// <summary>
        /// 微信支付时间戳
        /// </summary>
        /// <returns></returns>
        public static string getTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 微信支付统一下单
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="totalPrice"></param>
        /// <returns></returns>
        public static string GetXmlByWeChatPayOrder(string OrderNo,decimal totalPrice)
        {
            //统一下单
            string strNonce = GenerateRandomNumber(32);
            string strNotify = "https://test.gxzb168.com:8443/Plugin_Text/Plugin_Wx.Pay.notify_url_app";  //支付成功后回调通知的页面 
            int fee = Convert.ToInt32(totalPrice * 100);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("appid", APP_ID);
            parameters.Add("mch_id", MCH_ID);
            parameters.Add("nonce_str", strNonce);
            parameters.Add("body", "共享臻品-支付");
            parameters.Add("out_trade_no", OrderNo);
            parameters.Add("total_fee", fee.ToString());
            parameters.Add("spbill_create_ip", "127.0.0.1");
            parameters.Add("notify_url", strNotify);
            parameters.Add("trade_type", "APP");
            //将所有参数按Key字母排序
            string content = GetSignContent(parameters);
            com.cstc.ShareJewlryApp.Tools.iCryptProvider Dependency = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.iCryptProvider>();
            string signResult = Dependency.GetMD5(content + "&key=" + outkey).ToUpper();

            string xml = "<xml><appid>wxb86a48efa41420e8</appid><mch_id>1511180761</mch_id><nonce_str>" + strNonce + "</nonce_str><sign>" + signResult + "</sign><body>共享臻品-支付</body><out_trade_no>" + OrderNo + "</out_trade_no><total_fee>" + fee.ToString() + "</total_fee><spbill_create_ip>127.0.0.1</spbill_create_ip><notify_url>" + strNotify + "</notify_url><trade_type>APP</trade_type></xml>";
            return xml;
        }

        /// <summary>
        /// 微信支付二次签名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SecSign(string str)
        {
            var res = XDocument.Parse(str);
            string prePayid = res.Element("xml").Element("prepay_id").Value;
            string preNonce = res.Element("xml").Element("nonce_str").Value;
            string timestamp =getTimestamp();
            //二次签名
            Dictionary<string, string> parameters1 = new Dictionary<string, string>();
            parameters1.Add("appid", APP_ID);
            parameters1.Add("partnerid", MCH_ID);
            parameters1.Add("prepayid", prePayid);
            parameters1.Add("noncestr", preNonce);
            parameters1.Add("timestamp", timestamp);
            parameters1.Add("package", "Sign=WXPay");
            string content = GetSignContent(parameters1);
            com.cstc.ShareJewlryApp.Tools.iCryptProvider Dependency = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.iCryptProvider>();
            string signResult = Dependency.GetMD5(content + "&key=" + outkey).ToUpper();
            return signResult;
        }

        /// <summary>
        /// 微信支付二次签名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SecSign(string prePayid,string preNonce)
        {
            string timestamp = getTimestamp();
            //二次签名
            Dictionary<string, string> parameters1 = new Dictionary<string, string>();
            parameters1.Add("appid", APP_ID);
            parameters1.Add("partnerid", MCH_ID);
            parameters1.Add("prepayid", prePayid);
            parameters1.Add("noncestr", preNonce);
            parameters1.Add("timestamp", timestamp);
            parameters1.Add("package", "Sign=WXPay");
            string content = GetSignContent(parameters1);
            com.cstc.ShareJewlryApp.Tools.iCryptProvider Dependency = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.iCryptProvider>();
            string signResult = Dependency.GetMD5(content + "&key=" + outkey).ToUpper();
            return signResult;
        }
 
        public static bool 是否手机(string mobile)
        {

           return   System.Text.RegularExpressions.Regex.IsMatch(mobile, @"^[1]+[3,4,5,7,8,9]+\d{9}");
        }

       
    }
}
