using SkiServiceApp.Common.Events;

namespace SkiServiceApp.Common.Helpers
{
    /// <summary>
    /// Small helper class to allow simple global tracking of the search state without having to break MVVM.
    /// </summary>
    public static class SearchHelper
    {

        public static event EventHandler<SearchChangedEventArgs>? SearchChanged;

        public static void OnSearchChanged(SearchChangedEventArgs e)
        {
            SearchChanged?.Invoke(null, e);
        }
        
    }
}
