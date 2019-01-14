using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Tools
{
    
    public interface IBarcodeService
    {
        Stream compoundPhoto(string url, string templetGuid, float CoordinateX, float CoordinateY, int Length);  //传入图片地址将其作为背景绘制二维码（生成的二维码在方法内）

        Stream ConvertImageStream(string text, int Width, int Height);  //将输入的文本转换成二维码
    }
}
