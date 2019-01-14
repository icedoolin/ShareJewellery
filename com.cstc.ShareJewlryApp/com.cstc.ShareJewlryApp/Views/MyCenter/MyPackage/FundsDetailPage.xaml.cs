using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.MyCenter.MyPackage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FundsDetailPage: BasePage 
	{
        int 分页index = 0;

        bool 下拉刷新 = false;

        ObservableCollection<Data.FundsDetailData> datalist = new ObservableCollection<Data.FundsDetailData>();

        public FundsDetailPage ()
		{
			InitializeComponent ();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            获取资金明细();
        }

        void 获取资金明细()
        {
            Tools.AsyncMsg am_获取资金明细 = new Tools.AsyncMsg();

            string para = "PageNumber=" + 分页index + "&UserGUID=" + Data.UserInfoCache.UserGUID ;

            am_获取资金明细.Completion += (object obj, string ex) =>
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
                        //await DisplayAlert("错误", "解析资金明细数据错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                if (returnJson == "" && 分页index == 0)
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
                        //await DisplayAlert("错误", "解析资金明细数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                ObservableCollection<Data.FundsDetailData> tempList = null;
                try
                {
                    tempList = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Data.FundsDetailData>>(jar.ToString());

                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "转化资金明细数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                foreach (var row in tempList)
                {
                    datalist.Add(row);
                }

                if (分页index < 1)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ls_funds.ItemsSource = datalist;
                    });
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    分页index++;
                    
                    下拉刷新 = false;
                });
            };

            //am_获取资金明细.Cancel += (object obj, string ex) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        DisplayAlert("提示", "获取资金明细失败:" + ex.ToString(), "知道了");
            //    });
            //    return;
            //};

            Tools.NetClass.GetStringByUrl("QueryData", "App\\GetUserFundsDetailed_1_0_0_1", para, am_获取资金明细);

        }

        private void ls_commodity_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var row = new Data.FundsDetailData();

            try
            {
                row = (Data.FundsDetailData)e.Item;
            }
            catch (Exception ex)
            {

            }

            if (下拉刷新)
                return;

            if (row == datalist[datalist.Count - 1])
            {

                下拉刷新 = true;

                获取资金明细();
            }
        }
    }
}