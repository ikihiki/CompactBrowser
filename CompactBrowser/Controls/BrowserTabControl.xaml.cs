﻿using System;
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

    public class BrowserTabBase : Pivot
    {
        public static DependencyProperty NavigationBarProperty { get; } =
            DependencyProperty.Register("NavigationBar", typeof(object), typeof(BrowserTabBase), null);

        public object NavigationBar
        {
            get => GetValue(NavigationBarProperty);
            set => SetValue(NavigationBarProperty, value);
        }

        public static DependencyProperty NavigationBarTemplateProperty { get; } =
            DependencyProperty.Register("NavigationBarTemplate", typeof(DataTemplate), typeof(BrowserTabBase), null);

        public DataTemplate NavigationBarTemplate
        {
            get => (DataTemplate)GetValue(NavigationBarTemplateProperty);
            set => SetValue(NavigationBarTemplateProperty, value);
        }

        public static DependencyProperty IsPaneOpenProperty { get; } =
            DependencyProperty.Register("IsPaneOpen", typeof(bool), typeof(BrowserTabBase), null);

        public bool IsPaneOpen
        {
            get => (bool)GetValue(IsPaneOpenProperty);
            set => SetValue(IsPaneOpenProperty, value);
        }

        public static DependencyProperty PaneProperty { get; } =
            DependencyProperty.Register("Pane", typeof(object), typeof(BrowserTabBase), null);

        public object Pane
        {
            get => GetValue(PaneProperty);
            set => SetValue(PaneProperty, value);
        }

        public static DependencyProperty HeaderVisibilityProperty { get; } =
            DependencyProperty.Register("HeaderVisibility", typeof(Visibility), typeof(BrowserTabBase), null);

        public Visibility HeaderVisibility
        {
            get => (Visibility)GetValue(HeaderVisibilityProperty);
            set => SetValue(HeaderVisibilityProperty, value);
        }
    }


    public sealed partial class BrowserTabControl : BrowserTabBase
    {



        public BrowserTabControl()
        {
            this.InitializeComponent();
        }
    }
}
