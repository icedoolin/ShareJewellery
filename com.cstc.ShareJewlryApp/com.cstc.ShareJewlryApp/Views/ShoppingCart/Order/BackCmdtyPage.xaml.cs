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
    public partial class BackCmdtyPage: BasePage 
    {
        /// <summary>
        /// 订单GUID
        /// </summary> 
        public string orderGuid { get; set; } = "";
        ObservableCollection<Data.AfterSaleOrderData> orderList = null;    //售后商品数据源

        Data.DeliveryData 选中的物流 = new Data.DeliveryData();

        bool 按钮防呆 = false;

        public BackCmdtyPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            获取快递公司信息();
        }

        public  void 获取售后商品()
        {
            Tools.AsyncMsg am_获取售后商品 = new Tools.AsyncMsg();

            string para = "OrderGUID=" + orderGuid;

            am_获取售后商品.Completion += (object obj, string ex) =>
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
                    //Device.BeginInvokeOnMainThread( () =>
                    //{
                    //     DisplayAlert("错误", "解析售后商品数据错误！" + exc.Message, "知道了");
                    //    // Navigation.PopAsync(true);
                    //});
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
                    //Device.BeginInvokeOnMainThread( () =>
                    //{
                    //     DisplayAlert("错误", "解析售后商品数据包错误！" + exc.Message, "知道了");
                    //    // Navigation.PopAsync(true);
                    //});
                    return;
                }

                try
                {
                    orderList = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Data.AfterSaleOrderData>>(jar.ToString());

                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread( () =>
                    {
                         //DisplayAlert("错误", "转化售后商品数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                Device.BeginInvokeOnMainThread( () =>
                {
                    绘制列表();
                });

            };

            //am_获取售后商品.Cancel += (object obj, string ex) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        //DisplayAlert("提示", "获取售后商品失败:" + ex.ToString(), "知道了");
            //    });
            //    return;
            //};

            Tools.NetClass.GetStringByUrl("QueryData", "APP\\GetAfterOrderInfoFromOrderGUID_1_0_0_1,APP\\GetAfterOrderDetailedFromOrderGUID_1_0_0_1,APP\\GetAfterOrderPicFromOrderGUID_1_0_0_1", para, am_获取售后商品);
        }

        void 绘制列表()
        {
            st_cmdtydtl.Children.Clear();

            if (orderList == null || orderList.Count <= 0)
                return;


            foreach (var item in orderList)
            {
                var fatherStackLayout = new StackLayout() { Spacing = 0, BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.BackgroundColor, Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };

                int i = 0;//用来画分隔横线用

                var shopImg = new Image() { Source = "shoppingCart_shopLogo.png", HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, BackgroundColor = Color.Transparent, HeightRequest = 20, WidthRequest = 20 };

                var lb_shopName = new Label() { Text = item.ShopName, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center };

                var shopStackLayout = new StackLayout() { Padding = new Thickness(10, 0, 10, 0), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 10, HeightRequest = com.cstc.ShareJewlryApp.Style.Shape.height.barLenth };

                shopStackLayout.Children.Add(shopImg);
                shopStackLayout.Children.Add(lb_shopName);

                fatherStackLayout.Children.Add(shopStackLayout);

                foreach (var row in item.AfterOrderDetailed)
                {

                    i++;
                    var rowStack = new StackLayout() { Padding = new Thickness(10, 10, 0, 0), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 0 };

                    //  TapGestureRecognizer select_tap = new TapGestureRecognizer();
                    //  select_tap.CommandParameter = row;  //加入数据
                    //  select_tap.Tapped += tapped_selectCmdty;

                    //   rowStack.GestureRecognizers.Add(select_tap);//加入点击手势 

                    var stack_右边块 = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 10 };

                    var stack_右边块_上部 = new StackLayout() { Padding = new Thickness(0, 0, 10, 0), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 5 };

                    var img_商品 = new Image() { Source = row.JewelleryPicNameForImg, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, BackgroundColor = Color.Transparent, HeightRequest = 70, WidthRequest = 70 };

                    var stack_右边块_上部_商品名和参数 = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 5 };

                    var lb_商品名 = new com.cstc.ShareJewlryApp.Mycontrols.MultiLineLabel() { Text = row.JewelleryName, Lines = 2, LineBreakMode = LineBreakMode.TailTruncation, TextColor = Color.Black, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Start };

                    var lb_商品名参数 = new Label() { Text = row.specForShow, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.End, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Start };

                    stack_右边块_上部_商品名和参数.Children.Add(lb_商品名);
                    stack_右边块_上部_商品名和参数.Children.Add(lb_商品名参数);

                    stack_右边块_上部.Children.Add(img_商品);
                    stack_右边块_上部.Children.Add(stack_右边块_上部_商品名和参数);
                    stack_右边块.Children.Add(stack_右边块_上部);

                    if (item.OrderState == "购买")
                    {
                        var stack_右边块_下部 = new StackLayout() { Padding = new Thickness(0, 0, 10, 0), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.End, Spacing = 5 };

                        var stack_右边块_下部_数量部分 = new StackLayout() { BackgroundColor = Color.Transparent, BindingContext = item, IsVisible = true, Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, Spacing = 5 };

                        stack_右边块_下部_数量部分.SetBinding(IsVisibleProperty, "isShow");

                        var lb_金额描述 = new Label() { Text = "商品总价:", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center };
                        var lb_商品总价 = new Label() { BindingContext = row, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, Text = row.totalPriceForShow, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center };

                        lb_商品总价.SetBinding(Label.TextProperty, "totalPriceForShow");

                        var box1 = new BoxView() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, WidthRequest = 1 };

                        var lb_数量描述 = new Label() { Text = "数量:", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center };

                        var lb_数量 = new Label() { BindingContext = row, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, Text = row.number.ToString(), WidthRequest = 40, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                        lb_数量.SetBinding(Label.TextProperty, "number");

                        stack_右边块_下部_数量部分.Children.Add(lb_数量描述);
                        stack_右边块_下部_数量部分.Children.Add(lb_数量);

                        stack_右边块_下部.Children.Add(stack_右边块_下部_数量部分);
                        stack_右边块_下部.Children.Add(box1);
                        stack_右边块_下部.Children.Add(lb_金额描述);
                        stack_右边块_下部.Children.Add(lb_商品总价);


                        stack_右边块.Children.Add(stack_右边块_下部);
                    }

                    BoxView box_分割线 = new BoxView() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.End, HeightRequest = 0.5, BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.SpaceColor };
                    stack_右边块.Children.Add(box_分割线);


                    rowStack.Children.Add(stack_右边块);
                    fatherStackLayout.Children.Add(rowStack);
                }

                var st_orderPrice = new StackLayout() { BackgroundColor = Color.Transparent, Padding = new Thickness(10, 10, 10, 10), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };

                var lb_订单号描述 = new Label() { Text = "订单号:", BackgroundColor = Color.Transparent, TextColor = Color.Black, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center, HeightRequest = 30 };
                var lb_订单号 = new Label() { Text = item.OrderID, TextColor = Color.Black, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center, HeightRequest = 30 };

                var box = new BoxView() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, WidthRequest = 1, HeightRequest = 30 };

                st_orderPrice.Children.Add(lb_订单号描述);
                st_orderPrice.Children.Add(lb_订单号);
                st_orderPrice.Children.Add(box);
                if (item.OrderState == "购买")
                {
                    //  var lb_订单金额描述 = new Label() { Text = "订单金额:", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center, HeightRequest = 30 };
                    //  var lb_订单金额 = new Label() { Text = item.TotalPriceForShow, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center, HeightRequest = 30 };

                    //  st_orderPrice.Children.Add(lb_订单金额描述);
                    //  st_orderPrice.Children.Add(lb_订单金额);

                    //  商品总金额 = 商品总金额 + item.TotalPrice;
                }

                fatherStackLayout.Children.Add(st_orderPrice);
                st_cmdtydtl.Children.Add(fatherStackLayout);

            }
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


            //am_获取快递公司.Cancel += (object obj, string ex) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        DisplayAlert("提示", "获取快递公司信息失败:" + ex.ToString(), "知道了");
            //    });
            //    return;
            //};

            Tools.NetClass.GetStringByUrl("QueryData", "App\\GetAllLogisticsCompany", "", am_获取快递公司);
        }

        /// <summary>
        /// 关闭弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_closeBlock(object sender, EventArgs e)
        {
            block.IsVisible = false;
            frm_expressBox.IsVisible = false;
        }

        /// <summary>
        /// 显示物流弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_showExpressBox(object sender, EventArgs e)
        {
            block.IsVisible = true;
            frm_expressBox.IsVisible = true;
        }

        /// <summary>
        /// 确认订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_comfirmOrder(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            string 运单号 = ety_logiticsCode.Text.Trim();

            if (运单号=="" || 选中的物流.LogisticsCompanyName=="")
            {
                按钮防呆 = false;
                return;
            }

            Tools.AsyncMsg am_填写售后物流 = new Tools.AsyncMsg();

            string para = "AfterOrderInvoiceGUID=" + Guid.NewGuid().ToString() + "&AfterOrderGUID=" + orderList[0].AfterOrderGUID + "&OrderGUID=" + orderGuid + "&LogisticsCompanyName="+ 选中的物流.LogisticsCompanyName + "&LogisticsCompanyCode="+ 选中的物流.LogisticsCompanyCode + "&LogisticsNumber="+ 运单号;

            am_填写售后物流.Completion += (object obj, string ex) =>
            {
                ///提交成功后处理
                Device.BeginInvokeOnMainThread(() =>
                {
                    按钮防呆 = false;
                    Navigation.PopAsync(true);
                });
            
            };

            am_填写售后物流.Cancel += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    按钮防呆 = false;
                    //DisplayAlert("提示", "获取快递公司信息失败:" + ex.ToString(), "知道了");
                });
                return;
            };

            Tools.NetClass.GetStringByUrl("ExSql", "App\\InsertInvoice_1_0_0_1", para, am_填写售后物流);

            按钮防呆 = false;
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
    }
}