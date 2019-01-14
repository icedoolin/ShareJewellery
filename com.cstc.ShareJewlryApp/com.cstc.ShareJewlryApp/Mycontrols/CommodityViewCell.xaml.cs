using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Mycontrols
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommodityViewCell : ContentView
    {
        public CommodityViewCell()
        {
            InitializeComponent();
            //安卓和苹果的阴影效果有很大差异，苹果的时候不要
            if (Device.RuntimePlatform==Device.Android)
            {
                Grid_Main.BackgroundColor = Color.White;
                left_frame.HasShadow = true;
                right_frame.HasShadow = true;
            }
 
        }
        //声明委托事件
        public delegate void myevent(object sender, string e);
        //命名委托
        public event myevent SelectedCommodity;
        /// <summary>
        /// 选中单元格
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSelectedCommodity(string jewelleryGUID)
        {
            if (SelectedCommodity != null)
                SelectedCommodity(this, jewelleryGUID);
        }

        bool 按钮防呆 = false;
        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();
 
        public string jewelleryGUID { get; set; }


 
        private void tapped_commodity(object sender, EventArgs e)
        {
            if (按钮防呆) return;
            按钮防呆 = true;
             jewelleryGUID = ((Data.commodityData)(((StackLayout)sender).BindingContext)).JewelleryGUID;
            OnSelectedCommodity(jewelleryGUID);
            //hud.Show_StatusMessage("");
            按钮防呆 = false;
        }

        private void tapped_commodity_right(object sender, EventArgs e)
        {
            if (按钮防呆) return;
            按钮防呆 = true;
            jewelleryGUID = ((Data.commodityData)(((StackLayout)sender).BindingContext)).rightJewelleryGUID;
            OnSelectedCommodity(jewelleryGUID);
            按钮防呆 = false;
        }
     
    }
}