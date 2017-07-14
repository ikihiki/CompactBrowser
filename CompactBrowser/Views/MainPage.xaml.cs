using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace CompactBrowser.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : SessionStateAwarePage
    {
        ApplicationViewMode viewMode = ApplicationViewMode.Default;

        public MainPage()
        {
            this.InitializeComponent();

            var coreTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            var appTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;

            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
        }

        private void CoreTitleBar_IsVisibleChanged(Windows.ApplicationModel.Core.CoreApplicationViewTitleBar sender, object args)
        {
            if (viewMode == ApplicationViewMode.CompactOverlay)
            {
                if (sender.IsVisible)
                {
                    this.BottomAppBar.Visibility = Visibility.Visible;
                }
                else
                {
                    this.BottomAppBar.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void BrowserControl_NewWindowReqested(Uri obj)
        {
            ((ViewModels.MainPageViewModel)this.DataContext).AddBrowserWithUri.Execute(obj);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationView.GetForCurrentView().ViewMode == ApplicationViewMode.Default)
            {
                if (await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay))
                {
                    this.Pivot.Margin = new Thickness(0, -50, 0, 0);
                    TopAppBar.Visibility = Visibility.Collapsed;
                    BottomAppBar.Visibility = Visibility.Visible;
                    viewMode = ApplicationViewMode.CompactOverlay;
                }

            }
            else
            {
                if (await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default))
                {
                    this.Pivot.Margin = new Thickness(0, 0, 0, 0);
                    TopAppBar.Visibility = Visibility.Visible;
                    BottomAppBar.Visibility = Visibility.Collapsed;
                    viewMode = ApplicationViewMode.Default;
                }

            }


        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().Title = "PonterExtied";
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().Title = "PonterEntered";
        }

        private void TopOther_Click(object sender, RoutedEventArgs e)
        {
            Menu.IsPaneOpen = true;
        }
    }
}
