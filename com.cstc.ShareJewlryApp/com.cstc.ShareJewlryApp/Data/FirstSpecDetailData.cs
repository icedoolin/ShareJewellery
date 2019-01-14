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
    /// 主规格类
    /// </summary>
    public class FirstSpecDetailData :BindableObject
    {
        /// <summary>
        /// 主规格明细名
        /// </summary>
        public string FirstSpecDetailedName { get; set; }

        /// <summary>
        ///  主规格明细名GUID
        /// </summary>
        public string FirstSpecDetailedGUID { get; set; }


        /// <summary>
        /// 副规格明细
        /// </summary>
        ObservableCollection<SecondSpecData> _SecondSpecDetail = null;
        public ObservableCollection<SecondSpecData> SecondSpecDetail
        {
            get
            {
                if (_SecondSpecDetail == null)
                    _SecondSpecDetail = new ObservableCollection<SecondSpecData>();
                return _SecondSpecDetail;
            }
            set
            {
                _SecondSpecDetail = value;
                OnPropertyChanged("SecondSpecDetail");
            }
        }

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
