using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Data
{
    /// <summary>
    /// 资金明细类
    /// </summary>
     public class FundsDetailData
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; } = "";

        /// <summary>
        /// 变动金额
        /// </summary>
        public string Change { get; set; } = "";

        /// <summary>
        /// 时间
        /// </summary>
        public string CREATETIME { get; set; } = "";
    }
}
