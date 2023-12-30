using SkiServiceApp.Common;
using SkiServiceApp.Common.Extensions;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Models;
using System.ComponentModel;
using static Android.Graphics.ImageDecoder;

namespace SkiServiceApp.Services
{
    public class OrderService : INotifyPropertyChanged
    {
        private readonly IOrderAPIService _orderAPIService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public BatchObservableCollection<CustomListItem> Orders { get; set; } = new BatchObservableCollection<CustomListItem>();

        public BatchObservableCollection<CustomListItem> DashboardOrders { get; set; } = new BatchObservableCollection<CustomListItem>();

        public BatchObservableCollection<CustomListItem> UserListOrders { get; set; } = new BatchObservableCollection<CustomListItem>();

        public BatchObservableCollection<CustomListItem> ListOrders { get; set; } = new BatchObservableCollection<CustomListItem>();

        public OrderService(IOrderAPIService orderAPIService) { 
            _orderAPIService = orderAPIService;
        }

        public async Task Update()
        {
            var res = await _orderAPIService.GetAllAsync();
            if (res.IsSuccess)
            {
                var parsed = await res.ParseSuccess();
                Orders.BatchUpdate(parsed);

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Orders)));
            }
        }

        public BatchObservableCollection<CustomListItem> ApplyDashboardFilter(BatchObservableCollection<CustomListItem> source = null)
        {
            var filtered = (source ?? Orders).Where(x => x.Order.State.Id != 3 && !x.Order.IsDeleted && (x.Order.User is null || x.Order.User.Id == AuthManager.UserId)).ToList();
            var final = filtered
                .OrderBy(x => x.Order.User is null)
                .ThenBy(x => x.DaysLeft)
                .ThenByDescending(x => x.Order.Priority.Id);

            DashboardOrders.BatchUpdate(final.ToList());

            return DashboardOrders;
        }

        public BatchObservableCollection<CustomListItem> AppyListFilter(BatchObservableCollection<CustomListItem> source = null)
        {
            var filtered = (source ?? Orders).Where(x => x.Order.State.Id != 3 && !x.Order.IsDeleted && x.Order.User is null).ToList();

            var final = filtered
                .OrderBy(x => x.DaysLeft)
                .ThenByDescending(x => x.Order.Priority.Id);

            ListOrders.BatchUpdate(final.ToList());

            return ListOrders;
        }

        public BatchObservableCollection<CustomListItem> ApplyUserListFilter(BatchObservableCollection<CustomListItem> source = null)
        {
            var filtered = (source ?? Orders).Where(x => x.Order.User is not null && !x.Order.IsDeleted && x.Order.User.Id == AuthManager.UserId).ToList();
            var final = filtered
                .OrderBy(x => x.Order.State.Id)
                .ThenBy(x => x.DaysLeft)
                .ThenByDescending(x => x.Order.Priority.Id);

            UserListOrders.BatchUpdate(final.ToList());

            return UserListOrders;
        }
    }
}
