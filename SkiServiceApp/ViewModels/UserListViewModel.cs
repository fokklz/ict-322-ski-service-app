using CommunityToolkit.Maui.Views;
using SkiServiceApp.Common;
using SkiServiceApp.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using SkiServiceApp.Views;
using SkiServiceModels.DTOs.Responses;
using SkiServiceApp.Common.Types;
using SkiServiceApp.Interfaces.API;

namespace SkiServiceApp.ViewModels
{
    public class UserListViewModel : BaseNotifyHandler
    {

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
                items.Where(x => x.Order.User is not null && !x.Order.IsDeleted && x.Order.User.Id == AuthManager.UserId)
                     .OrderBy(x => x.Order.State.Id)
                     .ThenBy(x => x.DaysLeft)
                     .ThenByDescending(x => x.Order.Priority.Id)
        );

        public UserListViewModel()
        {
            Orders.Update();
        }

        private async void NavigateToDetailAsync(OrderResponse service)
        {
            if (service == null)
            {
                Debug.WriteLine("Service is null, cannot navigate to detail.");
                return;
            }

            // Navigieren Sie zur Detailansicht und übergeben Sie die ServiceId als Query-Parameter.
            await Shell.Current.GoToAsync($"{nameof(OrderDetailPage)}?ServiceId={service.Id}");
        }


    }
}
