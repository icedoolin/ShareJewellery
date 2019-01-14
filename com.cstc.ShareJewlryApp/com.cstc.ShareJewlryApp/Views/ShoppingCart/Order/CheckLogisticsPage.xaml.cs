using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.ShoppingCart.Order
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckLogisticsPage: BasePage 
    {
        public string 运单号 { get; set; } = "";
        public string 物流公司 { get; set; } = "";
        public CheckLogisticsPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }

        /// <summary>
        /// 获取物流信息
        /// </summary>
        public void loadData()
        {
            /*string url = "https://test.gxzb168.com:8443/ProxyService/PostQuery?nowTime=1535090285000&type=yunda&postid=3940238518031&tdsourcetag=s_pcqq_aiomsg";*/
            string url = "https://test.gxzb168.com:8443/ProxyService/PostQuery?nowTime=" + Guid.NewGuid().ToString() + "&type=" + 物流公司 + "&postid=" + 运单号;
            Tools.AsyncMsg am_查看物流 = new Tools.AsyncMsg();
            am_查看物流.Completion += (object jsonData, string jsonEve) =>
            {
                try
                {
                    //string jsonstring = NetClass.GetJsonByTag(jsonData == null ? "" : jsonData.ToString(), ref jsonEve);
                    string jsonstring = jsonData == null ? "" : jsonData.ToString();
                    if (jsonstring == "")
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("提示", "获取物流信息为空", "知道了");
                        });
                        return;
                    }
                    List<Data.LogisticsClass> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Data.LogisticsClass>>(jsonstring);
                    if (list != null && list.Count > 0)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                #region 上线条颜色第一设置透明
                                if (i == list.Count - 1)
                                {
                                    list[i].eColor = Color.Transparent;
                                }
                                else
                                {
                                    list[i].eColor = Color.FromHex("#787878");
                                }
                                #endregion
                                #region 下线条颜色最后一条设置为透明
                                if (i == 0)
                                {
                                    list[i].tColor = Color.Transparent;
                                }
                                else
                                {
                                    list[i].tColor = Color.FromHex("#787878");
                                }
                                #endregion
                            }
                            listView.ItemsSource = list;
                        });
                    }
                }
                catch (Exception e)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayAlert("提示", "物流信息查询失败", "知道了");
                    });
                    return;
                }
            };

            am_查看物流.Cancel += (object jsonError, string jsonMessage) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("提示", "物流信息查询失败", "知道了");
                });
                return;
            };

            //  NetClass.GetStringByUrl(url, am);

            Tools.NetClass.创建网络Get请求_logitics(url, am_查看物流);
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListView lv = sender as ListView;
            lv.SelectedItem = null;
        }
    }
}