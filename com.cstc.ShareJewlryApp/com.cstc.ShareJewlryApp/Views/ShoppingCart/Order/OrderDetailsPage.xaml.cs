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
    public partial class OrderDetailsPage: BasePage 
    {
        decimal 商品总金额 = 0;
        int 商品总数 = 0;
        bool 按钮防呆 = false;
        bool firstLoad = true;

        ObservableCollection<Data.OrderData> orderList = null;
        public string OrderGUID { get; set; } = "";

        public OrderDetailsPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (firstLoad)
            {
                firstLoad = false;
                return;
            }
            getOrderDtl();
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        public void getOrderDtl()
        {

            Tools.AsyncMsg am_获取订单详情 = new Tools.AsyncMsg();

            string para = "OrderGUID=" + OrderGUID;

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

                try
                {
                    orderList = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Data.OrderData>>(jar.ToString());

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
                    lb_recName.Text = "收件人： " + orderList[0].Addressee;
                    lb_recTel.Text = "手机： " + orderList[0].Tel;
                    lb_address.Text = orderList[0].DetailedAddress;

                    绘制购物车列表();
                    ls_OrderPara.HeightRequest = orderList[0].OrderParameter.Count * com.cstc.ShareJewlryApp.Style.Shape.height.MycenterbarLenth;
                    ls_OrderPara.ItemsSource = orderList[0].OrderParameter;
                    UI呈现();

                    if(orderList[0].FaultyPic.Count>0)
                    {
                        st_afterSaleImg.IsVisible = true;
                        添加报损图片(orderList[0].FaultyPic);
                    }
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

        void 添加报损图片(ObservableCollection<Data.FaultyPicData> tempList)
        {
            st_afterSaleImg.Children.Clear();
            var stack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 10, HeightRequest = com.cstc.ShareJewlryApp.Style.Shape.height.afterSaleImgHetght };

            st_afterSaleImg.Children.Add(stack);

            foreach (var temp in orderList[0].FaultyPic)
            {
                var row = (StackLayout)st_afterSaleImg.Children[st_afterSaleImg.Children.Count - 1];

                if (row.Children.Count == 5)
                {
                    var newStack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 10, HeightRequest = com.cstc.ShareJewlryApp.Style.Shape.height.afterSaleImgHetght };

                    var newImg = new Image { Source = temp.FaultyPicForShow, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, HeightRequest = com.cstc.ShareJewlryApp.Style.Shape.height.afterSaleImgHetght, WidthRequest = com.cstc.ShareJewlryApp.Style.Shape.height.afterSaleImgHetght };

                    newStack.Children.Add(newImg);

                    st_afterSaleImg.Children.Add(newStack);
                    st_afterSaleImg.HeightRequest = (st_afterSaleImg.Children.Count * com.cstc.ShareJewlryApp.Style.Shape.height.afterSaleImgHetght) + ((st_afterSaleImg.Children.Count + 1) * 10);

                }
                else
                {
                    var newImg = new Image { Source = temp.FaultyPicForShow, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, HeightRequest = com.cstc.ShareJewlryApp.Style.Shape.height.afterSaleImgHetght, WidthRequest = com.cstc.ShareJewlryApp.Style.Shape.height.afterSaleImgHetght };

                    row.Children.Add(newImg);
                }
            }


           

           
        }

        void UI呈现()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (orderList[0].OrderType == "待付款")
                {
                    page_head.RightText = "";
                    frm_pay.IsVisible = true;
                    frm_comfirmCmdty.IsVisible = false;
                    frm_revocation.IsVisible = false;
                    btn_complain.IsVisible = false;
                    btn_lookApply.IsVisible = false;
                    btn_lookLogitics.IsVisible = false;
                    frm_evaluation.IsVisible = false;
                    frm_addLogitics.IsVisible = false;
                    lb_totalCmdy.IsVisible = false;
                }
                else if (orderList[0].OrderType == "待发货")
                {
                    if (orderList[0].OrderState == "租借")
                    {
                        page_head.RightText = "取消订单";
                        frm_pay.IsVisible = false;
                        frm_comfirmCmdty.IsVisible = false;
                        frm_revocation.IsVisible = false;
                        btn_complain.IsVisible = false;
                        btn_lookApply.IsVisible = false;
                        btn_lookLogitics.IsVisible = false;
                        frm_evaluation.IsVisible = false;
                        frm_addLogitics.IsVisible = false;
                        lb_totalCmdy.IsVisible = false;
                    }
                    else if (orderList[0].OrderState == "购买")
                    {
                         page_head.RightText = "申请退款";  
 
                        frm_pay.IsVisible = false;
                        frm_comfirmCmdty.IsVisible = true;
                        frm_revocation.IsVisible = false;
                        btn_complain.IsVisible = false;
                        btn_lookApply.IsVisible = false;
                        btn_lookLogitics.IsVisible = false;
                        frm_evaluation.IsVisible = false;
                        frm_addLogitics.IsVisible = false;
                        lb_totalCmdy.IsVisible = false;
                    }

                }
                else if (orderList[0].OrderType == "待收货")
                {
                    page_head.RightText = "申请售后";
 
                    frm_pay.IsVisible = false;
                    frm_comfirmCmdty.IsVisible = true;
                    frm_revocation.IsVisible = false;
                    btn_complain.IsVisible = false;
                    btn_lookApply.IsVisible = false;
                    btn_lookLogitics.IsVisible = true;
                    frm_evaluation.IsVisible = false;
                    frm_addLogitics.IsVisible = false;
                    lb_totalCmdy.IsVisible = false;
                }
                else if (orderList[0].OrderType == "待评价")
                {
                    page_head.RightText = "申请售后";
                    frm_pay.IsVisible = false;
                    frm_comfirmCmdty.IsVisible = false;
                    frm_revocation.IsVisible = false;
                    btn_complain.IsVisible = false;
                    btn_lookApply.IsVisible = false;
                    btn_lookLogitics.IsVisible = false;
                    frm_evaluation.IsVisible = true;
                    frm_addLogitics.IsVisible = false;
                    lb_totalCmdy.IsVisible = false;
                }
                else if (orderList[0].OrderType == "已完成")
                {
                    page_head.RightText = "申请售后";
                    frm_pay.IsVisible = false;
                    frm_comfirmCmdty.IsVisible = false;
                    frm_revocation.IsVisible = false;
                    btn_complain.IsVisible = false;
                    btn_lookApply.IsVisible = false;
                    btn_lookLogitics.IsVisible = false;
                    frm_evaluation.IsVisible = true;
                    frm_addLogitics.IsVisible = false;
                    lb_totalCmdy.IsVisible = false;
                }
                else if (orderList[0].OrderType == "待处理" || orderList[0].OrderType == "申请取消")
                {
                    page_head.RightText = "";
                    frm_pay.IsVisible = false;
                    frm_comfirmCmdty.IsVisible = false;
                    frm_revocation.IsVisible = true;
                    btn_complain.IsVisible = false;
                    btn_lookApply.IsVisible = true;
                    btn_lookLogitics.IsVisible = false;
                    frm_evaluation.IsVisible = false;
                    frm_addLogitics.IsVisible = false;
                    lb_totalCmdy.IsVisible = false;
                }
                else if (orderList[0].OrderType == "同意售后")
                {
                    page_head.RightText = "";
                    frm_pay.IsVisible = false;
                    frm_comfirmCmdty.IsVisible = false;
                    frm_revocation.IsVisible = false;
                    btn_complain.IsVisible = false;
                    btn_lookApply.IsVisible = true;
                    btn_lookLogitics.IsVisible = false;
                    frm_evaluation.IsVisible = false;
                    frm_addLogitics.IsVisible = true;
                    lb_totalCmdy.IsVisible = false;
                }
                else if (orderList[0].OrderType == "售后完成")
                {
                    page_head.RightText = "";
                    frm_pay.IsVisible = false;
                    frm_comfirmCmdty.IsVisible = false;
                    frm_revocation.IsVisible = false;
                    btn_complain.IsVisible = false;
                    btn_lookApply.IsVisible = true;
                    btn_lookLogitics.IsVisible = false;
                    frm_evaluation.IsVisible = false;
                    frm_addLogitics.IsVisible = false;
                    lb_totalCmdy.IsVisible = false;
                }
                else if (orderList[0].OrderType == "拒绝售后")
                {
                    page_head.RightText = "";
                    frm_pay.IsVisible = false;
                    frm_comfirmCmdty.IsVisible = false;
                    frm_revocation.IsVisible = false;
                    btn_complain.IsVisible = false;
                    btn_lookApply.IsVisible = true;
                    btn_lookLogitics.IsVisible = false;
                    frm_evaluation.IsVisible = false;
                    frm_addLogitics.IsVisible = false;
                    lb_totalCmdy.IsVisible = false;
                }
                else if (orderList[0].OrderType == "已关闭")
                {
                    page_head.RightText = "";
                    frm_pay.IsVisible = false;
                    frm_comfirmCmdty.IsVisible = false;
                    frm_revocation.IsVisible = false;
                    btn_complain.IsVisible = false;
                    btn_lookApply.IsVisible = false;
                    btn_lookLogitics.IsVisible = false;
                    frm_evaluation.IsVisible = false;
                    frm_addLogitics.IsVisible = false;
                    lb_desc.IsVisible = false;
                }
                else if(orderList[0].OrderType == "归还中" || orderList[0].OrderType == "售后中" || orderList[0].OrderType == "用户报损")
                {
                    page_head.RightText = "";
                    frm_pay.IsVisible = false;
                    frm_comfirmCmdty.IsVisible = false;
                    frm_revocation.IsVisible = false;
                    btn_complain.IsVisible = false;
                    btn_lookApply.IsVisible = false;
                    btn_lookLogitics.IsVisible = true;
                    frm_evaluation.IsVisible = false;
                    frm_addLogitics.IsVisible = false;

                    lb_desc.IsVisible = true;
                    lb_desc.Text = "商家将在收货后7日内确认";
                    lb_totalCmdy.IsVisible = false;
                    lb_tptalPriceDesc.IsVisible = false;
                    lb_totalPrice.IsVisible = false;
                }
                else if(orderList[0].OrderType == "租借中")
                {
                    page_head.RightText = "申请售后";
                    frm_pay.IsVisible = false;
                    frm_comfirmCmdty.IsVisible = false;
                    frm_revocation.IsVisible = false;
                    btn_complain.IsVisible = false;
                    btn_lookApply.IsVisible = false;
                    btn_lookLogitics.IsVisible = false;
                    frm_evaluation.IsVisible = false;
                    frm_addLogitics.IsVisible = false;
                    lb_totalCmdy.IsVisible = false;
                }
                else if(orderList[0].OrderType == "卖家报损")
                {
                    page_head.RightText = "";
                    frm_pay.IsVisible = true;
                    frm_comfirmCmdty.IsVisible = false;
                    frm_revocation.IsVisible = false;
                    btn_complain.IsVisible = true;
                    btn_lookApply.IsVisible = false;
                    btn_lookLogitics.IsVisible = false;
                    frm_evaluation.IsVisible = false;
                    frm_addLogitics.IsVisible = false;
                    lb_totalCmdy.IsVisible = false;
                }
                else if (orderList[0].OrderType == "平台介入中")
                {
                    if(orderList[0].FaultyPrice=="")  ///售后被拒绝的平台介入
                    {
                        page_head.RightText = "";
                        frm_pay.IsVisible = false;
                        frm_comfirmCmdty.IsVisible = false;
                        frm_revocation.IsVisible = false;
                        btn_complain.IsVisible = false;
                        btn_lookApply.IsVisible = false;
                        btn_lookLogitics.IsVisible = false;
                        frm_evaluation.IsVisible = false;
                        frm_addLogitics.IsVisible = false;
                        lb_totalCmdy.IsVisible = false;
                    }
                    else                                //商家报损的平台介入
                    {
                        page_head.RightText = "";
                        frm_pay.IsVisible = true;
                        frm_comfirmCmdty.IsVisible = false;
                        frm_revocation.IsVisible = false;
                        btn_complain.IsVisible = false;
                        btn_lookApply.IsVisible = false;
                        btn_lookLogitics.IsVisible = false;
                        frm_evaluation.IsVisible = false;
                        frm_addLogitics.IsVisible = false;
                        lb_totalCmdy.IsVisible = false;
                    }
                }
            });
        }

        void 绘制购物车列表()
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

                var shopStackLayout = new StackLayout() { Padding = new Thickness(10, 0, 10, 0), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 10,HeightRequest=com.cstc.ShareJewlryApp.Style.Shape.height.barLenth };
                
                shopStackLayout.Children.Add(shopImg);
                shopStackLayout.Children.Add(lb_shopName);

                fatherStackLayout.Children.Add(shopStackLayout);

                foreach (var row in item.CommodityChildrenRows)
                {
                    商品总数++;
                    i++;
                    var rowStack = new StackLayout() { Padding = new Thickness(10, 10, 0, 0), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 0 };

                    TapGestureRecognizer select_tap = new TapGestureRecognizer();
                    select_tap.CommandParameter = row;  //加入数据
                    select_tap.Tapped += tapped_selectCmdty;

                    rowStack.GestureRecognizers.Add(select_tap);//加入点击手势 

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

                var lb_订单号描述 = new Label() { Text = "订单号:",BackgroundColor=Color.Transparent,TextColor=Color.Black, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center, HeightRequest = 30 };
                var lb_订单号 = new Label() { Text = item.OrderID, TextColor = Color.Black, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center, HeightRequest = 30 };

                var box = new BoxView() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, WidthRequest = 1 ,HeightRequest=30};

                st_orderPrice.Children.Add(lb_订单号描述);
                st_orderPrice.Children.Add(lb_订单号);
                st_orderPrice.Children.Add(box);
                if (item.OrderState == "购买")
                {
                    var lb_订单金额描述 = new Label() { Text = "订单金额:", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center, HeightRequest = 30 };
                    var lb_订单金额 = new Label() { Text = item.TotalPriceForShow, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center, HeightRequest = 30 };

                    st_orderPrice.Children.Add(lb_订单金额描述);
                    st_orderPrice.Children.Add(lb_订单金额);

                    商品总金额 = 商品总金额 + item.TotalPrice;
                }

                fatherStackLayout.Children.Add(st_orderPrice);
                st_cmdtydtl.Children.Add(fatherStackLayout);
                //  var rowStackLayout 

                if (item.OrderState == "购买")
                {
                    lb_totalCmdy.Text = "共" + 商品总数 + "件商品";
                    lb_totalPrice.Text = "¥" + 商品总金额;
                    商品总金额 = 0;
                }
                else if (item.OrderState == "租借")
                {
                    lb_totalCmdy.Text = "免费戴商品无需支付";
                    lb_tptalPriceDesc.IsVisible = false;
                    lb_totalPrice.IsVisible = false;
                }
            }

        }

        /// <summary>
        /// 关闭弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_cancle(object sender, EventArgs e)
        {
            block.IsVisible = false;
            st_PlatformCheckBox.IsVisible = false;
            st_PaycheckBox.IsVisible = false;
            st_cancelOrdercheckBox.IsVisible = false;
            st_comfirmCmdtyBox.IsVisible = false;
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
        /// 选择商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_selectCmdty(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            var row = (Data.commodityData)(((TappedEventArgs)e).Parameter);

            Views.HomePage.ProductDetails.ProductDetailsPage page = new HomePage.ProductDetails.ProductDetailsPage();
            page.JewelleryGUID = row.JewelleryGUID;
            //page.getCommodityDetailsData();
           // page.显示商品详情图片();

            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(page, true);
            });

            按钮防呆 = false;

        }

        /// <summary>
        /// 立即付款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_payOrder(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;


            if (orderList[0].OrderType == "待付款")
            {
                Views.ShoppingCart.Order.ChoosePayPage page = new ChoosePayPage();
                page.totalPrice = orderList[0].TotalPrice;
                page.UI呈现();
                Navigation.PushAsync(page, true);
            }
            else if(orderList[0].OrderType == "卖家报损" || orderList[0].OrderType == "平台介入中")
            {
                Views.ShoppingCart.Order.ChoosePayPage page = new ChoosePayPage();
                page.totalPrice = Convert.ToDecimal(orderList[0].FaultyPrice);
                page.支付标志 = "报损赔付";
                page.orderGuid = OrderGUID;
                page.报损单GUID = orderList[0].FaultyGUID;
                page.UI呈现();
                Navigation.PushAsync(page, true);
            }


            按钮防呆 = false;
        }


        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_cancleOrder(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            if (orderList[0].OrderType == "待付款")
            {
                block.IsVisible = true;
                st_cancelOrdercheckBox.IsVisible = true;
            }
            else if (orderList[0].OrderType == "待发货")
            {
                if (orderList[0].OrderState == "租借")
                {
                    block.IsVisible = true;
                    st_cancelOrdercheckBox.IsVisible = true;
                }
                else
                {
                    Views.ShoppingCart.Order.afterSalePage page = new ShoppingCart.Order.afterSalePage();
                    page.flag = "退款";
                    page.orderList = orderList[0];
                    page.刷新UI();
                    Navigation.PushAsync(page, true);
                }
            }
            按钮防呆 = false;
        }

        /// <summary>
        /// 关闭订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_comfirmCancel(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            Tools.AsyncMsg am_取消订单 = new Tools.AsyncMsg();

            if (orderList[0].OrderState == "购买")
            {
                string para = "OrderGUID=" + OrderGUID;

                am_取消订单.Completion += (object obj, string ex) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        getOrderDtl();
                        tapped_cancle(null, null);
                        按钮防呆 = false;
                    });
                };

                am_取消订单.Cancel += (object obj, string ex) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        按钮防呆 = false;
                        DisplayAlert("提示", "取消订单失败", "知道了");
                    });
                    return;
                };

                Tools.NetClass.GetStringByUrl("ExSql", "APP\\CloseOrder_1_0_0_1", para, am_取消订单);
            }
            else if (orderList[0].OrderState == "租借")
            {
                string para = "OrderGUID=" + OrderGUID + "&Reason= ";

                am_取消订单.Completion += (object obj, string ex) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        getOrderDtl();
                        tapped_cancle(null, null);
                        按钮防呆 = false;
                    });
                };

                am_取消订单.Cancel += (object obj, string ex) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        按钮防呆 = false;
                       // DisplayAlert("提示", "取消订单失败:" + ex.ToString(), "知道了");
                    });
                    return;
                };

                Tools.NetClass.GetStringByUrl("ExSql", "APP\\AllowRentOrderRefund_1_0_0_1", para, am_取消订单);
            }
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_comfirmCmdty(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            Tools.AsyncMsg am_确认收货 = new Tools.AsyncMsg();
            string para = "OrderGUID=" + OrderGUID;

            am_确认收货.Completion += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    getOrderDtl();
                    tapped_cancle(null, null);
                    按钮防呆 = false;
                });
            };

            am_确认收货.Cancel += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    按钮防呆 = false;
                    //DisplayAlert("提示", "确认收货失败:" + ex.ToString(), "知道了");
                });
                return;
            };

            Tools.NetClass.GetStringByUrl("ExSql", "APP\\SignInOrder_1_0_0_1", para, am_确认收货);

            按钮防呆 = false;
        }


        /// <summary>
        /// 撤销取消订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_revocation(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            Tools.AsyncMsg am_撤销取消订单 = new Tools.AsyncMsg();
            string para = "OrderGUID=" + OrderGUID;

            am_撤销取消订单.Completion += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    getOrderDtl();
                    //tapped_cancle(null, null);
                    按钮防呆 = false;
                });
            };

            am_撤销取消订单.Cancel += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    按钮防呆 = false;
                    DisplayAlert("提示", "撤销取消订单失败"  , "知道了");
                });
                return;
            };

            Tools.NetClass.GetStringByUrl("ExSql", "APP\\RevokeCloseOrder", para, am_撤销取消订单);

            按钮防呆 = false;
        }

        /// <summary>
        /// 填写物流
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_addLogitics(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;
            
            BackCmdtyPage page = new BackCmdtyPage();
            page.orderGuid = OrderGUID;
            page.获取售后商品();

            Navigation.PushAsync(page, true);

            按钮防呆 = false;
        }

        /// <summary>
        /// 评价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_evaluation(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            按钮防呆 = false;
        }

        /// <summary>
        /// 申请售后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_applyAfterSale(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            Views.ShoppingCart.Order.afterSalePage page = new ShoppingCart.Order.afterSalePage();
            page.orderGUID = OrderGUID;
            page.orderList = orderList[0];
            if (orderList[0].OrderState == "购买")
            {
                page.flag = "售后";
            }
            else if (orderList[0].OrderState == "租借")
            {
                page.flag = "免费戴售后";
            }
            page.刷新UI();
            Navigation.PushAsync(page, true);

            按钮防呆 = false;
        }

        /// <summary>
        /// 查看申请
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_lookApply(object sender, EventArgs e)
        {
            Views.ShoppingCart.Order.ApplicationPage page = new ApplicationPage();

            page.orderGUID = OrderGUID;
            page.getApplyData();

            Navigation.PushAsync(page, true);
        }

        /// <summary>
        /// 查看物流
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_lookLogitics(object sender, EventArgs e)
        {
            Views.ShoppingCart.Order.CheckLogisticsPage page = new CheckLogisticsPage();
            page.运单号 = orderList[0].LogisticsNumber;
            page.物流公司 = orderList[0].LogisticsCompanyCode;
            page.loadData();

            Navigation.PushAsync(page, true);
        }

        /// <summary>
        /// 平台介入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_complain(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            Tools.AsyncMsg am_平台介入 = new Tools.AsyncMsg();
            string para = "";
            if (orderList[0].OrderState == "购买")
            {
                para = "ShopGUID=" + orderList[0].ShopGUID + "&UserGUID=" + Data.UserInfoCache.UserGUID + "&OrderGUID=" + OrderGUID + "&Price=0&AppealType=售后";
            }
            else if (orderList[0].OrderState == "租借")
            {
                para = "ShopGUID=" + orderList[0].ShopGUID + "&UserGUID=" + Data.UserInfoCache.UserGUID + "&OrderGUID=" + OrderGUID + "&Price=" + orderList[0].FaultyPrice+ "&AppealType=报损";
            }

            am_平台介入.Completion += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    getOrderDtl();
                    tapped_cancle(null, null);
                    按钮防呆 = false;
                });
            };

            am_平台介入.Cancel += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    按钮防呆 = false;
                    //DisplayAlert("提示", "申请平台介入失败:" + ex.ToString(), "知道了");
                });
                return;
            };

            Tools.NetClass.GetStringByUrl("ExSql", "APP\\InsertAppeal_1_0_0_1", para, am_平台介入);

            按钮防呆 = false;
        }

        /// <summary>
        /// 是否平台介入弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_ShowComplain(object sender, EventArgs e)
        {
            st_PlatformCheckBox.IsVisible = true;
            block.IsVisible = true;
        }

        /// <summary>
        /// 弹出确认收货框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_ShowComfirmCmdty(object sender, EventArgs e)
        {
            st_comfirmCmdtyBox.IsVisible = true;
            block.IsVisible = true;
        }

        private void PageHead_BtnForwardClick(object sender, EventArgs e)
        {
            if (page_head.RightText == "取消订单")
                tapped_cancleOrder(sender, e);
            if (page_head.RightText == "申请售后")
                tapped_applyAfterSale(sender, e);
        }
    }
}