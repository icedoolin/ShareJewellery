using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace com.cstc.ShareJewlryApp.Mycontrols
{
    public class FontIcon : Label
    {
        public FontIcon()
        {
            FontFamily = "iconfont";  //这里填.ttf文件的文件名，且后缀（.ttf）不需要填写
        }
    }
}
