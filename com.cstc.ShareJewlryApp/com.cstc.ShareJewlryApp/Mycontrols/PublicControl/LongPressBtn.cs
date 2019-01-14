using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace com.cstc.ShareJewlryApp.Mycontrols
{
    public class LongPressBtn : Button
    {
        public event EventHandler LongClicked;

        public void OnLongClicked(object sender, EventArgs e)
        {
            LongClicked(sender, e);
        }


        public void HandleLongPress(LongPressBtn view, EventArgs eventArgs)
        {
            LongClicked(view, eventArgs);
            //  throw new NotImplementedException();
        }
    }
}
