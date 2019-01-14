using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.cstc.ShareJewlryApp.Data
{
    /// <summary>
    /// 商品副规格类
    /// </summary>
    public class SecondSpecData : BindableObject
    {
        public string SecondSpecDetailedGUID { get; set; }
        public string SecondSpecDetailedName { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

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
