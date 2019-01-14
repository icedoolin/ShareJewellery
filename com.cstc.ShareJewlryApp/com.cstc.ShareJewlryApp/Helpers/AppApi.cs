using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Helpers
{

    /// <summary>
    /// 软件所有接口名称
    /// 1.  异步机制无法写公共获取数据的方法，只能在每个需要用到的地方调用接口获取。
    /// 2.  同一个接口名称经常会发生改变。
    /// 所以每个接口名称统一在这里维护，而不用整个软件查找11112222sfdsfdsf
    /// </summary>
    public class AppApi
    {
        //获取商品列表
        public static string GetCommodityList = @"APP\GetAllSearchJewellery_1_0_0_2";
        //获取首页直通车商品数据
        public static string GetThroughTrainJewellery = @"App\GetThroughTrainJewellery_1_0_0_1";
        //获取商品详情
        public static string GetCommodityDetail = @"APP\GetJewelleryInfo_1_0_0_1,APP\GetJewelleryFirstSpecDetailed_1_0_0_1,APP\GetJewellerySecondSpecDetailed_1_0_0_1,APP\GetJewelleryParameter_1_0_0_1";
        //注册登录后获取用户数据 
        public static string LoginAndGetUserInfo = @"APP\RegisterAndLogin_1_0_0_3";
        //根据userguid和信息类型获取用户数据
        public static string GetUserInfo = @"APP\GetUserInfo_1_0_0_1";
        //获取珠宝款式 无参数
        public static string GetJewelleryType = @"APP\GetJewelleryType_1_0_0_1";
        //获取珠宝材质 无参数
        public static string GetJewelleryMaterial = @"APP\GetJewelleryMaterial_1_0_0_1";
        //获取店铺信息
        public static string GetShopInfo = @"APP\GetShopInfo_1_0_0_1";
        
        //获取会员价格信息 无参数
        public static string GetVipData = @"App\GetMembershipPrice_1_0_0_1 ";
        //提交购买会员订单
        public static string SetBuyVipOrder = @"App\BuyAMember_1_0_0_1";
        //获取分享模板信息
        public static string GetShareTemplate = @"APP\GetTemplate_1_0_0_2";
        //获取店铺客服信息： ShopGUID
        public static string GetShopCustomerService = @"APP\GetShopCustomerService_1_0_0_1";


    }
}
