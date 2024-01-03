using SkiServiceApp.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;

namespace SkiServiceApp.Common.Types
{
    /// <summary>
    /// Wrapping type for the OrderCollection to simplify the usage on multiple pages.
    /// 
    /// Any view implementing this type should use .Update on it inside the OnAppearing method.
    /// </summary>
    public class OrderCollection : ObservableCollection<CustomListItem>, INotifyCollectionChanged
    {
        private readonly Func<Task<IEnumerable<CustomListItem>>> origin;
        private Func<IEnumerable<CustomListItem>, IEnumerable<CustomListItem>>? sorting = null;

        /// <summary>
        /// Toggle notification suppression for the collection.
        /// </summary>
        public bool SuppressNotification { get; set; } = false;

        public OrderCollection(Func<Task<IEnumerable<CustomListItem>>> originFunc, Func<IEnumerable<CustomListItem>, IEnumerable<CustomListItem>>? sortingFunc = null) : base() {
            origin = originFunc;
            sorting = sortingFunc;
        }

        /// <summary>
        /// Update the collection with the latest data from the API. & Sort them.
        /// </summary>
        public async Task Update(Action? done = null)
        {
            var newItems = await origin.Invoke();
            SortAndNotify(newItems);
            done?.Invoke();
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
                Debug.WriteLine($"Sorting {sorted.Count} items.");
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    SuppressNotification = true;
                    Clear();
                    foreach (var item in sorted)
                    {
                        Add(item);
                    }
                    SuppressNotification = false;
                });
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
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
