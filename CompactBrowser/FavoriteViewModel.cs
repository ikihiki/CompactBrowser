using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompactBrowser
{
    public class FavoriteViewModel
    {
        public string Title { get; set; }
        public Uri Uri { get; set; }

        public FavoriteViewModel(string title, Uri uri)
        {
            Title = title;
            Uri = uri;
        }

        public override bool Equals(object obj)
        {
            var a = (FavoriteViewModel)obj;
            return a.Title == Title && a.Uri == Uri;
        }
        public override int GetHashCode()
        {
            return Uri.GetHashCode();
        }
    }


    class FavoriteViewModelEqualityComparer : IEqualityComparer<FavoriteViewModel>
    {
        public bool Equals(FavoriteViewModel b1, FavoriteViewModel b2)
        {
            if (b2 == null && b1 == null)
                return true;
            else if (b1 == null | b2 == null)
                return false;
            else if (b1.Uri == b2.Uri)
                return true;
            else
                return false;
        }

        public int GetHashCode(FavoriteViewModel bx)
        {
            return bx.Uri.GetHashCode();
        }
    }
}
