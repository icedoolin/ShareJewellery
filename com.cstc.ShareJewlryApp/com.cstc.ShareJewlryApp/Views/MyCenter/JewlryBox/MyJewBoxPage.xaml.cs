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
    public partial class MyJewBoxPage: BasePage 
    {
        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
        Tools.IWeChat wechat = DependencyService.Get<Tools.IWeChat>();
        public string 跳转标识 { get; set; } = "";
        bool 按钮防呆 = false;
        string 租借中的订单GUID = "";
        string 报损单GUID = "";
        string 删除珠宝GUID = "";
 
        string 物流号 = "";
        string 物流公司 = "";
        string 商铺GUID = "";
        decimal 报损金额 = 0;
 
        ObservableCollection<Data.commodityData> dataList = new ObservableCollection<Data.commodityData>();  //收藏盒数据源

        public MyJewBoxPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            st_StatusBar.HeightRequest = Helpers.MConfig.screenWidth * 0.8666;
            gd_img.HeightRequest = Helpers.MConfig.screenWidth * 0.4666; 
 
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if(Data.UserInfoCache.UserGUID =="")
            { 
                block.IsVisible = true;
                st_LoginBox.IsVisible = true;
            }
            else
            {
                getDressFreeOrderData();

            }
            //  GetCollectionjewelryData();
        }
 
        /// <summary>
        /// 获取正在免费戴的首饰信息
        /// </summary>
        public void getDressFreeOrderData()
        {
            Tools.AsyncMsg am_获取免费戴商品 = new Tools.AsyncMsg();

            string para = "UserGUID=" + Data.UserInfoCache.UserGUID;

            am_获取免费戴商品.Completion += (object obj, string ex) =>
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
                    return;
                }

                if (returnJson == "")
                {
                    img_dressfree.Source = "noRent.png";
                    st_outLine_img_dressfree.BackgroundColor = Color.White;
                    lb_commodityName.IsVisible = false;
                    btn_evalution.IsVisible = false;
                    btn_AddLogitics.IsVisible = false;
                    btn_intervene.IsVisible = false;
                    btn_lookArround.IsVisible = true;
                    btn_lookLogitics.IsVisible = false;
                    btn_lookOrder.IsVisible = false;
                    btn_Pay.IsVisible = false;
                    btn_backJewe.IsVisible = false;
                    return;
                }

                Newtonsoft.Json.Linq.JArray jar = null;
                try
                {
                    jar = Newtonsoft.Json.Linq.JArray.Parse(returnJson);
                }
                catch (Exception exc)
                {
                    return;
                }

                租借中的订单GUID = jar[0]["OrderGUID"].ToString();
                报损单GUID = jar[0]["FaultyGUID"].ToString();
                Device.BeginInvokeOnMainThread( () =>
                {
                    st_outLine_img_dressfree.BackgroundColor = Color.FromHex("#f0f0f0");
                    lb_commodityName.IsVisible = true;
                    lb_dressfreeDesc.TextColor = Color.FromHex("#fd0462");
                });
              

                string 图片 =Helpers.MConfig.picUrl + "Pic/shangpintupian/" + jar[0]["JewelleryPicName"].ToString().Substring(0, 2).ToUpper() + "/" + jar[0]["JewelleryPicName"].ToString();

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (jar[0]["OrderType"].ToString() == "待评价")
                    {
                        lb_commodityName.Text = jar[0]["JewelleryName"].ToString();
                        img_dressfree.Source = 图片;
                     // img_dressfree.Source = "evaluation.png";
                        lb_dressfreeDesc.Text = "喜欢就说出来嘛，不要害羞哦";
                        btn_evalution.IsVisible = true;
                        btn_AddLogitics.IsVisible = false;
                        btn_intervene.IsVisible = false;
                        btn_lookArround.IsVisible = false;
                        btn_lookLogitics.IsVisible = false;
                        btn_lookOrder.IsVisible = false;
                        btn_Pay.IsVisible = false;
                        btn_backJewe.IsVisible = false;

                    }
                    else if (jar[0]["OrderType"].ToString() == "归还中" || jar[0]["OrderType"].ToString() == "售后中" || jar[0]["OrderType"].ToString() == "用户报损")
                    {
                        lb_commodityName.Text = jar[0]["JewelleryName"].ToString();
                        img_dressfree.Source = 图片;
                        // img_dressfree.Source = "returnning.png";
                        lb_dressfreeDesc.Text = "商品归还中，等待卖家确认...";
                        btn_evalution.IsVisible = false;
                        btn_AddLogitics.IsVisible = false;
                        btn_intervene.IsVisible = false;
                        btn_lookArround.IsVisible = false;
                        btn_lookLogitics.IsVisible = true;
                        btn_lookOrder.IsVisible = true;
                        btn_Pay.IsVisible = false;
                        btn_backJewe.IsVisible = false;

                        物流号 = (jar[0]["LogisticsNumber"] == null) ? "" : jar[0]["LogisticsNumber"].ToString();
                        物流公司 = (jar[0]["LogisticsCompanyCode"] == null) ? "" : jar[0]["LogisticsCompanyCode"].ToString();

                    }
                    else if (jar[0]["OrderType"].ToString() == "待发货")
                    {
                        lb_commodityName.Text = jar[0]["JewelleryName"].ToString();
                        img_dressfree.Source = 图片;
                       // img_dressfree.Source = "delivery.png";

                        lb_dressfreeDesc.Text = "商家正在准备配送";
                        btn_evalution.IsVisible = false;
                        btn_AddLogitics.IsVisible = false;
                        btn_intervene.IsVisible = false;
                        btn_lookArround.IsVisible = false;
                        btn_lookLogitics.IsVisible = false;
                        btn_lookOrder.IsVisible = true;
                        btn_Pay.IsVisible = false;
                        btn_backJewe.IsVisible = false;

                    }
                    else if (jar[0]["OrderType"].ToString() == "卖家报损")
                    {
                        lb_commodityName.Text = jar[0]["JewelleryName"].ToString();
                        img_dressfree.Source = 图片;
                        // img_dressfree.Source = "locking.png";
                        lb_dressfreeDesc.Text = "您的【免费戴】商品有所损坏，该功能冻结";
                        btn_evalution.IsVisible = false;
                        btn_AddLogitics.IsVisible = false;
                        btn_intervene.IsVisible = false;
                        btn_lookArround.IsVisible = false;
                        btn_lookLogitics.IsVisible = false;
                        btn_lookOrder.IsVisible = true;
                        btn_Pay.IsVisible = true;
                        btn_backJewe.IsVisible = false;

                        报损金额 = Convert.ToDecimal(jar[0]["FaultyPrice"].ToString());
                    }
                    else if (jar[0]["OrderType"].ToString() == "待收货")
                    {
                        lb_commodityName.Text = jar[0]["JewelleryName"].ToString();
                        img_dressfree.Source = 图片;
                        // img_dressfree.Source = "takeDelivery.png";
                        lb_dressfreeDesc.Text = "您的珠宝飞速赶路中";
                        btn_evalution.IsVisible = false;
                        btn_AddLogitics.IsVisible = false;
                        btn_intervene.IsVisible = false;
                        btn_lookArround.IsVisible = false;
                        btn_lookLogitics.IsVisible = true;
                        btn_lookOrder.IsVisible = true;
                        btn_Pay.IsVisible = false;
                        btn_backJewe.IsVisible = false;
                        物流号 = jar[0]["LogisticsNumber"].ToString();
                        物流公司 = jar[0]["LogisticsCompanyCode"].ToString();
                    }
                    else if (jar[0]["OrderType"].ToString() == "平台介入中")
                    {
                        lb_commodityName.Text = jar[0]["JewelleryName"].ToString();
                        img_dressfree.Source = 图片;
                        //  img_dressfree.Source = "intervene.png";
                        lb_dressfreeDesc.Text = "商家拒绝售后（平台介入中）";
                        btn_evalution.IsVisible = false;
                        btn_AddLogitics.IsVisible = false;
                        btn_intervene.IsVisible = false;
                        btn_lookArround.IsVisible = false;
                        btn_lookLogitics.IsVisible = false;
                        btn_lookOrder.IsVisible = true;
                        btn_Pay.IsVisible = false;
                        btn_backJewe.IsVisible = false;
                    }
                    else if (jar[0]["OrderType"].ToString() == "拒绝售后")
                    {
                        lb_commodityName.Text = jar[0]["JewelleryName"].ToString();
                        img_dressfree.Source = 图片;
                        // img_dressfree.Source = "intervene.png";
                        lb_dressfreeDesc.Text = "商家拒绝售后";
                        btn_evalution.IsVisible = false;
                        btn_AddLogitics.IsVisible = false;
                        btn_intervene.IsVisible = false;
                        btn_lookArround.IsVisible = false;
                        btn_lookLogitics.IsVisible = false;
                        btn_lookOrder.IsVisible = true;
                        btn_Pay.IsVisible = false;
                        btn_backJewe.IsVisible = false;
                        商铺GUID = jar[0]["ShopGUID"].ToString();
                    }
                    else if (jar[0]["OrderType"].ToString() == "同意售后")
                    {
                        lb_commodityName.Text = jar[0]["JewelleryName"].ToString();
                        img_dressfree.Source = 图片;
                        //  img_dressfree.Source = "intervene.png";
                        lb_dressfreeDesc.Text = "商家同意售后";
                        btn_evalution.IsVisible = false;
                        btn_AddLogitics.IsVisible = true;
                        btn_intervene.IsVisible = false;
                        btn_lookArround.IsVisible = false;
                        btn_lookLogitics.IsVisible = false;
                        btn_lookOrder.IsVisible = true;
                        btn_Pay.IsVisible = false;
                        btn_backJewe.IsVisible = false;
                    }
                    else if (jar[0]["OrderType"].ToString() == "待处理" || jar[0]["OrderType"].ToString() == "申请取消")
                    {
                        lb_commodityName.Text = jar[0]["JewelleryName"].ToString();
                        img_dressfree.Source = 图片;
                        // img_dressfree.Source = "intervene.png";
                        string 月份 = "";
                        string 日数 = "";
                        try
                        {
                            月份 = Convert.ToInt32(jar[0]["AutomaticConsentTime"].ToString().Substring(5, 2)).ToString();
                            日数 = Convert.ToInt32(jar[0]["AutomaticConsentTime"].ToString().Substring(8, 2)).ToString();
                        }
                        catch (Exception exc)
                        {
                            //DisplayAlert("错误", "自动同意期限获取失败！" + exc.Message, "知道了");
                            return;
                        }

                        if (jar[0]["OrderType"].ToString() == "待处理")
                        {
                            lb_dressfreeDesc.Text = "您已发起售后，商家将在" + 月份 + "月" + 日数 + "日" + "自动同意";
                        }

                        if (jar[0]["OrderType"].ToString() == "申请取消")
                        {
                            lb_dressfreeDesc.Text = "您已发起申请取消订单，商家将在" + 月份 + "月" + 日数 + "日" + "自动同意";
                        }

                        btn_evalution.IsVisible = false;
                        btn_AddLogitics.IsVisible = false;
                        btn_intervene.IsVisible = false;
                        btn_lookArround.IsVisible = false;
                        btn_lookLogitics.IsVisible = false;
                        btn_lookOrder.IsVisible = true;
                        btn_Pay.IsVisible = false;
                        btn_backJewe.IsVisible = false;
                    }
                    else if (jar[0]["OrderType"].ToString() == "租借中")
                    {
                        img_dressfree.Source = 图片;
                        // img_dressfree.Source = "intervene.png";
                        lb_commodityName.Text= jar[0]["JewelleryName"].ToString();
                        lb_dressfreeDesc.Text = "佩戴中";
                        btn_backJewe.IsVisible = true;
                        btn_evalution.IsVisible = false;
                        btn_AddLogitics.IsVisible = false;
                        btn_intervene.IsVisible = false;
                        btn_lookArround.IsVisible = false;
                        btn_lookLogitics.IsVisible = false;
                        btn_lookOrder.IsVisible = true;
                        btn_Pay.IsVisible = false;

                    }
                    else
                    {
                        img_dressfree.Source = "noRent.png";
                        st_outLine_img_dressfree.BackgroundColor = Color.White;
                        lb_commodityName.IsVisible = false;
                        btn_evalution.IsVisible = false;
                        btn_AddLogitics.IsVisible = false;
                        btn_intervene.IsVisible = false;
                        btn_lookArround.IsVisible = true;
                        btn_lookLogitics.IsVisible = false;
                        btn_lookOrder.IsVisible = false;
                        btn_Pay.IsVisible = false;
                        btn_backJewe.IsVisible = false;
                    }


                });
            };
 
            Tools.NetClass.GetStringByUrl("QueryData", "APP\\GetLastOrderInfo_1_0_0_1", para, am_获取免费戴商品);
        }

        /// <summary>
        /// 获取收藏
        /// </summary>
        public void GetCollectionjewelryData()
        {
            Tools.AsyncMsg am_获取商品 = new Tools.AsyncMsg();

            string para = "UserGUID=" + Data.UserInfoCache.UserGUID;

            am_获取商品.Completion += (object obj, string ex) =>
            {
                string returnJson = obj.ToString();
                string ErrMsg = "";
                if (returnJson == "[]" || returnJson == "")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //ls_myJewelryBox.IsVisible = false;
                        st_noJwelry.IsVisible = true;
                    });

                    return;
                }
                   

                try
                {
                    returnJson = Tools.CommonClass.GetJsonByTag(returnJson, ref ErrMsg);
                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "解析首饰盒数据错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                if (returnJson == "")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //ls_myJewelryBox.IsVisible = false;
                        st_noJwelry.IsVisible = true;
                    });

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
                        //await DisplayAlert("错误", "解析首饰盒数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                dataList = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Data.commodityData>>(jar.ToString());

                Device.BeginInvokeOnMainThread(() =>
                {
                    ls_myJewelryBox.ItemsSource = dataList;
                });
            };
 
            Tools.NetClass.GetStringByUrl("QueryData", "APP\\GetCollectionFolderInfo_1_0_0_1", para, am_获取商品);
        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_closePage(object sender, EventArgs e)
        {

            if (跳转标识 == "选项卡")
                return;

            Device.BeginInvokeOnMainThread(() =>
            {
                if (按钮防呆)
                    return;
                按钮防呆 = true;

                Navigation.PopAsync(true);

                按钮防呆 = false;
            });
        }

        //void tapped_manage(object sender, EventArgs e)
        //{
        //    string flag = lb_manage.Text;

        //    Device.BeginInvokeOnMainThread(() =>
        //    {
        //        if (flag == "管理")
        //        {
        //            lb_manage.Text = "完成";
        //            foreach (var temp in dataList)
        //            {
        //                try
        //                {
        //                    temp.isManage = true;
        //                }
        //                catch (Exception ex)
        //                {
        //                    DisplayAlert("", ex.Message, "");
        //                }

        //                //break;
        //            }
        //        }
        //        else if (flag == "完成")
        //        {
        //            lb_manage.Text = "管理";
        //            foreach (var temp in dataList)
        //            {
        //                temp.isManage = false;
        //            }
        //        }
        //    });
        //}

        /// <summary>
        /// 取消弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_cancle(object sender, EventArgs e)
        {
            删除珠宝GUID = "";

            block.IsVisible = false;
            st_checkBox.IsVisible = false;
            st_ImterveneCheckBox.IsVisible = false;
        }


        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_delete(object sender, EventArgs e)
        {
            Tools.AsyncMsg am_删除商品 = new Tools.AsyncMsg();

            string para = "UserGUID=" + Data.UserInfoCache.UserGUID + "&JewelleryGUID=" + 删除珠宝GUID;

            am_删除商品.Completion += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {                        //更新用户信息
                    Helpers.AsyncMsg am_更新用户信息 = new Helpers.AsyncMsg();
                    am_更新用户信息.Completion += (object obss, string bbb) =>
                    {

                    };
                    Data.UserinfoMgr.RefreshUserInfoCache("个人信息", am_更新用户信息);
                    foreach (var temp in dataList)
                    {
                        if (temp.JewelleryGUID == 删除珠宝GUID)
                        {
                            //  Device.BeginInvokeOnMainThread(() =>
                            // {
                            dataList.Remove(temp);
                            tapped_cancle(null, null);
                            // });


                            break;
                        }
                    }

                    if (dataList.Count == 0)
                    {
                        // Device.BeginInvokeOnMainThread(() =>
                        // {
                        //ls_myJewelryBox.IsVisible = false;
                        st_noJwelry.IsVisible = true;
                        // });

                    }
                });


            };

 
            Tools.NetClass.GetStringByUrl("ExSql", "APP\\DeleteCollectionFolderJewellery_1_0_0_1", para, am_删除商品);
        }

        /// <summary>
        /// 打开还珠宝页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_OpenReturnJewelryPage(object sender, EventArgs e)
        {

            if (按钮防呆)
                return;
            按钮防呆 = true;

            ReturnJewelryPage page = new ReturnJewelryPage();
            page.orderGuid = 租借中的订单GUID;
            page.获取商品();
            Navigation.PushAsync(page, true);

            按钮防呆 = false;
        }

        /// <summary>
        /// 弹出取消收藏确认框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_ShowDelete(object sender, TappedEventArgs e)
        {
            var row = (Data.commodityData)e.Parameter;

            删除珠宝GUID = row.JewelleryGUID;

            block.IsVisible = true;
            st_checkBox.IsVisible = true;
        }

        /// <summary>
        /// 历史订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_OpenHistoryOrderPage(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 免费戴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_dressUpFree(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            Data.commodityData cdata = ((Data.commodityData)(((Frame)sender).BindingContext));
            if (cdata.JewelleryState)
            {
                Views.HomePage.ProductDetails.ProductDetailsPage page = new HomePage.ProductDetails.ProductDetailsPage();

                page.JewelleryGUID = cdata.JewelleryGUID;
 
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushAsync(page, true);

                });

            }
            按钮防呆 = false;

 
        }

        /// <summary>
        /// 购买珠宝
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_buyCmdty(object sender, TappedEventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            var row = (Data.commodityData)e.Parameter;

            if (!row.JewelleryState)
            {
                Navigation.PushAsync(new Views.HomePage.ProductDetails.ProductSoldOutPage(), true);
                按钮防呆 = false;
                return;
            }

            ObservableCollection<Data.commodityData> rows = new ObservableCollection<Data.commodityData>();

            rows.Add(row);

            Data.OrderData tempData = new Data.OrderData() { ShopGUID = row.ShopGUID, ShopName = row.ShopName, CommodityChildrenRows = rows };

            ObservableCollection<Data.OrderData> orderList = new ObservableCollection<Data.OrderData>();

            orderList.Add(tempData);

            Views.ShoppingCart.Order.OrderPage page = new ShoppingCart.Order.OrderPage();

            page.orderList = orderList;
            page.订单标志 = "购买";
            page.绘制列表();

            Navigation.PushAsync(page, true);

            按钮防呆 = false;
        }

 
        /// <summary>
        /// 查看订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_lookOrder(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            Views.ShoppingCart.Order.OrderDetailsPage page = new ShoppingCart.Order.OrderDetailsPage();

            page.OrderGUID = 租借中的订单GUID;
            page.getOrderDtl();

            Navigation.PushAsync(page, true);

            按钮防呆 = false;
        }

        /// <summary>
        /// 评价页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_OpenEvaJewelryPage(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            Views.ShoppingCart.Order.EvaluationPage page = new ShoppingCart.Order.EvaluationPage();

            page.页面标志 = "新增";
            page.orderGuid = 租借中的订单GUID;
            page.获取订单详情();
            Navigation.PushAsync(page, true);

            按钮防呆 = false;
        }


        /// <summary>
        /// 查看物流
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_lookLogitics(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            Views.ShoppingCart.Order.CheckLogisticsPage page = new ShoppingCart.Order.CheckLogisticsPage();

            page.运单号 = 物流号;
            page.物流公司 = 物流公司;
            page.loadData();

            Navigation.PushAsync(page, true);

            按钮防呆 = false;

        }

        /// <summary>
        /// 填写物流单号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_addBackCmdtyOrder(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;



            ShoppingCart.Order.BackCmdtyPage page = new ShoppingCart.Order.BackCmdtyPage();
            page.orderGuid = 租借中的订单GUID;
            page.获取售后商品();

            Navigation.PushAsync(page, true);


            按钮防呆 = false;
        }

        /// <summary>
        /// 去逛逛
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_lookAround(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            Navigation.PopToRootAsync(true);

            按钮防呆 = false;

        }

        /// <summary>
        /// 申请平台介入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_inretvene(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;


            Tools.AsyncMsg am_平台介入 = new Tools.AsyncMsg();
            string para = "";

            para = "ShopGUID=" + 商铺GUID + "&UserGUID=" + Data.UserInfoCache.UserGUID + "&OrderGUID=" + 租借中的订单GUID + "&Price=0&AppealType=售后";


            am_平台介入.Completion += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    getDressFreeOrderData();
                    //  tapped_cancle(null, null);
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
        /// 弹出确认平台介入弹框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_ShowInretveneBox(object sender, EventArgs e)
        {
            block.IsVisible = true;
            st_ImterveneCheckBox.IsVisible = true;
        }

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_pay(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            Views.ShoppingCart.Order.ChoosePayPage page = new ShoppingCart.Order.ChoosePayPage();
            page.支付标志 = "报损赔付";
            page.orderGuid = 租借中的订单GUID;
            page.totalPrice = 报损金额;
            page.报损单GUID = 报损单GUID;
            page.UI呈现();

            Navigation.PushAsync(page, true);

            按钮防呆 = false;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_openHomePage(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync(false);
        }

        /// <summary>
        /// 分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_openClassificationPage(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            Views.Classification.ClassificationPage page = new Classification.ClassificationPage();

            Navigation.PushAsync(page, false);


           // Navigation.PushAsync(Tools.tabbedPageClass.classificationPage, false);
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            //  Navigation.RemovePage(Tools.tabbedPageClass.shoppingCartMainPage);

            按钮防呆 = false;
        }

        /// <summary>
        /// 我的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_OpenMyPage(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;


            MyCenter.MyCenterPage page = new MyCenterPage();


            Navigation.PushAsync(page, false);

           // Navigation.PushAsync(Tools.tabbedPageClass.myCenterPage, false);


            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            //Navigation.RemovePage(Tools.tabbedPageClass.shoppingCartMainPage);

            按钮防呆 = false;
        }

        private void St_LoginBox_LoginSuccess(object sender, EventArgs e)
        {
            block.IsVisible = false;
            st_LoginBox.IsVisible = false;
        }

        private void St_LoginBox_LoginCancel(object sender, EventArgs e)
        {
            block.IsVisible = false;
            st_LoginBox.IsVisible = false;

        }
        /// <summary>
        /// 点选收藏盒珠宝
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_selectItem(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            Data.commodityData cdata = ((Data.commodityData)(((StackLayout)sender).BindingContext));
 
            if (!cdata.JewelleryState)
            {
                Navigation.PushAsync(new Views.HomePage.ProductDetails.ProductSoldOutPage(), true);
                按钮防呆 = false;
                return;
            }

            Views.HomePage.ProductDetails.ProductDetailsPage page = new HomePage.ProductDetails.ProductDetailsPage();
            page.JewelleryGUID = cdata.JewelleryGUID;
            // page.显示商品详情图片();
            //page.getCommodityDetailsData();

            Navigation.PushAsync(page, true);

            按钮防呆 = false;
        }

        private void tapped_ShowDelete(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 打开历史记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageHead_BtnForwardClick(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;
            Navigation.PushAsync(new JewelryBoxHistoryOrderPage(), true);
            按钮防呆 = false;
        }

        private void 搜索(object sender, EventArgs e)
        {

        }
    }
}