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
    public partial class PublicMessageBox : ContentView
    {
 
        //public Tools.AsyncMsg am_Return = new Tools.AsyncMsg();
        public PublicMessageBox()
        {
            InitializeComponent();
            Layout_Show.BackgroundColor = Color.FromRgba(0, 0, 0, 0.5);
 
            //IOS的按钮弧度半径为android的一半
            if (Device.RuntimePlatform == Device.iOS)
            {
                button_1.CornerRadius = 10;
                button_2.CornerRadius = 10;
                button_long.CornerRadius = 10;
            }
        }

        bool 按钮防呆 = false;
        Tools.Ihud hud = DependencyService.Get<Tools.Ihud>();

        public delegate void myevent(object sender, EventArgs e);
        //命名委托
        public event myevent Button1Clicked;
        public event myevent Button2Clicked;
        public event myevent ButtonLongClicked;
        /// <summary>
        /// 点击左边按钮
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnButton1Clicked(EventArgs e)
        {
            if (Button1Clicked != null)
                Button1Clicked(this, e);
        }
        /// <summary>
        /// 点击右边按钮
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnButton2Clicked(EventArgs e)
        {
            if (Button2Clicked != null)
                Button2Clicked(this, e);
        }
        /// <summary>
        /// 点击长条按钮
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnButtonLongClicked(EventArgs e)
        {
            if (ButtonLongClicked != null)
                ButtonLongClicked(this, e);
        }


        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return lbl_Title.Text; }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    lbl_Title.Text = value.TrimEnd();
                });
            }
        }
        /// <summary>
        /// 标题字号
        /// </summary>
        public double TitleFontSize
        {
            get { return lbl_Title.FontSize; }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    lbl_Title.FontSize = value;
                });
            }
        }

        /// <summary>
        /// 圆角区域整体高度，必须和宽度同时设定
        /// </summary>
        public int TotalHeight
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Main_Layout.HeightRequest = value * Helpers.MConfig.screenRatios;
                    //Main_Layout.Margin = new Thickness(50, (550 - value) / 2, 50, 0);
                });
            }
        }
        /// <summary>
        /// 圆角区域整体宽度，必须和高度同时设定
        /// </summary>
        public int TotalWidth
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Main_Layout.WidthRequest = value * Helpers.MConfig.screenRatios; 
                    //Main_Layout.Margin = new Thickness(50, (550 - value) / 2, 50, 0);
                });
            }
        }


        public string ImageBackGroundSource
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Image_BackGround.Source = value.Trim();
                    //if (Device.RuntimePlatform == Device.iOS)
                    //    Image_Title.Source = ImageSource.FromFile(value.ToLower());
                });
            }
        }

        /// <summary>
        /// 标题高度
        /// </summary>
        public int TitleHeight
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    lbl_Title.HeightRequest = value * Helpers.MConfig.screenRatios; ;
                    //sl_close.Margin = new Thickness(0, -1 * value - 10, 0, value - 55);
                });
            }
        }

        public string ImageBodySource
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Image_Body.Source = value.ToLower().Trim();
                    //if (Device.RuntimePlatform == Device.iOS)
                    //    Image_Body.Source = ImageSource.FromFile(value.ToLower());
                });
            }

        }
        /// <summary>
        /// 主体图片高度，用（整体宽度-10）*和图片的纵横比设定。
        /// 此处已经默认设定了margin(5,5,5,5)
        /// </summary>
        public int ImageBodyHeight
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Image_Body.HeightRequest = value * Helpers.MConfig.screenRatios; ;
                });
            }
        }

        public bool ImageBody_IsVisible
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Image_Body.IsVisible = value;
                });
            }
        }
        public double BodyFontSize
        {
            get { return Text_Body.FontSize; }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Text_Body.FontSize = value;
                });
            }
        }
        public string Body
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Text_Body.Text = value;
                });
            }
        }
        public Color TextColor_Body
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Text_Body.TextColor = value;
                });
            }
        }

        public TextAlignment HorizonTextAlignment_Body
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Text_Body.HorizontalTextAlignment = value;
                });
            }
        }

        public TextAlignment VerticalTextAlignment_Body
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Text_Body.VerticalTextAlignment = value;
                });
            }
        }


        public bool Button1_IsVisible
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
 
                    button_1.IsVisible = value;
                });
            }
        }
        public bool Button2_IsVisible
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
 
                    button_2.IsVisible = value;
                });
            }
        }
        public bool ButtonLong_IsVisible
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
 
                    button_long.IsVisible = value;
                });
            }
        }
        public string Text_Button1
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    button_1.Text = value;
                });
            }
        }
        public string Text_Button2
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    button_2.Text = value;
                });
            }
        }
        public string Text_ButtonLong
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    button_long.Text = value;
                });
            }
        }
        public Color BackGroundColor_Button1
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    button_1.BackgroundColor = value;
                });
            }
        }
        public Color BackGroundColor_Button2
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    button_2.BackgroundColor = value;
                });
            }
        }
        public Color BackGroundColor_ButtonLong
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    button_long.BackgroundColor = value;
                });
            }
        }
        public Color TextColor_ButtonLong
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    button_long.TextColor = value;
                });
            }
        }

        private void TapClose_Tapped(object sender, EventArgs e)
        {
            Title = "";
            ImageBodySource = "";
            Body = "";
            this.IsVisible = false;
            //am_Return.OnCancel();
        }

        private void TapBtn1_Tapped(object sender, EventArgs e)
        {
 
            if (按钮防呆)
                return;
            按钮防呆 = true;
            this.IsVisible = false;
            OnButton1Clicked(new EventArgs());

            按钮防呆 = false;
            //am_Return.OnCompletion(1, "");
        }

        private void TapBtn2_Tapped(object sender, EventArgs e)
        {

            if (按钮防呆)
                return;
            按钮防呆 = true;
            this.IsVisible = false;
            OnButton2Clicked(new EventArgs());

            按钮防呆 = false;
            //am_Return.OnCompletion(2, "");
        }

        private void TapBtnLong_Tapped(object sender, EventArgs e)
        {
            if (按钮防呆)
                return;
            按钮防呆 = true;
            this.IsVisible = false;
            OnButtonLongClicked(new EventArgs());
            按钮防呆 = false;

        }
    }
}