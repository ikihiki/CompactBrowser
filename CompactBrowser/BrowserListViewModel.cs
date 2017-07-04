using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;
using Windows.Storage;
using Newtonsoft.Json;

namespace CompactBrowser
{
    public class BrowserListViewModel
    {
        public ReactiveCollection<BrowserViewModel> Browsers { get; } = new ReactiveCollection<BrowserViewModel>();
        public ReactiveProperty<int> CurrentIndex { get; } = new ReactiveProperty<int>(0);
        public BrowserViewModel CurrentBrowser
        {
            get
            {
                return Browsers[CurrentIndex.Value];
            }
        }


        public ReactiveCollection<FavoriteViewModel> Favorites { get; } = new ReactiveCollection<FavoriteViewModel>();
        public ReadOnlyReactiveProperty<bool> CanFavorite { get; }
        private ReactiveProperty<bool> canFavorite = new ReactiveProperty<bool>();

        public ReactiveCommand Favorite { get; }

        private ReactiveProperty<bool> canRemoveBrowser = new ReactiveProperty<bool>(false);
        public ReactiveCommand<BrowserViewModel> RemoveBrowser { get; }

        private ReactiveProperty<bool> alwaysTrue = new ReactiveProperty<bool>(true);
        public ReactiveCommand AddBrowser { get; }

        public BrowserListViewModel()
        {

            Browsers.Add(new BrowserViewModel());
            CanFavorite = canFavorite.ToReadOnlyReactiveProperty();
            Favorite = canFavorite.ToReactiveCommand();
            Favorite.Subscribe(() =>
            {
                Favorites.Add(new FavoriteViewModel(CurrentBrowser.Title.Value, CurrentBrowser.Uri.Value));
                canFavorite.Value = false;
            });
            CurrentIndex.Subscribe(index => canFavorite.Value = !Favorites.Contains(new FavoriteViewModel(CurrentBrowser.Title.Value, CurrentBrowser.Uri.Value)));

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
            });
        }

        public void FavoriteJudge()
        {
            canFavorite.Value = !Favorites.Contains(new FavoriteViewModel(CurrentBrowser.Title.Value, CurrentBrowser.Uri.Value),new FavoriteViewModelEqualityComparer());
        }

        public void RemoveJudge()
        {
            canRemoveBrowser.Value = Browsers.Count > 1;
        }

        public async Task LoadAsync()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            try
            {
                Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await localFolder.GetFileAsync("favorite.dat");
                string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
                Favorites.AddRangeOnScheduler(JsonConvert.DeserializeObject<FavoriteViewModel[]>(text));
                FavoriteJudge();
            }
            catch
            { }
        }


        public async Task SaveAsync()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await localFolder.CreateFileAsync("favorite.dat", CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, JsonConvert.SerializeObject(Favorites.ToArray()));
        }
    }
}
