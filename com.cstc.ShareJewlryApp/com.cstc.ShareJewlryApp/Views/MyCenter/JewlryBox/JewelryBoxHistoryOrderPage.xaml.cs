using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.MyCenter.JewlryBox
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class JewelryBoxHistoryOrderPage: BasePage 
	{
        bool 按钮防呆 = false;
        int 分页index = 0;
        bool 下拉刷新 = false;
        double 商品行高 = 1.466 * ((Helpers.MConfig.screenWidth - 1) / 2);
        double 图片宽度 = (Helpers.MConfig.screenWidth - 1) / 2;


        ObservableCollection<Data.OrderData> orderList = new ObservableCollection<Data.OrderData>();
        public JewelryBoxHistoryOrderPage ()
		{
			InitializeComponent ();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
         
            getData();
            
        }

        /// <summary>
        /// 获取免费戴历史信息
        /// </summary>
        public void getData()
        {
            Tools.AsyncMsg am_获取免费戴历史 = new Tools.AsyncMsg();

            string para = "UserGUID=" + Data.UserInfoCache.UserGUID + "&PageNumber=" + 分页index;

            am_获取免费戴历史.Completion += (object obj, string ex) =>
            {
                下拉刷新 = false;
                string returnJson = obj.ToString();
                string ErrMsg = "";
                if ((returnJson == "[]" || returnJson == "" )&& 分页index==0)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        lb_tip.IsVisible = true;
                    });
                  
                    return;
                }
                  

                #region json转jarry
                try
                {
                    returnJson = Tools.CommonClass.GetJsonByTag(returnJson, ref ErrMsg);
                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "解析免费戴历史数据错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                if (returnJson == "")
                {
                    return;
                }

                Newtonsoft.Json.Linq.JArray jar = null;
                try
                {
                    jar = Newtonsoft.Json.Linq.JArray.Parse(returnJson);
                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "解析免费戴历史数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                #endregion
                ObservableCollection<Data.OrderData> tempList = null;

                try
                {
                    tempList = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Data.OrderData>>(jar.ToString());
                }
                catch (Exception exc)
                {
                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                    //    DisplayAlert("提示", "转换免费戴历史数据包失败:" + ex.ToString(), "知道了");
                    //});
                }

                ObservableCollection<Data.OrderData> newList = new ObservableCollection<Data.OrderData>();

                for (int i = 0; i < tempList.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        tempList[i].commodityHeight = 商品行高;
                        tempList[i].commodityWith = 图片宽度;
                        #region 添加左边列表数据

                        newList.Add(tempList[i]);
                        #endregion
                    }
                    else
                    {
                        #region 添加右边列表数据
                        if (!string.IsNullOrEmpty(tempList[i].JewelleryNameForFreeDress.ToString()))
                        {
                            newList[i / 2].rightCommodityChildrenRows = tempList[i].CommodityChildrenRows;
                            newList[i / 2].rightOrderType = tempList[i].OrderType;
                            newList[i / 2].rightIsShow = true; 
                        }
                        else
                        {
                            newList[i / 2].rightIsShow = false;
                        }
                        #endregion
                    }
                }





                foreach (var temp in newList)
                {
                    orderList.Add(temp);
                }

                if(分页index<1)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        lb_tip.IsVisible = false;
                        ls_list.ItemsSource = orderList;
                    });
                }
               
                分页index++;
            };
 
            Tools.NetClass.GetStringByUrl("QueryData", "APP\\GetAllAllowRentOrder_1_0_0_1,APP\\GetAllOrderDetailed_1_0_0_1,APP\\GetAllFeedback_1_0_0_1", para, am_获取免费戴历史);
        }

        private void ls_list_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var row = new Data.OrderData();

            try
            {
                row = (Data.OrderData)e.Item;
            }
            catch (Exception ex)
            {

            }

            if (下拉刷新)
                return;

            if (row == orderList[orderList.Count - 1])
            {
                下拉刷新 = true;
                getData();
            }
        }

        /// <summary>
        /// 免费戴左边商品 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_selectItem_left(object sender, TappedEventArgs e)
        {
            if (按钮防呆)
                return;

            按钮防呆 = true;

            var row = (Data.OrderData)e.Parameter;


            if(row.CommodityChildrenRows[0].JewelleryState==false)
            {
                Views.HomePage.ProductDetails.ProductSoldOutPage page = new HomePage.ProductDetails.ProductSoldOutPage();
                Navigation.PushAsync(page, true);
            }
            else
            {
                Views.HomePage.ProductDetails.ProductDetailsPage page = new HomePage.ProductDetails.ProductDetailsPage();

                page.JewelleryGUID = row.CommodityChildrenRows[0].JewelleryGUID;

              //  page.显示商品详情图片();

                //page.getCommodityDetailsData();

                Navigation.PushAsync(page, true);
            }

            按钮防呆 = false;
        }

        /// <summary>
        /// 免费戴右边商品 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_selectItem_right(object sender, TappedEventArgs e)
        {
            if (按钮防呆)
                return;

            按钮防呆 = true;

            var row = (Data.OrderData)e.Parameter;


            if (row.rightCommodityChildrenRows[0].JewelleryState == false)
            {
                Views.HomePage.ProductDetails.ProductSoldOutPage page = new HomePage.ProductDetails.ProductSoldOutPage();
                Navigation.PushAsync(page, true);
            }
            else
            {
                Views.HomePage.ProductDetails.ProductDetailsPage page = new HomePage.ProductDetails.ProductDetailsPage();

                page.JewelleryGUID = row.rightCommodityChildrenRows[0].JewelleryGUID;

                //  page.显示商品详情图片();

                //page.getCommodityDetailsData();

                Navigation.PushAsync(page, true);
            }

            按钮防呆 = false;
        }
    }
}