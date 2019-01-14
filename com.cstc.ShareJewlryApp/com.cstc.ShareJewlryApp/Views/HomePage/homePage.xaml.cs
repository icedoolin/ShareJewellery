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
    public partial class homePage : BasePage
    {
 
        double 商品行高 = 0;
        double 图片宽度 = 0;

        int PageNumber = 0;
        bool firstload = false;
        bool 下拉刷新 = false;
        string TypeGUID = "";   //当前页面要显示的珠宝款式
        double headhegiht = 0;   //表头行高，用于切换到其他专柜返回首页时显示listview.header的内容。

        ObservableCollection<Data.commodityData> dataList { get; set; }
        string logintype = "FirstTime";    //登录类型 1. 首次下载软件FirstTime 2. 成为会员 BuyVip   3 邀请好友   InviteFriend
        public homePage()
        {
            
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            //安卓和苹果的阴影效果有很大差异，苹果的时候不用阴影，需要背景色不一样
            if (Device.RuntimePlatform == Device.Android)
            {
                ls_commodity.BackgroundColor = Color.White;
            }
           


            scrl_Filp.Source =Helpers.MConfig.HomePageBannerUrl + "time=" + Guid.NewGuid().ToString();
            scrl_Filp.HeightRequest = Helpers.MConfig.screenWidth * 0.5333;
            scrl_Filp.IsEnabled = false;
            //Test_Show.BackgroundColor = Color.FromRgba(0, 0, 0, 0.5);
            headhegiht = head.Height;  //获取listview.header的高度
           
            Data.UserInfoCache.OpenCount++;
            if (Data.UserInfoCache.OpenCount == 1)
            {
                logintype = "FirstTime";
                st_LoginBox.IsVisible = true;
            }
        } 

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //新品上市行的高度
            stl_New.HeightRequest = Helpers.MConfig.screenWidth * 0.3066;
 
            商品行高 = 1.421 * ((Helpers.MConfig.screenWidth - 30) / 2);
            //图片宽度 = (Helpers.MConfig.screenWidth - 30) / 2;

            //刷新消息数量，更改小红点
            Helpers.AsyncMsg am_更新用户信息 = new Helpers.AsyncMsg();
            am_更新用户信息.Completion += (object obss, string bbb) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (Data.UserInfoCache.userInfo.NoticeNumber > 0)
                    {
                        Img_NewMessage.IsVisible = true;
                    }
                    else
                    {
                        Img_NewMessage.IsVisible = false;
                    }

                });
            };
            Data.UserinfoMgr.RefreshUserInfoCache("未读公告", am_更新用户信息);
            //加载商品内容
            if (firstload)
                return;
            firstload = true;
            ClearCommodtiyView();
            getCommodityData();

        }

        /// <summary>
        /// 按返回键
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackButtonPressed()
        {
            bool re = false;
            if (Device.RuntimePlatform.ToString() == Device.Android)
            {
                //如果有弹出窗口，先关闭窗口
                if (AdShow.IsVisible)
                {
                    AdShow.IsVisible = false;
                    return true;
                }
                //如果不是第一个专柜栏，跳回首页
                if (TypeGUID!="")
                {
                    TapToolBarButton_Tapped(st_DeptHome, null);
                    return true;
                }
            }
 
            return re;
        }


        /// <summary>
        /// 清理商品列表，并且绑定ItemSource=datalist
        /// </summary>
        private void ClearCommodtiyView()
        {
            //初始化
            PageNumber = 0;
            dataList = new ObservableCollection<Data.commodityData>();
            Device.BeginInvokeOnMainThread(() =>
            {
                ls_commodity.ItemsSource = dataList;
            });
        }
        /// <summary>
        /// 获取商品列表数据
        /// </summary>
        public void getCommodityData()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                st_ls_commodity_footer.IsVisible = true;
            });

            Helpers.AsyncMsg am_获取商品 = new Helpers.AsyncMsg();
            am_获取商品.Completion += (object obj, string ex) =>
            {
                try
                {               
                    List<Data.commodityData> newList = (List<Data.commodityData>)obj;   
                    //将分页数据添加到ItemSource数据中，ObservableCollection可以在内容改变后会通知UI改变
                    foreach (var row in newList)
                    {
                        dataList.Add(row);
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        PageNumber++;
                        if (newList.Count==5)
                            st_ls_commodity_footer.IsVisible = false;
                        下拉刷新 = false;
                    });
                }
                catch (Exception)
                {

                }
            };

            //获取
            Data.CommodityMgr.GetCommodityDataWithTwoColumn(am_获取商品, PageNumber,TypeGUID,"","","","","", 商品行高);
 
        }
 
        /// <summary>
        /// listView列表内容显现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            try
            {
                if (dataList!=null && row == dataList[dataList.Count - 1])
                {                    
                    下拉刷新 = true;
                    getCommodityData();
                }
            }
            catch (Exception ex)
            { }
        }


        /// <summary>
        /// webview点击跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Web_Navigating(object sender, WebNavigatedEventArgs e)
        {
            //if (按钮防呆)
            //    return;

            //按钮防呆 = true;

            //string _webUrl = e.Url.ToString();

            ////if (Device.RuntimePlatform.ToString() == Device.Android)
            ////{
            //string[] s = _webUrl.Split('&');
            //if (_webUrl.Contains("wangye"))
            //{
            //    Views.HomePage.WebPage page = new WebPage();
            //    page.webUrl = s[1].ToString();
            //    page.UI呈现();
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        Navigation.PushAsync(page, true);

            //    });

            //}
            //else if (_webUrl.Contains("shangpin"))
            //{
            //    Views.HomePage.ProductDetails.ProductDetailsPage page = new ProductDetails.ProductDetailsPage();
            //    page.JewelleryGUID = s[1].ToString();
            //    //page.getCommodityDetailsData();
            //    //  page.显示商品详情图片();

            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        Helpers.NavagationPageMgr.Open(Navigation, page);
            //        //Navigation.PushAsync(page, true);
            //    });

            //}
            //else if (_webUrl.Contains("fenlei"))
            //{

            //    Classification.ClassificationPage page = new Classification.ClassificationPage() { 款式 = s[1].ToString() };

            //    Navigation.PushAsync(page, true);

            //}



            ////Device.BeginInvokeOnMainThread(() =>
            ////{
            ////    scrl_Filp.GoBack();
            ////});

            //按钮防呆 = false;
        }


        /// <summary>
        /// 新品上市
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_OpenNewCmdty(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                string headname = "";
                int id = 0;
                if (sender==stl_1)
                {
                    headname = "新品上市";
                    id = 1;
                }
                if (sender == stl_2)
                {
                    headname = "几何爱情";
                    id = 2;
                }
                if (sender == stl_3)
                {
                    headname = "珠光宝气";
                    id = 3;
                }
                if (sender == stl_4)
                {
                    hud.Show_Toast("即将开通");
                    return;
                    //headname = "心愿盒";
                    //id = 1;
                }

                FastDirectPage page = new FastDirectPage();
                page.headName = headname;
                page.sign = id;
                Navigation.PushAsync(page, false);

            }
        }

 

        /// <summary>
        /// 打开搜索页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_openSearchPage(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                Navigation.PushAsync(new SearchPage(), true);
            }
        }
 
 
 
        /// <summary>
        /// 五大优势点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TapAdProduct_Tapped(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                Helpers.AsyncMsg am = new Helpers.AsyncMsg();
                am.Completion += (object obje, string exc) =>
                {
                    string returnJson = obje.ToString();
                    if (returnJson == "[]" || returnJson == "")
                        return;
                    try
                    {
                        Data.MessageBoxElement item = new Data.MessageBoxElement();
                        List<Data.MessageBoxElement> lists = Helpers.HttpHelper.GetItemList<Data.MessageBoxElement>(returnJson);
                        item = lists[0];
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            //AdShow.ImageBackGroundSource = item.TitleImage;
                            AdShow.Title = item.Title;
                            AdShow.ImageBodySource = item.BodyImage;
                            AdShow.Body = item.Content;
                            AdShow.IsVisible = true;
                        });

                    }
                    catch (Exception exce)
                    {
                        return;
                    }
                };

                //根据标题获取文本
                int id = 0;
                if (sender == AdProduct)
                {
                    id = 1;
                }
                else if (sender == AdService)
                {
                    id = 2;
                }
                else if (sender == AdBroadCast)
                {
                    id = 3;
                }
                else if (sender == AdSale)
                {
                    id = 4;
                }
                else if (sender == AdCommercial)
                {
                    id = 5;
                }
                string para = "&Type=会员优势" + "&Sequence=" + id;
                Helpers.HttpHelper.GetStringByUrl(@"APP\GetMemberAdvantagesAndEquity_1_0_0_1", para, am);

            }
        }




        /// <summary>
        /// 点击顶部专柜，显示对应商品列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TapToolBarButton_Tapped(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                Color normalcolor = Color.FromHex("#959595");
                Color selectcolor = Color.FromRgb(140, 206, 204);
                DeptHome.TextColor = normalcolor;
                DeptHome.FontAttributes = FontAttributes.None;
                DeptHomeLine.BackgroundColor = Color.White;
                DeptRing.TextColor = normalcolor;
                DeptRing.FontAttributes = FontAttributes.None;
                DeptRingLine.BackgroundColor = Color.White;
                DeptPendant.TextColor = normalcolor;
                DeptPendant.FontAttributes = FontAttributes.None;
                DeptPendantLine.BackgroundColor = Color.White;
                DeptNeckLace.TextColor = normalcolor;
                DeptNeckLace.FontAttributes = FontAttributes.None;
                DeptNeckLaceLine.BackgroundColor = Color.White;
                DeptBracelet.TextColor = normalcolor;
                DeptBracelet.FontAttributes = FontAttributes.None;
                DeptBraceletLine.BackgroundColor = Color.White;
                DeptEarring.TextColor = normalcolor;
                DeptEarring.FontAttributes = FontAttributes.None;
                DeptEarringLine.BackgroundColor = Color.White;


                bool isSelectChanged = true;
                if (sender == st_DeptHome)
                {
                    if (DeptHome.TextColor == selectcolor) isSelectChanged = false; ;
                    DeptHome.TextColor = selectcolor;
                    DeptHome.FontAttributes = FontAttributes.Bold;
                    DeptHomeLine.BackgroundColor = selectcolor;
                    TypeGUID = "";
                }
                if (sender == st_DeptRing)
                {
                    if (DeptRing.TextColor == selectcolor) isSelectChanged = false; ;
                    DeptRing.TextColor = selectcolor;
                    DeptRing.FontAttributes = FontAttributes.Bold;
                    DeptRingLine.BackgroundColor = selectcolor;
                    TypeGUID = "796059de-7540-4317-a25a-dd0e644cd375";
                }
                if (sender == st_DeptPendant)
                {
                    if (DeptPendant.TextColor == selectcolor) isSelectChanged = false; ;
                    DeptPendant.TextColor = selectcolor;
                    DeptPendant.FontAttributes = FontAttributes.Bold;
                    DeptPendantLine.BackgroundColor = selectcolor;
                    TypeGUID = "c4e4d5a9-3e09-4ce6-ac9b-eeef402d6bc1";
                }
                if (sender == st_DeptNeckLace)
                {
                    if (DeptNeckLace.TextColor == selectcolor) isSelectChanged = false; ;
                    DeptNeckLace.TextColor = selectcolor;
                    DeptNeckLace.FontAttributes = FontAttributes.Bold;
                    DeptNeckLaceLine.BackgroundColor = selectcolor;
                    TypeGUID = "d9681d93-5d1c-411a-a1ef-adce69010d88";
                }
                if (sender == st_DeptBracelet)
                {
                    if (DeptBracelet.TextColor == selectcolor) isSelectChanged = false; ;
                    DeptBracelet.TextColor = selectcolor;
                    DeptBracelet.FontAttributes = FontAttributes.Bold;
                    DeptBraceletLine.BackgroundColor = selectcolor;
                    TypeGUID = "d43105e0-5836-4cc3-84dd-2898c312a2a1";
                }
                if (sender == st_DeptEarring)
                {
                    if (DeptEarring.TextColor == selectcolor) isSelectChanged = false; ;
                    DeptEarring.TextColor = selectcolor;
                    DeptEarring.FontAttributes = FontAttributes.Bold;
                    DeptEarringLine.BackgroundColor = selectcolor;
                    TypeGUID = "d608915a-95f6-46a3-850b-b0ae765a1f2d";
                }

                if (isSelectChanged)
                {
                    //PageNumber = 0;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (TypeGUID == "")  //首页显示
                    {
                            head.HeightRequest = headhegiht;
                            head.IsVisible = true;
                        }
                        else
                        {
                            head.HeightRequest = 1;
                            head.VerticalOptions = LayoutOptions.Center;
                            head.IsVisible = false;
                        }
                    //刷新数据
                    ClearCommodtiyView();
                        getCommodityData();
                    });
                }
            }
        }

 





 

        /// <summary>
        /// 点击搜索跳转到搜索页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TapSearchPage_Tapped(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                Navigation.PushAsync(new HomePage.SearchPage(), false);
            }
        }

        /// <summary>
        /// 点击商品列表中的某一个，弹出详情页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommodityViewCell_SelectedCommodity(object sender, string e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                Data.CommodityMgr.ShowProductDetail(Navigation, e);
            }
        }

        private void St_LoginBox_LoginSuccess(object sender, EventArgs e)
        {

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (logintype == "BuyVip")
                    {
                        Navigation.PushAsync(new Views.MyCenter.BuyVipPage(), false);
                    }
                    if (logintype == "InviteFriend")
                    {
                        Navigation.PushAsync(new Views.MyCenter.InvitePage(), false);
                    }
                });
            }


        private void St_LoginBox_LoginCancel(object sender, EventArgs e)
        {

        }

        private void AdShow_Button1Clicked(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                if (Data.UserInfoCache.UserGUID == "")
                {
                    logintype = "BuyVip";
                    st_LoginBox.IsVisible = true;
                    return;
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushAsync(new Views.MyCenter.BuyVipPage(), true);
                });
            }
        }

        private void AdShow_Button2Clicked(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                if (Data.UserInfoCache.UserGUID == "")
                {
                    logintype = "InviteFriend";
                    st_LoginBox.IsVisible = true;

                    return;
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushAsync(new Views.MyCenter.InvitePage(), true);
                });
            }
        }

        private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            if (!st_ls_commodity_footer.IsVisible)
                getCommodityData();
        }

 

        /// <summary>
        /// 下拉刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ls_commodity_Refreshing(object sender, EventArgs e)
        {
            scrl_Filp.Source = Helpers.MConfig.HomePageBannerUrl + "time=" + Guid.NewGuid().ToString();
            ClearCommodtiyView();
            getCommodityData();
            ls_commodity.EndRefresh();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //log_layout.IsVisible = false;
        }

        /// <summary>
        /// 点击消息logo弹出消息中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TapMessageLogo_Tapped(object sender, EventArgs e)
        {
            if (Helpers.MConfig.isNormalClick)
            {
                //调试日志查看
                //log_layout.IsVisible = true;
                //log.Text = Data.UserInfoCache.Logs;
                //return;
                Navigation.PushAsync(new Views.HomePage.MessageCenter(), false);

            }
        }
    }
}