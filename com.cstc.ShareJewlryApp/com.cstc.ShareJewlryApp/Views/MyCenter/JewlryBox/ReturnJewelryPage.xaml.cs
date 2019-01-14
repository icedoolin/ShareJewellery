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
    public partial class ReturnJewelryPage: BasePage 
    {
        /// <summary>
        /// 订单GUID
        /// </summary> 
        public string orderGuid { get; set; } = "";
        int UseCrystalAmount = 0;
        decimal 需付清洗费 = 0;    //实际支付的费用
        ObservableCollection<Data.commodityData> orderList = null;    //售后商品数据源

        Data.commodityData item = new Data.commodityData();
        com.cstc.ShareJewlryApp.Tools.IWeChat WXDependency = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.IWeChat>();
        com.cstc.ShareJewlryApp.Tools.Ihud hud = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.Ihud>();
        Data.DeliveryData 选中的物流 = new Data.DeliveryData();
        bool 是否使用心动石 = false;
        bool 按钮防呆 = false;
        bool dataload = false;   //是否已经载入数据
        public ReturnJewelryPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            获取快递公司信息();
            //Tools.CommonClass.获取用户数据();
        }

        void UI呈现()
        {
            lb_CrystaDesc.Text = "拥有： " + Data.UserInfoCache.userInfo.UserCrystal + "个  已抵扣0元";

        }

        /// <summary>
        /// 打开珠宝遗失页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_openJewelryLostPage(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            JewelryLostPage page = new JewelryLostPage();

            Navigation.PushAsync(page, true);

            按钮防呆 = false;
        }

        public void 获取商品()
        {
            Tools.AsyncMsg am_获取商品 = new Tools.AsyncMsg();

            string para = "OrderGUID=" + orderGuid;

            am_获取商品.Completion += (object obj, string ex) =>
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
                        //await DisplayAlert("错误", "解析商品数据错误！" + exc.Message, "知道了");
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
                        //await DisplayAlert("错误", "解析商品数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                try
                {
                    orderList = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Data.commodityData>>(jar.ToString());
                    item = orderList[0];
                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "转化商品数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    //绘制列表();
                    //Tools.CommonClass.获取用户数据();
                    img_cmdty.Source = item.JewelleryPicNameForImg;
                    lb_cmdtyName.Text = item.JewelleryName;
                    lb_cmdtySpec.Text = item.SpecForShow;
                    lb_price.Text = "¥ " + item.CleaningFee.ToString();

                    dataload = true;
 
                    UI呈现();
                    
                });

            };

 

            Tools.NetClass.GetStringByUrl("QueryData", "APP\\GetOrderDetailed_1_0_0_1", para, am_获取商品);
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

        /// <summary>
        /// 选择物流
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_selectExpress(object sender, TappedEventArgs e)
        {
            var row = (Data.DeliveryData)e.Parameter;

            选中的物流 = row;

            lb_logiticsCompanny.RightText = row.LogisticsCompanyName;

            tapped_closeBlock(null, null);
        }


        /// <summary>
        /// 关闭遮罩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_closeBlock(object sender, EventArgs e)
        {
            block.IsVisible = false;
            frm_expressBox.IsVisible = false;
        }

        /// <summary>
        /// 付款提交订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_comfirmOrder(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            string 物流号 = ety_logiticsCode.Text.Trim();

            if (物流号 == "")
            {
                hud.Show_Toast("请输入物流单号");
                按钮防呆 = false;
                return;
            }

            if (选中的物流.LogisticsCompanyCode == "")
            {
                hud.Show_Toast("请选择物流公司");
                按钮防呆 = false;
                return;
            }

            if (需付清洗费 > 0)
            {

                Tools.AsyncMsg am_归还商品 = new Tools.AsyncMsg();

                string ReturnOrderGUID = Guid.NewGuid().ToString();

                string para = "ReturnOrderGUID=" + ReturnOrderGUID + "&OrderGUID=" + orderGuid + "&JewelleryGUID=" + orderList[0].JewelleryGUID + "&OrderDetailGUID=" + orderList[0].OredrDetailedGUID + "&LogisticsCompanyName=" + 选中的物流.LogisticsCompanyName + "&LogisticsCompanyCode=" + 选中的物流.LogisticsCompanyCode + "&LogisticsNumber=" + 物流号 + "&UserGUID=" + Data.UserInfoCache.UserGUID + "&CrystalNum=" + UseCrystalAmount;

                am_归还商品.Completion += (object obje, string exc) =>
                {

                    Views.ShoppingCart.Order.ChoosePayPage page = new ShoppingCart.Order.ChoosePayPage();
                    // page.报损单GUID = 报损单GUID;
                    page.还珠宝单GUID = ReturnOrderGUID;
                    page.支付标志 = "还珠宝";
                    page.totalPrice = 需付清洗费;
                    page.UI呈现();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Navigation.PushAsync(page, true);

                        按钮防呆 = false;
                    });

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

                Tools.NetClass.GetStringByUrl("ExSql", "APP\\InsertReturnJewellery_1_0_0_1", para, am_归还商品);
            }
            else if (需付清洗费 == 0)
            {
                Tools.AsyncMsg am_归还商品 = new Tools.AsyncMsg();

                string para = "ReturnOrderGUID=" + Guid.NewGuid().ToString() + "&OrderGUID=" + orderGuid + "&JewelleryGUID=" + orderList[0].JewelleryGUID + "&OrderDetailGUID=" + orderList[0].OredrDetailedGUID + "&LogisticsCompanyName=" + 选中的物流.LogisticsCompanyName + "&LogisticsCompanyCode=" + 选中的物流.LogisticsCompanyCode + "&LogisticsNumber=" + 物流号 + "&UserGUID=" + Data.UserInfoCache.UserGUID + "&CrystalNum=" + UseCrystalAmount;

                am_归还商品.Completion += (object obje, string exc) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Navigation.PopAsync(true);
                        按钮防呆 = false;

                    });
                    return;
                };

                am_归还商品.Cancel += (object obje, string exc) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        按钮防呆 = false;
                        //DisplayAlert("提示", "归还珠宝失败:" + exc.ToString(), "知道了");
                    });
                    return;
                };

                Tools.NetClass.GetStringByUrl("ExSql", "APP\\UseCrystalInsertReturnJewellery_1_0_0_1", para, am_归还商品);
            }



        }

        /// <summary>
        /// 弹出物流弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_showExpressBox(object sender, EventArgs e)
        {
            block.IsVisible = true;
            frm_expressBox.IsVisible = true;
        }

        /// <summary>
        /// 报损页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_breakage(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            Views.MyCenter.JewlryBox.JewelryLostPage page = new JewelryLostPage();
            page.orderGuid = orderGuid;
            page.orderList = orderList;
            page.UI呈现();

            Navigation.PushAsync(page, true);


            按钮防呆 = false;
        }


        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_closePage(object sender, EventArgs e)
        {

            if (按钮防呆)
                return;
            按钮防呆 = true;
            Navigation.PopAsync(true);
            按钮防呆 = false;

        }


        /// <summary>
        /// 中划线
        /// </summary>
        /// <param name="stringToChange"></param>
        /// <returns></returns>
        private string ConvertToStrikethrough(string stringToChange)
        {
            var newString = "";
            foreach (var character in stringToChange)
            {
                newString += $"{character}\u0336";
            }

            return newString;
        }

        /// <summary>
        /// 使用心动石
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_useDiscount(object sender, EventArgs e)
        {
            if (是否使用心动石) //不适用心动石
            {
                img_checkDiscount.Source = "unchoosed.png";
                是否使用心动石 = false;
                lb_price.Text = "¥ " +item.CleaningFee.ToString();
                lb_originalPrice.IsVisible = false;
                需付清洗费 =item.CleaningFee;
                lb_CrystaDesc.Text = "拥有： " + Data.UserInfoCache.userInfo.UserCrystal.ToString() + "个  已抵扣 0 元";
                ety_CrystalNum.IsEnabled = false;
                ety_CrystalNum.Text = "0";
            }
            else  //使用心动石
            {
                img_checkDiscount.Source = "choosed.png";
                是否使用心动石 = true;
                lb_originalPrice.Text = "¥ " + ConvertToStrikethrough(item.CleaningFee.ToString()); //原价

                lb_originalPrice.IsVisible = true;
                UseCrystalAmount = Convert.ToInt32(item.CleaningFee * Convert.ToDecimal(Helpers.MConfig.CrystalLimit) * 10);
                ety_CrystalNum.IsEnabled = true;
                Cal();




            }
        }

        /// <summary>
        /// 计算使用水晶情况
        /// </summary>
        private void Cal()
        {
            int MaxCrystalAmount = Convert.ToInt32(item.CleaningFee * Convert.ToDecimal(Helpers.MConfig.CrystalLimit) * 10);
            if (MaxCrystalAmount > Data.UserInfoCache.userInfo.UserCrystal)
            {
                MaxCrystalAmount = Data.UserInfoCache.userInfo.UserCrystal;
            }
            if (UseCrystalAmount > MaxCrystalAmount)
            {
                UseCrystalAmount = MaxCrystalAmount;
            }
            需付清洗费 = item.CleaningFee - UseCrystalAmount / 10;
 
            lb_CrystaDesc.Text = "拥有： " + Data.UserInfoCache.userInfo.UserCrystal.ToString() + "个  已抵扣" + (UseCrystalAmount/10).ToString() + "元";
 
            需付清洗费 = item.CleaningFee - UseCrystalAmount / 10;
            lb_price.Text = "¥ " + 需付清洗费.ToString();
            ety_CrystalNum.Text = UseCrystalAmount.ToString();
        }

        /// <summary>
        /// 输入水晶数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ety_CrystalNumChanged(object sender, TextChangedEventArgs e)
        {
            if (dataload != true)
                return;
            try
            {
                UseCrystalAmount = Convert.ToInt32(e.NewTextValue);
                Cal();
            }
            catch(Exception ex)
            {
                ety_CrystalNum.Text = "0";
            }

        }

        private void Pay_Box_PaySuccess(object sender, EventArgs e)
        {

        }
    }
}