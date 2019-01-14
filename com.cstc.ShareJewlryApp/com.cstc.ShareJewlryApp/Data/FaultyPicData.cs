using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Data
{

    /// <summary>
    /// 报损单图片类
    /// </summary>
    public class FaultyPicData   
    {
        /// <summary>
        /// 报损图片类
        /// </summary>
        public string OrderGUID { get; set; } = "";
        public string FaultyPic { get; set; } = "";

        public string FaultyPicForShow
        {
            get
            {
                if (FaultyPic != "")
                {
                    return Helpers.MConfig.picUrl + "Pic/SHANGPINBAOSUN/" + FaultyPic.Substring(0, 2).ToUpper() + "/" + FaultyPic;
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
