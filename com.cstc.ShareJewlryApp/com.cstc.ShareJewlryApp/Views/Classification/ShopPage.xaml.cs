using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.Classification
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopPage: BasePage 
    {
        List<Data.CommodityStype> Typelist = new List<Data.CommodityStype>();  //款式数据源
        ObservableCollection<Data.commodityData> dataList { get; set; }
 
        double 图片宽度 = (Helpers.MConfig.screenWidth - 30) / 2;
        double 商品行高 = 1.421 * ((Helpers.MConfig.screenWidth - 30) / 2);
        string SortColumn = "Synthesize";
        string Sort = "Asc";
        int PageNumber = 0;
        bool load = false;
        bool 下拉刷新 = false;
        public string shopGuid { get; set; } = "";
        string TypeGUID = "";
        string JewelleryName = "";   //搜索
 
        public string shopName { get; set; } = "";
  
        public ShopPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
          if (Device.RuntimePlatform==Device.iOS)
            {
                frame_1.CornerRadius = 10;
                frame_2.CornerRadius = 10;
            }
            //安卓和苹果的阴影效果有很大差异，苹果的时候不用阴影，需要背景色不一样
            if (Device.RuntimePlatform == Device.Android)
            {
                ls_commodity.BackgroundColor = Color.White;
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                gd_head.HeightRequest = 230 * Helpers.MConfig.screenRatios;
                lb_shopName.Text = shopName;
            });
 
            web_banner.HeightRequest = 0.85 * Helpers.MConfig.screenWidth;

        }

        protected override void OnAppearing()
        {
            if (load)
                return;
            load = true;
 
            获取珠宝款式();
            web_banner.Source = Helpers.MConfig.ShopwebUrl + "shopGuid=" + shopGuid;
        }
 
        public void 获取商铺信息()
        {
            Helpers.AsyncMsg am_获取商铺信息 = new Helpers.AsyncMsg();
            Data.ShopData item = new Data.ShopData();
            am_获取商铺信息.Completion += (object obj, string ex) =>
            {
                try
                {
                    var templist = (List<Data.ShopData>)obj;
                    item = templist[0];
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        img_bgImg.Source = item.ShopBackgroundForShow;
                        lb_shopNum.Text = "珠宝首饰" + item.ShopJewelleryNumber.ToString() + "款";
                    });
                }
                catch (Exception exc)
                {
                    return;
                }
 
            };
 
            Data.ShopDataMgr.GetShopInfo(am_获取商铺信息, shopGuid);
            //string para = "ShopGUID=" + shopGuid;
            //Tools.NetClass.GetStringByUrl("QueryData", "APP\\GetShopInfo_1_0_0_1", para, am_获取商铺信息);
        }

        public void 获取珠宝款式()
        {
            Helpers.AsyncMsg am_获取珠宝款式 = new Helpers.AsyncMsg();
            am_获取珠宝款式.Completion += (object obj, string ex) =>
            {
                try
                {
                    Typelist = (List<Data.CommodityStype>)obj;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Typelist[0].textColor = Color.White;
                        TypeGUID = "";  //第一次加载全部  
                        RefreshDataList();
                        款式bar呈现();

                    });
                }
                catch (Exception exc)
                {
                    return;
                }
            };
            Data.CommodityMgr.GetJewelleryType(am_获取珠宝款式);
        }

        public void 款式bar呈现()
        {
            foreach (var item in Typelist)
            {
                StackLayout st = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.Transparent,Spacing=5 };

                Label lb_type = new Label { Text = item.TypeName, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Fill, TextColor = Color.White, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs15, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
                BoxView bx = new BoxView { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, WidthRequest = 30, HeightRequest = 2, BackgroundColor = item.textColor, };
                st.Children.Add(lb_type);
                st.Children.Add(bx);

                TapGestureRecognizer select_tap = new TapGestureRecognizer();
                select_tap.CommandParameter = item;  //加入数据
                select_tap.Tapped += tapped_selectType;

                st.GestureRecognizers.Add(select_tap);//加入点击手势

                st_type.Children.Add(st);

            }
        }


        /// <summary>
        /// 选项栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_selectType(object sender, EventArgs e)
        {

            StackLayout row = (StackLayout)sender;

            if (row.Children[1].BackgroundColor == Color.White)
            {
                return;
            }

            foreach (StackLayout temp in st_type.Children)
            {


                temp.Children[1].BackgroundColor = Color.Transparent;

                Label lb_new = (Label)temp.Children[0];
                Label lb_old = (Label)row.Children[0];


                if (lb_old.Text == lb_new.Text)
                {
                    temp.Children[1].BackgroundColor = Color.White;
                    var selectItem = (Data.CommodityStype)(((TappedEventArgs)e).Parameter);
                    TypeGUID = selectItem.TypeGUID;
                    if (selectItem.TypeName == "全部") TypeGUID = "";
                    JewelleryName = "";
                    RefreshDataList();
                    //以款式获取珠宝();
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
                HomePage.CustomerService.CustomerServicePage page = new HomePage.CustomerService.CustomerServicePage();
                page.shopGUID = shopGuid;
                //page.getData();
                Navigation.PushAsync(page, true);
            }
        }


        /// <summary>
        /// 重置搜索框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_resetSearchBar(object sender, EventArgs e)
        {
            block.IsVisible = false;
            st_searchBar.IsVisible = false;
            ety_search.Unfocus();
            st_bar.IsVisible = true;
            ety_search.Text = "";
 
        }

        
        /// <summary>
        /// 重新刷新列表
        /// </summary>
        private void RefreshDataList()
        {
            if (dataList != null)
                dataList.Clear();
            else
                dataList = new ObservableCollection<Data.commodityData>();
            ls_commodity.ItemsSource = dataList;
            PageNumber = 0;
            getCommodityData();
        }

         

        /// <summary>
        /// 获取商品列表数据
        /// </summary>
        public void getCommodityData()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                st_ls_commodity_footer.IsVisible = true;
            });

            Helpers.AsyncMsg am_获取商品 = new Helpers.AsyncMsg();
            am_获取商品.Completion += (object obj, string ex) =>
            {
                try
                {               
                    List<Data.commodityData> newList = (List<Data.commodityData>)obj;   
                    //将分页数据添加到ItemSource数据中，ObservableCollection可以在内容改变后会通知UI改变
                    foreach (var row in newList)
                    {
                        dataList.Add(row);
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        PageNumber++;
                        if (newList.Count==5)
                            st_ls_commodity_footer.IsVisible = false;
                        下拉刷新 = false;
                    });
                }
                catch (Exception)
                {
                }
            };
            //获取
            Data.CommodityMgr.GetCommodityDataWithTwoColumn(am_获取商品, PageNumber,TypeGUID,"",JewelleryName,SortColumn,Sort,shopGuid, 商品行高);
        }

        /// <summary>
        /// 左边商品详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_commodity(object sender, TappedEventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {

                var row = (Data.commodityData)e.Parameter;

                Views.HomePage.ProductDetails.ProductDetailsPage page = new Views.HomePage.ProductDetails.ProductDetailsPage();
                page.JewelleryGUID = row.JewelleryGUID;
                //page.getCommodityDetailsData();
                // page.显示商品详情图片();

                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushAsync(page, true);
                });
            }
        }



        /// <summary>
        /// 右边商品详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_commodity_right(object sender, TappedEventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {

                var row = (Data.commodityData)e.Parameter;

                Views.HomePage.ProductDetails.ProductDetailsPage page = new HomePage.ProductDetails.ProductDetailsPage();
                page.JewelleryGUID = row.rightJewelleryGUID;
                //page.getCommodityDetailsData();
                // page.显示商品详情图片();

                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushAsync(page, true);

                });
            }
        }

        /// <summary>
        /// 下拉自动刷新商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ls_commodity_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var row = new Data.commodityData();

            try
            {
                row = (Data.commodityData)e.Item;
            }
            catch (Exception ex)
            {

            }

            if (下拉刷新)
                return;
            try
            {
                if (dataList != null && row == dataList[dataList.Count - 1])
                {
                    下拉刷新 = true;
                    getCommodityData();
                }
            }
            catch (Exception ex)
            { }

        }
 

        /// <summary>
        /// 显示搜索框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_showSearchBar(object sender, EventArgs e)
        {
            st_searchBar.IsVisible = true;
            st_bar.IsVisible = false;
            block.IsVisible = true;
        }

        private void 排序点击(object sender, EventArgs e)
        {
            Font_排序1.Text = "\ue601";
            Font_排序2.Text = "\ue601";
            Font_排序3.Text = "\ue601";
            if (sender == st_排序1)
            {
                if (SortColumn == "Synthesize")
                {
                    Font_排序1.Text = (Sort == "ASC" ? "\ue618" : "\ue602");
                    Sort = (Sort == "ASC" ? "DESC" : "ASC");
                }
                else
                {
                    Font_排序1.Text = "\ue602";
                    Sort = "ASC";
                    SortColumn = "Synthesize";  //综合排序
                }

            }
            else if (sender == st_排序2)
            {
                if (SortColumn == "Praise")
                {
                    Font_排序2.Text = (Sort == "ASC" ? "\ue618" : "\ue602");
                    Sort = (Sort == "ASC" ? "DESC" : "ASC");
                }
                else
                {
                    Font_排序2.Text = "\ue602";
                    Sort = "ASC";
                    SortColumn = "Praise";  //综合排序
                }

            }
            else if (sender == st_排序3)
            {
                if (SortColumn == "number")
                {
                    Font_排序3.Text = (Sort == "ASC" ? "\ue618" : "\ue602");
                    Sort = (Sort == "ASC" ? "DESC" : "ASC");
                }
                else
                {
                    Font_排序3.Text = "\ue602";
                    Sort = "ASC";
                    SortColumn = "number";  //综合排序
                }

            }
            RefreshDataList();
        }

        private void tapped_closePage(object sender, EventArgs e)
        {
            Navigation.PopAsync(true);
        }
 
        private void TapSearchbtn_Tapped(object sender, EventArgs e)
        {
            JewelleryName = ety_search.Text.Trim();

            if (JewelleryName == "")
                return;
            RefreshDataList();
 
        }

        /// <summary>
        /// 点击商品列表中的某一个，弹出详情页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommodityViewCell_SelectedCommodity(object sender, string e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                Data.CommodityMgr.ShowProductDetail(Navigation, e);
            }
        }
    }
}