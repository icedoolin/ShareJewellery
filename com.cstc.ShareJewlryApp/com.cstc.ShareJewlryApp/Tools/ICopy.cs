using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Tools
{
    public interface ICopy
    {
        void SaveNetImage(byte[] imgStream); //保存网络图片到本地
        void SavePictureToDisk(string filename, byte[] imageData);   //保存图片到本地（没用到）
        void SaveTest(string test);  //复制文本
    }
}
