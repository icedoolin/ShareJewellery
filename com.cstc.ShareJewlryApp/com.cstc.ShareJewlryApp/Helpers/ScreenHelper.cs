using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.cstc.ShareJewlryApp
{

    /// <summary>
    /// 用于计算不同手机屏幕的像素转换，达到自适应不同设备的目的
    /// 注意修改初始UI原始设计图的高度
    /// </summary>
    public class ScreenHelper
    {
        public ScreenHelper()
        {
            //计算出屏幕缩放比例，750是你的UI原始设计图的宽度
            flag = Helpers.MConfig.screenWidth/ 750.0;
            px = new PIXEL();
            m = new MARGIN();
        }
        public class PIXEL : Dictionary<string, double>
        {

            public new double this[string key]
            {
                get
                {
                    return Convert.ToDouble(key) * ScreenHelper.flag;
                }
                set
                {

                }
            }
        }
        public class MARGIN : Dictionary<string, Thickness>
        {
            public new Thickness this[string key]
            {
                get
                {
                    string[] p = key.Split('-');
                    if (p.Length == 1)
                        return new Thickness(Convert.ToDouble(p[0]) * ScreenHelper.flag);
                    return new Thickness(Convert.ToDouble(p[0]) * ScreenHelper.flag,
                        Convert.ToDouble(p[1]) * ScreenHelper.flag,
                        Convert.ToDouble(p[2]) * ScreenHelper.flag,
                        Convert.ToDouble(p[3]) * ScreenHelper.flag);
                }
                set
                {

                }
            }
        }

        public static double flag; 
        public PIXEL px
        {
            get;
        }
        public MARGIN m
        {
            get;
        }

    }

 
}
