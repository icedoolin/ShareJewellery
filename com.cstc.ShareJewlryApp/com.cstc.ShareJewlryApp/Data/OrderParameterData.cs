using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Data
{
    public class OrderParameterData
    {
        /// <summary>
        /// 订单参数类
        /// </summary>
        public string Row1 { get; set; } = "";  //列1（运单号，商品电话等）
        public string Row2 { get; set; } = "";   //列2（列1的值）
        public string OrderGUID { get; set; } = "";   //订单GUID
    }
}
