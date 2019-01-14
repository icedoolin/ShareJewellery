using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Data
{

    /// <summary>
    /// 团队类
    /// </summary>
    public class TeamData
    {
 
        public string GUID { get; set; } = "";   //被邀请人GUID
        public string Name { get; set; } = ""; // 被邀请人姓名        
        public string Photo { get; set; } = "";  // 被邀请人头像
        public string CREATETIME { get; set; } = "";
        public string SonNum { get; set; } = ""; // 直推人数
        public string Team { get; set; } = "";     // 团队人数
        public string ISMEMBER { get; set; } = "0";  // 是否是会员

        public string sonNumForShow
        {
            get
            {
                return "已邀请:" + SonNum + "人";
            }
        }

        public string teamForShow
        {
            get
            {
                return "团队:" + Team + "人";
            }
        } 

        public string PhotoForShow
        {
            get
            {
                if (Photo == "")
                {
                    return "unLogin_headImg.png";
                }
                else
                {
                    return Helpers.MConfig.picUrl + "Pic/yonghutouxiang/" + Photo.Substring(0, 2).ToUpper() + "/" + Photo;
                }

            }
        }

  

        /// <summary>
        /// 是否是会员
        /// </summary>
        public string isVip
        {
            get
            {
                if (ISMEMBER=="1")
                {
                    return "vipstar.png";
                }
                else if(ISMEMBER == "0")
                {
                    return "";
                }

                return "";
            }
        }

    }
}
