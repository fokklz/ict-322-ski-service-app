using SkiServiceApp.Common;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using SkiServiceApp.Models;
using SkiServiceModels.DTOs.Responses;
using SkiServiceApp.Common.Types;
using SkiServiceApp.Interfaces.API;

namespace SkiServiceApp.ViewModels
{
    public class ListViewModel : BaseNotifyHandler
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
                items.Where(x => x.Order.State.Id < 3 && !x.Order.IsDeleted && x.Order.User is null)
                     .OrderBy(x => x.DaysLeft)
                     .ThenByDescending(x => x.Order.Priority.Id)
        );

        public ListViewModel()
        {
            Orders.Update();
        }
    }
}
