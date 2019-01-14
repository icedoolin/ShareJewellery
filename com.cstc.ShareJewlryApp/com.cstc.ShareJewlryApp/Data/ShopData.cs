using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Data
{
    /// <summary>
    /// 商铺类
    /// </summary>
    public class ShopData
    {
        /// <summary>
        /// 商铺名称
        /// </summary>
        public string ShopName { get; set; } = "";

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel { get; set; } = "";

        /// <summary>
        /// 商铺地址
        /// </summary>
        public string ShopAdrress { get; set; } = "";

        /// <summary>
        /// 商铺LOGO
        /// </summary>
        public string ShopLOGO { get; set; } = "";

        /// <summary>
        /// 客服QQ
        /// </summary>
        public string ServiceQQ { get; set; } = "";

        /// <summary>
        /// 客服微信
        /// </summary>
        public string ServiceWechat { get; set; } = "";

        /// <summary>
        /// 商铺背景图
        /// </summary>
        public string ShopBackground { get; set; } = "";


        public string ShopBackgroundForShow
        {
            get
            {
                return Helpers.MConfig.picUrl + "Pic/shangpubeijing/" + ShopBackground.Substring(0, 2).ToUpper() + "/" + ShopBackground;
            }
        }

        /// <summary>
        /// 商铺珠宝数量
        /// </summary>
        public int ShopJewelleryNumber { get; set; } = 0;
    }



    public class ShopDataMgr
    {
        /// <summary>
        /// 获取店铺信息
        /// </summary>
        /// <param name="am_return"></param>
        public static void GetShopInfo(Helpers.AsyncMsg am_return,string ShopGUID)
        {
            string para = "&ShopGUID=" + ShopGUID;
            Helpers.HttpHelper.GetDataItemList<Data.ShopData>(Helpers.AppApi.GetShopInfo, para, am_return); 
        }
 
    }
}
