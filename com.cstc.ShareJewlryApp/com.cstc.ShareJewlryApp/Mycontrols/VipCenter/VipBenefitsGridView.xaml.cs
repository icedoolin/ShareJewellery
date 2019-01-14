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
	public partial class VipBenefitsGridView : ContentView
	{
		public VipBenefitsGridView ()
		{
			InitializeComponent ();
		}


        public int SelectedCellNO { get; set; }
        //声明委托事件
        public delegate void myevent(object sender, EventArgs e);
        //命名委托
        public event myevent SelectedTitleChanged;
        /// <summary>
        /// 选中单元格
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSelectedTitleChanged(EventArgs e)
        {
            if (SelectedTitleChanged != null)
                SelectedTitleChanged(this, e);
        }


        private void TapVipBenfitsDetail_Tapped(object sender, EventArgs e)
        {
            if (sender == cell_1)
            {
                SelectedCellNO = 1;
            }
            if (sender == cell_2)
            {
                SelectedCellNO = 2;
            }
            if (sender == cell_1)
            {
                SelectedCellNO = 1;
            }
            if (sender == cell_3)
            {
                SelectedCellNO = 3;
            }
            if (sender == cell_4)
            {
                SelectedCellNO = 4;
            }
            if (sender == cell_5)
            {
                SelectedCellNO = 5;
            }
            if (sender == cell_6)
            {
                SelectedCellNO = 6;
            }
            if (sender == cell_7)
            {
                SelectedCellNO = 7;
            }
            if (sender == cell_8)
            {
                SelectedCellNO = 8;
            }
            OnSelectedTitleChanged(new EventArgs());
            //VipBenefitsDetail_Show.IsVisible = true;
        }

 
        private void TapClose_Tapped(object sender, EventArgs e)
        {
            //VipBenefitsDetail_Show.IsVisible = false;
        }
    }
}