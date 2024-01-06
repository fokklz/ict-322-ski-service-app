using SkiServiceApp.Models;
using System.Collections.ObjectModel;

namespace SkiServiceApp.Interfaces
{
    public interface ISearchService
    {
        Command ClearCommand { get; set; }
        bool IsSearching { get; }
        string? Search { get; set; }
        string SearchBinding { get; set; }
        ObservableCollection<PickerItem<Func<string, Func<CustomListItem, bool>>>> SearchFields { get; set; }
        PickerItem<Func<string, Func<CustomListItem, bool>>> SelectedSearchField { get; set; }

        void ClearSearch();
    }
}