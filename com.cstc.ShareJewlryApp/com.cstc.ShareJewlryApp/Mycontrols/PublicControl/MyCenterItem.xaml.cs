using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Mycontrols
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyCenterItem : ContentView
    {
        public MyCenterItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 左边图片的尺寸
        /// </summary>
        double _LeftImgSize = 10;
        public double LeftImgSize
        {
            get
            {
                return _LeftImgSize;
            }

            set
            {
                if (value > 0)
                {
                    _LeftImgSize = value;
                }
                else
                {
                    img_left.Source = "";
                }

                img_left.HeightRequest = _LeftImgSize;
                img_left.WidthRequest = _LeftImgSize;
            }
        }

        /// <summary>
        /// 左边图片
        /// </summary>
        public ImageSource LeftIco
        {
            get
            {
                return img_left.Source;
            }
            set
            {
                img_left.Source = value;
                OnPropertyChanged("LeftIco");
            }
        }

        

        /// <summary>
        /// 右边图片的尺寸
        /// </summary>
        double _RightImgSize = 10;
        public double RightImgSize
        {
            get
            {
                return _RightImgSize;
            }

            set
            {
                if (value > 0)
                {
                    _RightImgSize = value;
                }
                else
                {
                    img_right.Source = "";
                }

                img_right.HeightRequest = _RightImgSize;
                img_right.WidthRequest = _RightImgSize;
            }
        }

        /// <summary>
        /// 右边图片
        /// </summary>
        public ImageSource RightIco
        {
            get
            {
                return img_right.Source;
            }
            set
            {
                img_right.Source = value;
                OnPropertyChanged("RightIco");
            }
        }

        /// <summary>
        /// 左边TXT
        /// </summary>
        public string LeftText
        {
            get
            {
                return lb_left.Text;
            }
            set
            {
                lb_left.Text = value;
            }
        }

        /// <summary>
        /// 左边TEXT字体样子
        /// </summary>
        public FontAttributes LeftTextAttributes
        {
            get
            {
                return lb_left.FontAttributes;
            }
            set
            {
                lb_left.FontAttributes = value;
            }
        }

        /// <summary>
        /// 左边字体颜色
        /// </summary>
        Color _LeftTextColor = Color.Black;
        public Color LeftTextColor
        {
            set
            {
                lb_left.TextColor = value;
            }
        }

        /// <summary>
        /// 左边字体大小
        /// </summary>
        public double LeftTextSize
        {
            set
            {
                lb_left.FontSize = value;
            }
        }

        /// <summary>
        /// 左边字体颜色
        /// </summary>
        Color _RightTextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
        public Color RightTextColor
        {
            set
            {
                lb_left.TextColor = value;
            }
        }


        /// <summary>
        /// 右边字体大小
        /// </summary>
        public double RightTextSize
        {
            set
            {
                if(value ==0)
                {
                    lb_right.IsVisible = false;
                }
                else
                {
                    lb_right.FontSize = value;
                }

              
            }
        }

        /// <summary>
        /// 控件PADDING
        /// </summary>
        public Thickness arroundPadding
        {
            set
            {
                st_father.Padding = value;
            }
        }
        /// <summary>
        /// 左边图片显示
        /// </summary>
        public bool LeftImgShow
        {
            set
            {
                if (value == false)
                    img_left.IsVisible = value;
            }
        }


        /// <summary>
        /// 右边图片显示
        /// </summary>
        public bool RightImgShow
        {
            set
            {
                if (value == false)
                    img_right.IsVisible = value;
            }
        }

        /// <summary>
        /// 右边TXT
        /// </summary>
        public string RightText
        {
            get
            {
                return lb_right.Text;
            }
            set
            {
                lb_right.Text = value;
              
            }

            
        }
        /// <summary>
        /// 控件间隔
        /// </summary>
        public double fatherSpacing
        {
            set
            {
                st_father.Spacing = value;
            }
        }
    }
}