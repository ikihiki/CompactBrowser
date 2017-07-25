using CompactBrowser.ViewModels;
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

        public MainPageViewModel ViewModel
        {
            get
            {
                return (MainPageViewModel)this.DataContext;
            }
        }

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
                    this.Pivot.Margin = new Thickness(0, 32, 0, 0);
                }
                else
                {
                    this.BottomAppBar.Visibility = Visibility.Collapsed;
                    this.Pivot.Margin = new Thickness();
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
                    Pivot.HeaderVisibility = Visibility.Collapsed;
                    BottomAppBar.Visibility = Visibility.Visible;
                    viewMode = ApplicationViewMode.CompactOverlay;
                    this.Pivot.Margin = new Thickness(0, 32, 0, 0);
                }

            }
            else
            {
                if (await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default))
                {
                    Pivot.HeaderVisibility = Visibility.Visible;
                    BottomAppBar.Visibility = Visibility.Collapsed;
                    viewMode = ApplicationViewMode.Default;
                    this.Pivot.Margin = new Thickness();
                }

            }


        }


        private void TopOther_Click(object sender, RoutedEventArgs e)
        {
            Pivot.IsPaneOpen = true;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Browsers.Count > 1)
            {
                ((ViewModels.MainPageViewModel)this.DataContext).RemoveBrowser.Execute((BrowserViewModel)((Button)sender).DataContext);
            }
            else
            {
                Application.Current.Exit();
            }
        }

        private void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (Uri.TryCreate(((TextBox)sender).Text, UriKind.Absolute, out var uri))
                {
                    ViewModel.CurrentBrowser.Value.Uri.Value = uri;
                }
            }
        }
    }
}
