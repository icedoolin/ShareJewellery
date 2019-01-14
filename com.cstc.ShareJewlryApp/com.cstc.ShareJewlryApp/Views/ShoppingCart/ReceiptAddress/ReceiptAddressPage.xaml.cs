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
    public partial class ReceiptAddressPage: BasePage 
    {
        bool 按钮防呆 = false;
        public string 入口标志 { get; set; } = "";
        public string 当前选中地址GUID { get; set; } = "";
        public Tools.AsyncMsg am_选择地址 = new Tools.AsyncMsg();

        Data.ReceiptAddressData 选中项 = new Data.ReceiptAddressData();

        ObservableCollection<Data.ReceiptAddressData> dataList = new ObservableCollection<Data.ReceiptAddressData>();


        public ReceiptAddressPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            获取收货地址();

        }

        public void 获取收货地址()
        {
            Tools.AsyncMsg am_获取收货地址 = new Tools.AsyncMsg();

            am_获取收货地址.Completion += (object obj, string ex) =>
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
                        //await DisplayAlert("错误", "解析地址数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                #endregion

                try
                {
                    dataList = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Data.ReceiptAddressData>>(jar.ToString());
                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "转换地址数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                }

                foreach (var temp in dataList)
                {

                    if(入口标志=="订单")    ////订单入口进来的显示‘当前’标签 否则显示'默认'标签
                    {
                        temp.isCurrenShow = true;
                        temp.isDefaultShow = false;
                    }
                    else
                    {
                        temp.isCurrenShow = false;
                        temp.isDefaultShow = true;
                    }

                    if (当前选中地址GUID == temp.ReceivingAddressGUID)
                    {
                        temp.Current = true;
                    }
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    ls_address.ItemsSource = dataList;
                });
            };

            //am_获取收货地址.Cancel += (object obj, string ex) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        DisplayAlert("提示", "获取收货地址失败:" + ex.ToString(), "知道了");
            //    });
            //    return;
            //};

            Tools.NetClass.GetStringByUrl("QueryData", "App\\GetReceivingAddress_1_0_0_1", "UserGUID=" + Data.UserInfoCache.UserGUID, am_获取收货地址);
        }

        /// <summary>
        /// 新增地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_addAdress(object sender, EventArgs e)
        {
            if (按钮防呆 == true)
                return;
 
            Views.ShoppingCart.ReceiptAddress.EdtiReceiptAddressPage page = new ShoppingCart.ReceiptAddress.EdtiReceiptAddressPage();
 
            page.am_地址更新.Completion += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    string 回传默认地址GUID = (string)obj;
                    if (回传默认地址GUID != "")
                    {
                        当前选中地址GUID = (string)obj;
                    }
                    获取收货地址();
                });

            };
            Device.BeginInvokeOnMainThread(() =>
            {
                page.getRegionData("");
                Navigation.PushAsync(page, true);

                按钮防呆 = false;
            });

        }

        /// <summary>
        /// 关闭页面
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
        /// 选择收货地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_selectAdreess(object sender, EventArgs e)
        {
            if (入口标志 != "订单")
                return;
 
            var row = (Data.ReceiptAddressData)(((Grid)sender).BindingContext);

            am_选择地址.OnCompletion(row, "");

            Navigation.PopAsync(true);

        }

        /// <summary>
        /// 编辑地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_edtAdress(object sender, EventArgs e)
        {
            //  var row = (Data.ReceiptAddressData)e.Parameter;

            EdtiReceiptAddressPage page = new EdtiReceiptAddressPage();
            page.editFlag = "修改";
            page.选择省份 = 选中项.Province;
            page.选择市 = 选中项.Municipality;
            page.选择区县 = 选中项.County;
            page.收件人 = 选中项.Addressee;
            page.联系电话 = 选中项.Tel;
            page.收件城市 = 选中项.CityName;
            page.详细地址 = 选中项.DetailedAddress;
            page.默认地址 = 选中项.Default;
            page.选择区县ID = 选中项.City;
            page.收货地址GUID = 选中项.ReceivingAddressGUID;

            page.am_地址更新.Completion += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    string 回传默认地址GUID = (string)obj;
                    if (回传默认地址GUID != "")
                    {
                        当前选中地址GUID = (string)obj;
                    }
                    当前选中地址GUID = (string)obj;
                    获取收货地址();
                });

            };


            page.修改UI呈现();
            Navigation.PushAsync(page, true);
 
            tapped_closeBlock(null, null);
        }


        /// <summary>
        /// 长按
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_longClicked(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var row = (Data.ReceiptAddressData)btn.CommandParameter;
            选中项 = row;

            block.IsVisible = true;
            frm_edtBox.IsVisible = true;

        }

        /// <summary>
        /// 关闭遮罩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_closeBlock(object sender, EventArgs e)
        {
            block.IsVisible = false;
            frm_edtBox.IsVisible = false;
            frm_deleteCheckBox.IsVisible = false;
        }

        /// <summary>
        /// 弹出确认删除弹框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_ShowdeleteAddressCheckBox(object sender, EventArgs e)
        {
            block.IsVisible = true;
            frm_deleteCheckBox.IsVisible = true;

        }

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_deleteAddress(object sender, EventArgs e)
        {
            Tools.AsyncMsg am_删除收货地址 = new Tools.AsyncMsg();

            string para = "ReceivingAddressGUID=" + 选中项.ReceivingAddressGUID;

            am_删除收货地址.Completion += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    dataList.Remove(选中项);
                    tapped_closeBlock(null, null);
                });

            };

            //am_删除收货地址.Cancel += (object obj, string ex) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        DisplayAlert("提示", "获取收货地址失败:" + ex.ToString(), "知道了");
            //    });
            //    return;
            //};

            Tools.NetClass.GetStringByUrl("ExSql", "APP\\DeleteReceivingAddress_1_0_0_1", para, am_删除收货地址);

        }

 
        /// <summary>
        /// 编辑地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TapEdit_Tapped(object sender, EventArgs e)
        {
            选中项 =(Data.ReceiptAddressData)( ((Label)sender).BindingContext);
            EdtiReceiptAddressPage page = new EdtiReceiptAddressPage();
            page.editFlag = "修改";
            page.选择省份 = 选中项.Province;
            page.选择市 = 选中项.Municipality;
            page.选择区县 = 选中项.County;
            page.收件人 = 选中项.Addressee;
            page.联系电话 = 选中项.Tel;
            page.收件城市 = 选中项.CityName;
            page.详细地址 = 选中项.DetailedAddress;
            page.默认地址 = 选中项.Default;
            page.选择区县ID = 选中项.City;
            page.收货地址GUID = 选中项.ReceivingAddressGUID;

            page.am_地址更新.Completion += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    string 回传默认地址GUID = (string)obj;
                    if (回传默认地址GUID != "")
                    {
                        当前选中地址GUID = (string)obj;
                    }
                    当前选中地址GUID = (string)obj;
                    获取收货地址();
                });

            };


            page.修改UI呈现();
            Navigation.PushAsync(page, true);

            tapped_closeBlock(null, null);
        }
        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TapDelete_Tapped(object sender, EventArgs e)
        {
            选中项 = (Data.ReceiptAddressData)(((Label)sender).BindingContext);
            block.IsVisible = true;
            frm_deleteCheckBox.IsVisible = true;
 
        }
    }
}