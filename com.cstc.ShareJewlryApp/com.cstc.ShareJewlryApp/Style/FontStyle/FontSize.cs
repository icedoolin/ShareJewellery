using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.cstc.ShareJewlryApp.Style.FontStyle
{
    public class FontSize
    {
        public static double buyVipHeadBtnFontSize { get { return 20; } }   ////购买会员福利字体大小
        public static double BigFontSize { get { return 15; } }       ///大字体
        public static double SmallFontSize { get { return 10; } }       ///小字体
        public static double MidFontSize { get { return 12; } }       ///中字体
        public static double Fs10
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android) return 10;
                else
                    return 9;
            }
        }
        public static double Fs11
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android) return 11;
                else
                    return 10.5;
            }
        }
        public static double Fs12
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android) return 12;
                else
                    return 11.5;
            }
        }

        public static double Fs13
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android) return 13;
                else
                    return 12;
            }
        }
        public static double Fs14
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android) return 14;
                else
                    return 13;
            }
        }
        public static double Fs15
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android) return 15;
                else
                    return 14;
            }
        }

        public static double Fs16
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android) return 16;
                else
                    return 15;
            }
        }
        public static double Fs17
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android) return 17;
                else
                    return 16;
            }
        }
        public static double Fs18
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android) return 18;
                else
                    return 17;
            }
        }
        public static double Fs19
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android) return 19;
                else
                    return 18;
            }
        }
        public static double Fs20
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android) return 20;
                else
                    return 19;
            }
        }
        public static double Fs21
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android) return 21;
                else
                    return 20;
            }
        }
        public static double Fs22
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android) return 22;
                else
                    return 21;
            }
        }
        public static double Fs23
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android) return 23;
                else
                    return 22;
            }
        }
        public static double Fs24
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android) return 24;
                else
                    return 23;
            }
        }


    }
}
