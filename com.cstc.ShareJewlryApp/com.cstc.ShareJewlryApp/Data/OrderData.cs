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
    /// 订单类
    /// </summary>
    public class OrderData : BindableObject
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 订单GUID
        /// </summary>
        public string OrderGUID { get; set; }

        /// <summary>
        /// 购物车GUID
        /// </summary>
        public string ShoppingCartGUID { get; set; }

        /// <summary>
        /// 商铺GUID
        /// </summary>
        public string ShopGUID { get; set; }
        /// <summary>
        /// 商铺名
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 商铺LOGO
        /// </summary>
        public string ShopLogoForImg { get; set; }

        /// <summary>
        /// 订单类型  购买/免费戴
        /// </summary>
        public string OrderState { get; set; }

        /// <summary>
        /// 售后单GUID
        /// </summary>
        public string AfterOrderGUID { get; set; }

        /// <summary>
        /// 商家电话
        /// </summary>
        public string ShopTel { get; set; }


        /// <summary>
        /// 物流单号
        /// </summary>
        public string LogisticsNumber { get; set; }

        /// <summary>
        /// 物流公司编码
        /// </summary>
        public string LogisticsCompanyCode { get; set; }

        /// <summary>
        /// 物流公司名称
        /// </summary>
        public string LogisticsCompanyName { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public string CREATETIME { get; set; }

        /// <summary>
        /// 报损价格
        /// </summary>

        public string FaultyPrice { get; set; } = "";

        /// <summary>
        /// 报损单GUID
        /// </summary>
        public string FaultyGUID { get; set; } = "";

        /// <summary>
        /// 单条订单总价格
        /// </summary>

        decimal _TotalPrice = 0;
        public decimal TotalPrice
        {
            get
            {
                decimal p = 0;
                foreach (var temp in CommodityChildrenRows)
                {
                    p = p + temp.Price * temp.number;
                }

                return p;
            }

            set
            {
                decimal p = 0;
                foreach (var temp in CommodityChildrenRows)
                {
                    p = p + temp.Price * temp.number;
                }

                _TotalPrice = p;
                OnPropertyChanged("TotalPrice");
                OnPropertyChanged("TotalPriceForShow");
            }

        }

        /// <summary>
        /// 金额(list显示专用)
        /// </summary>
        public string TotalPriceForShow
        {
            get
            {
                return "¥ " + TotalPrice.ToString();
            }
        }

        /// <summary>
        /// 订单收件人
        /// </summary>
        public string Addressee { get; set; } = "";

        /// <summary>
        /// 订单联系电话
        /// </summary>
        public string Tel { get; set; } = "";

        /// <summary>
        /// 订单收件地址
        /// </summary>
        public string DetailedAddress { get; set; } = "";

        /// <summary>
        /// 订单状态
        /// </summary>
        string _OrderType = "";
        public string OrderType
        {
            get
            {
                return _OrderType;
            }
            set
            {
                _OrderType = value;
                //if(value =="归还中")
                //{

                //}
            }

        }
        /// <summary>
        /// 免费戴历史记录 右边商品的订单状态
        /// </summary>
        public string rightOrderType { get; set; } = "";



        /// <summary>
        ///"管理"
        /// </summary>
        bool _isManage = false;
        public bool isManage
        {
            get
            {
                return _isManage;
            }
            set
            {
                _isManage = value;
                isShow = !_isManage;
                OnPropertyChanged("isManage");
            }
        }

        bool _isShow = true;
        public bool isShow
        {
            get
            {
                return _isShow;
            }
            set
            {
                _isShow = value;
                OnPropertyChanged("isShow");
            }
        }

        /// <summary>
        /// 是否被选中
        /// </summary>
        bool _isSelected = false;
        public bool isSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                if (value)
                {
                    selectImg = "chosed.png";
                }
                else
                {
                    selectImg = "unchose.png";
                }
            }
        }

        ImageSource _selectImg = "unchose.png";
        public ImageSource selectImg
        {
            get
            {
                return _selectImg;
            }
            set
            {
                _selectImg = value;
                OnPropertyChanged("selectImg");
            }
        }

        public double RowsHeight
        {
            get
            {
                return CommodityChildrenRows.Count * 110;
            }
        }


        /// <summary>
        /// 商品实例
        /// </summary>
        ObservableCollection<commodityData> _CommodityChildrenRows = null;
        public ObservableCollection<commodityData> CommodityChildrenRows
        {
            get
            {
                if (_CommodityChildrenRows == null)
                    _CommodityChildrenRows = new ObservableCollection<commodityData>();
                return _CommodityChildrenRows;
            }
            set
            {
                _CommodityChildrenRows = value;
                OnPropertyChanged("CommodityChildrenRows");
                OnPropertyChanged("TotalPrice");
                OnPropertyChanged("TotalPriceForShow");
            }
        }

        /// <summary>
        /// 商品实例  免费戴历史记录右边商品
        /// </summary>
        ObservableCollection<commodityData> _rightCommodityChildrenRows = null;
        public ObservableCollection<commodityData> rightCommodityChildrenRows
        {
            get
            {
                if (_rightCommodityChildrenRows == null)
                    _rightCommodityChildrenRows = new ObservableCollection<commodityData>();
                return _rightCommodityChildrenRows;
            }
            set
            {
                _rightCommodityChildrenRows = value;
                OnPropertyChanged("rightCommodityChildrenRows");

            }
        }


        /// <summary>
        /// 订单参数类
        /// </summary>
        ObservableCollection<OrderParameterData> _OrderParameter = null;
        public ObservableCollection<OrderParameterData> OrderParameter
        {
            get
            {
                if (_OrderParameter == null)
                    _OrderParameter = new ObservableCollection<OrderParameterData>();
                return _OrderParameter;
            }
            set
            {
                _OrderParameter = value;
                OnPropertyChanged("OrderParameter");
            }
        }

        /// <summary>
        /// 报损图片类
        /// </summary>
        ObservableCollection<FaultyPicData> _FaultyPic = null;
        public ObservableCollection<FaultyPicData> FaultyPic
        {
            get
            {
                if (_FaultyPic == null)
                    _FaultyPic = new ObservableCollection<FaultyPicData>();
                return _FaultyPic;
            }
            set
            {
                _FaultyPic = value;
                OnPropertyChanged("FaultyPic");
            }
        }


        /// <summary>
        /// 免费戴历史记录珠宝名
        /// </summary>
        public string JewelleryNameForFreeDress
        {
            get
            {
                if (CommodityChildrenRows.Count > 0)
                {
                    return CommodityChildrenRows[0].JewelleryName;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 免费戴历史记录 右边商品 珠宝名 
        /// </summary>
        public string rightJewelleryNameForFreeDress
        {
            get
            {
                if (rightCommodityChildrenRows.Count > 0)
                {
                    return rightCommodityChildrenRows[0].JewelleryName;
                }
                else
                {
                    return "";
                }
            }
        }


        /// <summary>
        /// 历史记录 价格
        /// </summary>
        public string PriceForFreeDress
        {
            get
            {
                if (CommodityChildrenRows.Count > 0)
                {
                    return CommodityChildrenRows[0].PriceForShow;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 历史记录 图片
        /// </summary>
        public string JewelleryPicForFreeDress
        {
            get
            {
                if (CommodityChildrenRows.Count > 0)
                {
                    return CommodityChildrenRows[0].JewelleryPicNameForImg;
                }
                else
                {
                    return "";
                }
            }
        }


        /// <summary>
        /// 历史记录 珠宝图片图片  右边商品 
        /// </summary>
        public string rightJewelleryPicForFreeDress
        {
            get
            {
                if (rightCommodityChildrenRows.Count > 0)
                {
                    return rightCommodityChildrenRows[0].JewelleryPicNameForImg;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 历史记录 珠宝图片 宽度
        /// </summary>
        public double commodityWith { get; set; } = 0;

        /// <summary>
        /// 历史记录 珠宝图片 高度
        /// </summary>
        public double commodityHeight { get; set; } = 0;

        /// <summary>
        /// 历史记录 右边珠宝是否显示
        /// </summary>
        public bool rightIsShow { get; set; } = false;
    }
}
