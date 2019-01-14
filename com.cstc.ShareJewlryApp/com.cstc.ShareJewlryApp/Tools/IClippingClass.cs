using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Tools
{
    public interface IClippingClass
    {
        byte[] ResizeImage(byte[] imageData, float maxLength);   //压缩图片
    }
}
