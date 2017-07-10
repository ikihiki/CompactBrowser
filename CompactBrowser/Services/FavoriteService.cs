using CompactBrowser.Models;
using Newtonsoft.Json;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CompactBrowser.Services
{

    public interface IFavoriteService
    {
        ReadOnlyReactiveCollection<Favorite> GetFavorite();
        Task SaveFavoriteAsync();
        Task LoadFavoriteAsync();
        void AddFavorite(Favorite favorite);
        void RemoveFavorite(Favorite favorite);
        bool ExsistFavorite(Favorite favorite);
        bool ExsistFavorite(Uri uri);
    }


    public class FavoriteService:IFavoriteService
    {
        private ReactiveCollection<Favorite> favorites = new ReactiveCollection<Favorite>();

        public void AddFavorite(Favorite favorite)
        {
            if (!ExsistFavorite(favorite))
            {
                favorites.Add(favorite);
            }
        }

        public bool ExsistFavorite(Favorite favorite) => favorites.Count(f => f.Uri == favorite.Uri) > 0;

        public bool ExsistFavorite(Uri uri) => favorites.Count(f => f.Uri == uri) > 0;

        public ReadOnlyReactiveCollection<Favorite> GetFavorite() => favorites.ToReadOnlyReactiveCollection();

        public async Task LoadFavoriteAsync()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            try
            {
                Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await localFolder.GetFileAsync("favorite.dat");
                string text = await FileIO.ReadTextAsync(sampleFile);
                favorites.AddRangeOnScheduler(JsonConvert.DeserializeObject<Favorite[]>(text));
            }
            catch
            { }

        }

        public void RemoveFavorite(Favorite favorite)
        {
            favorites.Remove(favorite);
        }

        public async Task SaveFavoriteAsync()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await localFolder.CreateFileAsync("favorite.dat", CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, JsonConvert.SerializeObject(favorites.ToArray()));

        }
    }
}
