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
    public partial class UnderlineEty : ContentView
    {
        public UnderlineEty()
        {
            InitializeComponent();
        }


        public string etyPlaceholder
        {
            get
            {
                return ety.Placeholder;
            }

            set
            {
                ety.Placeholder = value;
            }
        }

        public Keyboard keyBorad
        {
            get
            {
                return ety.Keyboard;
            }

            set
            {
                
                ety.Keyboard = value;
            }
        }
        
        public string Text
        {
            get
            {
                return ety.Text;
            }

            set
            {
                ety.Text = value;
            }
        }

        public double TextSize
        {
            get
            {
                return ety.FontSize;
            }

            set
            {
                ety.FontSize = value;
            }
        }

        public bool IsPassWord
        {
            get
            {
                return ety.IsPassword;
            }

            set
            {
                ety.IsPassword = value;
            }
        }

        public double icoSize
        {
            get
            {
                return img.HeightRequest;
            }
            set
            {
                img.HeightRequest = value;
                img.WidthRequest = value;
            }
        }

        public ImageSource ico
        {
            get
            {
                return img.Source;
            }
            set
            {
                img.Source = value;
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

        /// <summary>
        /// 控件间隔
        /// </summary>
        public Thickness fatherPadding
        {
            set
            {
                st_father.Padding = value;
            }
        }
    }
}