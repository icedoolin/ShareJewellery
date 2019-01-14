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
    public partial class OrderPage: BasePage 
    {
        string 当前地址GUID = "";
        int 商品总数 = 0;
        decimal 商品总金额 = 0;
        public string 订单标志 { get; set; } = "";
        ObservableCollection<Data.OrderData> _orderList = null;
        public ObservableCollection<Data.OrderData> orderList
        {
            get
            {
                if (_orderList == null)
                    _orderList = new ObservableCollection<Data.OrderData>();
                return _orderList;
            }

            set
            {
                _orderList = value;
            }
        }
        public OrderPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            // 测试();
            getDefaultAddress();
            //Tools.CommonClass.获取用户数据();
        }
 
        /// <summary>
        /// 获取默认地址
        /// </summary>
        void getDefaultAddress()
        {
            Tools.AsyncMsg am_获取默认地址 = new Tools.AsyncMsg();

            am_获取默认地址.Completion += (object obj, string ex) =>
            {
                string returnJson = obj.ToString();
                string ErrMsg = "";
                if (returnJson == "[]" || returnJson == "")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        st_address.IsVisible = false;
                        lb_tips.IsVisible = true;
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
                        //await DisplayAlert("错误", "解析地址数据错误！" + exc.Message, "知道了");
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
                        //await DisplayAlert("错误", "解析默认收货地址数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                #endregion

                ObservableCollection<Data.ReceiptAddressData> list = null;

                try
                {
                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Data.ReceiptAddressData>>(jar.ToString());
                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "转换默认收货地址数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    lb_recName.Text = list[0].AddresseeForShow;
                    lb_recTel.Text = list[0].TelForShow;
                    lb_address.Text = list[0].CityNameWithDetailedAddress;
                    当前地址GUID = list[0].ReceivingAddressGUID;
                });
            };

            //am_获取默认地址.Cancel += (object obj, string ex) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        DisplayAlert("提示", "获取默认收货地址失败:" + ex.ToString(), "知道了");
            //    });
            //    return;
            //};

            Tools.NetClass.GetStringByUrl("QueryData", "App\\GetDefaultAddress_1_0_0_1", "UserGUID=" + Data.UserInfoCache.UserGUID, am_获取默认地址);
        }

        public void 获取订单号()
        {
            Tools.AsyncMsg am_获取订单号 = new Tools.AsyncMsg();

            am_获取订单号.Completion += (object obj, string ex) =>
            {
                string returnJson = obj.ToString();
                string ErrMsg = "";
                if (returnJson == "[]" || returnJson == "")
                    return;

                #region json转jarry
                try
                {
                    returnJson = Tools.CommonClass.GetJsonByTag(returnJson, ref ErrMsg);
                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "解析订单号数据错误！" + exc.Message, "知道了");
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
                        //await DisplayAlert("错误", "解析订单号数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                #endregion

                ObservableCollection<Data.OrderData> list = null;

                try
                {
                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Data.OrderData>>(jar.ToString());
                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "转换订单号数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                }

                for (int i = 0; i < orderList.Count; i++)
                {
                    orderList[i].OrderID = list[i].OrderID;
                }

                Device.BeginInvokeOnMainThread(() =>
               {
                   绘制列表();
               });
            };

            //am_获取订单号.Cancel += (object obj, string ex) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        DisplayAlert("提示", "获取订单号失败:" + ex.ToString(), "知道了");
            //    });
            //    return;
            //};

            Tools.NetClass.GetStringByUrl("QueryData", "App\\GetAllOrderID_1_0_0_1", "OrderIDNum=" + orderList.Count, am_获取订单号);
        }

        public  void 绘制列表()
        {
            if (orderList == null || orderList.Count <= 0)
                return;

            foreach (var item in orderList)
            {
                var fatherStackLayout = new StackLayout() { Padding = new Thickness(10, 10, 0, 0), Spacing = 0, BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.BackgroundColor, Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };

                var shopImg = new Image() { Source = item.ShopLogoForImg, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, BackgroundColor = Color.Transparent, HeightRequest = 20, WidthRequest = 20 };

                var lb_shopName = new Label() { Text = item.ShopName,FontSize=com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center };

                var shopStackLayout = new StackLayout() { Padding = new Thickness(0, 0, 0, 0), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 10 };

                shopStackLayout.Children.Add(shopImg);
                shopStackLayout.Children.Add(lb_shopName);

                fatherStackLayout.Children.Add(shopStackLayout);

                int i = 0;//用来画分隔横线用
                foreach (var row in item.CommodityChildrenRows)
                {
                    i++;
                    商品总数++;
                    var rowStack = new StackLayout() { Padding = new Thickness(10, 10, 0, 0), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 5 };

                    var stack_右边块 = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 5 };

                    var stack_右边块_上部 = new StackLayout() { Padding = new Thickness(0, 0, 10, 0), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 5 };

                    var img_商品 = new Image() { Source = row.JewelleryPicNameForImg, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, BackgroundColor = Color.Transparent, HeightRequest = 80, WidthRequest = 80 };

                    var stack_右边块_上部_商品名和参数 = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 5 };

                    var lb_商品名 = new Label() { Text = row.JewelleryName, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Start };

                    var lb_商品名参数 = new Label() { Text = row.specForShow, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.End, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Start };

                    stack_右边块_上部_商品名和参数.Children.Add(lb_商品名);
                    stack_右边块_上部_商品名和参数.Children.Add(lb_商品名参数);

                    stack_右边块_上部.Children.Add(img_商品);
                    stack_右边块_上部.Children.Add(stack_右边块_上部_商品名和参数);

                    stack_右边块.Children.Add(stack_右边块_上部);

                    if (订单标志 == "购买")
                    {
                        var stack_右边块_下部 = new StackLayout() { Padding = new Thickness(0, 0, 10, 0), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.End, Spacing = 5 };

                        var stack_右边块_下部_数量部分 = new StackLayout() { BackgroundColor = Color.Transparent, BindingContext = item, IsVisible = true, Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, Spacing = 5 };

                        stack_右边块_下部_数量部分.SetBinding(IsVisibleProperty, "isShow");

                        var lb_金额描述 = new Label() { Text = "商品总价:", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10,HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center };
                        var lb_商品总价 = new Label() { BindingContext = row, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, Text = row.totalPriceForShow, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center };

                        lb_商品总价.SetBinding(Label.TextProperty, "totalPriceForShow");

                        var box = new BoxView() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, WidthRequest = 1 };

                        var lb_数量描述 = new Label() { Text = "数量:", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center };

                        var img_减号 = new Image() { Source = "reduce.png", HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, BackgroundColor = Color.Transparent, HeightRequest = 20, WidthRequest = 20 };

                        TapGestureRecognizer reduce_tap = new TapGestureRecognizer();
                        reduce_tap.CommandParameter = row;  //加入数据
                        reduce_tap.Tapped += tapped_reduce;

                        img_减号.GestureRecognizers.Add(reduce_tap);//加入点击手势


                        var lb_数量 = new Label() { BindingContext = row, Text = row.number.ToString(), FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, WidthRequest = 50, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                        lb_数量.SetBinding(Label.TextProperty, "number");

                        var img_加号 = new Image() { Source = "plus.png", HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, BackgroundColor = Color.Transparent, HeightRequest = 20, WidthRequest = 20 };

                        TapGestureRecognizer plus_tap = new TapGestureRecognizer();
                        plus_tap.CommandParameter = row;  //加入数据
                        plus_tap.Tapped += tapped_plus;

                        img_加号.GestureRecognizers.Add(plus_tap);//加入点击手势


                        stack_右边块_下部_数量部分.Children.Add(lb_数量描述);
                        stack_右边块_下部_数量部分.Children.Add(img_减号);
                        stack_右边块_下部_数量部分.Children.Add(lb_数量);
                        stack_右边块_下部_数量部分.Children.Add(img_加号);

                        stack_右边块_下部.Children.Add(lb_金额描述);
                        stack_右边块_下部.Children.Add(lb_商品总价);
                        stack_右边块_下部.Children.Add(box);
                        stack_右边块_下部.Children.Add(stack_右边块_下部_数量部分);

                        stack_右边块.Children.Add(stack_右边块_下部);
                        BoxView box_分割线 = new BoxView() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.End, HeightRequest = 0.5, BackgroundColor = Color.Silver };
                        stack_右边块.Children.Add(box_分割线);
                    }

                    
                    rowStack.Children.Add(stack_右边块);
                    fatherStackLayout.Children.Add(rowStack);

                }

                if (订单标志 == "购买")
                {
                    var st_orderPrice = new StackLayout() { BackgroundColor = Color.Transparent, Padding = new Thickness(10, 10, 10, 10), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start ,HeightRequest=com.cstc.ShareJewlryApp.Style.Shape.height.barLenth };

                  //  var lb_订单号描述 = new Label() { Text = "订单号:", HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center };
                  //  var lb_订单号 = new Label() { BindingContext = item, Text = item.OrderID, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center };

                    var box = new BoxView() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, WidthRequest = 1 };

                    var lb_订单金额描述 = new Label() { Text = "订单金额:", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center };
                    var lb_订单金额 = new Label() { BindingContext = item, Text = item.TotalPriceForShow, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center };
                    lb_订单金额.SetBinding(Label.TextProperty, "TotalPriceForShow");

                    // st_orderPrice.Children.Add(lb_订单号描述);
                    // st_orderPrice.Children.Add(lb_订单号);
                    st_orderPrice.Children.Add(box);

                    st_orderPrice.Children.Add(lb_订单金额描述);
                    st_orderPrice.Children.Add(lb_订单金额);

                    fatherStackLayout.Children.Add(st_orderPrice);

                    商品总金额 = 商品总金额 + item.TotalPrice;
                }

                st_prduct.Children.Add(fatherStackLayout);
            }

            if (订单标志 == "购买")
            {
                lb_totalCmdy.Text = "共" + 商品总数 + "件商品";
                lb_totalPrice.Text = "¥" + 商品总金额;
                商品总金额 = 0;
            }
            else if(订单标志 == "租借")
            {
                lb_totalCmdy.Text = "免费戴商品无需支付";
                lb_tptalPriceDesc.IsVisible = false;
                lb_totalPrice.IsVisible = false;
            }
            
        }

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_takeOrder(object sender, EventArgs e)
        {
            string 备注 = ety_remarks.Text.Trim();

            if(当前地址GUID=="")
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("提示", "没有收货地址", "知道了");
                });
                return;
 
            }
               

            Tools.AsyncMsg am_新增订单 = new Tools.AsyncMsg();

            #region 构造订单详情
            string orderjson = "";          //组合好的订单详情
                                            //组合好的商品详情
            foreach (var 店铺订单 in orderList)
            {
                string orderDetailjson = "";
                Dictionary<string, string> 订单详情 = new Dictionary<string, string>();
                foreach (var 商品 in 店铺订单.CommodityChildrenRows)
                {
                    Dictionary<string, string> 商品详情 = new Dictionary<string, string>();
                    商品详情.Add("OredrDetailedGUID", Guid.NewGuid().ToString());
                    商品详情.Add("SecondSpecGUID", 商品.SecondSpecGUID);
                    商品详情.Add("JewelleryGUID", 商品.JewelleryGUID);
                    商品详情.Add("number", 商品.number.ToString());
                    Tools.NetClass.AddPostJsonString(ref orderDetailjson, 商品详情);
                }

                订单详情.Add("OrderGUID", Guid.NewGuid().ToString());
                订单详情.Add("ShopGUID", 店铺订单.ShopGUID);
                订单详情.Add("UserGUID", Data.UserInfoCache.UserGUID);
                订单详情.Add("OrderState", 订单标志);
                订单详情.Add("Remarks", 备注);
                订单详情.Add("ReceivingAddressGUID", 当前地址GUID);
                订单详情.Add("ChildRows_APP\\\\InsertOredrDetailed_1_0_0_1", "[" + orderDetailjson + "]");
                Tools.NetClass.AddPostJsonString(ref orderjson, 订单详情);
            }
            #endregion


            am_新增订单.Completion += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if(订单标志 =="购买")
                    {
                        decimal 总价 = 0;
                        foreach(var 订单 in orderList)
                        {
                            总价 = 总价 + 订单.TotalPrice;
                        }
                        Views.ShoppingCart.Order.ChoosePayPage page = new ChoosePayPage();
                        page.totalPrice = 总价;
                        page.UI呈现();
                        Navigation.PushAsync(page, true);
                        //Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                    }
                    else
                    {
                        OrderSucessPage page = new OrderSucessPage();
                        page.flag = "免费戴";
                        page.UI呈现();
                        Navigation.PushAsync(page, true);
                        //Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                    }
                   
                });
              
            };

            //am_新增订单.Cancel += (object obj, string ex) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        DisplayAlert("提示", "新增订单失败", "知道了");
            //    });
            //    return;
            //};

            Tools.NetClass.PostDataByUrl("App\\InsertOrder_1_0_0_1", "ExSql", orderjson, am_新增订单);
        }

        /// <summary>
        /// 选择地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_seleceAddress(object sender, EventArgs e)
        {
            Views.ShoppingCart.ReceiptAddress.ReceiptAddressPage page = new ReceiptAddress.ReceiptAddressPage();
            page.入口标志 = "订单";
            page.当前选中地址GUID = 当前地址GUID;

            page.am_选择地址.Completion += (object obj, string ex) =>
            {
                var row = (Data.ReceiptAddressData)obj;

                Device.BeginInvokeOnMainThread(() =>
                {
                    st_address.IsVisible = true;
                    lb_tips.IsVisible = false;
                    lb_recName.Text = row.AddresseeForShow;
                    lb_recTel.Text = row.TelForShow;
                    lb_address.Text = row.CityNameWithDetailedAddress;
                    当前地址GUID = row.ReceivingAddressGUID;
                });
            };

            page.am_选择地址.Cancel += (object obj, string ex) =>
            {

            };

            Navigation.PushAsync(page, true);
        }
        
        /// <summary>
        /// “+”按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_reduce(object sender, EventArgs e)
        {
            var row = (Data.commodityData)(((TappedEventArgs)e).Parameter);

            string s = row.JewelleryGUID;
            if (row.number <= 1)
                return;
            row.number--;

            #region 触发改变当行订单金额
            foreach (var tempItem in orderList)
            {
                foreach (var temp in tempItem.CommodityChildrenRows)
                {
                    if (temp.JewelleryGUID == s)
                    {
                        tempItem.TotalPrice = 1;
                    }
                }
                商品总金额 = 商品总金额 + tempItem.TotalPrice;
            }
            #endregion

            lb_totalPrice.Text = "¥" + 商品总金额;
            商品总金额 = 0;
        }

        /// <summary>
        /// “-”按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_plus(object sender, EventArgs e)
        {
            var row = (Data.commodityData)(((TappedEventArgs)e).Parameter);

            string s = row.JewelleryGUID;

            row.number++;

            #region 触发改变当行订单金额
            foreach (var tempItem in orderList)
            {
                foreach (var temp in tempItem.CommodityChildrenRows)
                {
                    if (temp.JewelleryGUID == s)
                    {
                        tempItem.TotalPrice = 1;
                    }
                }
                商品总金额 = 商品总金额 + tempItem.TotalPrice;
            }
            #endregion

            lb_totalPrice.Text = "¥" + 商品总金额;
            商品总金额 = 0;

        }
    }
}