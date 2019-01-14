
using System.Collections.Generic;
using System.Linq;

using Android.Content;

using Android.Views;
using com.cstc.ShareJewlryApp.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using com.cstc.ShareJewlryApp.Views;
using BottomNavigationBar;
using BottomNavigationBar.Listeners;

[assembly: ExportRenderer(typeof(MainPage), typeof(MainPageRenderer))]
namespace com.cstc.ShareJewlryApp.Droid.Renderer
{
    public class MainPageRenderer : VisualElementRenderer<MainPage>, IOnTabClickListener
    {
        private BottomBar _bottomBar;

        private Page _currentPage;

        private int _lastSelectedTabIndex = -1;

        public MainPageRenderer()
        {
            //要求包装器不要自动添加子页面
            AutoPackage = false;
        }

        /// <summary>
        /// 选中后,加载新的页面内容
        /// </summary>
        /// <param name="position"></param>
        public void OnTabSelected(int position)
        {
            LoadPageContent(position);
        }

        public void OnTabReSelected(int position)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<MainPage> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                ClearElement(e.OldElement);
            }

            if (e.NewElement != null)
            {
                InitializeElement(e.NewElement);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearElement(Element);
            }

            base.Dispose(disposing);
        }
        /// <summary>
        /// 重写布局的方法
        /// </summary>
        /// <param name="changed"></param>
        /// <param name="l"></param>
        /// <param name="t"></param>
        /// <param name="r"></param>
        /// <param name="b"></param>
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            if (Element == null)
            {
                return;
            }

            int width = r - l;
            int height = b - t;

            _bottomBar.Measure(
                MeasureSpec.MakeMeasureSpec(width, MeasureSpecMode.Exactly),
                MeasureSpec.MakeMeasureSpec(height, MeasureSpecMode.AtMost));

            //这里需要重新测量位置和尺寸,为了重新布置tab菜单的位置 
            _bottomBar.Measure(
                MeasureSpec.MakeMeasureSpec(width, MeasureSpecMode.Exactly),
                MeasureSpec.MakeMeasureSpec(_bottomBar.ItemContainer.MeasuredHeight, MeasureSpecMode.Exactly));

            int barHeight = _bottomBar.ItemContainer.MeasuredHeight;

            _bottomBar.Layout(0, b - barHeight, width, b);

            float density = Resources.DisplayMetrics.Density;

            double contentWidthConstraint = width / density;
            double contentHeightConstraint = (height - barHeight) / density;

            if (_currentPage != null)
            {
                var renderer = Platform.GetRenderer(_currentPage);

                renderer.Element.Measure(contentWidthConstraint, contentHeightConstraint);
                renderer.Element.Layout(new Rectangle(0, 0, contentWidthConstraint, contentHeightConstraint));

                renderer.UpdateLayout();
            }
        }
        /// <summary>
        /// 初始化方法
        /// </summary>
        /// <param name="element"></param>
        private void InitializeElement(MainPage element)
        {
            PopulateChildren(element);
        }
        /// <summary>
        /// 生成新的底部控件
        /// </summary>
        /// <param name="element"></param>
        private void PopulateChildren(MainPage element)
        {
            //我们需要删除原有的底部控件,然后添加新的
            _bottomBar?.RemoveFromParent();

            _bottomBar = CreateBottomBar(element);
            AddView(_bottomBar);

            LoadPageContent(0);
        }
        /// <summary>
        /// 清除旧的底部控件
        /// </summary>
        /// <param name="element"></param>
        private void ClearElement(MainPage element)
        {
            if (_currentPage != null)
            {
                IVisualElementRenderer renderer = Platform.GetRenderer(_currentPage);

                if (renderer != null)
                {
                    renderer.ViewGroup.RemoveFromParent();
                    renderer.ViewGroup.Dispose();
                    renderer.Dispose();

                    _currentPage = null;
                }

                if (_bottomBar != null)
                {
                    _bottomBar.RemoveFromParent();
                    _bottomBar.Dispose();
                    _bottomBar = null;
                }
            }
        }
        /// <summary>
        /// 创建新的底部控件
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private BottomBar CreateBottomBar(MainPage element)
        {
            var bar = new BottomBar(Context);

            //根据您的需要配置底部的bar

            bar.SetOnTabClickListener(this);
            bar.UseFixedMode();

            PopulateBottomBarItems(bar, element.Children);

            bar.ItemContainer.SetBackgroundColor(element.BarBackgroundColor.ToAndroid());
            //设置选中按钮的背景色
            bar.SetActiveTabColor(element.BarTextColor.ToAndroid());

            //设置bar的背景色
            bar.ItemContainer.SetBackgroundColor(element.BarBackgroundColor.ToAndroid());

            return bar;
        }
        /// <summary>
        /// 查询原来底部的菜单,并添加到新的控件
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="pages"></param>
        private void PopulateBottomBarItems(BottomBar bar, IEnumerable<Page> pages)
        {
            var barItems = pages.Select(x => new BottomBarTab(Context.Resources.GetDrawable(x.Icon), x.Title));
            bar.SetItems(barItems.ToArray());
        }
        /// <summary>
        /// 通过选择的下标加载Page
        /// </summary>
        /// <param name="position"></param>
        private void LoadPageContent(int position)
        {
            if (position != _lastSelectedTabIndex)
            {
                Element.CurrentPage = Element.Children[position];

                if (Element.CurrentPage != null)
                {
                    LoadPageContent(Element.CurrentPage);
                }
            }

            _lastSelectedTabIndex = position;
        }

        /// <summary>
        /// 加载方法页面
        /// </summary>
        /// <param name="page"></param>
        private void LoadPageContent(Page page)
        {

            //释放旧的page
            if (_currentPage != null)
            {
                // var basePage = _currentPage as Basesrc:BasePage;
                //原文为Basesrc:BasePage，报错未找到Basesrc:BasePage类型，改为IPageController。
                var basePage = _currentPage as IPageController;
                basePage?.SendDisappearing();
                var renderer = Platform.GetRenderer(_currentPage);
                if (renderer != null)
                {
                    renderer.View.Visibility = ViewStates.Invisible;
                    RemoveView(renderer.View);
                }

            }
            //当前页面设置为新页面
            _currentPage = page;


            //加载新的page
            var newrenderer = Platform.GetRenderer(_currentPage);
            if (newrenderer == null)
            {
                newrenderer = Platform.CreateRenderer(_currentPage);
                Platform.SetRenderer(_currentPage, newrenderer);
            }
            else
            {
                // var basePage = _currentPage as Basesrc:BasePage;
                //原文为Basesrc:BasePage，报错未找到Basesrc:BasePage类型，改为IPageController。
                var basePage = _currentPage as IPageController;
                basePage?.SendAppearing();
            }

            AddView(newrenderer.ViewGroup);
            newrenderer.ViewGroup.Visibility = ViewStates.Visible;

            Element.CurrentPage = _currentPage;
        }
    }
}