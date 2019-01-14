using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Tools
{

    /// <summary>
    /// 没用
    /// </summary>

    public interface ISaveImage
    {


        void SavePictureToDisk(string filename, byte[] imageData);

        
    }
}
