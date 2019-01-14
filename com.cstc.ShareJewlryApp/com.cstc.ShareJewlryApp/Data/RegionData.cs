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
    /// 城市类
    /// </summary>
    public class RegionData :BindableObject
    {
        /// <summary>
        /// 城市名
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 首字母
        /// </summary>
        public string Initials { get; set; }

        /// <summary>
        /// 城市名
        /// </summary>
        public string CityID { get; set; }

        /// <summary>
        /// 城市名
        /// </summary>
        public string CityGUID { get; set; }

        /// <summary>
        /// 下辖城市数（默认设1为了区分不同,为了判别是否有下辖城市）
        /// </summary>
        public int SubordinateCity { get; set; } = 1;


        ObservableCollection<RegionData> _item = null;
        public ObservableCollection<RegionData> item
        {
            get
            {
                if (_item == null)
                    _item = new ObservableCollection<RegionData>();
                return _item;
            }
            set
            {
                _item = value;
                OnPropertyChanged("item");
            }
        }

        ObservableCollection<RegionData> _City = null;
        public ObservableCollection<RegionData> City
        {
            get
            {
                if (_City == null)
                    _City = new ObservableCollection<RegionData>();
                return _City;
            }
            set
            {
                _City = value;
                OnPropertyChanged("City");
            }
        }

        //ObservableCollection<RegionData> _市 = null;
        //public ObservableCollection<RegionData> 市
        //{
        //    get
        //    {
        //        if (_市 == null)
        //            _市 = new ObservableCollection<RegionData>();
        //        return _市;
        //    }
        //    set
        //    {
        //        _市 = value;
        //        OnPropertyChanged("_市");
        //    }
        //}

        //ObservableCollection<RegionData> _县 = null;
        //public ObservableCollection<RegionData> 县
        //{
        //    get
        //    {
        //        if (_县 == null)
        //            _县 = new ObservableCollection<RegionData>();
        //        return _县;
        //    }
        //    set
        //    {
        //        _县 = value;
        //        OnPropertyChanged("_县");
        //    }
        //}

        bool _Selected = false;
        public bool Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                if (_Selected == value)
                    return;

                _Selected = value;
                if (value)
                {
                    backColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                    FontColor = Color.White;
                    paddingColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                }
                else
                {
                    backColor = Color.White;
                    FontColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                    paddingColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                }
                OnPropertyChanged("Selected");
            }
        }

        Color _paddingColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
        public Color paddingColor
        {
            get
            {
                return _paddingColor;
            }

            set
            {
                _paddingColor = value;
                OnPropertyChanged("paddingColor");
            }
        }

        Color _FontColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
        public Color FontColor
        {
            get
            {
                return _FontColor;
            }

            set
            {
                _FontColor = value;
                OnPropertyChanged("FontColor");
            }
        }


        Color _backColor = Color.White;
        public Color backColor
        {
            get
            {
                return _backColor;
            }

            set
            {
                _backColor = value;
                OnPropertyChanged("backColor");
            }
        }


    }
}
