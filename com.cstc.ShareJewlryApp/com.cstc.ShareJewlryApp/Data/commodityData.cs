using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.cstc.ShareJewlryApp.Data
{
    /// <summary>
    /// 商品详情类
    /// </summary>
    public class commodityData : BindableObject
    {
        /// <summary>
        /// 订单明细GUID
        /// </summary>
        public string OredrDetailedGUID { get; set; }

        /// <summary>
        /// 珠宝GUID
        /// </summary>
        public string JewelleryGUID { get; set; }

        /// <summary>
        /// 珠宝名称
        /// </summary>
        string _JewelleryName = "";
        public string JewelleryName
        {
            get
            {
                return _JewelleryName;
            }
            set
            {
                if (value.Length > 50)
                {
                    _JewelleryName = value.Substring(0, 19) + "..";
                }
                else
                {
                    _JewelleryName = value;
                }

            }
        }

        /// <summary>
        /// 珠宝款式
        /// </summary>
        public string JewelleryType { get; set; } = "";

        /// <summary>
        /// 珠宝所属商铺名
        /// </summary>
        public string ShopName { get; set; } = "";

        /// <summary>
        /// 店铺GUID
        /// </summary>
        public string ShopGUID { get; set; } = "";

        /// <summary>
        /// 店铺LOGO
        /// </summary>
        public string ShopLOGO { get; set; } = "";

        public string ShopLOGOForImg
        {
            get
            {
                return Helpers.MConfig.picUrl + "Pic/shangpinLOGO/" + ShopLOGO.Substring(0, 2).ToUpper() + "/" + ShopLOGO;
            }
        }

        /// <summary>
        /// 好评率
        /// </summary>
        public string Praise { get; set; } = "";


        public string PraiseForShow
        {
            get
            {
                return Praise + "%";
            }
        }

        /// <summary>
        /// 右边商品好评率
        /// </summary>
        public string rightPraise { get; set; }

        public string rightPraiseForShow
        {
            get
            {
                return rightPraise + "%";
            }
        }

        /// <summary>
        /// 购物车GUID
        /// </summary>
        public string ShoppingCartGUID { get; set; }

        /// <summary>
        /// 是否可租借
        /// </summary>
        public bool AllowRent { get; set; } = false;

        bool _IsAfterSale = true;
        public bool IsAfterSale
        {
            get
            {
                return _IsAfterSale;
            }
            set
            {
                _IsAfterSale = value;
                OnPropertyChanged("IsAfterSale");
                OnPropertyChanged("IsAfterSaleForShow");
            }
        }

        /// <summary>
        /// 商品简介
        /// </summary>
        public string Describe { get; set; } = "";

        public string IsAfterSaleForShow
        {
            get
            {
                if (IsAfterSale)
                    return "是";
                return "否";
            }
        }


        /// <summary>
        /// 数量
        /// </summary>
        int _number = 0;
        public int number
        {
            get
            {
                return _number;
            }

            set
            {
                _number = value;
                OnPropertyChanged("number");
                OnPropertyChanged("maxNumber");
                OnPropertyChanged("totalPriceForShow");
            }
        }

        bool tempload = false;
        int _maxNumber = 0;
        public int maxNumber
        {
            get
            {
                return _maxNumber;
            }

            set
            {
                if (tempload)   
                    return;
                tempload = true;
                _maxNumber = number;

            }
        }


        /// <summary>
        /// 数量   显示用
        /// </summary>
        public string numberForOrder
        {
            get
            {
                return "数量： " + number.ToString();
            }
        }

        /// <summary>
        /// 订单页面 数量显示用
        /// </summary>
        public string numForMyOrderPage
        {
            get
            {
                return "x" + _number.ToString();
            }
        }

        /// <summary>
        /// 首页 数量   显示用
        /// </summary>
        public string NumForShow
        {
            get
            {
                return _number.ToString() + "人已下单";
            }
        }


        /// <summary>
        /// 珠宝价格
        /// </summary>
        decimal _Price = 0;
        public decimal Price
        {
            get
            {
                return _Price;
            }

            set
            {
                _Price = value;

            }
        }

        
        public string PriceForShow
        {
            get
            {
                return "¥ " + Price.ToString();
            }
        }

        public decimal totalPrice
        {
            get
            {
                return Price * number;
            }
        }

        public string totalPriceForShow
        {

            get
            {
                return "¥" + (Price * number).ToString();
            }
        }


        /// <summary>
        /// 右边珠宝GUID
        /// </summary>
        public string rightJewelleryGUID
        {
            get; set;
        }



        /// <summary>
        /// 右边珠宝名称
        /// </summary>

        public string rightJewelleryName { get; set; }

        /// <summary>
        /// 右边珠宝销售数量
        /// </summary>
        public int rightnumber { get; set; } = 0;

        public string rightnumberForShow
        {
            get
            {
                return rightnumber.ToString() + "人已下单";
            }
        }



        /// <summary>
        /// 右边珠宝价格
        /// </summary>
        public decimal rightPrice { get; set; }

        public string rightPriceForShow
        {
            get
            {
                return "¥" + rightPrice.ToString();
            }
        }

        public bool rightIsShow { get; set; } = false;

        public double commodityHeight { get; set; } = 0;

        public double commodityWith { get; set; } = 0;

        /// <summary>
        /// 商品主规格名称
        /// </summary>
        public string FirstSpecName { get; set; }

        /// <summary>
        /// 主规格明细名称
        /// </summary>
        public string FirstSpecDetailedName { get; set; }


        /// <summary>
        /// 副规格名称
        /// </summary>
        public string SecondSpecName { get; set; }

        /// <summary>
        /// 副规格明细名称
        /// </summary>
        public string SecondSpecDetailedName { get; set; }

        /// <summary>
        /// 副规格明细GUID
        /// </summary>
        public string SecondSpecGUID { get; set; }


        public string specForShow
        {
            get
            {
                return FirstSpecName + "： " + FirstSpecDetailedName + "  " + SecondSpecName + "： " + SecondSpecDetailedName;
            }
        }
        /// <summary>
        /// 商品规格明细
        /// </summary>
        ObservableCollection<FirstSpecDetailData> _FirstSpecDetail = null;
        public ObservableCollection<FirstSpecDetailData> FirstSpecDetail
        {
            get
            {
                if (_FirstSpecDetail == null)
                    _FirstSpecDetail = new ObservableCollection<FirstSpecDetailData>();
                return _FirstSpecDetail;
            }
            set
            {
                _FirstSpecDetail = value;
                OnPropertyChanged("FirstSpecDetail");
            }
        }

        /// <summary>
        /// 商品参数
        /// </summary>
        ObservableCollection<CommodityParaData> _Parameter = null;
        public ObservableCollection<CommodityParaData> Parameter
        {
            get
            {
                if (_Parameter == null)
                    _Parameter = new ObservableCollection<CommodityParaData>();
                return _Parameter;
            }
            set
            {
                _Parameter = value;
            }
        }

        /// <summary>
        /// 商品详情
        /// </summary>
        public string JewelleryDetail { get; set; } = "";

        /// <summary>
        /// 规格呈现
        /// </summary>
        public string SpecForShow
        {
            get
            {
                string s = "";
                s = FirstSpecName + "：" + FirstSpecDetailedName + "   " + SecondSpecName + "：" + SecondSpecDetailedName;
                return s;
            }
        }


        
        bool _isManage = false;
        public bool isManage
        {
            get
            {
                return _isManage;
            }
            set
            {
                _isManage = value;
                isShow = !_isManage;
                OnPropertyChanged("isManage");
            }
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        bool _isShow = true;
        public bool isShow
        {
            get
            {
                return _isShow;
            }
            set
            {
                _isShow = value;
                OnPropertyChanged("isShow");
            }
        }

        /// <summary>
        /// 首饰盒GUID
        /// </summary>
        public string CollectionFolderGUID { get; set; } = "";

        /// <summary>
        /// 总库存
        /// </summary>
        public int totalStock
        {
            get
            {

                int 库存 = 0;
                foreach (var 主规格 in FirstSpecDetail)
                {
                    foreach (var 副规格 in 主规格.SecondSpecDetail)
                    {
                        库存 = 库存 + 副规格.Stock;
                    }
                }

                return 库存;
            }
        }


        /// <summary>
        /// 是否选择
        /// </summary>
        bool _isSelected = false;
        public bool isSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                if (value)
                {
                    selectImg = "chosed.png";
                }
                else
                {
                    selectImg = "unchose.png";
                }
            }
        }

        ImageSource _selectImg = "unchose.png";
        public ImageSource selectImg
        {
            get
            {
                return _selectImg;
            }
            set
            {
                _selectImg = value;
                OnPropertyChanged("selectImg");
            }
        }

        /// <summary>
        /// 图片名
        /// </summary>
        public string JewelleryPicName { get; set; } = "";
        public string WhetherBeCollected { get; set; }

        /// <summary>
        /// 图片路径（显示图片用）
        /// </summary>
        public string JewelleryPicNameForImg
        {
            get
            {
                return Helpers.MConfig.picUrl+ "Pic/shangpintupian/" + JewelleryPicName.Substring(0, 2).ToUpper() + "/" + JewelleryPicName;
            }
        }

        /// <summary>
        /// 图片名
        /// </summary>
        public string RightJewelleryPicName { get; set; } = "";


        /// <summary>
        /// 右边图片路径（显示图片用）
        /// </summary>
        public string RightJewelleryPicNameForImg
        {
            get
            {
                if(RightJewelleryPicName=="")
                {
                    return "";
                }
                else
                {
                    return Helpers.MConfig.picUrl + "Pic/shangpintupian/" + RightJewelleryPicName.Substring(0, 2).ToUpper() + "/" + RightJewelleryPicName;
                }
               
            }
        }


        /// <summary>
        /// 评价星级
        /// </summary>
        int _Stars = 0;
        public int Stars
        {
            get
            {
                return _Stars;
            }
            set
            {
                _Stars = value;
                OnPropertyChanged("Stars");
                OnPropertyChanged("evaluationDes");
                OnPropertyChanged("one_star");
                OnPropertyChanged("two_star");
                OnPropertyChanged("three_star");
                OnPropertyChanged("four_star");
                OnPropertyChanged("five_star");
            }
        }


        /// <summary>
        /// 评价描述
        /// </summary>
        public string evaluationDes
        {
            get
            {
                if (Stars == 0)
                {
                    return "";
                }
                else if (Stars == 1)
                {
                    return "什么鬼，这个东西也太次了";
                }
                else if (Stars == 2)
                {
                    return "比想象中的差了一些";
                }
                else if (Stars == 3)
                {
                    return "一般般吧，没什么特别的";
                }
                else if (Stars == 4)
                {
                    return "我喜欢这个东西，近乎完美";
                }
                else if (Stars == 5)
                {
                    return "太棒了，此物仅为天上有，人间难得几回闻";
                }

                return "";
            }
        }


        /// <summary>
        /// 评语
        /// </summary>
        public string Content { get; set; } = "";

        #region 评价五颗星
        public string one_star
        {
            get
            {
                if (Stars > 0)
                {
                    return "star_shinning.png";
                }
                else
                {
                    return "star.png";
                }
            }
        }

        public string two_star
        {
            get
            {
                if (Stars > 1)
                {
                    return "star_shinning.png";
                }
                else
                {
                    return "star.png";
                }
            }
        }

        public string three_star
        {
            get
            {
                if (Stars > 2)
                {
                    return "star_shinning.png";
                }
                else
                {
                    return "star.png";
                }
            }
        }

        public string four_star
        {
            get
            {
                if (Stars > 3)
                {
                    return "star_shinning.png";
                }
                else
                {
                    return "star.png";
                }
            }
        }

        public string five_star
        {
            get
            {
                if (Stars == 5)
                {
                    return "star_shinning.png";
                }
                else
                {
                    return "star.png";
                }
            }
        }
        #endregion

        /// <summary>
        /// 物品上下架
        /// </summary>
        public bool JewelleryState { get; set; } = true;

        public string JewelleryStateForShow
        {
            get
            {
                string re = "免费戴";
                if (!JewelleryState) re = "已下架";
                return re;

            }
        }

        /// <summary>
        /// 清洁费
        /// </summary>
        public decimal CleaningFee { get; set; }
    }
    /// <summary>
    /// 首页款式类
    /// </summary>
    public class CommodityStype : BindableObject
    {
        public string TypeGUID { get; set; } //款式GUID
        public string TypeName { get; set; }// 款式名称
        public string TypeIDX { get; set; }// 款式顺序

        //   Color _BackgroundColor = Color.FromHex("#f0f0f0");
        Color _BackgroundColor = Color.White;
        public Color BackgroundColor
        {
            get
            {
                return _BackgroundColor;
            }
            set
            {
                _BackgroundColor = value;
                OnPropertyChanged();
            }
        }

        public string TypePic { get; set; }   //图片GUID

        public string photoForShow
        {
            get
            {
                if (TypePic != "")
                    return Helpers.MConfig.weixinpicUrl + TypePic.Substring(0, 1) + "/" + TypePic; // + ".png";

                return "";
            }
        }

        /// <summary>
        /// 选项 选定的颜色
        /// </summary>
        Color _textColor = Color.Transparent;
        public Color textColor
        {
            get
            {
                return _textColor;
            }
            set
            {
                _textColor = value;
                OnPropertyChanged();
            }
        }

    }
    /// <summary>
    /// 材质类
    /// </summary>
    public class CommodityMaterial : BindableObject
    {
        public string MaterialGUID { get; set; } //款式GUID
        public string MaterialName { get; set; }// 款式名称
                                                // public string TypeIDX { get; set; }// 款式顺序

        //  Color _BackgroundColor = Color.FromHex("#f0f0f0");
        Color _BackgroundColor = Color.White;
        public Color BackgroundColor
        {
            get
            {
                return _BackgroundColor;
            }
            set
            {
                _BackgroundColor = value;
                OnPropertyChanged();
            }
        }
    }
    /// <summary>
    /// 商品参数类
    /// </summary>
    public class CommodityParaData
    {
        public string ParameterGUID { get; set; }
        public string JewelleryGUID { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValues { get; set; }
    }
    public class CommodityMgr:BasePage
    {
        /// <summary>
        /// 打开商品明细
        /// </summary>
        /// <param name="jewelleryGUID"></param>
        public static void ShowProductDetail(INavigation navigation, string jewelleryGUID)
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


        /// <summary>
        /// 获取商品列表数据并填充成两列一行
        /// </summary>
        /// <param name="am"></param>
        /// <param name="PageNumber"></param>
        /// <param name="TypeGUID"></param>
        /// <param name="MaterialGUID"></param>
        /// <param name="JewelleryName"></param>
        /// <param name="SortColumn"></param>
        /// <param name="Sort"></param>
        /// <param name="ShopGUID"></param>
        /// <param name="commodityHeight">商品行高</param>
        public static void GetCommodityDataWithTwoColumn(Helpers.AsyncMsg am, int PageNumber, string TypeGUID,string MaterialGUID
            ,string JewelleryName,string SortColumn,string Sort,string ShopGUID, double commodityHeight)
        {

            Helpers.AsyncMsg am_获取商品 = new Helpers.AsyncMsg();
            am_获取商品.Completion += (object obj, string ex) =>
            {
                try
                {
                    List<Data.commodityData> tempList = (List<Data.commodityData>)obj;
                    List<Data.commodityData> newList = new List<Data.commodityData>();
                    //将获取的数据两行合并为一行
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        //添加左边列表的数据
                        if (i % 2 == 0)
                        {
                            tempList[i].commodityHeight = commodityHeight;
                            //tempList[i].commodityWith = 图片宽度;
                            newList.Add(tempList[i]);
                        }
                        else
                        {
                            // 添加右边列表数据  
                            if (!string.IsNullOrEmpty(tempList[i].JewelleryName.ToString()))
                            {
                                newList[i / 2].rightJewelleryGUID = tempList[i].JewelleryGUID;
                                newList[i / 2].rightJewelleryName = tempList[i].JewelleryName;
                                newList[i / 2].RightJewelleryPicName = tempList[i].JewelleryPicName;
                                newList[i / 2].rightnumber = tempList[i].number;
                                newList[i / 2].rightPrice = tempList[i].Price;
                                newList[i / 2].rightIsShow = true;
                                newList[i / 2].rightPraise = tempList[i].Praise;
                            }
                            else
                            {
                                //如果最后右边没有数据了，不显示右边
                                newList[i / 2].rightIsShow = false;
                            }
                        }
                    }

                    am.OnCompletion(newList, "");
                }
                catch (Exception)
                {
                    am.OnCancel();
                }
            };

            GetCommodityData(am_获取商品, PageNumber,TypeGUID,MaterialGUID,JewelleryName,SortColumn,Sort,ShopGUID);
        }


        /// <summary>
        /// 获取商品列表数据
        /// </summary>
        /// <param name="am"></param>
        /// <param name="PageNumber"></param>
        /// <param name="TypeGUID"></param>
        /// <param name="MaterialGUID"></param>
        /// <param name="JewelleryName"></param>
        /// <param name="SortColumn"></param>
        /// <param name="Sort"></param>
        /// <param name="ShopGUID"></param>
        public static void GetCommodityData(Helpers.AsyncMsg am_return, int PageNumber, string TypeGUID, string MaterialGUID
            , string JewelleryName, string SortColumn, string Sort, string ShopGUID)
        {            
            if (Sort == "") Sort = "Asc";
            if (SortColumn == "") SortColumn = "JewelleryGUID";

            string para = "&TypeGUID=" + TypeGUID + "&PageNumber=" + PageNumber
            + "&MaterialGUID=" + MaterialGUID + "&ShopGUID=" + ShopGUID
            + "&JewelleryName=" + JewelleryName
            + "&SortColumn=" + SortColumn + "&Sort=" + Sort;
            Helpers.HttpHelper.GetDataItemList<Data.commodityData>(Helpers.AppApi.GetCommodityList, para, am_return);
        }

        public static void GetThroughTrainJewellery(Helpers.AsyncMsg am_return, string Sign, string SortColumn, string Sort)
        {
            if (Sort == "") Sort = "Asc";
            if (SortColumn == "") SortColumn = "JewelleryGUID";

            string para = "&Sign=" + Sign
            + "&Column1=" + SortColumn + "&Sort=" + Sort;
            Helpers.HttpHelper.GetDataItemList<Data.commodityData>(Helpers.AppApi.GetThroughTrainJewellery, para, am_return);

        }

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="am"></param>
        public static void GetCommodityDetail(Helpers.AsyncMsg am_return, string JewelleryGUID)
        {
            string para = "&JewelleryGUID=" + JewelleryGUID + "&UserGUID=" + Data.UserInfoCache.UserGUID;
            Helpers.HttpHelper.GetDataItemList<Data.commodityData>(Helpers.AppApi.GetCommodityDetail, para, am_return);
        }

        /// <summary>
        /// 获取商品款式
        /// </summary>
        /// <param name="am"></param>
        public static void GetJewelleryType(Helpers.AsyncMsg am_return)
        {
            Helpers.HttpHelper.GetDataItemList<Data.CommodityStype>(Helpers.AppApi.GetJewelleryType, "", am_return);
        }
        /// <summary>
        /// 获取商品材质
        /// </summary>
        /// <param name="am_return"></param>
        public static void GetJewelleryMaterial(Helpers.AsyncMsg am_return)
        {
            Helpers.HttpHelper.GetDataItemList<Data.CommodityStype>(Helpers.AppApi.GetJewelleryMaterial, "", am_return);
        }



    }
}
