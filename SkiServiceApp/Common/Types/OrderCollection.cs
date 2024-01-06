using SkiServiceApp.Common.Events;
using SkiServiceApp.Common.Helpers;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SkiServiceApp.Common.Types
{
    /// <summary>
    /// Wrapping type for the OrderCollection to simplify the usage on multiple pages.
    /// 
    /// Any view implementing this type should use .Update on it inside the OnAppearing method.
    /// </summary>
    public class OrderCollection : ObservableCollection<CustomListItem>, INotifyCollectionChanged
    {
        private readonly IMainThreadInvoker _mainThreadInvoker;

        private readonly Func<Task<IEnumerable<CustomListItem>>> origin;
        private Func<IEnumerable<CustomListItem>, IEnumerable<CustomListItem>>? sorting = null;

        private IEnumerable<CustomListItem> _originalItems;

        /// <summary>
        /// Toggle notification suppression for the collection.
        /// </summary>
        public bool SuppressNotification { get; set; } = false;

        public OrderCollection(Func<Task<IEnumerable<CustomListItem>>> originFunc, Func<IEnumerable<CustomListItem>, IEnumerable<CustomListItem>>? sortingFunc = null) : base() {
            origin = originFunc;
            sorting = sortingFunc;

            _mainThreadInvoker = ServiceLocator.GetService<IMainThreadInvoker>();

            SearchHelper.SearchChanged += SearchEvent_SearchChanged;
        }

        /// <summary>
        /// Will resort the collection when the search changes.
        /// </summary>
        /// <param name="sender">The sender instance of the Event</param>
        /// <param name="e">The parameters for the Event</param>
        private void SearchEvent_SearchChanged(object? sender, SearchChangedEventArgs e)
        {
            Task.Run(() =>
            {
                if (!e.IsSearching)
                {
                    SortAndNotify(_originalItems);
                }
                else
                {
                    SortAndNotify(_originalItems.Where(e.Predicate).ToList());
                }
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Update the collection with the latest data from the API. & Sort them.
        /// </summary>
        public void Update(Action? done = null)
        {
            origin.Invoke().ContinueWith(x =>
            {
                var newItems = x.Result;
                _originalItems = newItems;
                SortAndNotify(newItems);
                done?.Invoke();
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Sort the collection and notify the view.
        /// </summary>
        /// <param name="items">Optionally a list of items to sort and then insert</param>
        public void SortAndNotify(IEnumerable<CustomListItem>? items = null)
        {
            if (sorting != null)
            {
                var sorted = sorting.Invoke(items ?? this).ToList();
                _mainThreadInvoker.BeginInvokeOnMainThread(() =>
                {
                    SuppressNotification = true;
                    Clear();
                    foreach (var item in sorted)
                    {
                        Add(item);
                    }
                    SuppressNotification = false;
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                });
            }
            else
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        /// <summary>
        /// Overrides the default OnCollectionChanged method to allow for suppressing the notification.
        /// </summary>
        /// <param name="e">The Event Parameters</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!SuppressNotification) base.OnCollectionChanged(e);
        }
    }
}
