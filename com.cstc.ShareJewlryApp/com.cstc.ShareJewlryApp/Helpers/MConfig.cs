using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;
namespace com.cstc.ShareJewlryApp.Helpers
{
    public class MConfig
    {
 
        /// <summary>
        ///xamarin的数值是按照宽度360的屏幕来设计的, 在安卓机上，360的宽度正好充满屏幕宽度
        ///在苹果机上，宽度有375，414，360的宽度无法充满整个屏幕。
        ///整个数值和屏幕的大小无关，4.7寸和6.5寸的屏幕获取的宽度都是375。
        ///所以，要获取实际机型的宽度，然后算出比例，用于布局计算
        /// </summary>
        public static double screenWidth { get; set; }
        /// <summary>
        /// 不同机型宽度尺寸比
        /// </summary>
        public static double screenRatios 
        {
            get
            {
                return screenWidth / 360;
            }
            set
            {
            }
        }
        //状态栏高度，暂不使用
        public static int statusbarHeight { get; set; }
        //微信开放平台上的AppID
        public static readonly string APP_ID = "wxb86a48efa41420e8";
        #region 各种url

        //public static string dbUrl = "https://test.gxzb168.com:6443/";  //测试用    
        //public static string alipayUrl = "https://test.gxzb168.com:6443/Plugin_Text?ClassName=Plugin_AL.PaySign_APP";   //测试用
        //public static string weixinpayUrl = "https://test.gxzb168.com:6443/Plugin_Text?ClassName=Plugin_Wx.Pay.UnifiedOrder_APP"; //测试用
       
        ////软件数据服务url
        public static string dbUrl = "https://test.gxzb168.com:8443/";
        ////微信支付url
        public static string weixinpayUrl = "https://test.gxzb168.com:8443/Plugin_Text?ClassName=Plugin_Wx.Pay.UnifiedOrder_APP";
        //// 支付宝支付url
        public static string alipayUrl = "https://test.gxzb168.com:8443/Plugin_Text?ClassName=Plugin_AL.PaySign_APP";
        //主图片库url
        public static string picUrl = "http://test.gxzb168.com:8811/";
        //商铺LOGO
        public static string shopLogoUrl(string ShopLOGO)
        {
            return Helpers.MConfig.picUrl + "Pic/shangpinLOGO/" + ShopLOGO.Substring(0, 2).ToUpper() + "/" + ShopLOGO;
        }
        //微信分享图片url
        public static string weixinpicUrl = "http://test.gxzb168.com:7443/ProxyService/ImageUrl?ImagePath=";
        //用于商品分享的内容url
        public static string ShareUrl = "http://test.gxzb168.com/ShareJewels/APP/share/invite.html?";
        //用于好友分享的内容url  
        public static string InviteUrl = "http://test.gxzb168.com/ShareJewels/APP/invite/invite.html?";
        //安卓更新地址
        public static string UpdUrl_Android = @"https://test.gxzb168.com:8812/DownLoad/ShareJewlryApp.apk";
        //苹果更新地址
        public static string UpdUrl_IOS = @"https://itunes.apple.com/WebObjects/MZStore.woa/wa/viewSoftware?id=1444398377&mt=8";
        #endregion
        #region webview引用的url
        public static string weburl = @"http://test.gxzb168.com/ShareJewels/APP/";
        //首页轮播图，参数time
        public static string HomePageBannerUrl = weburl +"banner/index.html?";
        //商品详情url
        public static string ProductDetailUrl(string JewelleryGUID)
        {
            return weburl + "jewelleryDetail/jewelleryDetail.html?jewelleryGuid=" + JewelleryGUID;
        }
        //商品明细轮播图,参数jewelleryGuid
        public static string ProductDetailHeadUrl(string JewelleryGUID)
        {
            return weburl + "jewelleryBanner/index.html?jewelleryGuid=" + JewelleryGUID;
        }
        //公司介绍url
        public static string CompanyDescUrl = "https://test.gxzb168.com:9443/Html?SqlCmdName=APP%5CGetCompanyIntroduction_1_0_0_1";
        //用户协议url        //服务条款url
        public static string TermsOfServiceUrl = weburl + "TermsOfService/index.html";
        //消息url
        public static string MessageUrl = weburl + "hotSpot/list.html?";
        //商铺url
        public static string ShopwebUrl = weburl + "shopBanner/index.html?";
        #endregion
 
