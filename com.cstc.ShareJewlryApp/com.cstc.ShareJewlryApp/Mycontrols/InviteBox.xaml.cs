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
    public partial class InviteBox : ContentView
    {
        public delegate void ClickEventHandler(object tag, EventArgs e);

        public event ClickEventHandler Click;

        public InviteBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string imgTitle
        {
            get
            {
                return lb_title_down.Text;
            }

            set
            {
                Device.BeginInvokeOnMainThread(() => 
                {
                    lb_title_down.Text = value;
                    lb_title_ctr.Text = value;
                });

              
            }
        }


        /// <summary>
        /// 是否被选择
        /// </summary>
        bool _IsSelect = false;
        public bool IsSelect
        {
            get
            {
                return _IsSelect;
            }

            set
            {
                _IsSelect = value;

                Device.BeginInvokeOnMainThread(() =>
                {
                    st_block.IsVisible = value;
                    st_block_down.IsVisible = !value;
                });
            }
        }

        public ImageSource imgsource
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    img.Source = value;
                });
               
            }
        }

        private void btn_select(object sender, EventArgs e)
        {
            if (Click != null)
            {
                Click(this, e);
            }
        }

        private void tapped_selectItem(object sender, EventArgs e)
        {
            if (Click != null)
            {
                Click(this, e);
            }
        }
    }
}