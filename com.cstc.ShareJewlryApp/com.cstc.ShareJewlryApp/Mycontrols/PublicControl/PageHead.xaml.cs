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
    public partial class PageHead : ContentView
    {
        bool 按钮防呆 = false;

        /// <summary>
        /// 如果页面中包含webview，判断是否是第一级，用于返回上级页面或关闭页面。
        /// </summary>
        public bool IsFirstPage { get; set; } = true;  
        public delegate void myevent(object sender, EventArgs e);
        //命名委托
        public event myevent BtnBackClick;
        public event myevent BtnForwardClick;
        public event myevent EtyEnter;
        public event myevent EtyLeave;
        /// <summary>
        /// 点击回退按钮
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnBtnBackClick(EventArgs e)
        {
            if (BtnBackClick != null)
                BtnBackClick(this, e);
        }
        /// <summary>
        /// 点击前进按钮
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnBtnForwardClick(EventArgs e)
        {
            if (BtnForwardClick != null)
                BtnForwardClick(this, e);
        }
        /// <summary>
        /// 进入搜索框
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnEtyEnter(EventArgs e)
        {
            if (EtyEnter != null)
                EtyEnter(this, e);
        }
        /// <summary>
        /// 离开搜索框
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnEtyLeave(EventArgs e)
        {
            if (EtyLeave != null)
                EtyLeave(this, e);
        }


        public interface IPageWorking
        {
            void GoBack();
        }

        public IPageWorking PageWorking = null;

        public PageHead()
        {
            InitializeComponent();
            //为状态栏保留位置高度，此处统一配置
            //if(Device.RuntimePlatform==Device.iOS)
            //{
            //    row0.Height = 30;                
            //}

            row1.Height = 50 * Helpers.MConfig.screenRatios;
            icon_back.Text = Helpers.SvgFontHelper.Back_Black;
        }

        /// <summary>
        /// 左边的内容，可以使用iconfont代码显示为图标
        /// </summary>
        public string LeftText
        {
            get { return icon_back.Text; }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    icon_back.Text = value;
                });
            }
        }
        /// <summary>
        /// 左边的字体大小，文本的时候比icon图标需要更大的字体
        /// </summary>
        public double BackTextFontSize
        {
            get { return icon_back.FontSize; }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    icon_back.FontSize = value;
                });
            }
        }
        /// <summary>
        /// 左边的字体颜色
        /// </summary>
        public Color TextColor_Back
        {
            get { return icon_back.TextColor; }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    icon_back.TextColor = value;
                });
            }
        }

        /// <summary>
        /// 右边的内容，可以使用iconfont代码显示为图标
        /// </summary>
        public string RightText
        {
            get { return icon_forward.Text; }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    st_right.IsVisible = true;
                    icon_forward.Text = value;
                });
            }
        }

        /// <summary>
        /// 右边的字体大小，文本的时候比icon图标需要更大的字体
        /// </summary>
        public double RightTextFontSize
        {
            get { return icon_forward.FontSize; }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {              
                    icon_forward.FontSize = value;
                });
            }
        }

        /// <summary>
        /// 右边的字体颜色
        /// </summary>
        public Color TextColor_Forward
        {
            get { return icon_forward.TextColor; }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    icon_forward.TextColor = value;
                });
            }
        }

        /// <summary>
        /// 内容颜色,改变整个标题的三个元素的颜色
        /// </summary>
        public Color TextColor
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    icon_back.TextColor = value;
                    headerName.TextColor = value;
                    icon_forward.TextColor = value;
                });
            }
        }

        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color BackGroundColor_Main
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    grid_head.BackgroundColor = value;
                });

            }
        }

        /// <summary>
        /// 背景图片。根据背景图片的纵横比计算图片占位高度
        /// </summary>
        public string BackGroudImage
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    img_bg.Source = value;                    
                });

            }
        }
        //public int BackGroundImageHeight
        //{
        //    set
        //    {
        //        Device.BeginInvokeOnMainThread(() =>
        //        {                    
        //            row2.Height = (value-Convert.ToInt32(row1.Height)- Convert.ToInt32(row2.Height) )* Helpers.MConfig.screenWidth;
        //        });
        //    }
        //}

        /// <summary>
        /// 左边内容是否可见，默认可见
        /// </summary>
        public bool IsLeftVisible
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    st_left.IsVisible = value;
                });

            }
        }


        /// <summary>
        /// 中间标题文本，有文本的时候显示，没有的时候隐藏
        /// </summary>
        public string Text
        {
            get { return headerName.Text; }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    headerName.IsVisible = (value != ""); 
                    headerName.Text = value;
                });

            }
        }
        /// <summary>
        /// 中间标题默认文本，有文本的时候显示，没有的时候隐藏
        /// </summary>
        public string SearchPlaceholder
        {
            get { return ety_content.Placeholder; }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    frame_Search.IsVisible = (value != "");
                    ety_content.IsVisible = (value!="");
                    ety_content.Placeholder = value;
                });
            }
        }
        /// <summary>
        /// 中间标题实际文本 
        /// </summary>
        public string SearchText
        {
            get { return ety_content.Text; }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ety_content.Text = value;
                });
            }
        }

        void Back_Taped(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;


            if (IsFirstPage == true)
            {
                Navigation.PopAsync(true);
            }
            else
            {
                OnBtnBackClick(new EventArgs());
 
            }
 
            按钮防呆 = false;
        }

        private void Forward_Taped(object sender, EventArgs e)
        {
            OnBtnForwardClick(new EventArgs());
        }

        private void ety_content_focused(object sender, FocusEventArgs e)
        {
            OnEtyEnter(new EventArgs());
        }

        private void ety_content_unfocused(object sender, FocusEventArgs e)
        {
            OnEtyLeave(new EventArgs());
        }
    }
}