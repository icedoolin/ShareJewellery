using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.HomePage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FastDirectPage: BasePage 
    {
        int 分页index = 0;
 
        double 图片宽度 = 0.4 * Helpers.MConfig.screenWidth + 10 + 2;//要加pading 的总高度
        double 商品行高 = 0.4 * Helpers.MConfig.screenWidth;
        bool load = false;
        bool 下拉刷新 = false;


        string SortColumn = "Synthesize";
        string Sort = "Asc";
        //标题
        public string headName
        {
            get
            { return head.Text; }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    head.Text = value;
                });
            }
        }
        //内容标识，用于从数据库获取不同内容
        public int sign { get; set; } = 0;

        ObservableCollection<Data.commodityData> dataList = new ObservableCollection<Data.commodityData>();
 

        public FastDirectPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {

            if (load)
                return;
            load = true;
 
            getCommodityData();
        }

 
        /// <summary>
        /// 获取商品信息
        /// </summary>
        public void getCommodityData()
        {

            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    st_ls_commodity_footer.IsVisible = true;
            //});
            Helpers.AsyncMsg am_获取商品 = new Helpers.AsyncMsg();
            am_获取商品.Completion += (object obj, string e) =>
            {
                try
                {
                    List<Data.commodityData> newList = (List<Data.commodityData>)obj;
                    //将分页数据添加到ItemSource数据中，ObservableCollection可以在内容改变后会通知UI改变
                    foreach (var row in newList)
                    {
                        row.commodityHeight = 商品行高;
                        row.commodityWith = 图片宽度;

                        dataList.Add(row);
                    }

                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                    //    PageNumber++;
                    //    if (newList.Count == 10)
                    //        st_ls_commodity_footer.IsVisible = false;
                    //    下拉刷新 = false;
                    //});

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ls_commodity.ItemsSource = dataList;
                    });

                }
                catch (Exception ex)
                {
                }
            };

            //获取
            Data.CommodityMgr.GetThroughTrainJewellery(am_获取商品, sign.ToString() ,SortColumn,Sort);
 

        }

 
        /// <summary>
        /// 打开商品详情（左边）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_commodity(object sender, TappedEventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                var row = (Data.commodityData)e.Parameter;

                Views.HomePage.ProductDetails.ProductDetailsPage page = new ProductDetails.ProductDetailsPage();
                page.JewelleryGUID = row.JewelleryGUID;
                //page.getCommodityDetailsData();
                //   page.显示商品详情图片();

                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushAsync(page, true);
                });
            }
        }

        /// <summary>
        /// 打开商品详情（右边）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_commodity_right(object sender, TappedEventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                var row = (Data.commodityData)e.Parameter;

                Views.HomePage.ProductDetails.ProductDetailsPage page = new ProductDetails.ProductDetailsPage();
                page.JewelleryGUID = row.rightJewelleryGUID;
                //page.getCommodityDetailsData();
                //  page.显示商品详情图片();

                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushAsync(page, true);

                });
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
            //PageNumber = 0;
            getCommodityData();
        }
    }
}