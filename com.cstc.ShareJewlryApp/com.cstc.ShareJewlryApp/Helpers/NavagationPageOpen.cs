using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace com.cstc.ShareJewlryApp.Helpers
{
    public class NavagationPageMgr
    {
        public static void Open(INavigation navigation, Page page)
        {
            try
            {
                if (navigation.NavigationStack.Contains(page))
                    navigation.RemovePage(page);
                navigation.PushAsync(page, false);
            }
            catch(Exception ex)
            { }
        }

        /// <summary>
        /// 打开商品明细
        /// </summary>
        /// <param name="jewelleryGUID"></param>
        public static void ShowProductDetail(INavigation navigation ,string jewelleryGUID)
        {
 
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Views.HomePage.ProductDetails.ProductDetailsPage page = new Views.HomePage.ProductDetails.ProductDetailsPage();
                    page.JewelleryGUID = jewelleryGUID;
 
                    Helpers.NavagationPageMgr.Open(navigation, page);
 
                });
            }
            catch (Exception ex)
            {

            }
 
        }
    }
}
