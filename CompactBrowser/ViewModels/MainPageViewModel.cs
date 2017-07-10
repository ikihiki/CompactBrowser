using CompactBrowser.Models;
using CompactBrowser.Services;
using Newtonsoft.Json;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Reactive.Linq;
using Windows.UI.Xaml;

namespace CompactBrowser.ViewModels
{
    public class MainPageViewModel
    {
        public ReactiveCollection<BrowserViewModel> Browsers { get; } = new ReactiveCollection<BrowserViewModel>();
        public ReactiveProperty<BrowserViewModel> CurrentBrowser { get; } = new ReactiveProperty<BrowserViewModel>();

        private IFavoriteService favoriteService;
        public ReadOnlyReactiveCollection<Favorite> Favorites { get; } 
        public ReactiveCommand Favorite { get; }

        private ReactiveProperty<bool> canRemoveBrowser = new ReactiveProperty<bool>(false);
        public ReactiveCommand<BrowserViewModel> RemoveBrowser { get; }

        private ReactiveProperty<bool> alwaysTrue = new ReactiveProperty<bool>(true);
        public ReactiveCommand AddBrowser { get; }
        public ReactiveCommand<Uri> AddBrowserWithUri { get; }

        public MainPageViewModel(IFavoriteService favoriteService)
        {
            this.favoriteService = favoriteService;
            Favorites = this.favoriteService.GetFavorite();



            Browsers.Add(new BrowserViewModel());
            CurrentBrowser.Value = Browsers.Last();
            
            
            Favorite = Favorites
                .ToCollectionChanged()
                .Select(f=>f.Value.Uri)
                .Merge(CurrentBrowser.Select(b=>b.Uri.Value))
                .Select(u => favoriteService.ExsistFavorite(CurrentBrowser.Value.Uri.Value))
                .ToReactiveCommand();



            Favorite.Subscribe(() =>
            {
                favoriteService.AddFavorite(new Models.Favorite {Uri=CurrentBrowser.Value.Uri.Value, Title = CurrentBrowser.Value.Title.Value});
            });

            RemoveBrowser = canRemoveBrowser.ToReactiveCommand<BrowserViewModel>();
            RemoveBrowser.Subscribe(vm =>
            {
                Browsers.Remove(vm);
                canRemoveBrowser.Value = Browsers.Count > 1;
            });

            

            AddBrowser = alwaysTrue.ToReactiveCommand();
            AddBrowser.Subscribe(() =>
            {
                Browsers.Add(new BrowserViewModel());
                canRemoveBrowser.Value = Browsers.Count > 1;
                CurrentBrowser.Value = Browsers.Last();
            });

            AddBrowserWithUri = alwaysTrue.ToReactiveCommand<Uri>();
            AddBrowserWithUri.Subscribe(uri =>
            {
                Browsers.Add(new BrowserViewModel(uri));
                canRemoveBrowser.Value = Browsers.Count > 1;
                //CurrentBrowser.Value = Browsers.Last();

            });


        }

        public void RemoveJudge()
        {
            canRemoveBrowser.Value = Browsers.Count > 1;
        }


    }
}
