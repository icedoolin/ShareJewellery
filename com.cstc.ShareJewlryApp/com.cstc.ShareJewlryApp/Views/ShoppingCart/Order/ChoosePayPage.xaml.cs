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
    public partial class ChoosePayPage: BasePage 
    {
       

        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
        Tools.IWeChat WXDependency = DependencyService.Get<Tools.IWeChat>();
        com.cstc.ShareJewlryApp.Tools.IAli aliPay = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.IAli>();
        public string orderGuid { get; set; } = "";
        public decimal totalPrice { get; set; } = 0;
        public string 支付标志 { get; set; } = "";   //报损赔付（卖家报损时）、买家报损赔付
        public string 报损单GUID { get; set; } = "";  //全额赔偿时候需要传递的报损单GUID
        public string 还珠宝单GUID { get; set; } = "";//还珠宝时需要传递的还珠宝标记单GUID

        public ChoosePayPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }

        public void UI呈现()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                lb_Price.Text = totalPrice.ToString();
            });
        }

        /// <summary>
        /// 余额付款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_chooseBalancePay(object sender, EventArgs e)
        {
            OrderSucessPage page = new OrderSucessPage();
            page.price = totalPrice;
            page.flag = "购买";
            page.UI呈现();

            Navigation.PushAsync(page, true);



            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    block.IsVisible = true;
            //    st_balancePay.IsVisible = true;
            //});
        }

        /// <summary>
        /// 关闭余额支付弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        void tapped_closeBalancePay(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                block.IsVisible = false;
                st_balancePay.IsVisible = false; ;
            });

        }

        /// <summary>
        /// 余额支付
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_Pay(object sender, EventArgs e)
        {
            OrderSucessPage page = new OrderSucessPage();
            page.price = totalPrice;
            page.flag = "购买";
            page.UI呈现();

            Navigation.PushAsync(page, true);
        }

        /// <summary>
        /// 微信付款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_WeiChatPay(object sender, EventArgs e)
        {
            hud.Show_StatusMessage("");

            int 金额 = (int)(totalPrice * 100);

            if(支付标志=="报损赔付")  //卖家报损赔付
            {
                Tools.AsyncMsg am_下报损单 = new Tools.AsyncMsg();

                string MarkGUID = Guid.NewGuid().ToString();

                string para = "FaultyGUID=" + 报损单GUID + "&MarkGUID=" + MarkGUID;

                am_下报损单.Completion += (object obj, string ex) =>
                {
                    string URL = "https://test.gxzb168.com:8443/Plugin_Text?ClassName=Plugin_Wx.Pay.UnifiedOrder_APP";
                    ///支付
                    Tools.AsyncMsg am_下单 = new Tools.AsyncMsg();

                    string para1 = "&device_info=" + Device.RuntimePlatform + "&body=报损赔付&detail=123&attach=报损&out_trade_no=" + MarkGUID.Replace("-", "") + "&total_fee=" + 金额;

                    // string para1 = "&device_info=" + Device.RuntimePlatform + "&body=报损赔付&detail=123&attach=报损&out_trade_no=" + Guid.NewGuid().ToString().Replace("-", "") + "&total_fee=" + 金额;

                    am_下单.Completion += (object obje, string exc) =>
                    {
                        string returnJson = obje.ToString();
                        string ErrMsg = "";

                        Newtonsoft.Json.Linq.JArray jar = null;

                        if (returnJson == "[]" || returnJson == "")
                        {
                            Device.BeginInvokeOnMainThread( () =>
                            {
                                hud.Dismiss();
                            });
                          
                            return;
                        }
                           

                        try
                        {
                            jar = Newtonsoft.Json.Linq.JArray.Parse(returnJson);
                        }
                        catch (Exception exce)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                hud.Dismiss();
                                //await DisplayAlert("错误", "解析微信下单数据包错误！" + exce.Message, "知道了");
                                // Navigation.PopAsync(true);
                              
                            });
                            return;
                        }


                        //调起支付
                        string prePayid = jar[0]["prepay_id"].ToString();
                        string preNonce = jar[0]["nonceStr"].ToString();
                        string timestamp = Tools.CommonClass.getTimestamp();
                        string sign = Tools.CommonClass.SecSign(prePayid, preNonce);
                        Tools.CommonClass.支付页面标志 = "卖家报损赔付";

                        Tools.CommonClass.am_卖家报损支付.Completion += (object s, string a) =>
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Navigation.PopAsync();
                            });
                        };

                      

                        WXDependency.WXPay(prePayid, preNonce, sign, timestamp);


                        Device.BeginInvokeOnMainThread(() =>
                        {
                            hud.Dismiss();
                        });
                  
                    };

                    am_下单.Cancel += (object obje, string exc) =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            hud.Dismiss();
                            DisplayAlert("提示", "微信下单失败:" , "知道了");
                        });
                        return;
                    };

                    URL = URL + para1;

                    Tools.NetClass.创建网络Get请求(URL, am_下单);
                };

                am_下报损单.Cancel += (object obj, string ex) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Dismiss();
                        DisplayAlert("提示", "支付失败", "知道了");
                    });
                    return;
                };

                Tools.NetClass.GetStringByUrl("ExSql", "App\\MarkFaultyOrder_1_0_0_1", para, am_下报损单);

            }
            else if(支付标志== "全额赔偿")
            {
                string URL = "https://test.gxzb168.com:8443/Plugin_Text?ClassName=Plugin_Wx.Pay.UnifiedOrder_APP";
                ///支付
                Tools.AsyncMsg am_下单 = new Tools.AsyncMsg();

                string para1 = "&device_info=" + Device.RuntimePlatform + "&body=全额赔偿&detail=123&attach=全额&out_trade_no=" + 报损单GUID.Replace("-", "") + "&total_fee=" + 金额;

               //  string para1 = "&device_info=" + Device.RuntimePlatform + "&body=报损赔付&detail=123&attach=报损&out_trade_no=" + Guid.NewGuid().ToString().Replace("-", "") + "&total_fee=" + 金额;

                am_下单.Completion += (object obje, string exc) =>
                {
                    string returnJson = obje.ToString();
                    string ErrMsg = "";

                    Newtonsoft.Json.Linq.JArray jar = null;

                    if (returnJson == "[]" || returnJson == "")
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            hud.Dismiss();
                        });

                        return;
                    }


                    try
                    {
                        jar = Newtonsoft.Json.Linq.JArray.Parse(returnJson);
                    }
                    catch (Exception exce)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            hud.Dismiss();
                            //await DisplayAlert("错误", "解析微信下单数据包错误！" + exce.Message, "知道了");
                            // Navigation.PopAsync(true);
                         
                        });
                        return;
                    }


                    //调起支付
                    string prePayid = jar[0]["prepay_id"].ToString();
                    string preNonce = jar[0]["nonceStr"].ToString();
                    string timestamp = Tools.CommonClass.getTimestamp();
                    string sign = Tools.CommonClass.SecSign(prePayid, preNonce);
                    Tools.CommonClass.支付页面标志 = "全额赔偿";

                    Tools.CommonClass.am_全额赔偿支付.Completion += (object s, string a) =>
                    {
                        Task.Delay(1000);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Navigation.PopAsync(true);
                            Navigation.PopAsync(false);
                            Navigation.PopAsync(false);
                        });
                    };



                    WXDependency.WXPay(prePayid, preNonce, sign, timestamp);


                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Dismiss();
                    });

                };

                am_下单.Cancel += (object obje, string exc) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Dismiss();
                        DisplayAlert("提示", "微信下单失败"  , "知道了");
                    });
                    return;
                };

                URL = URL + para1;

                Tools.NetClass.创建网络Get请求(URL, am_下单);
            }
            else if(支付标志=="还珠宝")
            {
                string URL = "https://test.gxzb168.com:8443/Plugin_Text?ClassName=Plugin_Wx.Pay.UnifiedOrder_APP";

                Tools.AsyncMsg am_下单 = new Tools.AsyncMsg();

              

                 string para1 = "&device_info=" + Device.RuntimePlatform + "&body=清洗费&detail=123&attach=清洗费&out_trade_no=" + 还珠宝单GUID.Replace("-", "") + "&total_fee=" + 金额;

              //  string para1 = "&device_info=" + Device.RuntimePlatform + "&body=清洗费&detail=123&attach=清洗费&out_trade_no=" + 还珠宝单GUID.Replace("-", "") + "&total_fee=1";

                am_下单.Completion += (object objec, string exce) =>
                {
                    string returnJson = objec.ToString();
                    //string ErrMsg = "";

                    Newtonsoft.Json.Linq.JArray jar = null;

                    if (returnJson == "[]" || returnJson == "")
                    {
                        Device.BeginInvokeOnMainThread( () =>
                        {
                            hud.Dismiss();

                        });
                     
                        return;
                    }
                       

                    try
                    {
                        jar = Newtonsoft.Json.Linq.JArray.Parse(returnJson);
                    }
                    catch (Exception excep)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            hud.Dismiss();
                            //await DisplayAlert("错误", "解析微信下单数据包错误！" + excep.Message, "知道了");
                            // Navigation.PopAsync(true);

                        });
                        return;
                    }

                    //调起支付
                    string prePayid = jar[0]["prepay_id"].ToString();
                    string preNonce = jar[0]["nonceStr"].ToString();
                    string timestamp = Tools.CommonClass.getTimestamp();
                    string sign = Tools.CommonClass.SecSign(prePayid, preNonce);


                    Tools.CommonClass.am_还珠宝支付.Completion += (object s, string a) =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Navigation.PopAsync(true);
                            Navigation.PopAsync(false);
                        });
                    };

                 

                    Tools.CommonClass.支付页面标志 = "还珠宝";

                    WXDependency.WXPay(prePayid, preNonce, sign, timestamp);
                 

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Dismiss();
                    });
                };

                am_下单.Cancel += (object objec, string exce) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Dismiss();
                        DisplayAlert("提示", "微信下单失败" , "知道了");
                    });
                    return;
                };

                URL = URL + para1;

                Tools.NetClass.创建网络Get请求(URL, am_下单);
            }
        }

        /// <summary>
        /// 支付宝付款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_AliPay(object sender, EventArgs e)
        {
            hud.Show_StatusMessage("");

            decimal 金额 = totalPrice;

            if (支付标志 == "报损赔付")  //卖家报损赔付
            {
                Tools.AsyncMsg am_下报损单 = new Tools.AsyncMsg();

                string MarkGUID = Guid.NewGuid().ToString();

                string para = "FaultyGUID=" + 报损单GUID + "&MarkGUID=" + MarkGUID;

                am_下报损单.Completion += (object obj, string ex) =>
                {
                    Tools.AsyncMsg am_支付宝下单 = new Tools.AsyncMsg();

                    string AliPayUrl = "https://test.gxzb168.com:8443/Plugin_Text?ClassName=Plugin_AL.PaySign_APP&total_amount=" + 金额 + "&subject=报损&body=报损赔付&out_trade_no=" + MarkGUID;

                    am_支付宝下单.Completion += (object obje, string exc) =>
                    {
                        string returnJson = obje.ToString();
                        string ErrMsg = "";

                        Newtonsoft.Json.Linq.JArray jar = null;

                        if (returnJson == "[]" || returnJson == "")
                        {

                            Device.BeginInvokeOnMainThread( () =>
                            {
                                hud.Dismiss();
                            });
                            
                            return;
                        }

                        try
                        {
                            jar = Newtonsoft.Json.Linq.JArray.Parse(returnJson);
                        }
                        catch (Exception exce)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                hud.Dismiss();
                                //await DisplayAlert("错误", "解析用户数据包错误！" + exce.Message, "知道了");
                                // Navigation.PopAsync(true);
                            });
                            return;
                        }

                        Tools.CommonClass.支付页面标志 = "卖家报损赔付";

                        Tools.CommonClass.am_卖家报损支付.Completion += (object s, string a) =>
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Navigation.PopAsync();
                            });
                        };

                        string key = jar[0]["sign"].ToString();

                        aliPay.AliPay(key);


                        Device.BeginInvokeOnMainThread(() =>
                        {
                            hud.Dismiss();
                        });
                    };

                    am_支付宝下单.Cancel += (object obje, string exc) =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            hud.Dismiss();
                            DisplayAlert("提示", "支付宝下单失败"  , "知道了");
                        });
                        return;
                    };

                    Tools.NetClass.创建网络Get请求(AliPayUrl, am_支付宝下单);

                };

                am_下报损单.Cancel += (object obj, string ex) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Dismiss();
                        DisplayAlert("提示", "支付失败"  , "知道了");
                    });
                    return;
                };

                Tools.NetClass.GetStringByUrl("ExSql", "App\\MarkFaultyOrder_1_0_0_1", para, am_下报损单);
            }
            else if (支付标志 == "全额赔偿")
            {
                Tools.AsyncMsg am_支付宝下单 = new Tools.AsyncMsg();

                string AliPayUrl = "https://test.gxzb168.com:8443/Plugin_Text?ClassName=Plugin_AL.PaySign_APP&total_amount=" + 金额 + "&subject=全额&body=全额赔偿&out_trade_no=" + 报损单GUID;

                am_支付宝下单.Completion += (object obje, string exc) =>
                {
                    string returnJson = obje.ToString();
                    string ErrMsg = "";

                    Newtonsoft.Json.Linq.JArray jar = null;

                    if (returnJson == "[]" || returnJson == "")
                    {
                        Device.BeginInvokeOnMainThread( () =>
                        {
                            hud.Dismiss();
                        });
                       
                        return;
                    }

                    try
                    {
                        jar = Newtonsoft.Json.Linq.JArray.Parse(returnJson);
                    }
                    catch (Exception exce)
                    {
                        Device.BeginInvokeOnMainThread( () =>
                        {
                            hud.Dismiss();
                            //DisplayAlert("错误", "解析用户数据包错误！" + exce.Message, "知道了");
                            // Navigation.PopAsync(true);
                        });
                        return;
                    }

                    Tools.CommonClass.支付页面标志 = "全额赔偿";

                    Tools.CommonClass.am_全额赔偿支付.Completion += (object s, string a) =>
                    {
                        Task.Delay(1000);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Navigation.PopAsync(true);
                            Navigation.PopAsync(false);
                            Navigation.PopAsync(false);
                        });
                    };

                    string key = jar[0]["sign"].ToString();

                    aliPay.AliPay(key);


                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Dismiss();
                    });
                };

                am_支付宝下单.Cancel += (object obje, string exc) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Dismiss();
                        DisplayAlert("提示", "支付宝下单失败" , "知道了");
                    });
                    return;
                };

                Tools.NetClass.创建网络Get请求(AliPayUrl, am_支付宝下单);
            }
            else if (支付标志 == "还珠宝")
            {
                Tools.AsyncMsg am_支付宝下单 = new Tools.AsyncMsg();

                string AliPayUrl = "https://test.gxzb168.com:8443/Plugin_Text?ClassName=Plugin_AL.PaySign_APP&total_amount=" + 金额 + "&subject=清洗费&body=清洗费&out_trade_no=" + 还珠宝单GUID;

                am_支付宝下单.Completion += (object obje, string exc) =>
                {
                    string returnJson = obje.ToString();
                    string ErrMsg = "";

                    Newtonsoft.Json.Linq.JArray jar = null;

                    if (returnJson == "[]" || returnJson == "")
                    {
                        hud.Dismiss();
                        Device.BeginInvokeOnMainThread( () =>
                        {
                            hud.Dismiss();
                        });
                        return;
                    }

                    try
                    {
                        jar = Newtonsoft.Json.Linq.JArray.Parse(returnJson);
                    }
                    catch (Exception exce)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            hud.Dismiss();
                            //await DisplayAlert("错误", "解析用户数据包错误！" + exce.Message, "知道了");
                            // Navigation.PopAsync(true);
                        });
                        return;
                    }

                    Tools.CommonClass.支付页面标志 = "还珠宝";

                    Tools.CommonClass.am_全额赔偿支付.Completion += (object s, string a) =>
                    {
                        Task.Delay(1000);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Navigation.PopAsync(true);
                            Navigation.PopAsync(false); 
                        });
                    };

                    string key = jar[0]["sign"].ToString();

                    aliPay.AliPay(key);


                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Dismiss();
                    });
                };

                am_支付宝下单.Cancel += (object obje, string exc) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Dismiss();
                        DisplayAlert("提示", "支付宝下单失败" , "知道了");
                    });
                    return;
                };

                Tools.NetClass.创建网络Get请求(AliPayUrl, am_支付宝下单);
            }

        }
    }
}