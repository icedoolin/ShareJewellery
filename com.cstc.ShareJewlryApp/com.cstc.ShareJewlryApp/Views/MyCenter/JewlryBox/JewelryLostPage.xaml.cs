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
    public partial class JewelryLostPage: BasePage 
    {
        public string orderGuid { get; set; } = "";
        public ObservableCollection<Data.commodityData> orderList { get; set; }    //售后商品数据源
        public string 报损类型 = "";
        bool 按钮防呆 = false;
        Data.DeliveryData 选中的物流 = null;
        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();

        public JewelryLostPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            获取快递公司信息();
        }

        void 获取快递公司信息()
        {
            Tools.AsyncMsg am_获取快递公司 = new Tools.AsyncMsg();

            am_获取快递公司.Completion += (object obj, string ex) =>
            {
                string returnJson = obj.ToString();
                string ErrMsg = "";
                if (returnJson == "[]" || returnJson == "")
                    return;

                try
                {
                    returnJson = Tools.CommonClass.GetJsonByTag(returnJson, ref ErrMsg);
                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "解析快递公司数据错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                if (returnJson == "")
                    return;

                Newtonsoft.Json.Linq.JArray jar = null;
                try
                {
                    jar = Newtonsoft.Json.Linq.JArray.Parse(returnJson);
                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "解析快递公司数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                ObservableCollection<Data.DeliveryData> dataList = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Data.DeliveryData>>(jar.ToString());

                dataList[dataList.Count - 1].IsLast = false;

                Device.BeginInvokeOnMainThread(() =>
                {
                    //ls_myJewelryBox.ItemsSource = dataList;
                    ls_express.ItemsSource = dataList;
                });
            };
 
            Tools.NetClass.GetStringByUrl("QueryData", "App\\GetAllLogisticsCompany", "", am_获取快递公司);
        }

        public void UI呈现()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                img_cmdty.Source = orderList[0].JewelleryPicNameForImg;
                lb_cmdtyName.Text = orderList[0].JewelleryName;
                lb_cmdtySpec.Text = orderList[0].specForShow;
                lb_cmdtyPrice.Text = orderList[0].PriceForShow;
                lb_totalprice.Text = orderList[0].PriceForShow;
            });
        }

        /// <summary>
        /// 弹出赔偿类型框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_ShowDamageType(object sender, EventArgs e)
        {
            block.IsVisible = true;
            frm_damageTypeBox.IsVisible = true;
        }

        /// <summary>
        /// 关闭遮罩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_closeBlock(object sender, EventArgs e)
        {
            block.IsVisible = false;
            frm_damageTypeBox.IsVisible = false;
            frm_expressBox.IsVisible = false;
        }

        /// <summary>
        /// 全额赔偿
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_sumDamageType(object sender, EventArgs e)
        {
            lb_damageType.RightText = "遗失（全额赔偿）";
            lb_priceDes.Text = "全额赔偿：";
            lb_priceDes.IsVisible = true;
            lb_totalprice.IsVisible = true;
            btn_ConfirmedDes.Text = "确认支付";
            btn_ConfirmedDes.IsVisible = true;

            tapped_closeBlock(null, null);
            报损类型 = "遗失";



            lb_LogisticsCompanyName.IsVisible = false;
            st_LogiticsCode.IsVisible = false;
        }

        /// <summary>
        /// 折价赔偿
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_discountDamageType(object sender, EventArgs e)
        {
            lb_damageType.RightText = "报损";
            lb_priceDes.Text = "报损金额将在商家收到货品7天内提供";
            lb_priceDes.IsVisible = true;
            lb_totalprice.IsVisible = false;
            btn_ConfirmedDes.Text = "送回珠宝";
            btn_ConfirmedDes.IsVisible = true;

            tapped_closeBlock(null, null);
            报损类型 = "报损";

            lb_LogisticsCompanyName.IsVisible = true;
            st_LogiticsCode.IsVisible = true;
        }

        /// <summary>
        /// 确认支付
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void taped_payDamage(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;


            if (报损类型 == "报损")
            {

                string 物流号 = ety_logiticsCode.Text.Trim();

                if (报损类型 == "")
                {
                    按钮防呆 = false;
                    return;
                }

                if (物流号 == "")
                {
                    按钮防呆 = false;
                    return;
                }

                Tools.AsyncMsg am_归还商品 = new Tools.AsyncMsg();

                //  string para = "OrderGUID=" + orderGuid + "&JewelleryGUID=" + orderList[0].JewelleryGUID + "&OrderDetailGUID=" + orderList[0].OredrDetailedGUID + "&LogisticsCompanyName=" + 选中的物流.LogisticsCompanyName + "&LogisticsCompanyCode=" + 选中的物流.LogisticsCompanyCode + "&LogisticsNumber=" + 物流号 + "&UserGUID=" + Data.UserInfoCache.UserGUID;

                string para = "OrderGUID=" + orderGuid + "&JewelleryGUID=" + orderList[0].JewelleryGUID + "&OrderDetailGUID=" + orderList[0].OredrDetailedGUID + "&LogisticsCompanyName=" + 选中的物流.LogisticsCompanyName + "&LogisticsCompanyCode=" + 选中的物流.LogisticsCompanyCode + "&LogisticsNumber=" + 物流号 + "&UserGUID=" + Data.UserInfoCache.UserGUID;

                am_归还商品.Completion += (object obje, string exc) =>
                {
                    Tools.AsyncMsg am_报损 = new Tools.AsyncMsg();

                    para = "OrderGUID=" + orderGuid + "&Sign=2&FaultyGUID=" + Guid.NewGuid().ToString();

                    am_报损.Completion += (object obj, string ex) =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            按钮防呆 = false;
                            hud.Show_Toast("报损成功");
                            Navigation.PopAsync(true);
                        }); 
                    };

                    am_报损.Cancel += (object obj, string ex) =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            按钮防呆 = false;
                            //DisplayAlert("提示", "报损失败:" + exc.ToString(), "知道了");
                        });
                        return;
                    };

                    Tools.NetClass.GetStringByUrl("ExSql", "App\\UserFaulty_1_0_0_1", para, am_报损);
                };

                am_归还商品.Cancel += (object obje, string exc) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        按钮防呆 = false;
                        //DisplayAlert("提示", "用户提交快递单失败:" + exc.ToString(), "知道了");
                    });
                    return;
                };

                Tools.NetClass.GetStringByUrl("ExSql", "App\\InsertReturnAfterOrderJewellery_1_0_0_1", para, am_归还商品);
            }
            else if (报损类型 == "遗失")
            {
                Tools.AsyncMsg am_报损 = new Tools.AsyncMsg();

                string 报损单GUID = Guid.NewGuid().ToString();

                string para = "OrderGUID=" + orderGuid + "&Sign=1&FaultyGUID=" + 报损单GUID;

                am_报损.Completion += (object obje, string exc) =>
                {
                    ////支付赔偿
                    Views.ShoppingCart.Order.ChoosePayPage page = new ShoppingCart.Order.ChoosePayPage();
                    page.报损单GUID = 报损单GUID;
                    page.支付标志 = "全额赔偿";
                    page.totalPrice = orderList[0].Price;
                    page.UI呈现();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Navigation.PushAsync(page, true);
                    });

                    按钮防呆 = false;


                };

                am_报损.Cancel += (object obje, string exc) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        按钮防呆 = false;
                        //DisplayAlert("提示", "用户报损失败:" + exc.ToString(), "知道了");
                    });
                    return;
                };

                Tools.NetClass.GetStringByUrl("ExSql", "App\\UserFaulty_1_0_0_1", para, am_报损);
            }
        }

        /// <summary>
        /// 弹出选择物流框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_ShowLogisticsCompany(object sender, EventArgs e)
        {
            block.IsVisible = true;
            frm_expressBox.IsVisible = true;
        }

        /// <summary>
        /// 选择物流
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_selectExpress(object sender, TappedEventArgs e)
        {
            选中的物流 = (Data.DeliveryData)e.Parameter;

            lb_LogisticsCompanyName.RightText = 选中的物流.LogisticsCompanyName;

            block.IsVisible = false;
            frm_expressBox.IsVisible = false;
        }
    }
}