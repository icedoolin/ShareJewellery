using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Tools
{
    public interface IWeChat
    {
        void WXLogin();  //微信登录授权

        void WXPay(string resultPayid, string resultNoncestr, string resultSign, string timestamp);   //微信支付


        /// <summary>
        /// 微信分享好友或朋友圈   
        /// 注意，微信分享的时候图片不能大于32K，否则无法分享
        /// </summary>
        /// <param name="url"></param>
        /// <param name="webPageTitle"></param>
        /// <param name="webPageDesc"></param>
        /// <param name="tag">Chat/Friends</param>
        /// <param name="imgUrl"></param>
        void shareWebPage(string url, string webPageTitle, string webPageDesc, string tag, string imgUrl);   //微信分享
    }
}
