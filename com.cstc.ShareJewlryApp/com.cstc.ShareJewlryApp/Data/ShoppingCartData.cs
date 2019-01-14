using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.cstc.ShareJewlryApp.Data
{
    public class ShoppingCartData : BindableObject
    {
        public string ShoppingCartGUID { get; set; }
        public string JewelleryGUID { get; set; }
        public string JewelleryName { get; set; }
        public int number { get; set; }
        public string ShopGUID { get; set; }
        public string ShopName { get; set; }
        public string firstSpecName { get; set; }
        public string firstSpecDetailedName { get; set; }
        public string SecondSpecName { get; set; }
        public string SecondSpecDetailedName { get; set; }
        public decimal Price { get; set; }
        public string AllowRent { get; set; }
        public string Stock { get; set; }

        bool _isManage = false;
        public bool isManage
        {
            get
            {
                return _isManage;
            }
            set
            {
                if(value)
                {
                    backColor = Color.Red;
                }

                _isManage = value;
                isShow = !value;
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
                isShow = value;
                OnPropertyChanged("isShow");
            }
           
        }

        Color _backColor = Color.Transparent;
        public Color backColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                backColor = value;
                OnPropertyChanged("backColor");
            }
        }

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
                //OnPropertyChanged("ChildrenString");
            }
        }
    }


}
