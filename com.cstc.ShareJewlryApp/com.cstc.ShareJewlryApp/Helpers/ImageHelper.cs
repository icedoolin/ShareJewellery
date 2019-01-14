using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Helpers
{
    public class ImageHelper
    {

        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <returns></returns>
        public static string GetUserHeadPortrait()
        {
             return MConfig.picUrl + "Pic/yonghutouxiang/" + Data.UserInfoCache.userInfo.Photo.Substring(0, 2) + "/" + Data.UserInfoCache.userInfo.Photo;
        }
    }
}
