using com.cstc.ShareJewlryApp.Data;
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
    public partial class ClassificationPage: BasePage 
    {

        ObservableCollection<Data.CommodityStype> Typelist = new ObservableCollection<Data.CommodityStype>();  //款式数据源
        ObservableCollection<Data.CommodityMaterial> Materiallist = new ObservableCollection<Data.CommodityMaterial>();  //材质数据源

        ObservableCollection<Data.commodityData> dataList { get; set; }

 
        public Tools.AsyncMsg am_页面显示 = new Tools.AsyncMsg();

        string SortColumn = "Synthesize";
        string Sort = "ASC";
        int PageNumber = 0;
        bool 下拉刷新 = false;
 
        public string TypeGUID { get; set; } = "";
        string MaterialGUID = "";


        double 商品行高 =  1.49 * ((Helpers.MConfig.screenWidth - (0.3 * Helpers.MConfig.screenWidth) - 20) / 2) + 10;
        double 图片宽度 = 0;
        bool firstload = false;

        public ClassificationPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            //安卓和苹果的阴影效果有很大差异，苹果的时候不用阴影，需要背景色不一样
            if (Device.RuntimePlatform == Device.Android)
            {
                ls_jewellery.BackgroundColor = Color.White;
            }
        }

 

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (firstload)
                return;
            firstload = true;

            获取珠宝款式();
            获取珠宝材质();
            am_页面显示.OnCompletion(null, null);
 
        }
 
        public void 获取珠宝款式()
        {
            Helpers.AsyncMsg am_获取珠宝款式 = new Helpers.AsyncMsg();
            am_获取珠宝款式.Completion += (object obj, string e) =>
            {
                try
                {
                    List<CommodityStype> lists = (List<CommodityStype>)obj;
                    foreach (var item in lists)
                    {
                        Typelist.Add(item);
                    }
 
                }
                catch(Exception ex)
                {

                }
                if (TypeGUID != "")
                {
                    foreach (var temp in Typelist)
                    {
                        if (temp.TypeGUID == TypeGUID)
                        {
                            temp.BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.BackgroundColor;
                            break;
                        }
                    }
                }
                else
                {
                    Typelist[0].BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.SpaceColor;
                }
 
                Device.BeginInvokeOnMainThread(() =>
                {
                    listview_btn.ItemsSource = Typelist;
                    RefreshDataList();
                });

            };
            Data.CommodityMgr.GetJewelleryType(am_获取珠宝款式);
 

        }

        public void 获取珠宝材质()
        {
            Helpers.AsyncMsg am_获取珠宝材质 = new Helpers.AsyncMsg();
            am_获取珠宝材质.Completion += (object obj, string e) =>
            {
                try
                {
                    List<CommodityMaterial> lists = (List<CommodityMaterial>)obj;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        listview_Material.ItemsSource = Materiallist;
                    });
                }
                catch (Exception ex) { }
            };
            Data.CommodityMgr.GetJewelleryMaterial(am_获取珠宝材质);
 
        }
 
        /// <summary>
        /// 款式选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void list_item(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectItem = (CommodityStype)((ListView)sender).SelectedItem;
                foreach (var item in Typelist)
                {
                    if (e.SelectedItem == item)
                    {
                        item.BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.SpaceColor;
                    }
                    else
                    {
                        // item.BackgroundColor = Color.FromHex("#f0f0f0");
                        item.BackgroundColor = Color.White;
                    }
                }
 
                TypeGUID = selectItem.TypeGUID;
                if (selectItem.TypeName == "全部") TypeGUID = "";
                MaterialGUID = "";
                RefreshDataList();
            }
        }

        /// <summary>
        /// 材质选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void list_Materialitem(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectItem = (CommodityMaterial)((ListView)sender).SelectedItem;
                foreach (var item in Materiallist)
                {
                    if (e.SelectedItem == item)
                    {
                        item.BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.SpaceColor;
                       // item.BackgroundColor = Color.White;
                    }
                    else
                    {
                        item.BackgroundColor = Color.White;
                        //item.BackgroundColor = Color.FromHex("#f0f0f0");
                    }
                }
 
                TypeGUID = "";  

                MaterialGUID = selectItem.MaterialGUID;
                if (selectItem.MaterialName == "全部") MaterialGUID = "";
                RefreshDataList();
            }
        }

        private void 款式切换(object sender, EventArgs e)
        {
            if (lbl_选择.Text.Equals("珠宝款式"))
            {
                st_Material.IsVisible = true;
                lbl_选择.Text = "珠宝材质";

            }
            else
            {
                st_Material.IsVisible = false;
                lbl_选择.Text = "珠宝款式";

            }
        }
 
        /// <summary>
        /// 下拉自动加载新信息
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

            if (dataList != null && row == dataList[dataList.Count - 1])
            {

                下拉刷新 = true;

                getCommodityData();

            }
        }
        
        /// <summary>
        /// 打开搜索页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_searchPage(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Views.HomePage.SearchPage page = new HomePage.SearchPage();
                    Navigation.PushAsync(page, true);
                });
            }
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
            ls_jewellery.ItemsSource = dataList;
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
                        if (newList.Count == 5)
                            st_ls_commodity_footer.IsVisible = false;
                        下拉刷新 = false;
                    });
                }
                catch (Exception)
                {
                }
            };
 
            //获取
            Data.CommodityMgr.GetCommodityDataWithTwoColumn(am_获取商品, PageNumber, TypeGUID, MaterialGUID, "", SortColumn, Sort, "", 商品行高);
        }

        /// <summary>
        /// 选中某个商品
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
    }
}