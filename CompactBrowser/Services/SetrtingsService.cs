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


    public interface ISettingService
    {
        Task SaveSettingsAsync();
        Task LoadSettingsAsync();
        ReactiveProperty<Settings> Settings { get; }
    }



    public class SettingsService : ISettingService
    {

        private ReactiveProperty<Settings> settings = new ReactiveProperty<Settings>();

        public ReactiveProperty<Settings> Settings => settings;

        public async Task LoadSettingsAsync()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            try
            {
                Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await localFolder.GetFileAsync("settings.dat");
                string text = await FileIO.ReadTextAsync(sampleFile);
                settings.Value = JsonConvert.DeserializeObject<Settings>(text);
                if(settings.Value == null)
                {
                    settings.Value = new Settings();
                }
            }
            catch
            {
                settings.Value = new Settings();
            }

        }

        public async Task SaveSettingsAsync()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await localFolder.CreateFileAsync("settings.dat", CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, JsonConvert.SerializeObject(settings.Value));
        }
    }

}
