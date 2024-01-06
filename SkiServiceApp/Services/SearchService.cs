using CommunityToolkit.Maui.Core.Platform;
using PropertyChanged;
using SkiServiceApp.Common;
using SkiServiceApp.Common.Events;
using SkiServiceApp.Common.Helpers;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Models;
using System.Collections.ObjectModel;

namespace SkiServiceApp.Services
{
    public class SearchService : BaseNotifyHandler, ISearchService
    {
        private readonly IMainThreadInvoker _mainThreadInvoker = ServiceLocator.GetService<IMainThreadInvoker>();

        public ObservableCollection<PickerItem<Func<string, Func<CustomListItem, bool>>>> SearchFields { get; set; }

        [OnChangedMethod(nameof(OnSelectedSearchFieldChanged))]
        public PickerItem<Func<string, Func<CustomListItem, bool>>> SelectedSearchField { get; set; }

        [OnChangedMethod(nameof(OnSearchBindingChanged))]
        public string SearchBinding { get; set; } = string.Empty;

        public string? Search { get; set; } = null;

        [DependsOn(nameof(Search))]
        public bool IsSearching => !string.IsNullOrEmpty(Search);

        public Command ClearCommand { get; set; }

        public SearchService()
        {
            SearchFields =
            [
                 new()
                {
                    DisplayText = Localization.Instance.ModifyDialog_ServiceLabel,
                    BackgroundValue = searchString => x => x.Service.StartsWith(searchString, StringComparison.OrdinalIgnoreCase)
                },
                new()
                {
                    DisplayText = Localization.Instance.ModifyDialog_EmailLabel,
                    BackgroundValue = searchString => x => x.Order.Email.StartsWith(searchString, StringComparison.OrdinalIgnoreCase)
                },
                new()
                {
                    DisplayText = Localization.Instance.ModifyDialog_NameLabel,
                    BackgroundValue = searchString => x => x.Order.Name.StartsWith(searchString, StringComparison.OrdinalIgnoreCase)
                },
                new()
                {
                    DisplayText = Localization.Instance.ModifyDialog_PhoneLabel,
                    BackgroundValue = searchString => x => x.Order.Phone.StartsWith(searchString, StringComparison.OrdinalIgnoreCase)
                },
                 new()
                {
                    DisplayText = Localization.Instance.ModifyDialog_PriorityLabel,
                    BackgroundValue = searchString => x => x.Priority.StartsWith(searchString, StringComparison.OrdinalIgnoreCase)
                },
                 new()
                {
                    DisplayText = Localization.Instance.ModifyDialog_StateLabel,
                    BackgroundValue = searchString => x => x.State.StartsWith(searchString, StringComparison.OrdinalIgnoreCase)
                }
            ];
            SelectedSearchField = SearchFields[0];
            ClearCommand = new Command(ClearSearch);
        }

        public void ClearSearch()
        {
            Search = null;
            SearchBinding = string.Empty;
        }

        private void OnSearchBindingChanged()
        {
            var key = SearchBinding.Trim();
            if (string.IsNullOrEmpty(key))
            {
                Search = null;
            }
            else
            {
                Search = key;
            }
            SearchHelper.OnSearchChanged(new SearchChangedEventArgs(Search, SelectedSearchField.BackgroundValue));
        }

        private void OnSelectedSearchFieldChanged()
        {
            SearchHelper.OnSearchChanged(new SearchChangedEventArgs(Search, SelectedSearchField.BackgroundValue));
        }
    }
}
