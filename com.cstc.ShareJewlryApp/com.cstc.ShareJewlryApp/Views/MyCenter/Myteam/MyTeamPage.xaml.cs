using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.cstc.ShareJewlryApp.Views.MyCenter.Myteam
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyTeamPage: BasePage 
    {
        int 分页index = 0;
        string 排序 = "ASC";
        string 排序标志 = "CREATETIME";

        ObservableCollection<Data.TeamData> orderList = new ObservableCollection<Data.TeamData>();
        public MyTeamPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            grid_head.HeightRequest = 150 * Helpers.MConfig.screenRatios;
            获取团队信息();
            UI呈现();
        }

        void UI呈现()
        {
            if (Data.UserInfoCache.userInfo.Photo != "")
            {
                img_myheadImg.Source = Helpers.MConfig.picUrl + "Pic/yonghutouxiang/" + Data.UserInfoCache.userInfo.Photo.Substring(0, 2) + "/" + Data.UserInfoCache.userInfo.Photo;
 
            }
            else
            {
                img_myheadImg.Source = "unLogin_headImg.png";
            }

            if (Data.UserInfoCache.userInfo.RemainderDays > 0)
            {
 
                img_vip.Source = "vipstar.png";
            }
            else
            {

                img_vip.Source = "";
            }

            lb_nickName.Text = Data.UserInfoCache.userInfo.Nickname;
            lb_Reward.Text = Data.UserInfoCache.userInfo.Reward + " 元";
            lb_FatherNickName.Text = Data.UserInfoCache.userInfo.FatherNickName;
            lb_InvitationNum.Text = "已邀请 " + Data.UserInfoCache.userInfo.InvitationNumber + " 人";
            lb_TeamNum.Text = "团队 " + Data.UserInfoCache.userInfo.Team + " 人";
        }

        void 获取团队信息()
        {
            Tools.AsyncMsg am_获取团队信息 = new Tools.AsyncMsg();

            string para = "UserGUID=" + Data.UserInfoCache.UserGUID + "&Column1=" + 排序标志 + "&Sort=" + 排序 + "&PageNumber=" + 分页index;

            am_获取团队信息.Completion += (object obj, string ex) =>
            {
                string returnJson = obj.ToString();
                string ErrMsg = "";
                if (returnJson == "[]" || returnJson == "")
                    return;

                #region json转jarry
                try
                {
                    returnJson = Tools.CommonClass.GetJsonByTag(returnJson, ref ErrMsg);
                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "解析团队信息数据错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                if (returnJson == "")
                {
                    return;
                }

                Newtonsoft.Json.Linq.JArray jar = null;
                try
                {
                    jar = Newtonsoft.Json.Linq.JArray.Parse(returnJson);
                }
                catch (Exception exc)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await DisplayAlert("错误", "解析团队信息数据包错误！" + exc.Message, "知道了");
                        // Navigation.PopAsync(true);
                    });
                    return;
                }

                #endregion

                ObservableCollection<Data.TeamData> tempList = null;

                try
                {
                    tempList = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Data.TeamData>>(jar.ToString());
                }
                catch (Exception exc)
                {
                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                    //    DisplayAlert("提示", "转换团队信息数据包失败:" + ex.ToString(), "知道了");
                    //});
                }



                foreach (var temp in tempList)
                {
                    orderList.Add(temp);
                }

                if (分页index < 1)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ls_list.ItemsSource = orderList;
                    });
                }

                分页index++;

            };

            //am_获取团队信息.Cancel += (object obj, string ex) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        DisplayAlert("提示", "获取团队信息失败:" + ex.ToString(), "知道了");
            //    });
            //    return;
            //};

            Tools.NetClass.GetStringByUrl("QueryData", "APP\\GetExtensionTeamInfo_1_0_0_2", para, am_获取团队信息);

        }

        private void ls_list_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (orderList.Count == 0)
                return;

            var row = new Data.TeamData();

            try
            {
                row = (Data.TeamData)e.Item;
            }
            catch (Exception ex)
            {

            }

            if (row == orderList[orderList.Count - 1])
            {

                获取团队信息();
            }
        }


        /// <summary>
        /// 全部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_All(object sender, EventArgs e)
        {
            if (lb_all.TextColor == com.cstc.ShareJewlryApp.Style.Color.Color.grayFont)
            {
                lb_all.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                box_all.BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;

                lb_identity.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                box_identity.BackgroundColor = Color.Transparent;

                lb_time.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                box_time.BackgroundColor = Color.Transparent;

                lb_team.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                box_team.BackgroundColor = Color.Transparent;

                排序 = "ASC";
                排序标志 = "CREATETIME";
                分页index = 0;
                orderList.Clear();
                获取团队信息();
            }

        }

        /// <summary>
        /// 按时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_time(object sender, EventArgs e)
        {
            if (lb_time.TextColor == com.cstc.ShareJewlryApp.Style.Color.Color.grayFont)
            {
                lb_all.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                box_all.BackgroundColor = Color.Transparent;

                lb_identity.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                box_identity.BackgroundColor = Color.Transparent;

                lb_time.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                box_time.BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;

                lb_team.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                box_team.BackgroundColor = Color.Transparent;

                排序 = "DESC";
                排序标志 = "CREATETIME";
                分页index = 0;
                orderList.Clear();
                获取团队信息();
            }
        }

        /// <summary>
        /// 按身份
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_identity(object sender, EventArgs e)
        {
            if (lb_identity.TextColor == com.cstc.ShareJewlryApp.Style.Color.Color.grayFont)
            {
                lb_all.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                box_all.BackgroundColor = Color.Transparent;

                lb_identity.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                box_identity.BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;

                lb_time.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                box_time.BackgroundColor = Color.Transparent;

                lb_team.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                box_team.BackgroundColor = Color.Transparent;

                排序 = "DESC";
                排序标志 = "Level";
                分页index = 0;
                orderList.Clear();
                获取团队信息();
            }
        }

        /// <summary>
        /// 按团队
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tapped_team(object sender, EventArgs e)
        {

            if (lb_team.TextColor == com.cstc.ShareJewlryApp.Style.Color.Color.grayFont)
            {
                lb_all.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                box_all.BackgroundColor = Color.Transparent;

                lb_identity.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                box_identity.BackgroundColor = Color.Transparent;

                lb_time.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.grayFont;
                box_time.BackgroundColor = Color.Transparent;

                lb_team.TextColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;
                box_team.BackgroundColor = com.cstc.ShareJewlryApp.Style.Color.Color.redFont;

                排序 = "DESC";
                排序标志 = "Team";
                分页index = 0;
                orderList.Clear();
                获取团队信息();
            }
        }
 
  
        private void TeamInfoViewCell_Clicked(object sender, string UserGUID)
        {         
            Views.MyCenter.Myteam.TeamSecPage page = new TeamSecPage();
            page.用户GUID = UserGUID;
            page.获取团队信息();
            Navigation.PushAsync(page, true);
        }
    }
}