        //每次最多使用水晶抵扣付款金额的比例：20%
        public static double CrystalLimit = 0.2;
        //App当前版本（根据ios和android各自获取）
        public static string AppCurrentVersion
        {
            get
            {
                Tools.IGetVersion getVersion = Xamarin.Forms.DependencyService.Get<Tools.IGetVersion>();
                return getVersion.CurrentVersion;
 
            }
        }

        // 两次点击按钮之间的点击间隔不能少于1000毫秒
        private static int MIN_CLICK_DELAY_TIME = 1000;
        //最后一次点击时间
        private static DateTime lastClickTime;
        
        /// <summary>
        /// 是否经过了间隔时间再点击的，防止快速双击或多次点击
        /// </summary>
        /// <returns></returns>
        public static bool isNormalClick
        {
            get
            {
                bool flag = false;
                DateTime curClickTime = System.DateTime.Now;//  System.currentTimeMillis();

                if ((curClickTime - lastClickTime).TotalMilliseconds >= MIN_CLICK_DELAY_TIME)
                {
                    flag = true;
                    lastClickTime = curClickTime;
                }

                return flag;
            }

        }



        #region 静态页面
        static Views.HomePage.homePage Homepage = null;
        public static Views.HomePage.homePage homePage
        {
            get
            {
                if (Homepage == null)
                {
                    Homepage = new Views.HomePage.homePage();
                }
                return Homepage;
            }
            set
            {
                Homepage = value;
            }
        }
        static Views.Classification.ClassificationPage ClassificationPage = null;
        public static Views.Classification.ClassificationPage classificationPage
        {
            get
            {
                if (ClassificationPage == null)
                {
                    ClassificationPage = new Views.Classification.ClassificationPage();
                }
                return ClassificationPage;
            }
            set
            {
                ClassificationPage = value;
            }
        }
        static Views.VIPCenter.VipInfoPage VipInfoPage = null;
        public static Views.VIPCenter.VipInfoPage vipInfoPage
        {
            get
            {
                if (VipInfoPage == null)
                {
                    VipInfoPage = new Views.VIPCenter.VipInfoPage();
                }
                return VipInfoPage;
            }
            set
            {
                VipInfoPage = value;
            }
        }
        static Views.MyCenter.BuyVipPage BuyVipPage = null;
        public static Views.MyCenter.BuyVipPage buyVipPage
        {
            get
            {
                if (BuyVipPage == null)
                {
                    BuyVipPage = new Views.MyCenter.BuyVipPage();
                }
                return BuyVipPage;
            }
            set
            {
                BuyVipPage = value;
            }
        }
        static Views.MyCenter.MyCenterPage MyCenterPage = null;
        public static Views.MyCenter.MyCenterPage myCenterPage
        {
            get
            {
                if (MyCenterPage == null)
                {
                    MyCenterPage = new Views.MyCenter.MyCenterPage();
                }
                return MyCenterPage;
            }
            set
            {
                MyCenterPage = value;
            }
        }
        static Views.HomePage.ProductDetails.ProductDetailsPage ProductDetailsPage = null;
        public static Views.HomePage.ProductDetails.ProductDetailsPage productDetailsPage
        {
            get
            {
                if (ProductDetailsPage == null)
                {
                    ProductDetailsPage = new Views.HomePage.ProductDetails.ProductDetailsPage();
                }
                return ProductDetailsPage;
            }
            set
            {
                ProductDetailsPage = value;
            }
        }
        #endregion 静态页面

    }
}
