using Plugin.Media;
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
    public partial class CheckIdentify: BasePage 
    {
        string 弹窗标志 = "";
 
        string 正面审核状态 = "";
        string 反面审核状态 = "";
        string 手持审核状态 = "";
        bool 正面照片存在 = false;  //是否已上传照片
        bool 反面照片存在 = false;
        bool 手持照片存在 = false;
        bool 按钮防呆 = false;
        List<Data.IDCardData> 待审核数据 = new List<Data.IDCardData>();
        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();

        public CheckIdentify()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            img_checkId_forward.HeightRequest = ((Helpers.MConfig.screenWidth - 30)/2) * 0.6666;
            img_checkId_back.HeightRequest = ((Helpers.MConfig.screenWidth - 30) / 2) * 0.6666;
            img_takePeoplePic.HeightRequest = Helpers.MConfig.screenWidth * 0.5217;
            UI呈现();
            获取用户身份证();
        }

        void UI呈现()
        {
            ///实名认证与否（1：认证，0：没有认证2:认证中,3:认证失败）
            Device.BeginInvokeOnMainThread(() =>
            {
                if (Data.UserInfoCache.userInfo.RealNameAuthentication == 1)
                {
                    lb_checkDesc.Text = "信息已通过，感谢您的配合";
                    lb_checkDesc.IsVisible = true;
                    btn_confirmed.IsVisible = false;
                    st_checkEty.IsVisible = false;
                }
                else if (Data.UserInfoCache.userInfo.RealNameAuthentication == 2)
                {
                    lb_checkDesc.Text = "正在飞速审核中...";
                   lb_checkDesc.IsVisible = true;
                    btn_confirmed.IsVisible = false;
                    st_checkEty.IsVisible = false;
                }
                else if (Data.UserInfoCache.userInfo.RealNameAuthentication == 3)
                {
                    lb_checkDesc.Text = "信息未通过，请确认照片是否清晰与正确";
                    lb_checkDesc.IsVisible = true;
                    btn_confirmed.IsVisible = true;
                    st_checkEty.IsVisible = true;
                }
                else if (Data.UserInfoCache.userInfo.RealNameAuthentication == 0)
                {
                    lb_checkDesc.IsVisible = false;
                    btn_confirmed.IsVisible = true;
                    st_checkEty.IsVisible = true;
                }
            });
        }

        void 获取用户身份证()
        {
            Tools.AsyncMsg am_获取用户身份证 = new Tools.AsyncMsg();

            string para = "UserGUID=" + Data.UserInfoCache.UserGUID;

            am_获取用户身份证.Completion += (object obje, string exc) =>
            {
                string returnJson = obje.ToString();
                string ErrMsg = "";
                if (returnJson == "[]" || returnJson == "")
                    return;

                try
                {
                    returnJson = Tools.CommonClass.GetJsonByTag(returnJson, ref ErrMsg);
                }
                catch (Exception ex)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "解析用户认证信息错误！" + ex.Message, "知道了");
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
                catch (Exception ex)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "解析用户认证信息包错误！" + ex.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                ObservableCollection<Data.IDCardData> TempList = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Data.IDCardData>>(jar.ToString());

                Device.BeginInvokeOnMainThread(() =>
              {
                  foreach (var temp in TempList)
                  {
                      if (temp.Remarks == "正面")
                      {
                          正面审核状态 = temp.CHECKIS;

                          img_checkId_forward.Source = temp.IDCardPhotoForShow;

                          if (正面审核状态 == "已审核")
                          {
                              正面照片存在 = true;
                          }

                          if (正面审核状态 == "已驳回")
                          {
                              lb_checkId_forward.IsVisible = true;
                          }

                      }
                      else if (temp.Remarks == "反面")
                      {
                          反面审核状态 = temp.CHECKIS;
                          img_checkId_back.Source = temp.IDCardPhotoForShow;

                          if (反面审核状态 == "已审核")
                          {
                              反面照片存在 = true;
                          }

                          if (反面审核状态 == "已驳回")
                          {
                              lb_checkId_back.IsVisible = true;
                          }
                      }
                      else if (temp.Remarks == "手持")
                      {
                          手持审核状态 = temp.CHECKIS;
                          img_takePeoplePic.Source = temp.IDCardPhotoForShow;

                          if (手持审核状态 == "已审核")
                          {
                              手持照片存在 = true;
                          }

                          if (手持审核状态 == "已驳回")
                          {
                              lb_takePeoplePic.IsVisible = true;
                          }
                      }
                  }
              });
            };

            //am_获取用户身份证.Cancel += (object obje, string exc) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        //DisplayAlert("提示", "获取认证信息失败:" + exc.ToString(), "知道了");
            //    });
            //    return;
            //};

            Tools.NetClass.GetStringByUrl("QueryData", "App\\GetIDCardInfo_1_0_0_1", para, am_获取用户身份证);
        }

        /// <summary>
        /// 提交正面照片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_takeForwrdPic(object sender, EventArgs e)
        {
            if (正面审核状态 == "已审核" || 正面审核状态 == "待审核")
                return;

            弹窗标志 = "正面";

            Device.BeginInvokeOnMainThread(() =>
            {
                block.IsVisible = true;
                frm_chooseBox.IsVisible = true;
            });

        }

        /// <summary>
        /// 提交背面照片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_takeBackPic(object sender, EventArgs e)
        {
            if (反面审核状态 == "已审核" || 反面审核状态 == "待审核")
                return;

            弹窗标志 = "反面";
            Device.BeginInvokeOnMainThread(() =>
            {
                block.IsVisible = true;
                frm_chooseBox.IsVisible = true;
            });
        }

        /// <summary>
        /// 提交手持照片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_takePeoplePic(object sender, EventArgs e)
        {
            if (手持审核状态 == "已审核" || 手持审核状态 == "待审核")
                return;

            弹窗标志 = "手持";
            Device.BeginInvokeOnMainThread(() =>
            {
                block.IsVisible = true;
                frm_chooseBox.IsVisible = true;
            });

        }

        void 上传图片(System.IO.Stream imageStream)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                hud.Show_StatusMessage("");

            });

            try
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
                imageData = rez.ResizeImage(imageData, 400);

                #endregion

                string CutImg = Convert.ToBase64String(imageData);

                Tools.AsyncMsg am = new Tools.AsyncMsg();

                string guid = System.Guid.NewGuid().ToString();
                string newheadPhotoGuid = guid + ".JPG";

                am.Completion += ((object sender_am, string e_am) =>
                {

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Data.IDCardData 待认证信息数据 = new Data.IDCardData() { IDCardGUID = guid, IDCardPhoto = newheadPhotoGuid, Remarks = 弹窗标志 };
                        if (弹窗标志 == "正面")
                        {
                            img_checkId_forward.BindingContext = 待认证信息数据;
                            正面照片存在 = true;
                        }
                        else if (弹窗标志 == "反面")
                        {
                            img_checkId_back.BindingContext = 待认证信息数据;
                            反面照片存在 = true;
                        }
                        else if (弹窗标志 == "手持")
                        {
                            img_takePeoplePic.BindingContext = 待认证信息数据;
                            手持照片存在 = true;
                        }

                        hud.Dismiss();
                    });

                });

                am.Cancel += ((object sender_am, string e_am) =>
                {
                    // isUpload = false;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayAlert("提示", "商品图片传输错误！"  , "知道了");
                    });
                });

                Tools.NetClass.PostImgByUrlForSmall("shenfenzheng\\" + newheadPhotoGuid.Substring(0, 2) + "\\", newheadPhotoGuid, CutImg, am);
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("提示", "上传图片失败！"  , "知道了");
                });
            }
 
            // img_Logo.UpLoadImage("", "", "MENUIMG\\" + newMenuPhotoGuid.Substring(0, 3) + "\\", newMenuPhotoGuid, CutImg, am);
        }

        /// <summary>
        /// 拍照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_takePic(object sender, EventArgs e)
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

                Device.BeginInvokeOnMainThread(() =>
                {
                    // img_Logo.ImageSource = imgS;
                    if (弹窗标志 == "正面")
                    {
                        img_checkId_forward.Source = imgS;
                    }
                    else if (弹窗标志 == "反面")
                    {
                        img_checkId_back.Source = imgS;
                    }
                    else if (弹窗标志 == "手持")
                    {
                        img_takePeoplePic.Source = imgS;
                    }

                    block.IsVisible = false;
                    frm_chooseBox.IsVisible = false;
                });
            });
        }

        /// <summary>
        /// 打开相册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_album(object sender, EventArgs e)
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
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            // UI.UIColor.hud.Show_Toast("读取文件流失败：" + ex.Message);
                        });
                        return null;
                    }

                    return stream;
                });

                Device.BeginInvokeOnMainThread(() =>
                {
                    // img_Logo.ImageSource = imgS;
                    if (弹窗标志 == "正面")
                    {
                        img_checkId_forward.Source = imgS;
                    }
                    else if (弹窗标志 == "反面")
                    {
                        img_checkId_back.Source = imgS;
                    }
                    else if (弹窗标志 == "手持")
                    {
                        img_takePeoplePic.Source = imgS;
                    }

                    block.IsVisible = false;
                    frm_chooseBox.IsVisible = false;
                });

            });
        }

        /// <summary>
        /// 关闭弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_closeBox(object sender, EventArgs e)
        {
            block.IsVisible = false;
            frm_chooseBox.IsVisible = false;
            弹窗标志 = "";
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_confirmed(object sender, EventArgs e)
        {

            if (按钮防呆)
                return;

            按钮防呆 = true;

            string 真实姓名 = ety_name.Text.Trim();
            string 身份证号 = ety_id.Text.Trim();


            if(真实姓名=="")
            {
                hud.Show_Toast("请输入姓名");
                按钮防呆 = false;
                return;
            }

            if (Tools.CommonClass.判断是否有特殊字符(真实姓名))
            {
                hud.Show_Toast("姓名包含特殊符号");
                按钮防呆 = false;
                return;
            }

            if (身份证号 == "")
            {
                hud.Show_Toast("请输入身份证号");
                按钮防呆 = false;
                return;
            }

            if (Tools.CommonClass.判断是否有特殊字符(身份证号))
            {
                hud.Show_Toast("身份证号码包含特殊符号");
                按钮防呆 = false;
                return;
            }

            if (身份证号.Length != 18)
            {
                hud.Show_Toast("身份证号码长度输入有误！");
                按钮防呆 = false;
                return;
            }
                //  ImageSource te = img_checkId_forward.Source;

                if (!正面照片存在 || !反面照片存在 || !手持照片存在)
            {
                hud.Show_Toast("图片提交不完整");
                按钮防呆 = false;
                return;
            }


            if (正面审核状态 != "已审核")
            {
                var row = (Data.IDCardData)img_checkId_forward.BindingContext;
                待审核数据.Add(row);
            }

            if (反面审核状态 != "已审核")
            {
                var row = (Data.IDCardData)img_checkId_back.BindingContext;
                待审核数据.Add(row);
            }

            if (手持审核状态 != "已审核")
            {
                var row = (Data.IDCardData)img_takePeoplePic.BindingContext;
                待审核数据.Add(row);
            }


            if (Data.UserInfoCache.userInfo.RealNameAuthentication == 0)
            {
                if (待审核数据.Count != 3)
                {
                    hud.Show_Toast("请完善审核信息");
                    按钮防呆 = false;
                    return;
                }
            }

            if (待审核数据.Count > 0)
            {
                Tools.AsyncMsg am_提交审核数据 = new Tools.AsyncMsg();

                string orderDetailjson = "";
                foreach (var row in 待审核数据)
                {

                    Dictionary<string, string> 审核详情 = new Dictionary<string, string>();

                    审核详情.Add("UserGUID", Data.UserInfoCache.UserGUID);
                    审核详情.Add("IDCardGUID", row.IDCardGUID);
                    审核详情.Add("IDCardPhoto", row.IDCardPhoto);
                    审核详情.Add("Remarks", row.Remarks);
                    审核详情.Add("RealName", 真实姓名);
                    审核详情.Add("IDCardNo", 身份证号);
                    
                    Tools.NetClass.AddPostJsonString(ref orderDetailjson, 审核详情);
                }

                am_提交审核数据.Completion += (object obj, string ex) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        按钮防呆 = false;
                        Data.UserInfoCache.userInfo.RealNameAuthentication = 2;
                        hud.Show_Toast("提交成功");
                        Navigation.PopAsync(true);
                    });
                };

                am_提交审核数据.Cancel += (object obj, string ex) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        按钮防呆 = false;
                       //DisplayAlert("提示", "提交审核数据失败:" + ex.ToString(), "知道了");
                    });
                    return;
                };

                Tools.NetClass.PostDataByUrl("APP\\UpdateIDCardInfo_1_0_0_1", "ExSql", orderDetailjson, am_提交审核数据);
            }
        }
    }
}