using SkiServiceApp.Common;
using SkiServiceApp.Common.Types;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Models;
using SkiServiceApp.Models.Charts;
using SkiServiceApp.ViewModels.Charts;
using SkiServiceModels.DTOs.Responses;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SkiServiceApp.ViewModels
{
    public class DashboardViewModel : BaseNotifyHandler
    {
        public Command ListCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync("///list");
        });

        public Command UserListCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync("///user-list");
        });

        public OrderCollection Orders { get; } = new OrderCollection(
            originFunc: async () =>
            {
                var orderService = ServiceLocator.GetService<IOrderAPIService>();
                var result = await orderService?.GetAllAsync();
                if (result.IsSuccess)
                {
                    var parsed = await result.ParseSuccess();
                    return parsed?.Select(x => new CustomListItem(x)) ?? new ObservableCollection<CustomListItem>();
                }
                else
                {
                    return new ObservableCollection<CustomListItem>();
                }
            },
            sortingFunc: items =>
                items.Where(x => x.Order.State.Id < 3 && !x.Order.IsDeleted && (x.Order.User is null || x.Order.User.Id == AuthManager.UserId))
                     .OrderBy(x => x.Order.User is null)
                     .ThenBy(x => x.DaysLeft)
                     .ThenByDescending(x => x.Order.Priority.Id)
        );

        public DashboardViewModel()
        {
            Orders.Update();
        }
    }
}
