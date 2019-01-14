using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Data
{
    /// <summary>
    /// 身份认证类
    /// </summary>
    public class IDCardData
    {
   

        /// <summary>
        /// 身份证图片GUID
        /// </summary>
        public string IDCardGUID { get; set; } = "";

        /// <summary>
        /// 身份证照片名称
        /// </summary>
        public string IDCardPhoto { get; set; } = "";

        /// <summary>
        /// 审核状态（已审核/被驳回）
        /// </summary>
        public string CHECKIS { get; set; } = "";

        /// <summary>
        /// 备注（正面，反面，手持）
        /// </summary>
        public string Remarks { get; set; } = "";

        public string IDCardPhotoForShow
        {
            get
            {
                return Helpers.MConfig.picUrl + "Pic/shenfenzheng/" + IDCardPhoto.Substring(0, 2) + "/" + IDCardPhoto;
            }
        }
    }
}
