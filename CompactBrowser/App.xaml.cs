using Prism.Autofac.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Prism.Windows.Navigation;
using CompactBrowser.Services;
using System.Diagnostics;

namespace CompactBrowser
{
    /// <summary>
    /// 既定の Application クラスを補完するアプリケーション固有の動作を提供します。
    /// </summary>
    sealed partial class App : PrismAutofacApplication
    {

        private IFavoriteService favoriteService = new FavoriteService();
        private ISettingService settingService = new SettingsService();

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            RegisterInstance(favoriteService, typeof(IFavoriteService), registerAsSingleton: true);
            RegisterInstance(settingService, typeof(ISettingService), registerAsSingleton: true);
            return base.OnInitializeAsync(args);
        }


        protected override async Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                await favoriteService.LoadFavoriteAsync();
                await settingService.LoadSettingsAsync();
            }
            
            NavigationService.Navigate("Main", null);
        }

        protected override async Task OnSuspendingApplicationAsync()
        {
            await favoriteService.SaveFavoriteAsync();
            await settingService.SaveSettingsAsync();
        }
    }
}
