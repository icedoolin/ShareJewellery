using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.cstc.ShareJewlryApp.Data
{
    public class PaymentWeixin
    {
        public string prepay_id { get; set; }
        public string nonceStr { get; set; }
        
    }

    public class PaymentAli
    {
        public string sign { get; set; }
    }


    public class PaymentMgr
    {

        /// <summary>
        /// 获取服务器微信支付下单信息 返回PaymentWeixin
        /// </summary>
        /// <param name="PayMoney"></param>
        /// <param name="bodydesc"></param>
        /// <param name="out_trade_no"></param>
        /// <param name="am"></param>
        public static void GetPaymentWeixin(double PayMoney, string bodydesc, string out_trade_no,  Helpers.AsyncMsg am)
        {
            Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
            Helpers.AsyncMsg am_获取数据 = new Helpers.AsyncMsg();
            Data.PaymentWeixin item = new PaymentWeixin();
            am_获取数据.Completion += (object obj, string ex) =>
            {
                string returnJson = obj.ToString();
                if (returnJson == "[]" || returnJson == "")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Show_Toast("获取信息失败！");
                    });
                    am.OnCancel();

                }
                try
                {
                    List<Data.PaymentWeixin> lists = Helpers.HttpHelper.GetItemList<Data.PaymentWeixin>(returnJson);
                    item = lists[0];
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

            //微信支付的金额需要*100
            int 金额 = Convert.ToInt32(PayMoney * 100);
            string para1 = "&device_info=" + Device.RuntimePlatform
            + "&body=" + bodydesc //"超值会员:" + vipData.detail
            + "&detail=123&attach=购买会员"
            + "&out_trade_no=" + out_trade_no.Replace("-", "")
            + "&total_fee=" + 金额;
            string weixinURL = Helpers.MConfig.weixinpayUrl + para1;
            Helpers.HttpHelper.HttpGet(weixinURL, am_获取数据);
 

        }


        /// <summary>
        /// 获取服务器支付宝支付下单信息 返回PaymentAli
        /// </summary>
        /// <param name="PayMoney"></param>
        /// <param name="bodydesc"></param>
        /// <param name="out_trade_no"></param>
        /// <param name="am"></param>
        public static void GetPaymentAli(double PayMoney, string bodydesc, string out_trade_no, Helpers.AsyncMsg am)
        {
            Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
            Helpers.AsyncMsg am_获取数据 = new Helpers.AsyncMsg();
            Data.PaymentAli item = new PaymentAli();
            am_获取数据.Completion += (object obj, string ex) =>
            {
                string returnJson = obj.ToString();
                if (returnJson == "[]" || returnJson == "")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Show_Toast("获取信息失败！");
                    });
                    am.OnCancel();

                }
                try
                {
                    List<Data.PaymentAli> lists = Helpers.HttpHelper.GetItemList<Data.PaymentAli>(returnJson);
                    item = lists[0];
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

             
            string para1 = "&total_amount=" + PayMoney 
                + "&subject=购买会员" 
                + "&body=" + bodydesc 
                + "&out_trade_no=" + out_trade_no;
            string AliPayUrl = Helpers.MConfig.alipayUrl + para1;
            Helpers.HttpHelper.HttpGet(AliPayUrl, am_获取数据);
 

        }
    }
}
