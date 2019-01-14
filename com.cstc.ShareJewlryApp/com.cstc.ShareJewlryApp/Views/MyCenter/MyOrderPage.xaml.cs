using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.MyCenter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyOrderPage: BasePage 
    {
        bool 按钮防呆 = false;
        string menuFlag = "全部";
        int 分页index = 0;
        public int state { get; set; } = 1;  //1全部2进行中3已完成0已取消
        bool scrollLocker = false;

        Data.OrderData 取消的订单 = null;

        ObservableCollection<Data.OrderData> orderList = new ObservableCollection<Data.OrderData>();
        public MyOrderPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            getAllOrderData();
        }

        /// <summary>
        /// 获取订单数据
        /// </summary>
        public void getAllOrderData()
        {

            Tools.AsyncMsg am_获取订单数据 = new Tools.AsyncMsg();

            string para = "UserGUID=" + Data.UserInfoCache.UserGUID + "&OrderType=" + state.ToString() + "&PageNumber=" + 分页index;

            am_获取订单数据.Completion += (object obj, string ex) =>
            {
                string returnJson = obj.ToString();
                string ErrMsg = "";

                Newtonsoft.Json.Linq.JArray jar = null;

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
                        //await DisplayAlert("错误", "解析订单数据错误！" + exc.Message, "知道了");
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
                        //await DisplayAlert("错误", "解析订单数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                ObservableCollection<Data.OrderData> tempList = null;

                try
                {
                    tempList = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Data.OrderData>>(jar.ToString());

                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "转化订单数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                foreach (var temp in tempList)
                {
                    orderList.Add(temp);
                }

                Device.BeginInvokeOnMainThread(() =>
               {
                   绘制列表(tempList);
               });

                分页index++;
                scrollLocker = false;
            };

            //am_获取订单数据.Cancel += (object obj, string ex) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        DisplayAlert("提示", "获取订单数据失败:" + ex.ToString(), "知道了");
            //    });
            //    return;
            //};

            Tools.NetClass.GetStringByUrl("QueryData", "APP\\GetAllOrder_1_0_0_1,APP\\GetAllOrderDetailed_1_0_0_1,APP\\GetAllFeedback_1_0_0_1", para, am_获取订单数据);
        }

        void 绘制列表(ObservableCollection<Data.OrderData> tempList)
        {
            foreach (var item in tempList)
            {
                var fatherStackLayout = new StackLayout() { Spacing = 0, BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.BackgroundColor, Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };

                foreach (var row in item.CommodityChildrenRows)
                {
                    var rowStack = new StackLayout() { Padding = new Thickness(10, 10, 10, 10), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Spacing = 5 };

                    TapGestureRecognizer checkOrderDtl_tap = new TapGestureRecognizer();
                    checkOrderDtl_tap.CommandParameter = item;  //加入数据
                    checkOrderDtl_tap.Tapped += tapped_checkOrderDtl;

                    rowStack.GestureRecognizers.Add(checkOrderDtl_tap);//加入点击手势

                    var cmdtyImg = new Image() { Source = row.JewelleryPicNameForImg, BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.FillAndExpand, WidthRequest = 80, HeightRequest = 80 };

                    rowStack.Children.Add(cmdtyImg);

                    var stack_右部 = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Spacing = 0 };

                    var lb_商品名 = new com.cstc.ShareJewlryApp.Mycontrols.MultiLineLabel() { Text = row.JewelleryName,TextColor=Color.Black, Lines=2,FontSize=com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, LineBreakMode = LineBreakMode.TailTruncation };

                    var lb_数量 = new Label() { Text = "× " + row.number, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start, LineBreakMode = LineBreakMode.NoWrap, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    var stack_金额订单行 = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 0 };

                    var lb_金额 = new Label() { Text = row.PriceForShow, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, LineBreakMode = LineBreakMode.NoWrap, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont };

                    var lb_状态 = new Label() { Text = item.OrderType, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start, LineBreakMode = LineBreakMode.NoWrap, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    stack_金额订单行.Children.Add(lb_金额);
                    stack_金额订单行.Children.Add(lb_状态);

                    stack_右部.Children.Add(lb_商品名);
                    stack_右部.Children.Add(lb_数量);
                    stack_右部.Children.Add(stack_金额订单行);

                    rowStack.Children.Add(stack_右部);

                    fatherStackLayout.Children.Add(rowStack);


                }

                var bx_分割线 = new BoxView() { HeightRequest = 1, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.LineColor };

                fatherStackLayout.Children.Add(bx_分割线);

                var st_更多栏 = new StackLayout() { Padding = new Thickness(10, 5, 10, 5), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 5, HeightRequest = com.cstc.ShareJewlryApp.Style.Shape.height.barLenth };

                var img_更多 = new Image() { Source = "order_more.png", HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, HeightRequest = 15, WidthRequest = 15 };
                var lb_更多 = new Label() { Text = "更多", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HeightRequest = com.cstc.ShareJewlryApp.Style.Shape.height.barLenth };

                TapGestureRecognizer more_tap = new TapGestureRecognizer();
                more_tap.CommandParameter = item;  //加入数据
                more_tap.Tapped += tapped_more;


                img_更多.GestureRecognizers.Add(more_tap);//加入点击手势 
                lb_更多.GestureRecognizers.Add(more_tap);//加入点击手势 

                st_更多栏.Children.Add(img_更多);

                st_更多栏.Children.Add(lb_更多);

                var box = new BoxView() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, WidthRequest = 1 };
                st_更多栏.Children.Add(box);

                if (item.OrderType == "待付款")
                {
                    var 外框_取消订单 = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 30,  CornerRadius = 3, Padding = new Thickness(0.5, 0.5, 0.5, 0.5), BackgroundColor =com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    var 内框_取消订单 = new Frame() { CornerRadius = 3, Padding = new Thickness(3, 3, 3, 3), BackgroundColor = Color.White };

                    var lb_取消订单 = new Label() { Text = "取消订单", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                    内框_取消订单.Content = lb_取消订单;
                    外框_取消订单.Content = 内框_取消订单;


                    TapGestureRecognizer cancel_tap = new TapGestureRecognizer();
                    cancel_tap.CommandParameter = item;  //加入数据
                    cancel_tap.Tapped += tapped_cancelOrder;

                    lb_取消订单.GestureRecognizers.Add(cancel_tap);//加入点击手势
                    st_更多栏.Children.Add(外框_取消订单);


                    var 外框_立即付款 = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 30,  CornerRadius = 3, Padding = new Thickness(0.5, 0.5, 0.5, 0.5), BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    var 内框_立即付款 = new Frame() { CornerRadius = 3, Padding = new Thickness(3, 3, 3, 3), BackgroundColor = Color.White };

                    var lb_立即付款 = new Label() { Text = "立即付款", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                    内框_立即付款.Content = lb_立即付款;
                    外框_立即付款.Content = 内框_立即付款;

                    TapGestureRecognizer pay_tap = new TapGestureRecognizer();
                    pay_tap.CommandParameter = item;  //加入数据
                    pay_tap.Tapped += tapped_PayOrder;

                    lb_立即付款.GestureRecognizers.Add(pay_tap);//加入点击手势

                    st_更多栏.Children.Add(外框_立即付款);
                }


                if(item.OrderType =="申请取消")
                {
                     var 外框_查看订单 = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 30,  CornerRadius = 3, Padding = new Thickness(0.5, 0.5, 0.5, 0.5), BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    var 内框_查看订单 = new Frame() { CornerRadius = 3, Padding = new Thickness(3, 3, 3, 3), BackgroundColor = Color.White };

                    var lb_查看订单 = new Label() { Text = "查看订单", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                    内框_查看订单.Content = lb_查看订单;
                    外框_查看订单.Content = 内框_查看订单;

                    TapGestureRecognizer Check_tap = new TapGestureRecognizer();
                    Check_tap.CommandParameter = item;  //加入数据
                    Check_tap.Tapped += tapped_CheckOrder;

                    lb_查看订单.GestureRecognizers.Add(Check_tap);//加入点击手势

                    st_更多栏.Children.Add(外框_查看订单);
                }

                if (item.OrderType == "已关闭")
                {
                    var 外框_查看订单 = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 30,  CornerRadius = 3, Padding = new Thickness(0.5, 0.5, 0.5, 0.5), BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    var 内框_查看订单 = new Frame() { CornerRadius = 3, Padding = new Thickness(3, 3, 3, 3), BackgroundColor = Color.White };

                    var lb_查看订单 = new Label() { Text = "查看订单", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                    内框_查看订单.Content = lb_查看订单;
                    外框_查看订单.Content = 内框_查看订单;

                    TapGestureRecognizer Check_tap = new TapGestureRecognizer();
                    Check_tap.CommandParameter = item;  //加入数据
                    Check_tap.Tapped += tapped_CheckOrder;

                    lb_查看订单.GestureRecognizers.Add(Check_tap);//加入点击手势

                    st_更多栏.Children.Add(外框_查看订单);
                }

                if (item.OrderType == "待发货")
                {
                    var 外框_申请退款 = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 30,  CornerRadius = 3, Padding = new Thickness(0.5, 0.5, 0.5, 0.5), BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    var 内框_申请退款 = new Frame() { CornerRadius = 3, Padding = new Thickness(3, 3, 3, 3), BackgroundColor = Color.White };

                    var lb_申请退款 = new Label() { Text = "申请退款", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                    内框_申请退款.Content = lb_申请退款;
                    外框_申请退款.Content = 内框_申请退款;

                    TapGestureRecognizer returnMonney_tap = new TapGestureRecognizer();
                    returnMonney_tap.CommandParameter = item;  //加入数据
                    returnMonney_tap.Tapped += tapped_returnMonney;

                    lb_申请退款.GestureRecognizers.Add(returnMonney_tap);//加入点击手势

                    st_更多栏.Children.Add(外框_申请退款);

                    var 外框_查看订单 = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 30,   CornerRadius = 3, Padding = new Thickness(0.5, 0.5, 0.5, 0.5), BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    var 内框_查看订单 = new Frame() { CornerRadius = 3, Padding = new Thickness(3, 3, 3, 3), BackgroundColor = Color.White };

                    var lb_查看订单 = new Label() { Text = "查看订单", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                    内框_查看订单.Content = lb_查看订单;
                    外框_查看订单.Content = 内框_查看订单;


                    TapGestureRecognizer Check_tap = new TapGestureRecognizer();
                    Check_tap.CommandParameter = item;  //加入数据
                    Check_tap.Tapped += tapped_CheckOrder;

                    lb_查看订单.GestureRecognizers.Add(Check_tap);//加入点击手势
                    st_更多栏.Children.Add(外框_查看订单);

                }

                if (item.OrderType == "待收货")
                {
                    var 外框_查看物流 = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 30,  CornerRadius = 3, Padding = new Thickness(0.5, 0.5, 0.5, 0.5), BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    var 内框_查看物流 = new Frame() { CornerRadius = 3, Padding = new Thickness(3, 3, 3, 3), BackgroundColor = Color.White };

                    var lb_查看物流 = new Label() { Text = "查看物流", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                    内框_查看物流.Content = lb_查看物流;
                    外框_查看物流.Content = 内框_查看物流;
                    TapGestureRecognizer CheckWuliu_tap = new TapGestureRecognizer();
                    CheckWuliu_tap.CommandParameter = item;  //加入数据
                    CheckWuliu_tap.Tapped += tapped_CheckLogistics;

                    lb_查看物流.GestureRecognizers.Add(CheckWuliu_tap);//加入点击手势

                    st_更多栏.Children.Add(外框_查看物流);

                    var 外框_确认收货 = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 30,   CornerRadius = 3, Padding = new Thickness(0.5, 0.5, 0.5, 0.5), BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    var 内框_确认收货 = new Frame() { CornerRadius = 3, Padding = new Thickness(3, 3, 3, 3), BackgroundColor = Color.White };

                    var lb_确认收货 = new Label() { Text = "确认收货", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                    内框_确认收货.Content = lb_确认收货;
                    外框_确认收货.Content = 内框_确认收货;

                    TapGestureRecognizer comfirmOrder_tap = new TapGestureRecognizer();
                    comfirmOrder_tap.CommandParameter = item;  //加入数据
                    comfirmOrder_tap.Tapped += tapped_comfirmOrder;

                    lb_确认收货.GestureRecognizers.Add(comfirmOrder_tap);//加入点击手势

                    st_更多栏.Children.Add(外框_确认收货);
                }


                   if (item.OrderType == "待评价")
                   {
                //       var btn_物流 = new Mycontrols.grayCornerBtn() { Text = "查看物流", HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.FillAndExpand };
                //       TapGestureRecognizer CheckWuliu_tap = new TapGestureRecognizer();
                //       CheckWuliu_tap.CommandParameter = item;  //加入数据
                //       CheckWuliu_tap.Tapped += tapped_CheckLogistics;

                //       btn_物流.GestureRecognizers.Add(CheckWuliu_tap);//加入点击手势

                //       st_更多栏.Children.Add(btn_物流);

                //       var btn_申请售后 = new Mycontrols.grayCornerBtn() { Text = "申请售后", HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.FillAndExpand };
                //       TapGestureRecognizer afterSale_tap = new TapGestureRecognizer();
                //       afterSale_tap.CommandParameter = item;  //加入数据
                //       afterSale_tap.Tapped += tapped_afterSale;

                //        btn_申请售后.GestureRecognizers.Add(afterSale_tap);//加入点击手势

                //       st_更多栏.Children.Add(btn_申请售后);

                //var img_减号 = new Image() { Source = "reduce.png", HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, BackgroundColor = Color.Transparent, HeightRequest = 20, WidthRequest = 20 };

                //TapGestureRecognizer reduce_tap = new TapGestureRecognizer();
                //reduce_tap.CommandParameter = item;  //加入数据
                //reduce_tap.Tapped += tapped_Evaluation;

                //img_减号.GestureRecognizers.Add(reduce_tap);//加入点击手势
                //st_更多栏.Children.Add(img_减号);

                var 外框_评价 = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 30, WidthRequest=60,  CornerRadius = 3, Padding = new Thickness(0.5, 0.5, 0.5, 0.5), BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                var 内框_评价 = new Frame() { CornerRadius = 3, Padding = new Thickness(3, 3, 3, 3), BackgroundColor = Color.White };

                var lb_评价 = new Label() { Text = "评价", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                内框_评价.Content = lb_评价;
                外框_评价.Content = 内框_评价;

                TapGestureRecognizer Evaluation_tap = new TapGestureRecognizer();
                Evaluation_tap.CommandParameter = item;  //加入数据
                Evaluation_tap.Tapped += tapped_Evaluation;

                lb_评价.GestureRecognizers.Add(Evaluation_tap);//加入点击手势
                st_更多栏.Children.Add(外框_评价);

                 }

                if (item.OrderType == "已完成")
                {
                    var 外框_查看物流 = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 30,   CornerRadius = 3, Padding = new Thickness(0.5, 0.5, 0.5, 0.5), BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    var 内框_查看物流 = new Frame() { CornerRadius = 3, Padding = new Thickness(3, 3, 3, 3), BackgroundColor = Color.White };

                    var lb_查看物流 = new Label() { Text = "查看物流", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                    内框_查看物流.Content = lb_查看物流;
                    外框_查看物流.Content = 内框_查看物流;

                    TapGestureRecognizer CheckWuliu_tap = new TapGestureRecognizer();
                    CheckWuliu_tap.CommandParameter = item;  //加入数据
                    CheckWuliu_tap.Tapped += tapped_CheckLogistics;

                    lb_查看物流.GestureRecognizers.Add(CheckWuliu_tap);//加入点击手势
                    st_更多栏.Children.Add(外框_查看物流);

                    var 外框_申请售后 = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 30,   CornerRadius = 3, Padding = new Thickness(0.5, 0.5, 0.5, 0.5), BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    var 内框_申请售后 = new Frame() { CornerRadius = 3, Padding = new Thickness(3, 3, 3, 3), BackgroundColor = Color.White };

                    var lb_申请售后 = new Label() { Text = "申请售后", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                    内框_申请售后.Content = lb_申请售后;
                    外框_申请售后.Content = 内框_申请售后;
                    TapGestureRecognizer afterSale_tap = new TapGestureRecognizer();
                    afterSale_tap.CommandParameter = item;  //加入数据
                    afterSale_tap.Tapped += tapped_afterSale;

                    lb_申请售后.GestureRecognizers.Add(afterSale_tap);//加入点击手势

                    st_更多栏.Children.Add(外框_申请售后);


                    var 外框_修改评价 = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 30,   CornerRadius = 3, Padding = new Thickness(0.5, 0.5, 0.5, 0.5), BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    var 内框_修改评价 = new Frame() { CornerRadius = 3, Padding = new Thickness(3, 3, 3, 3), BackgroundColor = Color.White };

                    var lb_修改评价 = new Label() { Text = "修改评价", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                    内框_修改评价.Content = lb_修改评价;
                    外框_修改评价.Content = 内框_修改评价;
                    TapGestureRecognizer Evaluation_tap1 = new TapGestureRecognizer();
                    Evaluation_tap1.CommandParameter = item;  //加入数据
                    //Evaluation_tap1.Tapped += tapped_Evaluation;

                    lb_修改评价.GestureRecognizers.Add(Evaluation_tap1);//加入点击手势

                    st_更多栏.Children.Add(外框_修改评价);
                }
                if (item.OrderType == "待处理")
                {
                    var 外框_查看售后 = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 30,   CornerRadius = 3, Padding = new Thickness(0.5, 0.5, 0.5, 0.5), BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    var 内框_查看售后 = new Frame() { CornerRadius = 3, Padding = new Thickness(3, 3, 3, 3), BackgroundColor = Color.White };

                    var lb_查看售后 = new Label() { Text = "查看售后", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                    内框_查看售后.Content = lb_查看售后;
                    外框_查看售后.Content = 内框_查看售后;
                    TapGestureRecognizer CheckAfterSale_tap = new TapGestureRecognizer();
                    CheckAfterSale_tap.CommandParameter = item;  //加入数据
                    CheckAfterSale_tap.Tapped += tapped_CheckAfterSale;

                    lb_查看售后.GestureRecognizers.Add(CheckAfterSale_tap);//加入点击手势
                    st_更多栏.Children.Add(外框_查看售后);
                }
                if (item.OrderType == "同意售后")
                {
                    var 外框_查看售后 = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 30,   CornerRadius = 3, Padding = new Thickness(0.5, 0.5, 0.5, 0.5), BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    var 内框_查看售后 = new Frame() { CornerRadius = 3, Padding = new Thickness(3, 3, 3, 3), BackgroundColor = Color.White };

                    var lb_查看售后 = new Label() { Text = "查看售后", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                    内框_查看售后.Content = lb_查看售后;
                    外框_查看售后.Content = 内框_查看售后;

                    TapGestureRecognizer CheckAfterSale_tap = new TapGestureRecognizer();
                    CheckAfterSale_tap.CommandParameter = item;  //加入数据
                    CheckAfterSale_tap.Tapped += tapped_CheckAfterSale;

                    lb_查看售后.GestureRecognizers.Add(CheckAfterSale_tap);//加入点击手势
                    st_更多栏.Children.Add(外框_查看售后);

                    var 外框_填写物流 = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 30,   CornerRadius = 3, Padding = new Thickness(0.5, 0.5, 0.5, 0.5), BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    var 内框_填写物流 = new Frame() { CornerRadius = 3, Padding = new Thickness(3, 3, 3, 3), BackgroundColor = Color.White };

                    var lb_填写物流 = new Label() { Text = "填写物流", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                    内框_填写物流.Content = lb_填写物流;
                    外框_填写物流.Content = 内框_填写物流;

                    TapGestureRecognizer addLogistics_tap = new TapGestureRecognizer();
                    addLogistics_tap.CommandParameter = item;  //加入数据
                    addLogistics_tap.Tapped += tapped_CheckAfterSale;

                    lb_填写物流.GestureRecognizers.Add(addLogistics_tap);//加入点击手势
                    st_更多栏.Children.Add(外框_填写物流);
                }
                if (item.OrderType == "售后完成")
                {
                    var 外框_查看售后 = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 30,   CornerRadius = 3, Padding = new Thickness(0.5, 0.5, 0.5, 0.5), BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    var 内框_查看售后 = new Frame() { CornerRadius = 3, Padding = new Thickness(3, 3, 3, 3), BackgroundColor = Color.White };

                    var lb_查看售后 = new Label() { Text = "查看售后", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                    内框_查看售后.Content = lb_查看售后;
                    外框_查看售后.Content = 内框_查看售后;

                    TapGestureRecognizer CheckAfterSale_tap = new TapGestureRecognizer();
                    CheckAfterSale_tap.CommandParameter = item;  //加入数据
                    CheckAfterSale_tap.Tapped += tapped_CheckAfterSale;

                    lb_查看售后.GestureRecognizers.Add(CheckAfterSale_tap);//加入点击手势
                    st_更多栏.Children.Add(外框_查看售后);
                }
                if (item.OrderType == "拒绝售后")
                {
                    //var btn_查看售后 = new Mycontrols.grayCornerBtn() { Text = "查看售后", HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.FillAndExpand };

                    var 外框_查看售后 = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 30,   CornerRadius = 3, Padding = new Thickness(0.5, 0.5, 0.5, 0.5), BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    var 内框_查看售后 = new Frame() { CornerRadius = 3, Padding = new Thickness(3, 3, 3, 3), BackgroundColor = Color.White };

                    var lb_查看售后 = new Label() { Text = "查看售后", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                    内框_查看售后.Content = lb_查看售后;
                    外框_查看售后.Content = 内框_查看售后;

                    TapGestureRecognizer CheckAfterSale_tap = new TapGestureRecognizer();
                    CheckAfterSale_tap.CommandParameter = item;  //加入数据
                    CheckAfterSale_tap.Tapped += tapped_CheckAfterSale;

                    lb_查看售后.GestureRecognizers.Add(CheckAfterSale_tap);//加入点击手势
                    st_更多栏.Children.Add(外框_查看售后);
                }
                if (item.OrderType == "异常订单")
                {
                    //var btn_查看售后 = new Mycontrols.grayCornerBtn() { Text = "查看售后", HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.FillAndExpand };
                    var 外框_查看售后 = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 30,   CornerRadius = 3, Padding = new Thickness(0.5, 0.5, 0.5, 0.5), BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont };

                    var 内框_查看售后 = new Frame() { CornerRadius = 3, Padding = new Thickness(3, 3, 3, 3), BackgroundColor = Color.White };

                    var lb_查看售后 = new Label() { Text = "查看售后", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

                    内框_查看售后.Content = lb_查看售后;
                    外框_查看售后.Content = 内框_查看售后;

                    TapGestureRecognizer CheckAfterSale_tap = new TapGestureRecognizer();
                    CheckAfterSale_tap.CommandParameter = item;  //加入数据
                    CheckAfterSale_tap.Tapped += tapped_CheckAfterSale;

                    lb_查看售后.GestureRecognizers.Add(CheckAfterSale_tap);//加入点击手势
                    st_更多栏.Children.Add(外框_查看售后);
                }

                fatherStackLayout.Children.Add(st_更多栏);

                var bx_分隔条 = new BoxView() { HeightRequest = 15, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.SpaceColor };

                fatherStackLayout.Children.Add(bx_分隔条);

                st_order.Children.Add(fatherStackLayout);
            }
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_cancelOrder(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            block.IsVisible = true;
            frm_CancelOrderCheckBox.IsVisible = true;

            取消的订单 = (Data.OrderData)((TappedEventArgs)e).Parameter;

            按钮防呆 = false;
        }

        /// <summary>
        /// 立即付款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_PayOrder(object sender, EventArgs e)
        {

            if (按钮防呆)
                return;
            按钮防呆 = true;

            var row = (Data.OrderData)((TappedEventArgs)e).Parameter;

            Views.ShoppingCart.Order.ChoosePayPage page = new ShoppingCart.Order.ChoosePayPage();

            page.totalPrice = row.TotalPrice;

            Navigation.PushAsync(page, true);

            按钮防呆 = false;


        }

        /// <summary>
        /// 查看订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_CheckOrder(object sender, EventArgs e)
        {

            if (按钮防呆)
                return;
            按钮防呆 = true;


            var row = (Data.OrderData)((TappedEventArgs)e).Parameter;

            Views.ShoppingCart.Order.OrderDetailsPage page = new ShoppingCart.Order.OrderDetailsPage();

            page.OrderGUID = row.OrderGUID;
            page.getOrderDtl();

            Navigation.PushAsync(page, true);

            按钮防呆 = false;
        }

        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_returnMonney(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            var row = (Data.OrderData)((TappedEventArgs)e).Parameter;
            Views.ShoppingCart.Order.afterSalePage page = new ShoppingCart.Order.afterSalePage();
            page.flag = "退款";
            page.orderList = row;
            page.刷新UI();
            Navigation.PushAsync(page, true);

            按钮防呆 = false;

        }

        /// <summary>
        /// 查看物流
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_CheckLogistics(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            var row = (Data.OrderData)((TappedEventArgs)e).Parameter;
            Views.ShoppingCart.Order.CheckLogisticsPage page = new ShoppingCart.Order.CheckLogisticsPage();
            page.运单号 = "";
            page.loadData();


            按钮防呆 = false;
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_comfirmOrder(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            var row = (Data.OrderData)((TappedEventArgs)e).Parameter;


            按钮防呆 = false;
        }

        /// <summary>
        /// 申请售后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_afterSale(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;
            var row = (Data.OrderData)((TappedEventArgs)e).Parameter;
            Views.ShoppingCart.Order.afterSalePage page = new ShoppingCart.Order.afterSalePage();
            page.orderList = row;
            page.flag = "售后";
            page.刷新UI();
            Navigation.PushAsync(page, true);

            按钮防呆 = false;
        }

        /// <summary>
        /// 评价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_Evaluation(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            var row = (Data.OrderData)((TappedEventArgs)e).Parameter;

            Views.ShoppingCart.Order.EvaluationPage page = new ShoppingCart.Order.EvaluationPage();

            page.orderData = row;
            page.UI呈现();

            Navigation.PushAsync(page, true);

            按钮防呆 = false;
        }

        /// <summary>
        /// 查看售后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_CheckAfterSale(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            var row = (Data.OrderData)((TappedEventArgs)e).Parameter;
            Views.ShoppingCart.Order.ApplicationPage page = new ShoppingCart.Order.ApplicationPage();
            page.orderGUID = row.OrderGUID; 
            
           // page.刷新UI();
            Navigation.PushAsync(page, true);

            按钮防呆 = false;
        }


        void 重置订单列表()
        {
            分页index = 0;
            scrollLocker = false;
            st_order.Children.Clear();
            orderList.Clear();
        }

        void tapped_allOeder(object sender, TappedEventArgs e)   //'全部'按钮
        {
            // Label lb = (Label)sender;
            #region UI呈现

            if (menuFlag == "全部")
                return;

            menuFlag = "全部";
            重置订单列表();
            state = 1;
            getAllOrderData();
            Device.BeginInvokeOnMainThread(() =>
            {
                lb_allOrder.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                lb_nowOrder.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                lb_finishOrder.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                lb_cancelOrder.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
            });
            #endregion
        }

        void tapped_nowOeder(object sender, TappedEventArgs e)   //'进行中'按钮
        {
            //  Label lb = (Label)sender;
            #region UI呈现
            if (menuFlag == "进行中")
                return;

            menuFlag = "进行中";
            重置订单列表();
            state = 2;
            getAllOrderData();

            Device.BeginInvokeOnMainThread(() =>
            {
                lb_allOrder.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                lb_nowOrder.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                lb_finishOrder.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                lb_cancelOrder.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
            });
            #endregion
        }

        void tapped_finishOeder(object sender, TappedEventArgs e)   //'已完成'按钮
        {
            // Label lb = (Label)sender;
            #region UI呈现
            if (menuFlag == "已完成")
                return;

            menuFlag = "已完成";
            重置订单列表();
            state = 3;
            getAllOrderData();
            Device.BeginInvokeOnMainThread(() =>
            {
                lb_allOrder.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                lb_nowOrder.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                lb_finishOrder.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                lb_cancelOrder.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
            });
            #endregion
        }

        void tapped_cancelOeder(object sender, TappedEventArgs e)   //'已取消'按钮
        {
            // Label lb = (Label)sender;
            #region UI呈现
            if (menuFlag == "已取消")
                return;

            menuFlag = "已取消";
            state = 0;
            getAllOrderData();
            Device.BeginInvokeOnMainThread(() =>
            {
                lb_allOrder.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                lb_nowOrder.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                lb_finishOrder.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                lb_cancelOrder.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
            });
            #endregion
        }

        private void Scrolled_scl_order(object sender, ScrolledEventArgs e)
        {
            if (scl_order.ScrollY >= scl_order.ContentSize.Height - scl_order.Height - 100 && !scrollLocker)
            {
                scrollLocker = true;
                getAllOrderData();

            }
        }


        /// <summary>
        /// 订单更多按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_more(object sender, EventArgs e)
        {
            var row = (Data.OrderData)((TappedEventArgs)e).Parameter;

         //   if(row.OrderType =="") //订单状态  控制更多按钮  显示什么选项

            frm_edtBox.IsVisible = true;
            block.IsVisible = true;
        }


        /// <summary>
        /// 关闭弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_closeBlock(object sender, EventArgs e)
        {
            block.IsVisible = false;
            frm_deleteCheckBox.IsVisible = false;
            frm_edtBox.IsVisible = false;
            frm_CancelOrderCheckBox.IsVisible = false;
        }

        /// <summary>
        /// 查看订单详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_checkOrderDtl(object sender,EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;


            var row = (Data.OrderData)((TappedEventArgs)e).Parameter;

            Views.ShoppingCart.Order.OrderDetailsPage page = new ShoppingCart.Order.OrderDetailsPage();

            page.OrderGUID = row.OrderGUID;
            page.getOrderDtl();

            Navigation.PushAsync(page, true);

            按钮防呆 = false;
        }
        

        /// <summary>
        /// 确认取消订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_ComfirmedCancelOrder(object sender,EventArgs e)
        {
            Tools.AsyncMsg am_取消订单 = new Tools.AsyncMsg();

            string para = "OrderGUID=" + 取消的订单.OrderGUID;

            am_取消订单.Completion += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    重置订单列表();
                    getAllOrderData();
                    tapped_closeBlock(null, null);
                });
            };

            am_取消订单.Cancel += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("提示", "取消订单失败" , "知道了");
                });
                return;
            };

            Tools.NetClass.GetStringByUrl("ExSql", "APP\\CloseOrder_1_0_0_1", para, am_取消订单);
        }
    }
}