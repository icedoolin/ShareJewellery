using Plugin.Media;
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
    public partial class afterSalePage: BasePage 
    {
        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
        Data.commodityData 选中商品;
        List<string> 售后图片GUIDs = new List<string>();  //删除的图片数据
        public string orderGUID { get; set; } = "";
        Data.OrderData _orderList = null;
        bool 按钮防呆 = false;
        public Data.OrderData orderList
        {
            get
            {
                if (_orderList == null)
                    _orderList = new Data.OrderData();
                return _orderList;
            }

            set
            {
                _orderList = value;
            }
        }

        /// <summary>
        /// 申请售后的类型 （免费戴售后、退款、正常售后）
        /// </summary>
        public string flag { get; set; } = ""; 

        public afterSalePage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }

        public void 刷新UI()
        {
            if (flag == "退款")
            {
                lb_headName.Text = "申请退款";
                st_afterSaleImg.IsVisible = false;
                lb_afteSaleType.RightText = "全额退款";
                ety_afterSaleReson.Placeholder = "请输入退款原因";

            }
            else if (flag == "售后")
            {
                lb_headName.Text = "申请售后";
                st_afterSaleImg.IsVisible = true;
                ety_afterSaleReson.Placeholder = "请输入售后原因";
            }
            else if (flag == "免费戴售后")
            {
                lb_headName.Text = "申请售后";
                st_afterSaleImg.IsVisible = true;
                lb_afteSaleType.RightText = "仅退货";
                ety_afterSaleReson.Placeholder = "请输入售后原因";
            }

            绘制列表();

        }

        void 绘制列表()
        {
            if (orderList == null)
                return;

            var fatherStackLayout = new StackLayout() { Padding = new Thickness(0, 0, 0, 0), Spacing = 0, BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.BackgroundColor, Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };

            int i = 0;//用来画分隔横线用
            foreach (var row in orderList.CommodityChildrenRows)
            {
                i++;
                var rowStack = new StackLayout() { Padding = new Thickness(0, 10, 0, 0), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 5 };

                var stack_右边块 = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 5 };

                var stack_右边块_上部 = new StackLayout() { Padding = new Thickness(10, 0, 10, 0), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 5 };

                var img_商品 = new Image() { Source = "homePage_new.png", HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, BackgroundColor = Color.Transparent, HeightRequest = 80, WidthRequest = 80 };

                var stack_右边块_上部_商品名和参数 = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 5 };

                var lb_商品名 = new Label() { Text = row.JewelleryName, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Start };

                var lb_商品名参数 = new Label() { Text = row.specForShow, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.End, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Start };

                stack_右边块_上部_商品名和参数.Children.Add(lb_商品名);
                stack_右边块_上部_商品名和参数.Children.Add(lb_商品名参数);

                stack_右边块_上部.Children.Add(img_商品);
                stack_右边块_上部.Children.Add(stack_右边块_上部_商品名和参数);

                stack_右边块.Children.Add(stack_右边块_上部);


                var stack_右边块_下部 = new StackLayout() { Padding = new Thickness(10, 0, 10, 0), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.End, Spacing = 5 };

                var stack_右边块_下部_数量部分 = new StackLayout() { BackgroundColor = Color.Transparent, BindingContext = row, IsVisible = true, Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, Spacing = 5 };

                stack_右边块_下部_数量部分.SetBinding(IsVisibleProperty, "isShow");

                var lb_金额描述 = new Label() { Text = "商品总价:", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center };
                var lb_商品总价 = new Label() { BindingContext = row, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, Text = row.totalPriceForShow, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center };

                lb_商品总价.SetBinding(Label.TextProperty, "totalPriceForShow");

                var temp_box = new BoxView() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, WidthRequest = 1 };

                var lb_数量描述 = new Label() { Text = "数量:", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center };

                var img_减号 = new Image() { Source = "reduce.png", HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, BackgroundColor = Color.Transparent, HeightRequest = 20, WidthRequest = 20 };

                TapGestureRecognizer reduce_tap = new TapGestureRecognizer();
                reduce_tap.CommandParameter = row;  //加入数据
                reduce_tap.Tapped += tapped_reduce;

                img_减号.GestureRecognizers.Add(reduce_tap);//加入点击手势


                var lb_数量 = new Label() { BindingContext = row, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, Text = row.number.ToString(), WidthRequest = 50, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

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
                stack_右边块_下部.Children.Add(temp_box);
                stack_右边块_下部.Children.Add(stack_右边块_下部_数量部分);

                stack_右边块.Children.Add(stack_右边块_下部);

                BoxView box_分割线 = new BoxView() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.End, HeightRequest = 0.5, BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.SpaceColor };
                stack_右边块.Children.Add(box_分割线);

                var st_售后 = new StackLayout() { Orientation = StackOrientation.Horizontal, Padding = new Thickness(10, 0, 10, 0), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, HeightRequest = com.cstc.ShareJewlryApp.Style.Shape.height.barLenth, Spacing = 5 };

                TapGestureRecognizer Selected_tap = new TapGestureRecognizer();
                Selected_tap.CommandParameter = row;  //加入数据
                Selected_tap.Tapped += tapped_Selected;

                st_售后.GestureRecognizers.Add(Selected_tap);//加入点击手势

                var box_填充 = new BoxView() { HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.Transparent, WidthRequest = 1 };

                box_填充.GestureRecognizers.Add(Selected_tap);//加入点击手势
                var lb_参与售后 = new Label() { Text = "参与售后", FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs12, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center };
                lb_参与售后.GestureRecognizers.Add(Selected_tap);//加入点击手势
                var lb_售后描述 = new Label() { Text = row.IsAfterSaleForShow, FontSize = com.cstc.ShareJewlryApp.Style.FontStyle.FontSize.Fs10, BindingContext = row, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center };

                lb_售后描述.SetBinding(Label.TextProperty, "IsAfterSaleForShow");
                lb_售后描述.GestureRecognizers.Add(Selected_tap);//加入点击手势
                var img_箭头 = new Image() { Source = "right_arrow.png", HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, HeightRequest = 12, WidthRequest = 8 };
                img_箭头.GestureRecognizers.Add(Selected_tap);//加入点击手势

                st_售后.Children.Add(lb_参与售后);
                st_售后.Children.Add(box_填充);
                st_售后.Children.Add(lb_售后描述);
                st_售后.Children.Add(img_箭头);

                stack_右边块.Children.Add(st_售后);

                BoxView box_分割块 = new BoxView() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.End, HeightRequest = 15, BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.SpaceColor };
                stack_右边块.Children.Add(box_分割块);
                rowStack.Children.Add(stack_右边块);
                fatherStackLayout.Children.Add(rowStack);

            }

            st_cmdty.Children.Add(fatherStackLayout);

        }

        /// <summary>
        /// 是否参与售后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_Selected(object sender, EventArgs e)
        {
            if (flag == "免费戴售后")
            {
                hud.Show_Toast("无需操作");
            }

            return;

            var row = (Data.commodityData)(((TappedEventArgs)e).Parameter);
            选中商品 = row;

            frm_SelectBox.IsVisible = true;
            block.IsVisible = true;
        }

        /// <summary>
        /// 商品数减1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_reduce(object sender, EventArgs e)
        {
            if (flag == "退款" || flag == "免费戴售后")
            {
                hud.Show_Toast("无需操作");
            }
            else if (flag == "售后")
            {
                var row = (Data.commodityData)(((TappedEventArgs)e).Parameter);
                if (row.number < 0)
                    return;
                row.number--;
            }
        }
       
        /// <summary>
        /// 商品数增1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_plus(object sender, EventArgs e)
        {
            if (flag == "退款" || flag == "免费戴售后")
            {
                hud.Show_Toast("无需操作");
            }
            else if (flag == "售后")
            {
                var row = (Data.commodityData)(((TappedEventArgs)e).Parameter);
                if (row.number >= row.maxNumber)
                    return;
                row.number++;
            }
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
        /// 弹出添加相片的方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_addAfterSaleImg(object sender, EventArgs e)
        {
            st_photoBox.IsVisible = true;
            block.IsVisible = true;
        }

        /// <summary>
        /// 从相册选择照片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_pickAlum(object sender, EventArgs e)
        {
            if (!Plugin.Media.CrossMedia.Current.IsPickPhotoSupported)
            {
                DisplayAlert("无法打开相册", "本APP未有权限进行此操作!", "知道了");
                return;
            }
            //  var page = Views.ImageClipping.ClippingPage.ClippingPagePageObj;

            //调用相册  
            Plugin.Media.CrossMedia.Current.PickPhotoAsync().ContinueWith(t =>
            {
                var file = t.Result;
                if (file == null)
                    return;
                Tools.AsyncMsg am_图片转换完成 = new Tools.AsyncMsg();
                //  System.IO.Stream stream = null;

                am_图片转换完成.Completion += ((object sender_am, string e_am) =>
                {
                    var stream = file.GetStream();
                    上传图片(stream);
                    file.Dispose();
                });

                ImageSource imgS = ImageSource.FromStream(() =>
                {
                    System.IO.Stream stream = null;
                    try
                    {
                        stream = file.GetStream();
                        am_图片转换完成.OnCompletion();
                    }
                    catch (System.Exception ex)
                    {
                        return null;
                    }

                    return stream;
                });

                Device.BeginInvokeOnMainThread(() =>
                {
                    添加图片到UI(imgS);
                });
            });
        }

        void 添加图片到UI(ImageSource image)
        {
            var row = (StackLayout)st_afterSaleImg.Children[st_afterSaleImg.Children.Count - 1];

            if (row.Children.Count == 5)
            {
                var newStack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Spacing = 10, HeightRequest = com.cstc.ShareJewlryApp.Style.Shape.height.afterSaleImgHetght };

                var newImg = new Image { Source = image, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, HeightRequest = com.cstc.ShareJewlryApp.Style.Shape.height.afterSaleImgHetght, WidthRequest = com.cstc.ShareJewlryApp.Style.Shape.height.afterSaleImgHetght };

                newStack.Children.Add(newImg);

                st_afterSaleImg.Children.Add(newStack);
                st_afterSaleImg.HeightRequest = (st_afterSaleImg.Children.Count * com.cstc.ShareJewlryApp.Style.Shape.height.afterSaleImgHetght) + ((st_afterSaleImg.Children.Count + 1) * 10);

                scl_cmdty.ScrollToAsync(newStack, ScrollToPosition.End, false);
            }
            else
            {
                var newImg = new Image { Source = image, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, HeightRequest = com.cstc.ShareJewlryApp.Style.Shape.height.afterSaleImgHetght, WidthRequest = com.cstc.ShareJewlryApp.Style.Shape.height.afterSaleImgHetght };

                row.Children.Add(newImg);
            }

            tapped_CloseBlock(null, null);
        }

        /// <summary>
        /// 关闭弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_CloseBlock(object sender, EventArgs e)
        {
            block.IsVisible = false;
            st_photoBox.IsVisible = false;
            frm_chooseBox.IsVisible = false;
            frm_SelectBox.IsVisible = false;
        }

        /// <summary>
        /// 拍照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_takePhoto(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                DisplayAlert("无法打开摄像头", "当前没有可用的摄像头设备!", "知道了");
                return;
            }

            string FileName = System.DateTime.Now.ToString("yyMMddHHmmssfff") + ".jpg";
            Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions { Directory = "Swisin", Name = FileName }).ContinueWith(t =>
            {
                var file = t.Result;
                if (file == null)
                    return;
                Tools.AsyncMsg am_图片转换完成 = new Tools.AsyncMsg();
                //  System.IO.Stream stream = null;

                am_图片转换完成.Completion += ((object sender_am, string e_am) =>
                {
                    var stream = file.GetStream();
                    上传图片(stream);
                    file.Dispose();
                });

                ImageSource imgS = ImageSource.FromStream(() =>
                {
                    System.IO.Stream stream = null;
                    try
                    {
                        stream = file.GetStream();
                        am_图片转换完成.OnCompletion();
                    }
                    catch (System.Exception ex)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            // UI.UIColor.hud.Show_Toast("读取文件流失败：" + ex.Message);
                        });
                        return null;
                    }

                    return stream;
                });

                // ImageSource = imgS;

                Device.BeginInvokeOnMainThread(() =>
                {
                    添加图片到UI(imgS);
                });
            });
        }

        void 上传图片(System.IO.Stream imageStream)
        {
            #region 转成byte数组
            byte[] imageData;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                imageStream.CopyTo(ms);
                imageData = ms.ToArray();
            }
            #endregion

            #region 压缩图片


            Tools.IClippingClass rez = DependencyService.Get<Tools.IClippingClass>();  //压缩图片
            imageData = rez.ResizeImage(imageData, 500);

            #endregion

            string CutImg = Convert.ToBase64String(imageData);

            Tools.AsyncMsg am = new Tools.AsyncMsg();

            string guid = System.Guid.NewGuid().ToString();
            string newheadPhotoGuid = guid + ".JPG";

            am.Completion += ((object sender_am, string e_am) =>
            {
                售后图片GUIDs.Add(newheadPhotoGuid);
                // 添加图片到UI(ImageSource);

            });

            am.Cancel += ((object sender_am, string e_am) =>
            {
                // isUpload = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("提示", "商品图片传输错误！" , "知道了");
                });
            });

            Tools.NetClass.PostImgByUrlForSmall("shouhoutupian\\" + newheadPhotoGuid.Substring(0, 2) + "\\", newheadPhotoGuid, CutImg, am);

            // img_Logo.UpLoadImage("", "", "MENUIMG\\" + newMenuPhotoGuid.Substring(0, 3) + "\\", newMenuPhotoGuid, CutImg, am);
        }

        /// <summary>
        /// 悬着退款类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_afterSaleTypeBox(object sender, EventArgs e)
        {
            if (flag == "退款" || flag == "免费戴售后")
            {
                hud.Show_Toast("无需选择售后类型");
            }
            else if (flag == "售后")
            {
                frm_chooseBox.IsVisible = true;
                block.IsVisible = true;
            }

        }

        /// <summary>
        /// 退款类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_afterSaleTypeBackMonney(object sender, EventArgs e)
        {
            lb_afteSaleType.RightText = "仅退款";
            tapped_CloseBlock(null, null);
        }

        /// <summary>
        /// 退款类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_afterSaleTypeBackMonneyAndCmdty(object sender, EventArgs e)
        {
            lb_afteSaleType.RightText = "退货退款";
            tapped_CloseBlock(null, null);
        }

        /// <summary>
        /// 提交售后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_comfirmAfterSale(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            string 售后原因 = ety_afterSaleReson.Text.Trim();
            string 售后类型 = lb_afteSaleType.RightText.Trim();

            if (flag == "退款")
            {


                Tools.AsyncMsg am_提交售后 = new Tools.AsyncMsg();

                string para = "OrderGUID=" + orderList.OrderGUID + "&Reason=" + 售后原因;

                am_提交售后.Completion += (object obj, string ex) =>
                {

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Show_Toast("提交成功");
                        ///跳转到提交申请页面
                        Navigation.PopAsync(true);
                        按钮防呆 = false;
                    });
                };

                am_提交售后.Cancel += (object obj, string ex) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //DisplayAlert("提示", "获取用户数据失败:" + ex.ToString(), "知道了");
                        按钮防呆 = false;
                    });
                    return;
                };

                Tools.NetClass.GetStringByUrl("ExSql", "App\\OrderRefund_1_0_0_1", para, am_提交售后);
            }
            else
            {
                string 售后单GUID = Guid.NewGuid().ToString();
                string orderjson = "";

                string orderDetailjson = "";
                string imgJson = "";
                Dictionary<string, string> 售后单详情 = new Dictionary<string, string>();
                foreach (var 售后商品 in orderList.CommodityChildrenRows)
                {
                    if (售后商品.IsAfterSale)
                    {
                        Dictionary<string, string> 售后商品详情 = new Dictionary<string, string>();
                        售后商品详情.Add("OrderDetailGUID", 售后商品.OredrDetailedGUID);
                        if (flag == "售后")
                        {
                            售后商品详情.Add("AfterOrderType", 售后类型);
                        }
                        else if (flag == "免费戴售后")
                        {
                            售后商品详情.Add("AfterOrderType", "仅退货");
                        }
                        售后商品详情.Add("JewelleryGUID", 售后商品.JewelleryGUID);
                        售后商品详情.Add("AfterOrderGUID", 售后单GUID);
                        售后商品详情.Add("SecondSpecDetailedGUID", 售后商品.SecondSpecGUID);
                        售后商品详情.Add("number", 售后商品.number.ToString());
                        Tools.NetClass.AddPostJsonString(ref orderDetailjson, 售后商品详情);
                    }
                }

                售后单详情.Add("AfterOrderGUID", 售后单GUID);
                售后单详情.Add("Reason", 售后原因);
                售后单详情.Add("UserGUID", Data.UserInfoCache.UserGUID);
                售后单详情.Add("OrderGUID", orderGUID);
                if (flag == "售后")
                {
                    售后单详情.Add("AfterOrderType", 售后类型);
                }
                else if (flag == "免费戴售后")
                {
                    售后单详情.Add("AfterOrderType", "仅退货");
                }
                售后单详情.Add("ChildRows_APP\\\\InsertAfterOrderDetailed_1_0_0_1", "[" + orderDetailjson + "]");

                if (售后图片GUIDs.Count > 0)
                {
                    foreach (var 售后图片guid in 售后图片GUIDs)
                    {
                        Dictionary<string, string> 售后商品图片 = new Dictionary<string, string>();

                        售后商品图片.Add("AfterOrderGUID", 售后单GUID);
                        售后商品图片.Add("AfterOrderPic", 售后图片guid);
                        Tools.NetClass.AddPostJsonString(ref imgJson, 售后商品图片);

                    }
                }

                售后单详情.Add("ChildRows_APP\\\\InsertAfterOrderPic_1_0_0_1", "[" + imgJson + "]");
                //  售后单详情.Add("ChildRows_APP\\\\InsertOredrDetailed_1_0_0_1", "[" + orderDetailjson + "]");
                Tools.NetClass.AddPostJsonString(ref orderjson, 售后单详情);

                Tools.AsyncMsg am_免费戴售后 = new Tools.AsyncMsg();

                am_免费戴售后.Completion += (object obj, string ex) =>
                {

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Show_Toast("提交成功");
                        ///跳转到提交申请页面
                        Navigation.PopAsync(true);
                        按钮防呆 = false;
                    });
                };


                am_免费戴售后.Cancel += (object obj, string ex) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayAlert("提示", "提交售后失败", "知道了");
                        按钮防呆 = false;
                    });
                    return;
                };

                Tools.NetClass.PostDataByUrl("App\\InsertAfterOrder_1_0_0_1", "ExSql", orderjson, am_免费戴售后);
            }

        }

        /// <summary>
        /// 参与售后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_isAfterSaled(object sender, EventArgs e)
        {
            选中商品.IsAfterSale = true;
            tapped_CloseBlock(null, null);
        }

        /// <summary>
        /// 不参与售后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_unAfterSaled(object sender, EventArgs e)
        {
            选中商品.IsAfterSale = false;
            tapped_CloseBlock(null, null);
        }
    }
}