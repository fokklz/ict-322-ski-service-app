using SkiServiceApp.Models;
using SkiServiceModels.DTOs.Responses;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SkiServiceApp.Common.Extensions
{
    public class BatchObservableCollection<T> : ObservableCollection<T>, INotifyCollectionChanged
    {
        public bool SuppressNotification { get; set; } = false;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!SuppressNotification) base.OnCollectionChanged(e);
        }

        public void SendNotification()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }

    public static class BatchObservableCollectionExtension
    {
        public static void BatchUpdate(this BatchObservableCollection<CustomListItem> collection, IList<CustomListItem> newItems)
        {
            collection.SuppressNotification = true;
            collection.Clear();
            foreach (var item in newItems)
            {
                collection.Add(item);
            }
            /*if (newItems.Count >= collection.Count)
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    collection[i] = newItems[i];
                }
                if (collection.Count < newItems.Count)
                {
                    for (int i = collection.Count; i < newItems.Count; i++)
                    {
                        collection.Add(newItems[i]);
                    }
                }
            }
            else
            { // newItems is smaller than collection
                for (int i = 0; i < newItems.Count; i++)
                {
                    collection[i] = newItems[i];
                }
                for (int i = newItems.Count; i < collection.Count; i++)
                {
                    collection.RemoveAt(i);
                }
                // inverse the above to avoide index out of range exception

                for (int i = collection.Count - 1; i >= newItems.Count; i--)
                {
                    collection.RemoveAt(i);
                }
            }*/
            collection.SuppressNotification = false;
            collection.SendNotification();
        }

        public static void BatchUpdate(this BatchObservableCollection<CustomListItem> collection, IList<OrderResponseAdmin> newItems)
        {
            collection.BatchUpdate(newItems.Select(x => new CustomListItem(x)).ToList());
        }
    }
}
