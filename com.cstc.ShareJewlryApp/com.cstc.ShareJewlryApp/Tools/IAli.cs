using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Tools
{
    //支付宝接口
    public interface IAli
    {
        void AliPay(string orderInfo);
    }
}
