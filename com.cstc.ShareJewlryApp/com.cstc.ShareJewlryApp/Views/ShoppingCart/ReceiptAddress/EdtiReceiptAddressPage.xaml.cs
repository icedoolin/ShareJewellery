using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.ShoppingCart.ReceiptAddress
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EdtiReceiptAddressPage: BasePage 
    {
        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
        public Tools.AsyncMsg am_地址更新 = new Tools.AsyncMsg();
        int load = 0;
        public string 收件人 { get; set; } = "";
        public string 联系电话 { get; set; } = "";
        public string 收件城市 { get; set; } = "";
        public string 详细地址 { get; set; } = "";
        public bool 默认地址 { get; set; } = false;

        string chooseFlag = "省";
        public string 选择省份 { get; set; } = "";
        string 选择省份ID = "";
        public string 选择市 { get; set; } = "";
        string 选择市ID = "";
        public string 选择区县 { get; set; } = "";
        public string 选择区县ID { get; set; } = "";
         
        public string editFlag { get; set; } = "新增";
        public string 收货地址GUID { get; set; } = "";
        bool 按钮防呆 = false;

        Tools.AsyncMsg am_绘制完毕 = new Tools.AsyncMsg();
        ObservableCollection<Data.RegionData> dataList = new ObservableCollection<Data.RegionData>();  

        ObservableCollection<Data.RegionData> provinceList = new ObservableCollection<Data.RegionData>();  //省份数据源
        ObservableCollection<Data.RegionData> cityList = new ObservableCollection<Data.RegionData>();     //市数据源
        ObservableCollection<Data.RegionData> AreasList = new ObservableCollection<Data.RegionData>();    //区县数据源

        public EdtiReceiptAddressPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            // getRegionData("");
        }

        public void 修改UI呈现()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                lb_ProvinceDetails.Text = 选择省份;
                lb_CityDetails.Text = 选择市;
                lb_DistrictDetails.Text = 选择区县;
                ety_Name.Text = 收件人;
                ety_tel.Text = 联系电话;
                lb_addressCity.RightText = 收件城市;
                ety_detailAddress.Text = 详细地址;
                sw_defultAddress.IsToggled = 默认地址;

                getRegionData("");
            });

        }
 
        /// <summary>
        /// 根据父级地名GUID获取下级数据
        /// </summary>
        /// <param name="父级地名GUID"></param>
        public void getRegionData(string 父级地名GUID)
        {
            Tools.AsyncMsg am_获取地名 = new Tools.AsyncMsg();

            string para = "FatherID=" + 父级地名GUID;

            am_获取地名.Completion += (object obj, string ex) =>
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
                        //await DisplayAlert("错误", "解析地点的数据错误！" + exc.Message, "知道了");
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
                        //await DisplayAlert("错误", "解析地点数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }
                #endregion
                if (chooseFlag == "省")
                {
                    provinceList = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Data.RegionData>>(jar.ToString());

                    绘制城市选择器(provinceList);
                }
                else if (chooseFlag == "市")
                {
                    cityList = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Data.RegionData>>(jar.ToString());

                    绘制城市选择器(cityList);
                }
                else if (chooseFlag == "区")
                {
                    AreasList = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Data.RegionData>>(jar.ToString());

                    绘制城市选择器(AreasList);
                }
            };

            //am_获取地名.Cancel += (object obj, string ex) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        DisplayAlert("提示", "获取地区失败:" + ex.ToString(), "知道了");
            //    });
            //    return;
            //};


            if (chooseFlag == "省")
            {
                Tools.NetClass.GetStringByUrl("QueryData", "APP\\GetProvinceInitials_1_0_0_1,APP\\GetProvince_1_0_0_1", para, am_获取地名);
            }
            else if (chooseFlag == "市")
            {
                Tools.NetClass.GetStringByUrl("QueryData", "APP\\GetMunicipalityInitials_1_0_0_1,APP\\GetMunicipality_1_0_0_1", para, am_获取地名);
            }
            else if (chooseFlag == "区")
            {
                Tools.NetClass.GetStringByUrl("QueryData", "APP\\GetCountyInitials_1_0_0_1,APP\\GetCounty_1_0_0_1", para, am_获取地名);
            }


        }

        void 绘制城市选择器(ObservableCollection<Data.RegionData> list)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                #region 移除原有控件
                st_Picker.Children.Clear();
                scl_Picker.ScrollToAsync(0, 0, false);
                #endregion

                if (list == null || list.Count <= 0)
                    return;

                foreach (var item in list)
                {
                    if (item.City.Count > 0)
                    {
                        var lb_索引标题 = new Label { IsVisible = true, Text = item.Initials, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.Center, LineBreakMode = LineBreakMode.NoWrap, TextColor = Color.Black };

                        st_Picker.Children.Add(lb_索引标题);

                        var sl_规格行 = new StackLayout { IsVisible = true, Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.Transparent, Spacing = 5 };

                        st_Picker.Children.Add(sl_规格行);

                        double Swidth = 0;

                        if (Device.RuntimePlatform.ToString() == Device.Android)
                        {
                            Swidth = Helpers.MConfig.screenWidth / 24;
                        }
                        else if (Device.RuntimePlatform.ToString() == Device.iOS)
                        {
                            Swidth = Helpers.MConfig.screenWidth / 17;
                        }

                        ///24
                        var 存放宽度 = Helpers.MConfig.screenWidth;
                        var 剩余存放宽度 = 存放宽度;



                        foreach (var itemDetail in item.City)
                        {
                            var sl_规格 = new Frame { HasShadow = false, CornerRadius = 3, BindingContext = itemDetail, BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, Padding = new Thickness(1, 1, 1, 1) };

                            var sl_规格_内框 = new Frame { HasShadow = false, CornerRadius = 3, BindingContext = itemDetail, BackgroundColor = Color.White, Padding = new Thickness(2, 2, 2, 2) };

                            sl_规格.SetBinding(BackgroundColorProperty, "paddingColor");

                            string 规格文本 = itemDetail.CityName;


                            if (规格文本.Length == 1)
                            {
                                if (Device.RuntimePlatform.ToString() == Device.Android)
                                {
                                    规格文本 = "   " + 规格文本 + "   ";
                                }
                                else if (Device.RuntimePlatform.ToString() == Device.iOS)
                                {
                                    规格文本 = " " + 规格文本 + " ";
                                }
                            }
                            else if (规格文本.Length == 2)
                            {
                                规格文本 = 规格文本.Insert(1, "  ");
                            }

                            if (规格文本.Length > 20)
                                规格文本 = 规格文本.Substring(0, 18) + "..";

                            var lb_规格文本 = new Label { Text = " " + 规格文本 + " ", FontSize = 13, HorizontalTextAlignment = TextAlignment.Center, BackgroundColor = Color.White, LineBreakMode = LineBreakMode.NoWrap, TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont, HorizontalOptions = LayoutOptions.Start, VerticalTextAlignment = TextAlignment.Center, BindingContext = itemDetail, HeightRequest = Helpers.MConfig.screenWidth / 13 };
                            TapGestureRecognizer choose_tap = new TapGestureRecognizer();

                            lb_规格文本.SetBinding(BackgroundColorProperty, "backColor");
                            lb_规格文本.SetBinding(Label.TextColorProperty, "FontColor");

                            sl_规格_内框.SetBinding(BackgroundColorProperty, "backColor");
 

                            choose_tap.CommandParameter = itemDetail;  //加入数据

                            if (editFlag == "修改")
                            {
                                if (chooseFlag == "省")
                                {
                                    if (选择省份 == itemDetail.CityName)
                                    {
                                        itemDetail.Selected = true;
                                        // chooseFlag = "市";
                                        选择省份ID = itemDetail.CityID;
                                    }
                                }
                                else if (chooseFlag == "市")
                                {
                                    if (选择市 == itemDetail.CityName)
                                    {
                                        itemDetail.Selected = true;
                                        //  chooseFlag = "区";
                                        选择市ID = itemDetail.CityID;

                                    }
                                }
                                else if (chooseFlag == "区")
                                {
                                    if (选择区县 == itemDetail.CityName)
                                    {
                                        itemDetail.Selected = true;
                                    }
                                }
                            }

                            choose_tap.Tapped += tapges_chooseItem;

                            sl_规格_内框.GestureRecognizers.Add(choose_tap);//加入点击手势

                            sl_规格_内框.Content = lb_规格文本;

                            sl_规格.Content = sl_规格_内框;

                            #region 计算控件占用宽度
                            double 控件占用宽度 = 0;

                            if (lb_规格文本.Text.Length > 0)
                            {
                                int 文本长度 = lb_规格文本.Text.Replace(" ", "").Length;
                                int 空格长度 = lb_规格文本.Text.Length - 文本长度;
                                控件占用宽度 = 文本长度 * Swidth + 空格长度 * (Swidth / 4) + 10;
                            }
                            else
                            {
                                控件占用宽度 = 20;
                            }

                            #endregion

                            这里:
                            if (控件占用宽度 <= 剩余存放宽度 || 剩余存放宽度 == 存放宽度)
                            {
                                sl_规格行.Children.Add(sl_规格);

                                剩余存放宽度 -= 控件占用宽度;
                            }
                            else
                            {
                                //另起一行
                                var sl_规格新行 = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.Transparent, Spacing = 5 };

                                st_Picker.Children.Add(sl_规格新行);

                                sl_规格行 = sl_规格新行;
                                剩余存放宽度 = 存放宽度;
                                goto 这里;
                            }
                        }
                    }


                }

                am_绘制完毕.OnCompletion();

                if (editFlag == "修改")
                {
                    if (load == 2)
                        return;
                    load++;

                    if (chooseFlag == "省")
                    {
                        tapped_CityDetails(null, null);
                    }
                    else if (chooseFlag == "市")
                    {
                        tapped_DistrictDetails(null, null);
                    }

                }
            });

        }

         /// <summary>
         /// 请选择 （省）
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        void tapped_ProvinceDetails(object sender, EventArgs e)
        {
            #region UI呈现

            if (chooseFlag == "省")
                return;

            chooseFlag = "省";

            am_绘制完毕.Completion += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    box_ProvinceDetails.BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                    box_CityDetails.BackgroundColor = Color.Transparent;

                    box_DistrictDetails.BackgroundColor = Color.Transparent;
                });
            };
            #endregion

            getRegionData("");
        }


        /// <summary>
        /// 请选择  （市）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_CityDetails(object sender, EventArgs e)
        {
            #region UI呈现

            if (chooseFlag == "市")
                return;

            chooseFlag = "市";

            am_绘制完毕.Completion += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    box_ProvinceDetails.BackgroundColor = Color.Transparent;
                    box_CityDetails.BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                    box_DistrictDetails.BackgroundColor = Color.Transparent;
                });
            };

            #endregion
            getRegionData(选择省份ID);
        }


       /// <summary>
       /// 请选择 （区县）
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        void tapped_DistrictDetails(object sender, EventArgs e)
        {
            #region UI呈现

            if (chooseFlag == "区")
                return;

            chooseFlag = "区";

            am_绘制完毕.Completion += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    box_ProvinceDetails.BackgroundColor = Color.Transparent;
                    box_CityDetails.BackgroundColor = Color.Transparent;
                    box_DistrictDetails.BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                });
            };

            #endregion

            getRegionData(选择市ID);
        }

        /// <summary>
        /// 弹出城市选择器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_chooseAreas(object sender, TappedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                block.IsVisible = true;
                st_province.IsVisible = true;
            });
        }

        /// <summary>
        /// 遮罩窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_block(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                block.IsVisible = false;
                st_province.IsVisible = false;
            });
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_save(object sender, EventArgs e)
        {

            if (按钮防呆)
                return;

            按钮防呆 = true;

            string 收件人 = ety_Name.Text.Trim();
            string 联系电话 = ety_tel.Text.Trim();
            string 详细地址 = ety_detailAddress.Text.Trim();
            bool 默认地址 = sw_defultAddress.IsToggled;

            string city = "";

            if (收件人 == "" || 联系电话 == "" || 详细地址 == "" || 选择区县ID == "")
            {
                hud.Show_Toast("信息不完整");

                按钮防呆 = false;
                return;
            }


            //if(!Tools.CommonClass.是否手机(联系电话))
            //{
            //    hud.Show_Toast("请输入正确的手机号码");

            //    按钮防呆 = false;
            //    return;
            //}

            string para = "";



            if (editFlag == "新增")
            {
                收货地址GUID = Guid.NewGuid().ToString();

                para = "ReceivingAddressGUID=" + 收货地址GUID + "&UserGUID=" + Data.UserInfoCache.UserGUID + "&Addressee=" + 收件人 + "&DetailedAddress=" + 详细地址 + "&Tel=" + 联系电话 + "&City=" + 选择区县ID + "&Default=" + 默认地址;
            }
            else if (editFlag == "修改")
            {
                para = "ReceivingAddressGUID=" + 收货地址GUID + "&UserGUID=" + Data.UserInfoCache.UserGUID + "&Addressee=" + 收件人 + "&DetailedAddress=" + 详细地址 + "&Tel=" + 联系电话 + "&City=" + 选择区县ID + "&Default=" + 默认地址;
            }

            Tools.AsyncMsg am_编辑收货地址 = new Tools.AsyncMsg();

            am_编辑收货地址.Completion += (object obj, string ex) =>
            {

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (默认地址)
                    {
                        am_地址更新.OnCompletion(收货地址GUID, null);
                    }
                    else
                    {
                        am_地址更新.OnCompletion("", null);
                    }
                    按钮防呆 = false;
                    Navigation.PopAsync(true);
                });
            };

            am_编辑收货地址.Cancel += (object obj, string ex) =>
            {
                按钮防呆 = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    //DisplayAlert("提示", "编辑收货地址失败:" + ex.ToString(), "知道了");
                });
                return;
            };


            if (editFlag == "新增")
            {
                Tools.NetClass.GetStringByUrl("ExSql", "App\\InsertReceivingAddress_1_0_0_1", para, am_编辑收货地址);

            }
            else if (editFlag == "修改")
            {
                Tools.NetClass.GetStringByUrl("ExSql", "App\\UpdateReceivingAddress_1_0_0_1", para, am_编辑收货地址);
            }


         

        }

        /// <summary>
        /// 关闭遮罩窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_closePage(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PopAsync(true);
            });
        }


        /// <summary>
        /// 点选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapges_chooseItem(object sender, EventArgs e)
        {
            Data.RegionData row = (Data.RegionData)(((TappedEventArgs)e).Parameter);

            /// 分级根据情况分选
            if (chooseFlag == "省")
            {
                foreach (var temp in provinceList)
                {
                    foreach (var temp_省 in temp.City)
                    {
                        if (temp_省.CityID != row.CityID)
                        {
                            temp_省.Selected = false;
                        }
                        else
                        {
                            temp_省.Selected = true;
                            选择省份 = row.CityName;
                            lb_ProvinceDetails.Text = 选择省份;
                            lb_CityDetails.Text = "请选择";
                            lb_DistrictDetails.Text = "请选择";
                            选择省份ID = row.CityID;

                            if (row.SubordinateCity < 1)   //判别是否存在下辖城市
                            {
                                lb_addressCity.RightText = 选择省份;
                                选择区县ID = row.CityGUID;

                                tapped_block(null, null);
                            }
                            else
                                tapped_CityDetails(null, null);
                        }
                    }

                }
            }
            else if (chooseFlag == "市")
            {
                foreach (var temp in cityList)
                {
                    foreach (var temp_市 in temp.City)
                    {
                        if (temp_市.CityName != row.CityName)
                        {
                            temp_市.Selected = false;
                        }
                        else
                        {
                            temp_市.Selected = true;
                            选择市 = row.CityName;
                            lb_CityDetails.Text = 选择市;
                            lb_DistrictDetails.Text = "请选择";
                            选择市ID = row.CityID;

                            if(row.SubordinateCity <1)   //判别是否存在下辖城市
                            {
                                lb_addressCity.RightText = 选择省份 + " " + 选择市;
                                选择区县ID = row.CityGUID;

                                tapped_block(null, null);
                            }
                            else
                                tapped_DistrictDetails(null, null);

                        }
                    }
                }

            }
            else if (chooseFlag == "区")
            {
                foreach (var temp in AreasList)
                {
                    foreach (var temp_县 in temp.City)
                    {
                        if (temp_县.CityName != row.CityName)
                        {
                            temp_县.Selected = false;
                        }
                        else
                        {
                            temp_县.Selected = true;
                            选择区县 = row.CityName;
                            lb_DistrictDetails.Text = 选择区县;
                            选择区县ID = row.CityGUID;
                            lb_addressCity.RightText = 选择省份 + " " + 选择市 + " " + 选择区县;

                            tapped_block(null, null);
                        }
                    }
                }
            }

        }
    }
}