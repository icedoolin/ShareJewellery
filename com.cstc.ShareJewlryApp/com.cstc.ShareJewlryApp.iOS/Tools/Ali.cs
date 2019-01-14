using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using AlipaySDKBinding;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(com.cstc.ShareJewlryApp.iOS.Tools.Ali))]
namespace com.cstc.ShareJewlryApp.iOS.Tools
{
    public class Ali : com.cstc.ShareJewlryApp.Tools.IAli
    {

        /// <summary>
        /// 支付报支付
        /// </summary>
        /// <param name="orderInfo">订单明细</param>
        public void AliPay(string orderInfo)
        {
            //支付宝回调实际上不走这里的CompletionBlock，而是走appdelegate里面的openurl 
            //注意scheme需要在infolist--高级，urltype那里添加软件的标识符，这里调用传入，支付成功后才能返回软件
            AlipaySDKBinding.AlipaySDK.DefaultService.PayOrder(orderInfo, "com.cstc.ShareJewelryApp", 
                new CompletionBlock(aa => { }));
        }
 
 
    }
 
}