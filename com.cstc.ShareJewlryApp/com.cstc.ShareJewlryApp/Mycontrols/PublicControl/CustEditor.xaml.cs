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
    public partial class CustEditor : ContentView
    {
        public static readonly BindableProperty TextProperty = 
            BindableProperty.Create("Text", typeof(string), typeof(CustEditor), null, defaultBindingMode: BindingMode.TwoWay, 
                propertyChanged: TextPropertyChanged);
        public event EventHandler<TextChangedEventArgs> TextChanged;
        public CustEditor()
        {
            InitializeComponent();
            lb_textCount.Text = "0/" + TextCount;
        }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get
            {
                //if (edt_content.Text == null)
                //    return string.Empty;
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustEditor)bindable;
            if (newValue != null && newValue.ToString() !="")
            {
                control.edt_content.Text = newValue.ToString();
                //control.Text = newValue.ToString();
            }

        }

        /// <summary>
        /// 背景颜色
        /// </summary>
        Color _FrameBackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.SpaceColor;
        public Color FrameBackgroundColor
        {
            get
            {
                return borderFrame.BackgroundColor;
            }
            set
            {
                _FrameBackgroundColor = value;
                borderFrame.BackgroundColor = value;
            }
        }


        /// <summary>
        /// 字体颜色
        /// </summary>
        Color _TextColor = Color.Black;
        public Color TextColor
        {
            get
            {
                return _TextColor;
            }
            set
            {
                _TextColor = value;
                edt_content.TextColor = value;
            }
        }

        /// <summary>
        /// 字体大小
        /// </summary>
        public double FontSize
        {
            get
            {
                return edt_content.FontSize;
            }
            set
            {
                edt_content.FontSize = value;
            }
        }
        /// <summary>
        /// 圆角弧度
        /// </summary>
        public float CornerRadius
        {
            get
            {
                return borderFrame.CornerRadius;
            }
            set
            {
                borderFrame.CornerRadius = value;
            }
        }
        /// <summary>
        /// 输入键盘
        /// </summary>
        public Keyboard Keyboard
        {
            get
            {
                return edt_content.Keyboard;
            }
            set
            {
                edt_content.Keyboard = value;
            }
        }
        /// <summary>
        /// 占位符
        /// </summary>
        string _EditorPlaceholder = "";
        public string EditorPlaceholder
        {
            get
            {
                return _EditorPlaceholder;
            }

            set
            {
                _EditorPlaceholder = value;

                if (edt_content.Text == "")
                {
                    tempEditorPlaceholder = value;
                }
            }
        }

        public string tempEditorPlaceholder
        {
            get
            {
                return _EditorPlaceholder;
            }

            set
            {
                edt_content.Text = value;
                edt_content.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
            }
        }


        void edt_content_focused(object sender, EventArgs e)
        {
            borderFrame.BackgroundColor = Color.FromHex("fcf5ec");


            if (!edt_content.TextColor.Equals(TextColor) )
            {
                tempEditorPlaceholder = "";
            }

            edt_content.TextColor = TextColor;

            edt_content.Focus();

        }
        private void edt_content_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (edt_content.Text.Length > _TextCount)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    edt_content.Text = edt_content.Text.Remove(_TextCount);
                });

            }
            if (edt_content.Text != _EditorPlaceholder)
                Text = edt_content.Text;



            if ((edt_content.Text != null || edt_content.Text.Length <= _TextCount) && edt_content.Text != EditorPlaceholder)
            {
                lb_textCount.Text = edt_content.Text.Length + "/" + TextCount;
            }
        }

        private void edt_content_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {

        }
 
        int _TextCount = 1000;
        public int TextCount
        {
            get
            {
                return _TextCount;
            }
            set
            {
                _TextCount = value;
            }
        }

        void edt_content_unfocused(object sender, EventArgs e)
        {
            borderFrame.BackgroundColor = _FrameBackgroundColor;

            if (edt_content.Text == "")
            {
                tempEditorPlaceholder = EditorPlaceholder;
            }
        }

        private void edt_content_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(sender, e);
            }
        }


    }
}