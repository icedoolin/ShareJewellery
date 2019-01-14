using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.cstc.ShareJewlryApp.Data
{
    /// <summary>
    /// 售后单类
    /// </summary>
    public class AfterSaleOrderData  : BindableObject
    {
        /// <summary>
        /// 商铺名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 商铺名称
        /// </summary>
        public string ShopGUID { get; set; }

        /// <summary>
        /// 售后订单类别
        /// </summary>
        public string AfterOrderType { get; set; }

        /// <summary>
        /// 售后理由
        /// </summary> 
        public string Reason { get; set; }

        /// <summary>
        /// 结果（同意售后，拒绝售后）
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderType { get; set; }


        /// <summary>
        /// 商家回复
        /// </summary>
        public string Reply { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderState { get; set; } = "";

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 售后单GUID
        /// </summary>
        public string AfterOrderGUID { get; set; }

        /// <summary>
        /// 商品实例
        /// </summary>
        ObservableCollection<commodityData> _AfterOrderDetailed = null;
        public ObservableCollection<commodityData> AfterOrderDetailed
        {
            get
            {
                if (_AfterOrderDetailed == null)
                    _AfterOrderDetailed = new ObservableCollection<commodityData>();
                return _AfterOrderDetailed;
            }
            set
            {
                _AfterOrderDetailed = value;
                OnPropertyChanged("AfterOrderDetailed"); 
            }
        }

        ObservableCollection<AfterSalePicData> _AfterOrderPic = null;
        public ObservableCollection<AfterSalePicData> AfterOrderPic
        {
            get
            {
                if (_AfterOrderPic == null)
                    _AfterOrderPic = new ObservableCollection<AfterSalePicData>();
                return _AfterOrderPic;
            }
            set
            {
                _AfterOrderPic = value;
                OnPropertyChanged("AfterOrderPic");
            }
        }

    }
}
