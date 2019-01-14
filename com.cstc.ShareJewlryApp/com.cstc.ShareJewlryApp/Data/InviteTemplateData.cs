using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.cstc.ShareJewlryApp.Data
{
    /// <summary>
    /// 微信分享模板类
    /// </summary>
    public class InviteTemplateData
    {
        /// <summary>
        /// 模板GUID
        /// </summary>
        public string TemplateGUID { get; set; } = "";
        /// <summary>
        /// 分享标题
        /// </summary>
        public string ShareTitle { get; set; } = "";

        /// <summary>
        /// 模板标题
        /// </summary>
        public string TemplateTitle { get; set; } = "";

        /// <summary>
        /// 文案    网页用
        /// </summary>
        public string Copywriting { get; set; } = "";


        /// <summary>
        /// 模板图片名称
        /// </summary>
        public string AppTemplatePic { get; set; } = "";
        public string AppTemplatePicForShow
        {
            get
            {
                if (AppTemplatePic == "")
                    return "";
                return Helpers.MConfig.weixinpicUrl + AppTemplatePic.Substring(0, 1) + "/" + AppTemplatePic; 
            }
               
        }


        /// <summary>
        /// 模板缩略图名称
        /// </summary>
        public string AppTemplateThumbnail { get; set; } = "";
        public string AppTemplateThumbnailForShow
        {
            get
            {
                if (AppTemplateThumbnail == "")
                    return "";
                return Helpers.MConfig.weixinpicUrl + AppTemplateThumbnail.Substring(0, 1) + "/" + AppTemplateThumbnail;// + ".png";
            }
        }


        /// <summary>
        /// X轴坐标
        /// </summary>
        public float CoordinateX { get; set; } = 0;
        /// <summary>
        /// Y轴坐标
        /// </summary>
        public float CoordinateY { get; set; } = 0;
        /// <summary>
        /// 二维码长度
        /// </summary>
        public int Length { get; set; } = 0;

 
 
    }


    public class InviteDataMgr
    {
        /// <summary>
        /// 获取微信分享模板信息
        /// </summary>
        /// <param name="Sign">信息类型 商品/好友</param>
        public static void GetShareTemplate(string Sign, Helpers.AsyncMsg am)
        {
            Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
            Helpers.AsyncMsg am_获取数据 = new Helpers.AsyncMsg();
            List<Data.InviteTemplateData> lists = new List<InviteTemplateData>();
            Data.InviteTemplateData item = new InviteTemplateData();
            am_获取数据.Completion += (object obj, string ex) =>
            {
                string returnJson = obj.ToString();
                if (returnJson == "[]" || returnJson == "")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Show_Toast("获取分享信息失败！");
                    });
                    am.OnCancel();

                }
                try
                {
                     lists = Helpers.HttpHelper.GetItemList<Data.InviteTemplateData>(returnJson);              
 
                }
                catch (Exception exc)
                {
                    am.OnCancel();
                    return;
                }
                if (Sign == "好友")
                {
                    am.OnCompletion(lists, "");
                }
                if (Sign == "商品")
                {
                    item = lists[0];
                    am.OnCompletion(item, "");
                }

            };

            am_获取数据.Cancel += (object obj, string ex) =>
            {
                am.OnCancel();
            };


            string para ="&Sign=" + Sign;
            Helpers.HttpHelper.GetStringByUrl(Helpers.AppApi.GetShareTemplate, para, am_获取数据);
        }


        /// <summary>
        /// 获取用于分享的网页链接
        /// </summary>
        /// <param name="JewelleryGUID"></param>
        /// <param name="parentGUID"></param>
        /// <returns></returns>
        public static string GetcmtyShareUrl(string JewelleryGUID, string parentGUID)
        {
            string para = "JewelleryGUID=" + JewelleryGUID + "&parentGUID=" + parentGUID;
            string url = Helpers.MConfig.ShareUrl + para;
            return url;
        }



    }
}
