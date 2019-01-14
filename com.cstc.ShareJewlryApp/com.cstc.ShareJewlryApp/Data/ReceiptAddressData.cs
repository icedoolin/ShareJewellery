using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Data
{
    /// <summary>
    /// 收货地址类
    /// </summary>
    public class ReceiptAddressData
    {
        public string ReceivingAddressGUID { get; set; }
        public string Addressee { get; set; }
        public string AddresseeForShow
        {
            get
            {
                return "收件人： " + Addressee;
            }
        }
        public string DetailedAddress { get; set; }
        public string Tel { get; set; }
        public string TelForShow
        {
            get
            {
                return "手机： " + Tel;
            }
        }

        /// <summary>
        /// 收件城市名称（省市县）
        /// </summary>
        public string CityName { get; set; } = "";


        /// <summary>
        /// 收件城市GUID
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 收件城市GUID
        /// </summary>
        public string CityGUID { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// 默认地址
        /// </summary>
        bool _Default = false;
        public bool Default
        { get
            {
                return _Default;
            }
            set
            {
                _Default = value;
                //Current = value;
            }
        } 

        /// <summary>
        /// 当前地址
        /// </summary>
        public bool Current { get; set; } = false;

        /// <summary>
        /// 省市县+详细地址
        /// </summary>
        public string CityNameWithDetailedAddress
        {
            get
            {
                return CityName + DetailedAddress;
            }
        }


        /// <summary>
        /// 判断是都显示当前标签或者默认地址标签
        /// </summary>
        public bool isCurrenShow { get; set; } = false;

        public bool isDefaultShow { get; set; } = false;
    }
}
