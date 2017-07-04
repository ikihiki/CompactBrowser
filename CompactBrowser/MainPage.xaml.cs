using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace CompactBrowser
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private bool pointer;

        public MainPage()
        {
            this.InitializeComponent();

        }

        private void HamburgerMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            var context = (BrowserListViewModel)this.DataContext;

            context.CurrentBrowser.Value.Navigate.Execute(((FavoriteViewModel)e.ClickedItem).Uri.ToString());
        }

        private void BrowserControl_NewWindowReqested(Uri obj)
        {
            var context = (BrowserListViewModel)this.DataContext;

            context.AddBrowserWithUri.Execute(obj);
        }

        private void BrowserControl_NavigationCompleted()
        {
            var context = (BrowserListViewModel)this.DataContext;

            context.FavoriteJudge();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationView.GetForCurrentView().ViewMode == ApplicationViewMode.Default)
            {
                HamburgerMenu.Margin = new Thickness(-HamburgerMenu.CompactPaneLength, -50, 0, 0);
                this.Margin = new Thickness(0, 0, 0, -15);
                this.Pivot.Margin = new Thickness(0, 0, 0, 15);
                this.AppBar.Margin = new Thickness(0, 10, 0, 0);
                await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay);
            }
            else
            {
                HamburgerMenu.Margin = new Thickness(0, 0, 0, 0);
                this.Margin = new Thickness(0, 0, 0, 0);
                this.Pivot.Margin = new Thickness(0, 0, 0, 25);
                this.AppBar.Margin = new Thickness(0, 0, 0, 0);
                await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default);

            }
        }

        private void Page_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ApplicationView.GetForCurrentView().ViewMode == ApplicationViewMode.CompactOverlay && pointer == false)
            {
                this.AppBar.Visibility = Visibility.Collapsed;
                this.Pivot.Margin = new Thickness(0, 0, 0, 15);
            }

        }

        private void Page_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ApplicationView.GetForCurrentView().ViewMode == ApplicationViewMode.CompactOverlay)
            {
                this.AppBar.Visibility = Visibility.Visible;
                this.Pivot.Margin = new Thickness(0, 0, 0, 25);
            }

        }

        private void Page_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            pointer = false;
        }

        private void Page_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            pointer = true;
        }

        private void Pivot_PivotItemLoaded(Pivot sender, PivotItemEventArgs args)
        {

        }
    }
}
