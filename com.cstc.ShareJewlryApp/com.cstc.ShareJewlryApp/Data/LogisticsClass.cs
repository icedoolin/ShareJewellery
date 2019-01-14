using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.cstc.ShareJewlryApp.Data
{
    /// <summary>
    /// 物流类
    /// </summary>
    public class LogisticsClass
    {
        //public DateTime? time { get; set; }

        public DateTime ftime { get; set; }//获取的物流日期时间

        string _Time = "00:00";
        public string Time//显示时间
        {
            get
            {
                if (ftime != null)
                {
                    _Time = ftime.ToString("HH:mm");
                }
                return _Time;
            }
        }

        string _Data = "00-00";
        public string Data//显示日期
        {
            get
            {
                if (ftime != null)
                {
                    _Data = ftime.ToString("MM-dd");
                }
                return _Data;
            }
        }

        public string context { get; set; }//物流内容

        Color _bcolor = Color.FromHex("#787878");

        public Color bcolor//物流状态显示的颜色
        {
            get
            {
                if (context.Contains("签收") || context.Contains("派件") || context.Contains("送件") || context.Contains("派送"))
                {
                    _bcolor = Color.FromHex("#f2566f");
                }
                else
                {
                    Color _bcolor = Color.FromHex("#787878");
                }
                return _bcolor;
            }
        }

        string _Text = "\ue830";
        public string Text//物流状态显示的图标（默认圆点）
        {
            get
            {
                if (context.Contains("已出库") || context.Contains("已发货") || context.Contains("已收件"))
                {
                    _Text = "\ue605";
                }
                if (context.Contains("揽件") || context.Contains("运输中"))
                {
                    _Text = "\ue606";
                }
                if (context.Contains("派件") || context.Contains("送件") || context.Contains("派送"))
                {
                    _Text = "\ue603";
                }
                if (context.Contains("签收"))
                {
                    _Text = "\ue604";
                }
                return _Text;
            }
        }

        double _Fontsize = 7;
        public double Fontsize//物流状态显示的大小（默认7）
        {
            get
            {
                if (context.Contains("已出库") || context.Contains("已发货") || context.Contains("已收件"))
                {
                    _Fontsize = 20;
                }
                if (context.Contains("揽件") || context.Contains("运输中"))
                {
                    _Fontsize = 20;
                }
                if (context.Contains("派件") || context.Contains("送件") || context.Contains("派送"))
                {
                    _Fontsize = 20;
                }
                if (context.Contains("签收"))
                {
                    _Fontsize = 20;
                }
                return _Fontsize;
            }
        }


        public Color tColor { get; set; }//显示物流状态上线条的颜色
        public Color eColor { get; set; }//显示物流状态下线条的颜色

    }
}
