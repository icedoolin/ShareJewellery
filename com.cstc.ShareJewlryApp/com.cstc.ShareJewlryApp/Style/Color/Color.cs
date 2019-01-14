using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Style.Color
{
    public class Color
    {
        public static Xamarin.Forms.Color SpaceColor { get { return Xamarin.Forms.Color.FromHex("f2f2f2"); } }   //间隔颜色
        public static Xamarin.Forms.Color BackgroundColor { get { return Xamarin.Forms.Color.FromHex("fcfcfc"); } }   //背景色
        public static Xamarin.Forms.Color LineColor { get { return Xamarin.Forms.Color.FromHex("c8c8c8"); } }   //分割线颜色
        public static Xamarin.Forms.Color grayFont { get { return Xamarin.Forms.Color.FromHex("787878"); } }   //灰字的灰度
        public static Xamarin.Forms.Color redFont { get { return Xamarin.Forms.Color.FromHex("8ccecc"); } }    //蓝色字体
        public static Xamarin.Forms.Color blackFont
        {
            get { return Xamarin.Forms.Color.FromHex("3C3C3C"); }    //黑字黑度
        }
    }
}
