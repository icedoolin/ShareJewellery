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
    public partial class HeadPortrait : ContentView
    {

        //
        // 摘要:
        //     Backing store for the Text bindable property.
        public static readonly BindableProperty ImageSourceStringProperty=
            BindableProperty.Create("ImageSourceString", typeof(string), typeof(HeadPortrait), "", defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: ImageSourceStringPropertyChanged);
        //public event EventHandler<TextChangedEventArgs> ImageSourceStringChanged;
 
        public HeadPortrait()
        {
            InitializeComponent();
        }
        private static void ImageSourceStringPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (HeadPortrait)bindable;
            if (newValue != null && newValue.ToString() != "")
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    control.headportrait.Source = newValue.ToString();
                });
            }

        }

        public string ImageSourceString
        {
            get
            {
                return (string)GetValue(ImageSourceStringProperty);
            }
        set
            {
                SetValue(ImageSourceStringProperty, value);

            }
        }

        [TypeConverter(typeof(ImageSourceConverter))]
        public ImageSource ImageSource
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    headportrait.Source = value;
                });
            }
        }

        public int TotalHeight
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    frameLayout.WidthRequest = value;
                    frameLayout.HeightRequest = value;
                    frameLayout.CornerRadius = value;
                    headportrait.HeightRequest = value * 1.5;
                    headportrait.WidthRequest = value * 1.5;
                });
            }
        }

    }
}