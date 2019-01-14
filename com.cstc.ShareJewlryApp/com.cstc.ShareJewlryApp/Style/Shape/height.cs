using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Style.Shape
{
    public class height
    {
        public static double barLenth { get { return 40; } }   // 栏高度
        public static double MycenterbarLenth { get { return 50; } }  //“我的” 设置项高度
        public static double afterSaleImgHetght { get { return (Helpers.MConfig.screenWidth - 60) / 5; } } //申请售后图片高度
    }
}
