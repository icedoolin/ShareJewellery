using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.ShoppingCart.Order
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EvaluationPage: BasePage 
    {
        int 星级 = 0;
        /// <summary>
        /// 评价数据源
        /// </summary>
        Data.OrderData _orderData = null;
        public string 页面标志 { get; set; } = "新增";

        /// <summary>
        /// 订单数据
        /// </summary>
        public Data.OrderData orderData
        {
            get
            {
                if (_orderData == null)
                    _orderData = new Data.OrderData();
                return _orderData;
            }

            set
            {
                _orderData = value;
            }
        }

        /// <summary>
        /// 订单GUID
        /// </summary>
        public string orderGuid { get; set; } = "";

        public EvaluationPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }

        public void 获取订单详情()
        {
            Tools.AsyncMsg am_获取订单详情 = new Tools.AsyncMsg();

            string para = "OrderGUID=" + orderGuid;

            am_获取订单详情.Completion += (object obj, string ex) =>
            {
                string returnJson = obj.ToString();
                string ErrMsg = "";

                if (returnJson == "[]" || returnJson == "")
                    return;

                Newtonsoft.Json.Linq.JArray jar = null;
                try
                {
                    returnJson = Tools.CommonClass.GetJsonByTag(returnJson, ref ErrMsg);
                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "解析订单详情数据错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                if (returnJson == "")
                    return;

                try
                {
                    jar = Newtonsoft.Json.Linq.JArray.Parse(returnJson);
                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "解析订单详情数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }


                ObservableCollection<Data.OrderData> temp = null;
                try
                {
                    temp = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Data.OrderData>>(jar.ToString());

                    orderData = temp[0];

                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "转化订单详情数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    UI呈现();
                });
            };

            //am_获取订单详情.Cancel += (object obj, string ex) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        DisplayAlert("提示", "获取订单详情失败:" + ex.ToString(), "知道了");
            //    });
            //    return;
            //};

            Tools.NetClass.GetStringByUrl("QueryData", "APP\\GetOrderInfo_1_0_0_1,APP\\GetOrderDetailed_1_0_0_1,APP\\GetOrderParameter_1_0_0_1,APP\\GetFaultyPic_1_0_0_1", para, am_获取订单详情);
        }

        public void UI呈现()
        {
            ls_evaList.ItemsSource = orderData.CommodityChildrenRows;
            lb_orderID.Text = "订单号:" + orderData.OrderID;
            lb_time.Text = "下单时间:" + orderData.CREATETIME;
            lb_shopTel.Text = "商家电话" + orderData.ShopTel;
        }


        void tapped_oneStar(object sender, TappedEventArgs e)
        {
            var row = (Data.commodityData)e.Parameter;

            if (row.Stars != 1)
            {
                row.Stars = 1;
            }
        }

        void tapped_twoStar(object sender, TappedEventArgs e)
        {
            var row = (Data.commodityData)e.Parameter;

            if (row.Stars != 2)
            {
                row.Stars = 2;
            }

        }

        void tapped_threeStar(object sender, TappedEventArgs e)
        {
            var row = (Data.commodityData)e.Parameter;

            if (row.Stars != 3)
            {
                row.Stars = 3;
            }

        }

        void tapped_fourStar(object sender, TappedEventArgs e)
        {
            var row = (Data.commodityData)e.Parameter;

            if (row.Stars != 4)
            {
                row.Stars = 4;
            }
        }

        void tapped_fiveStar(object sender, TappedEventArgs e)
        {
            var row = (Data.commodityData)e.Parameter;

            if (row.Stars != 5)
            {
                row.Stars = 5;
            }
        }

        private void editor_textChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "请输入您的评价")
            {

            }
            else
            {
                var row = (Data.commodityData)(((Editor)sender).BindingContext);


                foreach (var temp in orderData.CommodityChildrenRows)
                {
                    if (temp.JewelleryGUID == row.JewelleryGUID)
                    {
                        temp.Content = e.NewTextValue;
                    }
                }
            }


        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_comfirmedEvaluation(object sender, EventArgs e)
        {
            if (页面标志 == "新增")
            {
                Tools.AsyncMsg am_新增评价 = new Tools.AsyncMsg();
                #region 构造评价详情

                string orderDetailjson = ""; //组合好的评价
                Dictionary<string, string> 订单评价 = new Dictionary<string, string>();
                foreach (var 各商品 in orderData.CommodityChildrenRows)
                {
                    Dictionary<string, string> 评价 = new Dictionary<string, string>();
                    评价.Add("JewelleryGUID", 各商品.JewelleryGUID);
                    评价.Add("UserGUID", Data.UserInfoCache.UserGUID);
                    评价.Add("Content", 各商品.Content);
                    评价.Add("Stars", 各商品.Stars.ToString());
                    评价.Add("OredrDetailedGUID", 各商品.OredrDetailedGUID);
                    Tools.NetClass.AddPostJsonString(ref orderDetailjson, 评价);
                }

                am_新增评价.Completion += (object obj, string ex) =>
                {
                    //评价成功

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Navigation.PopAsync(true);
                    });
                };

                //am_新增评价.Cancel += (object obj, string ex) =>
                //{
                //    Device.BeginInvokeOnMainThread(() =>
                //    {
                //        DisplayAlert("提示", "评价失败:" + ex.ToString(), "知道了");
                //    });
                //    return;
                //};

                Tools.NetClass.PostDataByUrl("App\\InsertFeedback_1_0_0_1", "ExSql", orderDetailjson, am_新增评价);

                #endregion
            }
            else if (页面标志 == "修改")
            {

            }


        }
    }
}