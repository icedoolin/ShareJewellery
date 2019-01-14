using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.cstc.ShareJewlryApp.Data
{
    /// <summary>
    /// 会员类
    /// </summary>
    public class VipData : BindableObject
    {

        public double Price { get; set; }
        public string MembershipPriceGUID { get; set; }
        public bool isHot { get; set; }
        public string detail { get; set; }
        bool _Selected = false;
        public bool Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                if (_Selected == value)
                    return;

                _Selected = value;
                if (value)
                {
                    backColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                    FontColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                }
                else
                {
                    backColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                    FontColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                }
                OnPropertyChanged("Selected");
            }
        }

        Color _FontColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
        public Color FontColor
        {
            get
            {
                return _FontColor;
            }

            set
            {
                _FontColor = value;
                OnPropertyChanged("FontColor");
            }
        }


        Color _backColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
        public Color backColor
        {
            get
            {
                return _backColor;
            }

            set
            {
                _backColor = value;
                OnPropertyChanged("backColor");
            }
        }


    }


    /// <summary>
    /// 弹出框内容
    /// </summary>
    public class MessageBoxElement
    {
        public string Type { get; set; }   //        类别（传’会员优势’或’会员权益’）
        public string Sequence { get; set; }    //序号
        public string Title { get; set; }    //标题
        public string Content { get; set; }    //内容
        public string TitleImage { get; set; }   //标题图片
        public string BodyImage { get; set; }   //主体图片
    }


    public class VipDataMgr
    {
        /// <summary>
        /// 获取VIP价格信息
        /// </summary>
        /// <param name="am"></param>
        public static void GetVipData(Helpers.AsyncMsg am)
        {
            Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
            Helpers.AsyncMsg am_获取数据 = new Helpers.AsyncMsg();
            Data.VipData item = new VipData(); 
            am_获取数据.Completion += (object obj, string ex) =>
            {
                string returnJson = obj.ToString();
                if (returnJson == "[]" || returnJson == "")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        hud.Show_Toast("获取会员价格失败！");
                    });
                    am.OnCancel();
                    
                }
                try
                {
                    List<Data.VipData> lists = Helpers.HttpHelper.GetItemList<Data.VipData>(returnJson);
                    item = lists[0];
                }
                catch (Exception exc)
                {
                    am.OnCancel();
                    return;
                }
                am.OnCompletion(item,"");
            };

            am_获取数据.Cancel += (object obj, string ex) =>
            {
                am.OnCancel();
            };

             
            Helpers.HttpHelper.GetStringByUrl(Helpers.AppApi.GetVipData, "", am_获取数据);

        }
    }
 



}
