using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.HomePage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage: BasePage 
    {
        ObservableCollection<Data.commodityData> Datalist = new ObservableCollection<Data.commodityData>();
        string 搜索关键字 = "";
        string 排序 = "Synthesize";
        int 分页index = 0;
        bool 下拉刷新 = false;
        bool 按钮防呆 = false;
        public SearchPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            UI呈现();
            获取热门关键字();
            pagehead.SearchPlaceholder = Data.UserInfoCache.userInfo.platformHotKeyword==""? "黄金" : Data.UserInfoCache.userInfo.platformHotKeyword  ;

        }

        

        private void tapped_closePage(object sender, EventArgs e)
        {
            Navigation.PopAsync(true);

        }

        void UI呈现()
        {
           if(Data.UserInfoCache.SearchHistory1 !="")
            {
                st_history.IsVisible = true;
                lb_SearchHistory1.Text = Data.UserInfoCache.SearchHistory1;
                lb_SearchHistory1.IsVisible = true; 
            }

            if (Data.UserInfoCache.SearchHistory2 != "")
            {
                lb_SearchHistory2.Text = Data.UserInfoCache.SearchHistory2;
                lb_SearchHistory2.IsVisible = true;
            }

            if (Data.UserInfoCache.SearchHistory3 != "")
            {
                lb_SearchHistory3.Text = Data.UserInfoCache.SearchHistory3;
                lb_SearchHistory3.IsVisible = true;
            }

        }

        void  获取热门关键字()
        {
            Tools.AsyncMsg am_获取热门关键字 = new Tools.AsyncMsg();

            am_获取热门关键字.Completion += (object obj, string ex) =>
            {

                string returnJson = obj.ToString();
                string ErrMsg = "";
                if (returnJson == "[]" || returnJson == "")
                {
                    Device.BeginInvokeOnMainThread( () =>
                    {
                        st_hotKey.IsVisible = false;
                    });
                    return;
                }
                   

                try
                {
                    returnJson = Tools.CommonClass.GetJsonByTag(returnJson, ref ErrMsg);
                }
                catch (Exception exc)
                {
                    //Device.BeginInvokeOnMainThread( () =>
                    //{
                    //     DisplayAlert("错误", "解析热门关键字数据错误！" + exc.Message, "知道了");
                    //    // Navigation.PopAsync(true);
                         
                    //});
                    return;
                }

                Newtonsoft.Json.Linq.JArray jar = null;
                try
                {
                    jar = Newtonsoft.Json.Linq.JArray.Parse(returnJson);
                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread( () =>
                    {
                         //DisplayAlert("错误", "解析热门关键字数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                        下拉刷新 = false;
                    });
                    return;
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    st_hotKey.IsVisible = true;

                    for (int i = 0; i < jar.Count; i++)
                    {
                        if (i == 0)
                        {
                            lb_HotKeyword1.Text = jar[i]["HotKeyword"].ToString();
                            lb_HotKeyword1.IsVisible = true;
                        }

                        if (i == 1)
                        {
                            lb_HotKeyword2.Text = jar[i]["HotKeyword"].ToString();
                            lb_HotKeyword2.IsVisible = true;
                        }

                        if (i == 2)
                        {
                            lb_HotKeyword3.Text = jar[i]["HotKeyword"].ToString();
                            lb_HotKeyword3.IsVisible = true;
                        }
                    }

                });
               
                

                
            };

            //am_获取热门关键字.Cancel += (object obj, string ex) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        DisplayAlert("提示", "获取热门关键字失败:" + ex.ToString(), "知道了"); 
            //    });
            //    return;
            //};

            Tools.NetClass.GetStringByUrl("QueryData", "APP\\GetHotKeyword_1_0_0_1", "", am_获取热门关键字);
        }

        #region 搜索框
        Color _FrameBackgroundColor = Color.FromHex("#f0f0f0");
 

        void ety_content_focused(object sender, EventArgs e)
        {
            //borderFrame.BackgroundColor = Color.FromHex("fcf5ec");
            st_fastSearchBar.IsVisible = true;
            //block.IsVisible = true;
            
        }

        void ety_content_unfocused(object sender, EventArgs e)
        {
            //borderFrame.BackgroundColor = _FrameBackgroundColor;
            st_fastSearchBar.IsVisible = false;
            //block.IsVisible = false;
        }

        #endregion

        private void 搜索(object sender, EventArgs e)
        {
           
           string searchString = pagehead.SearchText.Trim();
            if (searchString=="")
                searchString= pagehead.SearchPlaceholder.Trim();
            if (searchString == "")
                return;

            搜索关键字 = searchString;
            分页index = 0;
            Datalist.Clear();
            st_sortBar.IsVisible = true;

            tapped_All(null, null);

            Data.UserInfoCache.SearchHistory3 = Data.UserInfoCache.SearchHistory2;
            Data.UserInfoCache.SearchHistory2 = Data.UserInfoCache.SearchHistory1;
            Data.UserInfoCache.SearchHistory1 = 搜索关键字;
            UI呈现();


        }


        /// <summary>
        /// 获取珠宝信息
        /// </summary>
        public void getJewelleryData()
        {
            Tools.AsyncMsg am_获取全部珠宝 = new Tools.AsyncMsg();

            string para = "PageNumber=" + 分页index + "&JewelleryName=" + 搜索关键字 + "&Column1=" + 排序;

            am_获取全部珠宝.Completion += (object obj, string ex) =>
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
                        //await DisplayAlert("错误", "解析珠宝数据错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                        下拉刷新 = false;
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
                        //await DisplayAlert("错误", "解析珠宝数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                        下拉刷新 = false;
                    });
                    return;
                }

                ObservableCollection<Data.commodityData> templist = null;

                try
                {
                    templist = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Data.commodityData>>(jar.ToString());

                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "转化珠宝数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                        下拉刷新 = false;
                    });
                    return;
                }

                foreach (var row in templist)
                {
                    Datalist.Add(row);
                }

                if (分页index < 1)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ls_jewellery.ItemsSource = Datalist;
                    });
                }

                分页index++;
                下拉刷新 = false;
            };

            am_获取全部珠宝.Cancel += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //DisplayAlert("提示", "搜索珠宝失败:" + ex.ToString(), "知道了");
                    下拉刷新 = false;
                });
                return;
            };

            Tools.NetClass.GetStringByUrl("QueryData", "APP\\GetSearchJewellery_1_0_0_1", para, am_获取全部珠宝);
        }

        ///// <summary>
        ///// 关闭遮罩
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void tapped_closeBlock(object sender, EventArgs e)
        //{
        //    st_fastSearchBar.IsVisible = false;
        //    block.IsVisible = false;
        //}

        /// <summary>
        /// 按照全部排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_All(object sender, EventArgs e)
        {
            if (lb_all.TextColor == com.cstc.ShareJewlryApp.Style.Color.Color.grayFont)
            {
                lb_all.TextColor = Color.Black;
                lb_identity.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                lb_time.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
               
            }
            分页index = 0;
            Datalist.Clear();
            排序 = "Synthesize";
            getJewelleryData();

        }

        
        /// <summary>
        /// 按照好评排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_time(object sender, EventArgs e)
        {
            if (lb_time.TextColor == com.cstc.ShareJewlryApp.Style.Color.Color.grayFont)
            {
                lb_all.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                lb_identity.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                lb_time.TextColor = Color.Black;
                分页index = 0;
                Datalist.Clear();
                排序 = "Praise";
                getJewelleryData();
            }

          
        }

        /// <summary>
        /// 按照销量败絮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_identity(object sender, EventArgs e)
        {
            if (lb_identity.TextColor == com.cstc.ShareJewlryApp.Style.Color.Color.grayFont)
            {
                lb_all.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                lb_identity.TextColor = Color.Black;
                lb_time.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                分页index = 0;
                Datalist.Clear();
                排序 = "number";
                getJewelleryData();
            }

       
        }

        /// <summary>
        /// 打开商品详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_openCmdtyDtlPage(object sender, TappedEventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            var row = (Data.commodityData)e.Parameter;

            Views.HomePage.ProductDetails.ProductDetailsPage page = new ProductDetails.ProductDetailsPage();
            page.JewelleryGUID = row.JewelleryGUID;
            //page.getCommodityDetailsData();
          //  page.显示商品详情图片();

            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(page, true);
            });
            按钮防呆 = false;
        }

        private void ls_commodity_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var row = new Data.commodityData();

            try
            {
                row = (Data.commodityData)e.Item;
            }
            catch (Exception ex)
            {

            }

            if (下拉刷新)
                return;

            if (row == Datalist[Datalist.Count - 1])
            {

                下拉刷新 = true;

                getJewelleryData();

            }
        }

        /// <summary>
        /// 选择热搜关键词
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_selectKey(object sender, EventArgs e)
        {
            var lb = (Label)sender;

            pagehead.SearchText = lb.Text;
            搜索(null,null);
 
        }
    }
}