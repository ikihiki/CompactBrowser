using System;
﻿using CompactBrowser.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CompactBrowser.Controls
{
    public sealed partial class BrowserControl : UserControl
    {

        public event Action<Uri> NewWindowReqested;
        public event Action NavigationCompleted;
        public event Action NavigationStarting;

        public BrowserControl()
        {
            this.InitializeComponent();
            this.webView.RegisterPropertyChangedCallback(WebView.DocumentTitleProperty,
                (obj, property) =>
                {
                    var webView = (WebView)obj;
                    ((BrowserViewModel)webView.DataContext).ChangeTitle(webView.DocumentTitle);
                });

            this.webView.RegisterPropertyChangedCallback(WebView.CanGoBackProperty,
                (obj, property) =>
                {
                    var webView = (WebView)obj;
                    ((BrowserViewModel)webView.DataContext).ChangeCanGoBack(webView.CanGoBack);
                });

            this.webView.RegisterPropertyChangedCallback(WebView.CanGoForwardProperty,
                (obj, property) =>
                {
                    var webView = (WebView)obj;
                    ((BrowserViewModel)webView.DataContext).ChangeCanGoForward(webView.CanGoForward);
                });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var context = (BrowserViewModel)this.DataContext;
            context.GoBack.Subscribe(() => this.webView.GoBack());
            context.GoForward.Subscribe(() => this.webView.GoForward());
            context.Refresh.Subscribe(() => this.webView.Refresh());
            context.Navigate.Subscribe(text => 
            {
                try
                {
                    this.webView.Navigate(new Uri(text));
                }catch (UriFormatException)
                {
                    this.webView.Navigate(new Uri("http://www.google.co.jp/"));
                }
            });

            context.Uri.Subscribe(uri => { if (this.webView.Source != uri) { this.webView.Source = uri; } });
            this.webView.Source = context.Uri.Value;

            this.webView.RegisterPropertyChangedCallback(WebView.SourceProperty,
                (obj, property) =>
                {
                    var webView = (WebView)obj;
                    ((BrowserViewModel)webView.DataContext).Uri.Value = webView.Source;
                });

        }

        private void webView_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            NewWindowReqested?.Invoke(args.Uri);
            args.Handled = true;
        }

        private void webView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            var context = (BrowserViewModel)this.DataContext;
            context.IsBusy.Value = false;
            NavigationCompleted?.Invoke();
        }

        private void webView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            var context = (BrowserViewModel)this.DataContext;
            context.IsBusy.Value = true;
            context.Uri.Value = args.Uri;

            NavigationStarting?.Invoke();
        }
    }
}
