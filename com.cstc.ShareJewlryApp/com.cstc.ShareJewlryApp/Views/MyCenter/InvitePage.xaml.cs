using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.MyCenter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InvitePage: BasePage 
    {
        bool 按钮防呆 = false;
        com.cstc.ShareJewlryApp.Tools.IWeChat WXDependency = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.IWeChat>();
        com.cstc.ShareJewlryApp.Tools.IBarcodeService photocompoundPhoto = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.IBarcodeService>();
        com.cstc.ShareJewlryApp.Tools.ICopy savePhoto = DependencyService.Get<com.cstc.ShareJewlryApp.Tools.ICopy>();
        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
        Data.InviteTemplateData 选中项 = new Data.InviteTemplateData();
        Image QRcodeImg = new Image() { Aspect = Aspect.Fill };//二维码图

        byte[] 分享图 = null;


        public InvitePage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            获取图片();
        }
  
        void 获取图片()
        {
            Helpers.AsyncMsg am_获取信息 = new Helpers.AsyncMsg();
            am_获取信息.Completion += (object obj, string ex) =>
            {
                List<Data.InviteTemplateData> lists = (List<Data.InviteTemplateData>)obj;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Mycontrols.InviteBox firstImg = null;
                    ///添加到滑动框
                    int i = 0;
                    foreach (var temp in lists)
                    {
                        Mycontrols.InviteBox tempImg = new Mycontrols.InviteBox() { imgsource = temp.AppTemplateThumbnailForShow, imgTitle = temp.TemplateTitle,
                            IsSelect = false, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.FillAndExpand, WidthRequest = 80 };

                        tempImg.BindingContext = temp;
                        tempImg.Click += tapped_selectImg;
                        //添加手势
                        st_img.Children.Add(tempImg);
                        if (i == 0)
                        {
                            firstImg = tempImg;
                        }
                        i++;
                    }
 
                    tapped_selectImg(firstImg, null);
                    //WXDependency.shareWebPage(Data.InviteDataMgr.GetcmtyShareUrl(JewelleryGUID, Data.UserInfoCache.UserGUID), item.TemplateTitle, item.ShareTitle, sign, item.AppTemplateThumbnailForShow);
                });
            };

 
            Data.InviteDataMgr.GetShareTemplate("好友", am_获取信息);
 

        }

        /// <summary>
        /// 选择模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tapped_selectImg(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                hud.Show_StatusMessage("");
            });
            var btn = (Mycontrols.InviteBox)sender;

            if (btn.IsSelect)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    hud.Dismiss();
                });
                return;
            }
            if (rl_img.Children.Count > 1)
                rl_img.Children.Remove(QRcodeImg);

            foreach (Mycontrols.InviteBox temp in st_img.Children)
            {
                temp.IsSelect = false;
            }

            btn.IsSelect = true;

            选中项 = (Data.InviteTemplateData)btn.BindingContext;
            

            img_showImg.Source = 选中项.AppTemplatePicForShow;

            //double 实际X = img_showImg.Width * (选中项.CoordinateX / (double)490);

            //double 实际Y = img_showImg.Height * (选中项.CoordinateY / (double)650);

            double QR_实际尺寸 = img_showImg.Height * ((double)选中项.Length / (double)650);

            var QRstream = photocompoundPhoto.ConvertImageStream(Helpers.MConfig.InviteUrl + "templetGuid=" + 选中项.TemplateGUID + "&parentGuid=" + Data.UserInfoCache.UserGUID, (int)QR_实际尺寸, (int)QR_实际尺寸);

            QRcodeImg.Source = ImageSource.FromStream(() => QRstream);



            rl_img.Children.Add(QRcodeImg,
                Constraint.RelativeToView(img_showImg, (Parent, sibling) =>
                {
                    return rl_img.Width- QR_实际尺寸-10;
                }), Constraint.RelativeToView(img_showImg, (parent, sibling) =>
                {
                    return rl_img.Height - QR_实际尺寸 - 10;
                }), Constraint.RelativeToParent((parent) =>
                {
                    return QR_实际尺寸;
                }), Constraint.RelativeToParent((parent) =>
                {
                    return QR_实际尺寸;
                }));
 

            Device.BeginInvokeOnMainThread(() =>
            {
                hud.Dismiss();
            });

        }
 
        /// <summary>
        /// 关闭遮罩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_cancel(object sender, EventArgs e)
        {
            block.IsVisible = false;
            st_shareBox.IsVisible = false;
            frm_SelectShareTypeBox.IsVisible = false;
        }

        /// <summary>
        /// 分享 
        /// </summary>
        /// <param name="sign">好友或者朋友圈 Chat/Friends</param>
        private void Share(string sign)
        {
            //要使用商品模板里面的小图   item.AppTemplateThumbnailForShow
            Helpers.AsyncMsg am_获取信息 = new Helpers.AsyncMsg();
            am_获取信息.Completion += (object obj, string ex) =>
            {
                Data.InviteTemplateData item = (Data.InviteTemplateData)obj;
                Device.BeginInvokeOnMainThread(() =>
                {
                    string URL = Helpers.MConfig.InviteUrl + "templetGuid=" + 选中项.TemplateGUID + "&parentGuid=" + Data.UserInfoCache.UserGUID;
                    WXDependency.shareWebPage(URL, 选中项.ShareTitle, 选中项.Copywriting, sign, item.AppTemplateThumbnailForShow);
                });
            };

 
            Data.InviteDataMgr.GetShareTemplate("商品", am_获取信息);

        }

        /// <summary>
        /// 分享给朋友
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_shareChat(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;

            Share("Chat");
            tapped_cancel(null, null);
            按钮防呆 = false;
        }

        /// <summary>
        /// 分享朋友圈
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_shareFriends(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;
            Share("Friends");
            tapped_cancel(null, null);
            按钮防呆 = false;
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_savePhoto(object sender, EventArgs e)
        {

           
            //   var imgS = img_showImg.Source;
            var s = photocompoundPhoto.compoundPhoto(选中项.AppTemplatePicForShow, 选中项.TemplateGUID, 选中项.CoordinateX, 选中项.CoordinateY, 选中项.Length);

            分享图 = new byte[s.Length];
            s.Read(分享图, 0, 分享图.Length);
            s.Seek(0, SeekOrigin.Begin);
            savePhoto.SaveNetImage(分享图);

            tapped_cancel(null, null);
        }


        /// <summary>
        /// 点击邀请按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_sendFriends(object sender, EventArgs e)
        {
            st_shareBox.IsVisible = true;
            frm_SelectShareTypeBox.IsVisible = false;
            block.IsVisible = true;
        }


    }
}