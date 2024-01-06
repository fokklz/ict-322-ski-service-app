using SkiServiceApp.Models;

namespace SkiServiceApp.Common.Events
{
    public class SearchChangedEventArgs
    {
        public string Search { get; private set; }

        public Func<string, Func<CustomListItem, bool>> Filter { get; private set; }

        public Func<CustomListItem, bool> Predicate => Filter(Search);

        public bool IsSearching { get; private set; }

        public SearchChangedEventArgs(string search, Func<string, Func<CustomListItem, bool>> filter)
        {
            Search = search;
            Filter = filter;
            IsSearching = !string.IsNullOrEmpty(Search);
        }


    }
}
