using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.MyCenter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyMsgPage: BasePage 
    {

        Tools.IWeChat wechat = DependencyService.Get<Tools.IWeChat>();
        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
        bool 按钮防呆 = false;
        public Tools.AsyncMsg am_更新数据 = new Tools.AsyncMsg();
        List<string> DeletePhotoGUIDs = new List<string>();  //删除的图片数据
        string _headPhoto = "";
 
        public MyMsgPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            呈现用户数据();
        }

        public void 呈现用户数据()
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                lb_NickName.RightText = Data.UserInfoCache.userInfo.Nickname;
                if (Data.UserInfoCache.userInfo.Tel != "")
                    lb_BindingPhone.RightText =Data.UserInfoCache.userInfo.Tel.Substring(0, 3) + "*****" 
                    +Data.UserInfoCache.userInfo.Tel.Substring(8, 3);
                lb_BindingweChat.RightText =Data.UserInfoCache.userInfo.WeChatName;
                if (Data.UserInfoCache.userInfo.AlipayName != "")
                {
                    if (Data.UserInfoCache.userInfo.AlipayName.Length <= 2)
                    {
                        lb_BindingAli.RightText =Data.UserInfoCache.userInfo.AlipayName.Substring(0, 1) + "*";
                    }
                    else
                    {
                        lb_BindingAli.RightText =Data.UserInfoCache.userInfo.AlipayName.Substring(0, 1) + "*" +Data.UserInfoCache.userInfo.AlipayName.Substring(2);
                    }
                }


                img_HeadImg.Source = Data.UserInfoCache.userInfo.PhotoForShow;
 
            });

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            am_更新数据.OnCompletion();
        }

        void tapped_ChagePwd(object sender, EventArgs e)  //修改密码
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (按钮防呆)
                    return;
                按钮防呆 = true;

                Navigation.PushAsync(new ChangePwdPage(), true);


                按钮防呆 = false;
            });

        }

        void tapped_BindingPhone(object sender, EventArgs e)  //绑定手机
        {

            if (按钮防呆)
                return;
            按钮防呆 = true;

            BindingPhonePage page = new BindingPhonePage();
            page.Phone = Data.UserInfoCache.userInfo.Tel;

            page.am_绑定手机.Completion += (object obj, string Phone) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    呈现用户数据();
                });
            };

            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(page, true);
                按钮防呆 = false;
            });

        }

        void tapped_ChangeNickname(object sender, EventArgs e)  //修改昵称
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            ChageNickNamePage page = new ChageNickNamePage();
            page.NickName = lb_NickName.RightText.Trim();


            page.am_保存昵称.Completion += (object obj, string NickName) =>
            {
                呈现用户数据();
            };

            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(page, true);
                按钮防呆 = false;
            });

        }

        void btn_takePhoto_Clicked(object sender, EventArgs e)   //"拍照"按钮
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
                    tapped_ClosedBlock(null, null);
                    img_HeadImg.Source = imgS;
                });
            });
        }

        void btn_Album_Clicked(object sender, EventArgs e)  //"从相册选择一张"按钮
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
                    tapped_ClosedBlock(null, null);
                    img_HeadImg.Source = imgS;
                });
            });
        }

        /// <summary>
        /// 管理收货地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_ManageAddress(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            Views.ShoppingCart.ReceiptAddress.ReceiptAddressPage page = new ShoppingCart.ReceiptAddress.ReceiptAddressPage();

            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(page, true);
                按钮防呆 = false;
            });
        }

        void 上传图片(System.IO.Stream imageStream)
        {

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
                imageData = rez.ResizeImage(imageData, 500);

                #endregion

                string CutImg = Convert.ToBase64String(imageData);

                Tools.AsyncMsg am = new Tools.AsyncMsg();

                string guid = System.Guid.NewGuid().ToString();
                string newheadPhotoGuid = guid + ".JPG";

                am.Completion += ((object sender_am, string e_am) =>
                {
                    更新头像(newheadPhotoGuid);
                });

                am.Cancel += ((object sender_am, string e_am) =>
                {
                    // isUpload = false;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayAlert("提示", "商品图片传输错误！" , "知道了");
                    });
                });

                Tools.NetClass.PostImgByUrlForSmall("yonghutouxiang\\" + newheadPhotoGuid.Substring(0, 2) + "\\", newheadPhotoGuid, CutImg, am);

                // img_Logo.UpLoadImage("", "", "MENUIMG\\" + newMenuPhotoGuid.Substring(0, 3) + "\\", newMenuPhotoGuid, CutImg, am);
            }
            catch (Exception ex)
            {

                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("提示", "上传头像失败:"  , "知道了");
                });
            }

          
        }

        void 更新头像(string newguid)
        {
            Tools.AsyncMsg am_更新头像 = new Tools.AsyncMsg();
            string para = "UserGUID=" + Data.UserInfoCache.UserGUID + "&Photo=" + newguid;

            am_更新头像.Completion += (object obj, string ex) =>
            {
                Helpers.AsyncMsg am_更新用户信息 = new Helpers.AsyncMsg();
                am_更新用户信息.Completion += (object obss, string bbb) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        呈现用户数据();
                    });
                };
 
                Data.UserinfoMgr.RefreshUserInfoCache("获取头像", am_更新用户信息);
                if (DeletePhotoGUIDs != null && DeletePhotoGUIDs.Count > 0)
                {
                    删除旧头像();

                }

            };
 

            Tools.NetClass.GetStringByUrl("ExSql", "App\\UpdatePhoto_1_0_0_1", para, am_更新头像);
        }

        void 删除旧头像()
        {
            foreach (var deleteGuid in DeletePhotoGUIDs)
            {
                Tools.NetClass.DeleteImgByUrl("yonghutouxiang\\" + deleteGuid.Substring(0, 2) + "\\", deleteGuid, new Tools.AsyncMsg());
                Tools.NetClass.DeleteImgByUrlForSmall("yonghutouxiang\\" + deleteGuid.Substring(0, 2) + "\\", deleteGuid, new Tools.AsyncMsg());
            }
            // Tools.NetClass.DeleteImgByUrl("MENUIMG\\" + guid.Substring(0, 3) + "\\", guid, new Tools.AsyncMsg());
        }

        /// <summary>
        /// 关闭遮罩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_ClosedBlock(object sender, EventArgs e)
        {
            block.IsVisible = false;
            st_PhotoType.IsVisible = false;

        }

        /// <summary>
        /// 显示 选择拍照、相册弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_ShowPhotoType(object sender, EventArgs e)
        {
            block.IsVisible = true;
            st_PhotoType.IsVisible = true;
        }

        /// <summary>
        /// 打开绑定支付宝
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_BindAliPay(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            BindAliPayPage page = new BindAliPayPage();

            page.am_支付宝页面.Completion += (object obj, string NickName) =>
            {
                呈现用户数据();
            };

            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(page, true);
                按钮防呆 = false;
            });
        }

        /// <summary>
        /// 绑定微信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_BindWetChat(object sender, EventArgs e)
        {
            if (Data.UserInfoCache.userInfo.WeChatName != "") return;
            hud.Show_StatusMessage("");
            Tools.CommonClass.微信授权标志 = "绑定";
            if (按钮防呆)
                return;
            按钮防呆 = true;

            Tools.CommonClass.am_微信绑定.Completion += (object obj, string s) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    hud.Dismiss();
                });

                微信绑定(); 
            };

            Tools.CommonClass.am_微信绑定.Cancel += (object obj, string s) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    hud.Dismiss(); 
                }); 
            };

            wechat.WXLogin();

            按钮防呆 = false;
            hud.Dismiss();

        }

        void 微信绑定()
        {
            Tools.AsyncMsg am_绑定= new Tools.AsyncMsg();

            string para = "Wechatopenid=" + Tools.CommonClass.微信OpenID + "&WeChatName=" + Tools.CommonClass.微信昵称 + "&UserGUID=" + Data.UserInfoCache.UserGUID;

            am_绑定.Completion += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    hud.Show_Toast("绑定成功");
                    //更新WeChatName字段
                    Helpers.AsyncMsg am_更新用户信息 = new Helpers.AsyncMsg();
                    am_更新用户信息.Completion += (object obss, string bbb) =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            呈现用户数据();
                        });
                    };
                    am_更新用户信息.Cancel += (object obss, string bbb) =>
                    {

                    };
                    Data.UserinfoMgr.RefreshUserInfoCache("微信绑定", am_更新用户信息);
 

                });
                
            };

            am_绑定.Cancel += (object obj, string ex) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("提示", "微信绑定失败" , "知道了");

                });
                return;
            };
 
            Tools.NetClass.GetStringByUrl("ExSql", "App\\BindingUserWechat_1_0_0_1", para, am_绑定);
        }

        private void tapped_quite(object sender, EventArgs e)
        {
            if (Data.UserInfoCache.UserGUID == "")
            {
                return;
            }
            st_checkBox.IsVisible = true;
            block.IsVisible = true;
        }

        private void tapped_yes(object sender, EventArgs e)
        {
            st_checkBox.IsVisible = false;
            block.IsVisible = false;

            hud.Show_Toast("退出成功");

            Data.UserinfoMgr.ClearUserInfoCache();
            //转到我的页面
            if (按钮防呆)
                return;
            按钮防呆 = true;

            MyCenterPage page = new MyCenterPage();
 
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(page, true);
                按钮防呆 = false;
            });

        }

        private void tapped_cancle(object sender, EventArgs e)
        {
            st_checkBox.IsVisible = false;
            block.IsVisible = false;
        }
    }
}