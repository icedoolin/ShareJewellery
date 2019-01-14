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
    public partial class ProductSoldOutPage: BasePage 
    {
        int PageNumber = 0;
        double 商品行高 = 0;
        bool 下拉刷新 = false;
 
        /// <summary>
        /// 购买珠宝/免费带列表
        /// </summary>
        ObservableCollection<Data.commodityData> dataList { get; set; }
 
        public ProductSoldOutPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            商品行高 = 1.421 * ((Helpers.MConfig.screenWidth - 30) / 2);
            ClearCommodtiyView();
            getCommodityData();
        }

        /// <summary>
        /// 清理商品列表，并且绑定ItemSource=datalist
        /// </summary>
        private void ClearCommodtiyView()
        {
            //初始化
            dataList = new ObservableCollection<Data.commodityData>();
            Device.BeginInvokeOnMainThread(() =>
            {
                ls_commodity.ItemsSource = dataList;
            });
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
            Data.CommodityMgr.GetCommodityDataWithTwoColumn(am_获取商品, PageNumber, "", "", "", "", "", "", 商品行高);

        }


        /// <summary>
        /// 拉到最后刷新新信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ls_commodity_Appearing(object sender, ItemVisibilityEventArgs e)
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

        private void CommodityViewCell_SelectedCommodity(object sender, string e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                Data.CommodityMgr.ShowProductDetail(Navigation, e);
            }
        }
    }
}