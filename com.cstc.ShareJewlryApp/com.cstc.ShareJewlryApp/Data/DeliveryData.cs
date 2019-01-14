using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Data
{

    /// <summary>
    /// 快递类（还珠宝、报损选择快递的类型）
    /// </summary>
    public class DeliveryData
    {
        public string LogisticsCompanyGUID { get; set; } = "";
        public string LogisticsCompanyInitials { get; set; } = "";
        public string LogisticsCompanyCode { get; set; } = "";
        public string LogisticsCompanyName { get; set; }
        public bool IsLast { get; set; } = true;
    }
}
