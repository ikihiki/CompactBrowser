using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using System.Reactive.Subjects;

namespace CompactBrowser.ViewModels
{
    public class BrowserViewModel
    {
        public ReactiveCommand GoBack { get; }
        public ReadOnlyReactiveProperty<bool> CanGoBack { get; }
        private ReactiveProperty<bool> canGoBack = new ReactiveProperty<bool>(false);

        public ReactiveCommand GoForward { get; }
        public ReadOnlyReactiveProperty<bool> CanGoForward { get; }
        private ReactiveProperty<bool> canGoForward = new ReactiveProperty<bool>(false);

        public ReactiveCommand Refresh { get; }
        public ReactiveCommand<string> Navigate { get; }
        private Subject<bool> alwaysTrue = new Subject<bool>();

        public ReadOnlyReactiveProperty<string> Title { get; }
        private ReactiveProperty<string> title =  new ReactiveProperty<string>("新しいタブ");

        public ReactiveProperty<Uri> Uri { get; } = new ReactiveProperty<Uri>(new System.Uri("http://www.google.co.jp"));

        public ReactiveProperty<bool> IsBusy { get; } = new ReactiveProperty<bool>(false);

        public BrowserViewModel()
        {
            

            CanGoBack = canGoBack.ToReadOnlyReactiveProperty();
            GoBack = canGoBack.ToReactiveCommand();

            CanGoForward = canGoForward.ToReadOnlyReactiveProperty();
            GoForward = canGoForward.ToReactiveCommand();

            Refresh = alwaysTrue.ToReactiveCommand();

            Navigate = alwaysTrue.ToReactiveCommand<string>();

            alwaysTrue.OnNext(true);

            Title = title.ToReadOnlyReactiveProperty();
        }

        public BrowserViewModel(Uri uri)
        {
            Uri.Value = uri;

            CanGoBack = canGoBack.ToReadOnlyReactiveProperty();
            GoBack = canGoBack.ToReactiveCommand();

            CanGoForward = canGoForward.ToReadOnlyReactiveProperty();
            GoForward = canGoForward.ToReactiveCommand();

            Refresh = alwaysTrue.ToReactiveCommand();

            Navigate = alwaysTrue.ToReactiveCommand<string>();

            alwaysTrue.OnNext(true);

            Title = title.ToReadOnlyReactiveProperty();
        }



        public void ChangeTitle(string title)
        {
            this.title.Value = title;
        }


        public void ChangeCanGoBack(bool can)
        {
            this.canGoBack.Value = can;
        }

        public void ChangeCanGoForward(bool can)
        {
            this.canGoForward.Value = can;
        }

    }
}
