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
    public partial class TeamInfoViewCell : ContentView
    {
        public TeamInfoViewCell()
        {
            InitializeComponent();
        }
        //声明委托事件
        public delegate void myevent(object sender, string UserGUID);
        //命名委托
        public event myevent Clicked;
 
        /// <summary>
        /// 点击
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnClicked( string UserGUID )
        {
            if (Clicked != null)
                Clicked(this, UserGUID);
        }

        

        private void tapped_selectItem(object sender, EventArgs e)
        {
            Data.TeamData teamData = (Data.TeamData)(((StackLayout)sender).BindingContext);
            try
            {
                if (Convert.ToInt32(teamData.Team) > 0 && Convert.ToInt32(teamData.SonNum) > 0)
                {
                    OnClicked(teamData.GUID);
                }
            }
            catch (Exception ex) { }
        }
    }
}