using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.HomePage.ProductDetails
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetailsPage: BasePage 
    {
        com.cstc.ShareJewlryApp.Tools.IWeChat WXDependency = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.IWeChat>();
 
        string 详情标记 = "";
        string 主规格 = "";
        string 选中的副规格 = "";
        string 副规格GUID = "";
        decimal 选中商品单价 = 0;
        string 订单标记 = "";
        bool isclosed = false;  //如果页面已经关闭，则取消加载详情
        //bool 是否收藏 = false;

        bool load = false;
        Data.commodityData ProItem = new Data.commodityData();
 
        public string JewelleryGUID { get; set; }
 
        public ProductDetailsPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            scl_flips.HeightRequest = 360 * Helpers.MConfig.screenRatios;
 
        }
 
        protected override void OnAppearing()
        {
            getCommodityDetailsData();

            scl_flips.Source =Helpers.MConfig.ProductDetailHeadUrl(JewelleryGUID);
        }
        protected override void OnDisappearing()
        {
            //base.OnDisappearing();
            
        }
        protected override bool OnBackButtonPressed()
        {
            bool re = false;
            if (Device.RuntimePlatform.ToString() == Device.Android)
            {
                if (AlertBox.IsVisible)
                {
                    AlertBox.IsVisible = false;
                    return true;
                }
            }
            isclosed = true;
            return re;
        }


 
        /// <summary>
        /// 分享 
        /// </summary>
        /// <param name="sign">好友或者朋友圈 Chat/Friends</param>
        public void Share(string sign)
        {
            Helpers.AsyncMsg am_获取信息 = new Helpers.AsyncMsg();
            am_获取信息.Completion += (object obj, string ex) =>
            {
                Data.InviteTemplateData item = (Data.InviteTemplateData)obj;
                Device.BeginInvokeOnMainThread(() =>
                {
                    WXDependency.shareWebPage(Data.InviteDataMgr.GetcmtyShareUrl(JewelleryGUID, Data.UserInfoCache.UserGUID),
                        item.TemplateTitle, item.ShareTitle, sign, item.AppTemplateThumbnailForShow);
                });
            };
 
            Data.InviteDataMgr.GetShareTemplate( "商品" ,am_获取信息);
        }



        /// <summary>
        /// 获取商品详情
        /// </summary>
        public void getCommodityDetailsData()
        {
            Helpers.AsyncMsg am_获取商品详情 = new Helpers.AsyncMsg();

            am_获取商品详情.Completion += (object obj, string ex) =>
            {
                if (isclosed) return;
                List<Data.commodityData> templist = (List<Data.commodityData>)obj;
                ProItem = templist[0];
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (ProItem.totalStock < 1)   //有无库存
                    {
                        lb_commodityName.Text = ProItem.JewelleryName + "  [暂无库存]";
                    }
                    else
                    {
                        lb_commodityName.Text = ProItem.JewelleryName;  //商品名
                    }

                    lb_commodiName.Text = ProItem.JewelleryName;  //弹出框商品名
                    if (ProItem.WhetherBeCollected == "1")  //商品被收藏
                    {
                        //是否收藏 = true;
                        img_like.Source = "ilike_red.png";
                        lb_like.Text = "已收藏";
                        lb_like.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                    }
                    if (Convert.ToBoolean(ProItem.AllowRent) && ProItem.totalStock > 0)  //允许免费戴
                    {
                        lb_freeDress.IsVisible = true;
                        lb_freeDressBtn.IsVisible = true;
                    }
                    else
                    {
                        lb_freeDress.IsVisible = false;
                        lb_freeDressBtn.IsVisible = false;
                    }

                    lb_stock.Text = "库存：" + ProItem.totalStock.ToString();          //商品库存
                    lb_appraise.Text = "好评率：" + ProItem.Praise + "%";     //好评率
                    lb_salesVolume.Text = ProItem.number + "人已下单";
                    lb_shopNameItem.LeftText = ProItem.ShopName; //商铺名
                    lb_shopNameItem.LeftIco =  Helpers.MConfig.shopLogoUrl(ProItem.ShopLOGO); //商铺LOGO

                    img_cmdty.Source = ProItem.JewelleryPicNameForImg;   //图片
                    ls_cmdtyPara.ItemsSource = ProItem.Parameter;
                    if (ProItem.Parameter.Count == 0) ls_cmdtyPara.HeightRequest = 100;
                    else
                        ls_cmdtyPara.HeightRequest = (ProItem.Parameter.Count * (com.cstc.ShareJewlryApp.Style.Shape.height.MycenterbarLenth + 0.5)) + 15;
                    //web_procomDtl.WidthRequest = 640d;
                    //web_procomDtl.HeightRequest = 640d;
                    //商品详情
                    web_procomDtl.Source = Helpers.MConfig.ProductDetailUrl(JewelleryGUID);
                });
            };
   
          Data.CommodityMgr.GetCommodityDetail(am_获取商品详情,  JewelleryGUID);
            //Tools.NetClass.GetStringByUrl("QueryData", "APP\\GetJewelleryInfo_1_0_0_1,APP\\GetJewelleryFirstSpecDetailed_1_0_0_1,APP\\GetJewellerySecondSpecDetailed_1_0_0_1,APP\\GetJewelleryParameter_1_0_0_1", para, am_获取商品详情);
        }

        private void 绘制主规格视图()
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    #region 移除原有控件
                    st_MenuPara.Children.Clear();

                    #endregion

                    if (ProItem == null )
                        return;

 
                        var lb_规格组标题 = new Label { IsVisible = true, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, Text = ProItem.FirstSpecName + "：", HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center, LineBreakMode = LineBreakMode.NoWrap, TextColor = Color.Black };

                        st_MenuPara.Children.Add(lb_规格组标题);

                        var sl_规格行 = new StackLayout { IsVisible = true, Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.Transparent, Spacing = 5 };

                        st_MenuPara.Children.Add(sl_规格行);

                        double Swidth = 0;

                        if (Device.RuntimePlatform.ToString() == Device.Android)
                        {
                            Swidth = Helpers.MConfig.screenWidth / 24;
                        }
                        else if (Device.RuntimePlatform.ToString() == Device.iOS)
                        {
                            Swidth = Helpers.MConfig.screenWidth / 17;
                        }

                        var 存放宽度 = Helpers.MConfig.screenWidth;
                        var 剩余存放宽度 = 存放宽度;

                        foreach (var 主规格明细 in ProItem.FirstSpecDetail)
                        {

                            var sl_规格 = new Frame { HasShadow = false, CornerRadius = 3,
                                BindingContext = 主规格明细, BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont,
                                Padding = new Thickness(1, 1, 1, 1) };

                            var sl_规格_内框 = new Frame { HasShadow = false, CornerRadius = 3,
                                BindingContext = 主规格明细, BackgroundColor = Color.White,
                                Padding = new Thickness(2, 2, 2, 2) };

                            sl_规格.SetBinding(BackgroundColorProperty, "paddingColor");

                            string 规格文本 = 主规格明细.FirstSpecDetailedName;

                            if (规格文本.Length == 1)
                            {
                                if (Device.RuntimePlatform.ToString() == Device.Android)
                                {
                                    规格文本 = "   " + 规格文本 + "   ";
                                }
                                else if (Device.RuntimePlatform.ToString() == Device.iOS)
                                {
                                    规格文本 = " " + 规格文本 + " ";
                                }
                            }
                            else if (规格文本.Length == 2)
                            {
                                规格文本 = 规格文本.Insert(1, "  ");
                            }

                            if (规格文本.Length > 20)
                                规格文本 = 规格文本.Substring(0, 18) + "..";

                            var lb_规格文本 = new Label { Text = " " + 规格文本 + " ", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12,
                                HorizontalTextAlignment = TextAlignment.Center,
                                BackgroundColor = Color.Transparent, LineBreakMode = LineBreakMode.NoWrap,
                                TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont,
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalTextAlignment = TextAlignment.Center,
                                BindingContext = 主规格明细, HeightRequest = Helpers.MConfig.screenWidth / 16 };
                            TapGestureRecognizer choose_tap = new TapGestureRecognizer();

                            lb_规格文本.SetBinding(BackgroundColorProperty, "backColor");
                            lb_规格文本.SetBinding(Label.TextColorProperty, "FontColor");

                            sl_规格_内框.SetBinding(BackgroundColorProperty, "backColor");
                            //sl_规格_内框.SetBinding(Label.TextColorProperty, "FontColor");

                            choose_tap.CommandParameter = 主规格明细;  //加入数据
                            choose_tap.Tapped += tapges_choosePara;

                            sl_规格_内框.GestureRecognizers.Add(choose_tap);//加入点击手势

                            sl_规格_内框.Content = lb_规格文本;

                            sl_规格.Content = sl_规格_内框;

                            #region 计算控件占用宽度
                            double 控件占用宽度 = 0;

                            if (lb_规格文本.Text.Length > 0)
                            {
                                int 文本长度 = lb_规格文本.Text.Replace(" ", "").Length;
                                int 空格长度 = lb_规格文本.Text.Length - 文本长度;
                                控件占用宽度 = 文本长度 * Swidth + 空格长度 * (Swidth / 4) + 10;
                            }
                            else
                            {
                                控件占用宽度 = 20;
                            }

                            #endregion

                            这里:
                            if (控件占用宽度 <= 剩余存放宽度 || 剩余存放宽度 == 存放宽度)
                            {
                                sl_规格行.Children.Add(sl_规格);

                                剩余存放宽度 -= 控件占用宽度;
                            }
                            else
                            {
                                //另起一行
                                var sl_规格新行 = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.Transparent, Spacing = 5 };

                                st_MenuPara.Children.Add(sl_规格新行);

                                sl_规格行 = sl_规格新行;
                                剩余存放宽度 = 存放宽度;
                                goto 这里;
                            }
                        }
                     
                });
            }
            catch (Exception ex) { }
        }

        private void 绘制副规格规格视图(string 选定的主规格)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                #region 移除原有控件
                st_secMenuPara.Children.Clear();

                #endregion

                if (ProItem.FirstSpecDetail == null || ProItem.FirstSpecDetail.Count <= 0)
                    return;

 
                    var lb_规格组标题 = new Label { IsVisible = true, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, Text = ProItem.SecondSpecName + "：", HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center, LineBreakMode = LineBreakMode.NoWrap, TextColor = Color.Black };

                    st_secMenuPara.Children.Add(lb_规格组标题);

                    var sl_规格行 = new StackLayout { IsVisible = true, Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.Transparent, Spacing = 5 };

                    st_secMenuPara.Children.Add(sl_规格行);

                    double Swidth = 0;

                    if (Device.RuntimePlatform.ToString() == Device.Android)
                    {
                        Swidth = Helpers.MConfig.screenWidth / 24;
                    }
                    else if (Device.RuntimePlatform.ToString() == Device.iOS)
                    {
                        Swidth = Helpers.MConfig.screenWidth / 17;
                    }

                    ///24
                    var 存放宽度 = Helpers.MConfig.screenWidth;
                    var 剩余存放宽度 = 存放宽度;


                    foreach (var 主规格 in ProItem.FirstSpecDetail)
                    {
                        if (选定的主规格 == 主规格.FirstSpecDetailedName)
                        {
                            foreach (var 副规格明细 in 主规格.SecondSpecDetail)
                            {
                                if (副规格明细.Stock < 1)
                                {
                                    var sl_规格 = new Frame { HasShadow = false, CornerRadius = 3, BindingContext = 副规格明细, BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.LineColor, Padding = new Thickness(1, 1, 1, 1) };

                                    var sl_规格_内框 = new Frame { HasShadow = false, CornerRadius = 3, BindingContext = 副规格明细, BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.LineColor, Padding = new Thickness(2, 2, 2, 2) };
                                    string 规格文本 = 副规格明细.SecondSpecDetailedName;
                                    if (规格文本.Length == 1)
                                    {
                                        if (Device.RuntimePlatform.ToString() == Device.Android)
                                        {
                                            规格文本 = "   " + 规格文本 + "   ";
                                        }
                                        else if (Device.RuntimePlatform.ToString() == Device.iOS)
                                        {
                                            规格文本 = " " + 规格文本 + " ";
                                        }
                                    }
                                    else if (规格文本.Length == 2)
                                    {
                                        规格文本 = 规格文本.Insert(1, "  ");
                                    }

                                    if (规格文本.Length > 20)
                                        规格文本 = 规格文本.Substring(0, 18) + "..";

                                    var lb_规格文本 = new Label { Text = " " + 规格文本 + " ", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, HorizontalTextAlignment = TextAlignment.Center, BackgroundColor = Color.Transparent, LineBreakMode = LineBreakMode.NoWrap, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.Start, VerticalTextAlignment = TextAlignment.Center, BindingContext = 副规格明细, HeightRequest = Helpers.MConfig.screenWidth / 16 };

                                    sl_规格_内框.Content = lb_规格文本;

                                    sl_规格.Content = sl_规格_内框;

                                    #region 计算控件占用宽度
                                    double 控件占用宽度 = 0;

                                    if (lb_规格文本.Text.Length > 0)
                                    {
                                        int 文本长度 = lb_规格文本.Text.Replace(" ", "").Length;
                                        int 空格长度 = lb_规格文本.Text.Length - 文本长度;
                                        控件占用宽度 = 文本长度 * Swidth + 空格长度 * (Swidth / 4) + 10;
                                    }
                                    else
                                    {
                                        控件占用宽度 = 20;
                                    }

                                    #endregion

                                    这里:
                                    if (控件占用宽度 <= 剩余存放宽度 || 剩余存放宽度 == 存放宽度)
                                    {
                                        sl_规格行.Children.Add(sl_规格);

                                        剩余存放宽度 -= 控件占用宽度;
                                    }
                                    else
                                    {
                                        //另起一行
                                        var sl_规格新行 = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.Transparent, Spacing = 5 };

                                        st_secMenuPara.Children.Add(sl_规格新行);

                                        sl_规格行 = sl_规格新行;
                                        剩余存放宽度 = 存放宽度;
                                        goto 这里;
                                    }
                                }
                                else
                                {
                                    var sl_规格 = new Frame { HasShadow = false, CornerRadius = 3, BindingContext = 副规格明细, BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, Padding = new Thickness(1, 1, 1, 1) };

                                    var sl_规格_内框 = new Frame { HasShadow = false, CornerRadius = 3, BindingContext = 副规格明细, BackgroundColor = Color.White, Padding = new Thickness(2, 2, 2, 2) };

                                    sl_规格.SetBinding(BackgroundColorProperty, "paddingColor");

                                    string 规格文本 = 副规格明细.SecondSpecDetailedName;

                                    if (规格文本.Length == 1)
                                    {
                                        if (Device.RuntimePlatform.ToString() == Device.Android)
                                        {
                                            规格文本 = "   " + 规格文本 + "   ";
                                        }
                                        else if (Device.RuntimePlatform.ToString() == Device.iOS)
                                        {
                                            规格文本 = " " + 规格文本 + " ";
                                        }
                                    }
                                    else if (规格文本.Length == 2)
                                    {
                                        规格文本 = 规格文本.Insert(1, "  ");
                                    }

                                    if (规格文本.Length > 20)
                                        规格文本 = 规格文本.Substring(0, 18) + "..";

                                    var lb_规格文本 = new Label { Text = " " + 规格文本 + " ", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, HorizontalTextAlignment = TextAlignment.Center, BackgroundColor = Color.White, LineBreakMode = LineBreakMode.NoWrap, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.Start, VerticalTextAlignment = TextAlignment.Center, BindingContext = 副规格明细, HeightRequest = Helpers.MConfig.screenWidth / 16 };
                                    TapGestureRecognizer choose_tap = new TapGestureRecognizer();

                                    lb_规格文本.SetBinding(BackgroundColorProperty, "backColor");
                                    lb_规格文本.SetBinding(Label.TextColorProperty, "FontColor");

                                    sl_规格_内框.SetBinding(BackgroundColorProperty, "backColor");
                                    //sl_规格_内框.SetBinding(Label.TextColorProperty, "FontColor");

                                    choose_tap.CommandParameter = 副规格明细;  //加入数据
                                    choose_tap.Tapped += tapges_chooseSecPara;

                                    sl_规格_内框.GestureRecognizers.Add(choose_tap);//加入点击手势

                                    sl_规格_内框.Content = lb_规格文本;

                                    sl_规格.Content = sl_规格_内框;

                                    #region 计算控件占用宽度
                                    double 控件占用宽度 = 0;

                                    if (lb_规格文本.Text.Length > 0)
                                    {
                                        int 文本长度 = lb_规格文本.Text.Replace(" ", "").Length;
                                        int 空格长度 = lb_规格文本.Text.Length - 文本长度;
                                        控件占用宽度 = 文本长度 * Swidth + 空格长度 * (Swidth / 4) + 10;
                                    }
                                    else
                                    {
                                        控件占用宽度 = 20;
                                    }

                                    #endregion

                                    这里:
                                    if (控件占用宽度 <= 剩余存放宽度 || 剩余存放宽度 == 存放宽度)
                                    {
                                        sl_规格行.Children.Add(sl_规格);

                                        剩余存放宽度 -= 控件占用宽度;
                                    }
                                    else
                                    {
                                        //另起一行
                                        var sl_规格新行 = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.Transparent, Spacing = 5 };

                                        st_secMenuPara.Children.Add(sl_规格新行);

                                        sl_规格行 = sl_规格新行;
                                        剩余存放宽度 = 存放宽度;
                                        goto 这里;
                                    }
                                }
                            }
                     
                    }
                    #region 旧代码
            
                    #endregion
                }
            });
        }

        /// <summary>
        /// 选择规格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapges_choosePara(object sender, EventArgs e)
        {
            Data.FirstSpecDetailData row = (Data.FirstSpecDetailData)(((TappedEventArgs)e).Parameter);

            if (主规格 == row.FirstSpecDetailedName)
                return;

            重置副规格选中状态();
            主规格 = row.FirstSpecDetailedName;

            foreach (var temp in ProItem.FirstSpecDetail)
            {
                if (row.FirstSpecDetailedName != temp.FirstSpecDetailedName)
                {
                    temp.Selected = false;
                }
                else
                {
                    temp.Selected = true;
                }
            }

            绘制副规格规格视图(row.FirstSpecDetailedName);
        }

        /// <summary>
        /// 选择副规格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapges_chooseSecPara(object sender, EventArgs e)
        {
            Data.SecondSpecData row = (Data.SecondSpecData)(((TappedEventArgs)e).Parameter);

            foreach (var temp in ProItem.FirstSpecDetail)
            {
                if (主规格 == temp.FirstSpecDetailedName)
                {
                    foreach (var 副规格 in temp.SecondSpecDetail)
                    {
                        if (row.SecondSpecDetailedGUID != 副规格.SecondSpecDetailedGUID)
                        {
                            副规格.Selected = false;

                        }
                        else
                        {
                            副规格.Selected = true;
                            //lb_price.Text = "¥ " + (副规格.Price * Convert.ToInt32(lb_num.Text));   //总价格
                            //lb_commodiPrice.Text = "¥ " + 副规格.Price;  //单品价格
                            副规格GUID = 副规格.SecondSpecDetailedGUID;
                            选中的副规格 = 副规格.SecondSpecDetailedName;
                            //选中商品单价 = 副规格.Price;
                            lb_takeOrder.IsVisible = false;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 商品详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_ProductDetails(object sender, EventArgs e)
        {
            #region UI呈现
            if (详情标记 == "商品详情")
                return;

            详情标记 = "商品详情";


            Device.BeginInvokeOnMainThread(() =>
            {
                lb_ProductDetails.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                box_ProductDetails.BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                web_procomDtl.IsVisible = true;
                lb_standardDetails.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                box_standardDetails.BackgroundColor = Color.Transparent;
                ls_cmdtyPara.IsVisible = false;
            });


            #endregion
        }


        /// <summary>
        /// 参数规格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_standardDetails(object sender, EventArgs e)
        {
            #region UI呈现
            if (详情标记 == "规格信息")
                return;

            详情标记 = "规格信息";

            Device.BeginInvokeOnMainThread(() =>
            {
                lb_ProductDetails.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                box_ProductDetails.BackgroundColor = Color.Transparent;
                web_procomDtl.IsVisible = false;
                lb_standardDetails.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                box_standardDetails.BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                ls_cmdtyPara.IsVisible = true;
            });
            #endregion
        }

        /// <summary>
        /// 免费戴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_freedressUp(object sender, EventArgs e)
        {
            if (Data.UserInfoCache.UserGUID == "")
            {
                st_LoginBox.IsVisible = true;
                return;
            }


            if (Data.UserInfoCache.userInfo.RemainderDays < 1)
            {
                BuyVipAlert_Show.IsVisible = true;
                return;
            }

            //更新AllowRent字段
            Helpers.AsyncMsg am_更新用户信息 = new Helpers.AsyncMsg();
            am_更新用户信息.Completion += (object obss, string bbb) =>
            {
                if (Data.UserInfoCache.userInfo.AllowRent == "冻结")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        AlertBox.Text = Data.UserInfoCache.userInfo.LockReasons;
                        AlertBox.ImgSource = "AlertBoxBgImage_OnlyOne.png";
                        AlertBox.IsVisible = true;
                    });
                    return;
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    订单标记 = "租借";
                    绘制主规格视图();
                    st_comdyNum.IsVisible = false;
                    lb_num.IsVisible = false;
                    lb_price.IsVisible = false;
                    lb_priceDesc.IsVisible = false;
                    lb_freeDesc.IsVisible = true;
                    btn_takeOrder.IsVisible = true;
                    btn_addShopCart.IsVisible = false;
                    block.IsVisible = true;
                    st_ChooseStandard.IsVisible = true;
                });
            };
 
            Data.UserinfoMgr.RefreshUserInfoCache("会员信息", am_更新用户信息);
 

        }

        /// <summary>
        /// 选择规格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_selectPara(object sender, EventArgs e)
        {
            tapped_freedressUp(null,null);
        }

 
        /// <summary>
        ///  遮罩层关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_block(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                block.IsVisible = false;
                st_ChooseStandard.IsVisible = false;
                st_shareBox.IsVisible = false;
                重置规格弹窗UI();
            });

        }

        /// <summary>
        /// “提交订单”按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_takeOrder(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                if (副规格GUID == "")
                {

                    return;
                }


                int 购买数量 = Convert.ToInt32(lb_num.Text.Trim());

                Data.commodityData comData = new Data.commodityData() { OredrDetailedGUID = Guid.NewGuid().ToString(), JewelleryPicName = ProItem.JewelleryPicName, JewelleryName = ProItem.JewelleryName, FirstSpecName = ProItem.FirstSpecName, FirstSpecDetailedName = 主规格, SecondSpecName = ProItem.SecondSpecName, SecondSpecDetailedName = 选中的副规格, SecondSpecGUID = 副规格GUID, JewelleryGUID = ProItem.JewelleryGUID, number = 购买数量, Price = 选中商品单价 };

                ObservableCollection<Data.commodityData> rows = new ObservableCollection<Data.commodityData>();

                rows.Add(comData);

                Data.OrderData tempData = new Data.OrderData() { ShopGUID = ProItem.ShopGUID, ShopName = ProItem.ShopName, ShopLogoForImg = ProItem.ShopLOGOForImg, CommodityChildrenRows = rows };

                ObservableCollection<Data.OrderData> orderList = new ObservableCollection<Data.OrderData>();

                orderList.Add(tempData);

                Views.ShoppingCart.Order.OrderPage page = new ShoppingCart.Order.OrderPage();

                page.orderList = orderList;
                page.订单标志 = 订单标记;

                page.绘制列表();

                Navigation.PushAsync(page, true);
                tapped_block(null, null);
            }
        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_closePage(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                Navigation.PopAsync(true);
            }
        }

 

        /// <summary>
        /// 向上滑动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_top(object sender, EventArgs e)
        {
            scl_prduct.ScrollToAsync(0, 0, false);
        }

        /// <summary>
        /// 数量减少
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_reduce(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(lb_num.Text);
            if (i == 0)
                return;
            i--;
            lb_num.Text = i.ToString();

            decimal 总价 = i * 选中商品单价;

            lb_price.Text = "¥ " + 总价;
        }

        /// <summary>
        /// 数量增加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_plus(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(lb_num.Text);
            i++;
            lb_num.Text = i.ToString();

            decimal 总价 = i * 选中商品单价;

            lb_price.Text = "¥ " + 总价;

        }
 
        void 重置规格弹窗UI()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                //lb_price.Text = "";
                lb_num.Text = "1";
               // lb_commodiPrice.Text = "";
                btn_Collection.IsVisible = false;
                btn_addShopCart.IsVisible = false;
                lb_takeOrder.IsVisible = true;
                重置规格选中状态();
                重置副规格选中状态();
                st_secMenuPara.Children.Clear();
            });
        }

        void 重置规格选中状态()
        {
            主规格 = "";
            副规格GUID = "";
            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (var temp in ProItem.FirstSpecDetail)
                {
                    temp.Selected = false;
                    foreach (var 副规格 in temp.SecondSpecDetail)
                    {
                        副规格.Selected = false;
                    }
                }
            });
        }

        void 重置副规格选中状态()
        {
            副规格GUID = "";
            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (var temp in ProItem.FirstSpecDetail)
                {
                    foreach (var 副规格 in temp.SecondSpecDetail)
                    {
                        副规格.Selected = false;
                    }
                }

                lb_takeOrder.IsVisible = true;
               // lb_price.Text = "";
                //lb_commodiPrice.Text = "";
            });

        }

        /// <summary>
        /// 添加到购物车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_addShopCart(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                if (副规格GUID == "")
                {

                    return;
                }


                if (Data.UserInfoCache.UserGUID == "")
                {
                    hud.Show_Toast("请先登录");

                    return;
                }

                string 商铺GUID = ProItem.ShopGUID;
                int 购买数量 = Convert.ToInt32(lb_num.Text.Trim());

                Tools.AsyncMsg am_添加到购物车 = new Tools.AsyncMsg();

                string para = "UserGUID=" + Data.UserInfoCache.UserGUID + "&SecondSpecDetailedGUID=" + 副规格GUID + "&number=" + 购买数量 + "&ShopGUID=" + 商铺GUID + "&JewelleryGUID=" + JewelleryGUID;

                am_添加到购物车.Completion += (object obj, string ex) =>
                {
                ///ui变化加入购物车成功
                Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Show_Toast("添加成功");
                        tapped_block(null, null);

                    });
                };

                am_添加到购物车.Cancel += (object obj, string ex) =>
                {

                    return;
                };

                Tools.NetClass.GetStringByUrl("ExSql", "App\\InsertUserShoppingCartk_1_0_0_1", para, am_添加到购物车);
            }
        }


        /// <summary>
        /// 弹出添加购物车选规格弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_showAddShopCart(object sender, EventArgs e)
        {
            绘制主规格视图();
            Device.BeginInvokeOnMainThread(() =>
            {
                st_comdyNum.IsVisible = true;
                lb_num.IsVisible = true;
                //lb_price.IsVisible = true;
                //lb_priceDesc.IsVisible = true;
                lb_freeDesc.IsVisible = false;
                btn_takeOrder.IsVisible = false;
                btn_addShopCart.IsVisible = true;
                btn_Collection.IsVisible = false;

                block.IsVisible = true;
                st_ChooseStandard.IsVisible = true;
            });
        }

        /// <summary>
        /// 收藏商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_Collection(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                if (Data.UserInfoCache.UserGUID == "")
                {
                    hud.Show_Toast("请先登录");

                    return;
                }

                if (ProItem.WhetherBeCollected == "1")
                {
                    Tools.AsyncMsg am_取消收藏 = new Tools.AsyncMsg();

                    string para = "UserGUID=" + Data.UserInfoCache.UserGUID + "&JewelleryGUID=" + JewelleryGUID;

                    am_取消收藏.Completion += (object obj, string ex) =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                        hud.Show_Toast("取消收藏成功");
                            ProItem.WhetherBeCollected = "0";
                                img_like.Source = "ilike.png";
                            lb_like.Text = "收藏";
                            lb_like.TextColor = Color.Black;

                        //更新用户信息
                        Helpers.AsyncMsg am_更新用户信息 = new Helpers.AsyncMsg();
                            am_更新用户信息.Completion += (object obss, string bbb) =>
                            {

                            };
                            Data.UserinfoMgr.RefreshUserInfoCache("个人信息", am_更新用户信息);
                            tapped_block(null, null);
                        });
                    };

                    am_取消收藏.Cancel += (object obj, string ex) =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {

                            DisplayAlert("提示", "取消收藏失败", "知道了");
                        });
                        return;
                    };

                    Tools.NetClass.GetStringByUrl("ExSql", "App\\DeleteCollectionFolderJewellery_1_0_0_1", para, am_取消收藏);   ///接口未修改

                }
                else  if (ProItem.WhetherBeCollected != "1")
                {
                    string 商铺GUID = ProItem.ShopGUID;
                    int 购买数量 = Convert.ToInt32(lb_num.Text.Trim());

                    Tools.AsyncMsg am_添加到首饰盒 = new Tools.AsyncMsg();

                    string para = "UserGUID=" + Data.UserInfoCache.UserGUID + "&JewelleryGUID=" + JewelleryGUID;


                    am_添加到首饰盒.Completion += (object obj, string ex) =>
                    {
                    ///ui变化添加到首饰盒成功
                    Device.BeginInvokeOnMainThread(() =>
                        {
                            hud.Show_Toast("收藏成功");
                            ProItem.WhetherBeCollected = "1";
                            img_like.Source = "ilike_red.png";
                            lb_like.Text = "已收藏";
                            lb_like.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;

                        //更新用户信息
                        Helpers.AsyncMsg am_更新用户信息 = new Helpers.AsyncMsg();
                            am_更新用户信息.Completion += (object obss, string bbb) =>
                            {

                            };
                            Data.UserinfoMgr.RefreshUserInfoCache("个人信息", am_更新用户信息);
                            tapped_block(null, null);
                        });
                    };

                    am_添加到首饰盒.Cancel += (object obj, string ex) =>
                    {

                        return;
                    };

                    Tools.NetClass.GetStringByUrl("ExSql", "App\\InsertUserCollectionFolder_1_0_0_1", para, am_添加到首饰盒);
                }
            }

        }

        /// <summary>
        /// 客服页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_OpenCustPage(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                CustomerService.CustomerServicePage page = new CustomerService.CustomerServicePage();
                page.shopGUID = ProItem.ShopGUID;
                //page.getData();
                Navigation.PushAsync(page, true);
            }
        }


        /// <summary>
        /// 弹出收藏页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_ShowCollectionPage(object sender, EventArgs e)
        {
            绘制主规格视图();
            Device.BeginInvokeOnMainThread(() =>
            {
                st_comdyNum.IsVisible = true;
                lb_num.IsVisible = true;
               // lb_price.IsVisible = true;
               // lb_priceDesc.IsVisible = true;
                lb_freeDesc.IsVisible = false;
                btn_takeOrder.IsVisible = false;
                btn_addShopCart.IsVisible = false;
                btn_Collection.IsVisible = true;
                block.IsVisible = true;
                st_ChooseStandard.IsVisible = true;
            });
        }

        /// <summary>
        /// 进店看看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_openShopPage(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                Views.Classification.ShopPage page = new Classification.ShopPage() { shopName = lb_shopNameItem.LeftText };
                page.shopGuid = ProItem.ShopGUID;
                page.获取商铺信息();
                Navigation.PushAsync(page, true);

            }

        }
 
 
        /// <summary>
        /// 分享
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_share(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                st_shareBox.IsVisible = true;
                st_shareBox.BackgroundColor = Color.White;  //安卓第二次分享会变透明
                block.IsVisible = true;
            });
        }

        /// <summary>
        /// 分享给朋友
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_shareChat(object sender, EventArgs e)
        {
            Share("Chat");
            //WXDependency.shareWebPage(com.cstc.ShareJewlryApp.Style.Constant.Constant.GetcmtyShareUrl(JewelleryGUID, Data.UserInfoCache.UserGUID), InviteList[0].Title, InviteList[0].Content, "Chat", InviteList[0].picForShow);
            // WXDependency.shareWebPage(com.cstc.ShareJewlryApp.Style.Constant.Constant.cmtyShareUrl + JewelleryGUID, InviteList[0].Title, InviteList[0].Content, "Chat", "");
            tapped_block(null, null);
        }

        /// <summary>
        /// 分享到朋友圈
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_shareFriends(object sender, EventArgs e)
        {
            Share("Friends");
            //  WXDependency.shareWebPage(com.cstc.ShareJewlryApp.Style.Constant.Constant.cmtyShareUrl + JewelleryGUID, InviteList[0].Title, InviteList[0].Content, "Friends", "");
            tapped_block(null, null);
        }

 


 
        private void St_LoginBox_LoginSuccess(object sender, EventArgs e)
        {
 
        }

        private void St_LoginBox_LoginCancel(object sender, EventArgs e)
        {
 
        }

 
    }
}