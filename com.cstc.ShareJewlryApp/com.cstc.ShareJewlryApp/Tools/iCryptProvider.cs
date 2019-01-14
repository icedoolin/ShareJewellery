using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Tools
{
    public interface iCryptProvider
    {
        string CreateMD5(string text);  //创建MD5
        string GetMD5(string encypStr);  //获取MD5
    }
}
