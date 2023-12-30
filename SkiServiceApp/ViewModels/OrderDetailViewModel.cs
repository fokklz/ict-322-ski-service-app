using SkiServiceApp.Common;
using SkiServiceApp.Models;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Diagnostics;
using SkiServiceApp.Resources.Helper;
using SkiServiceModels.DTOs.Responses;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Services;
using SkiServiceApp.Components.Dialogs;
using SkiServiceModels.DTOs.Requests;
using SkiServiceApp.Views;

namespace SkiServiceApp.ViewModels
{
    public class OrderDetailViewModel : BaseNotifyHandler
    {
        private readonly IOrderAPIService _orderAPIService;
        private readonly OrderService _orderService;


        public CustomListItem Entry { get; set; }

        public ICommand CancelCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand NextStateCommand { get; set; }

        public OrderDetailViewModel()
        {
            _orderAPIService = ServiceLocator.GetService<IOrderAPIService>();
            _orderService = ServiceLocator.GetService<OrderService>();

            CancelCommand = new Command(async () => await ExecuteCancelCommand());
            EditCommand = new Command(async () => await ExecuteEditCommand());
            NextStateCommand = new Command(async () => await ExecuteStatusCommand());

        }

        public async Task LoadServiceDetails(int serviceId)
        {
            var data = await _orderAPIService.GetAsync(serviceId);
            if (data != null)
            {
                Entry = new CustomListItem(await data.ParseSuccess());
                OnPropertyChanged(nameof(Entry));
            }
        }

        private async Task ExecuteCancelCommand()
        {
            await DialogService.ShowDialog(new CancelDialog(Entry), async (result) =>
            {
                if (result)
                {
                    await _orderAPIService.DeleteAsync(Entry.Order.Id);
                    await Task.Run(async () => await _orderService.Update());
                    await Shell.Current.GoToAsync("..");
                }
            },
            submitText: Localization.Instance.CancelDialog_Submit,
            titleText: Localization.Instance.CancelDialog_Title);
        }

        private async Task ExecuteEditCommand()
        {
            var dialogInstance = new ModifyDialog(Entry);
            await DialogService.ShowDialog(dialogInstance, async (result) =>
            {
                if (result)
                {
                    await _orderAPIService.UpdateAsync(Entry.Order.Id, new UpdateOrderRequest
                    {
                        ServiceId = dialogInstance.SelectedService.BackgroundValue,
                        PriorityId = dialogInstance.SelectedPriority.BackgroundValue,
                        StateId = dialogInstance.SelectedState.BackgroundValue,
                        UserId = dialogInstance.Entry.Order.User?.Id,
                        Name = dialogInstance.Entry.Order.Name,
                        Email = dialogInstance.Entry.Order.Email,
                        Phone = dialogInstance.Entry.Order.Phone,
                    });
                    await Task.Run(async () => await _orderService.Update());
                    await LoadServiceDetails(Entry.Order.Id);
                }
            },
            submitText: Localization.Instance.ModifyDialog_Submit,
            titleText: Localization.Instance.ModifyDialog_Title);
        }

        private async Task ExecuteStatusCommand()
        {
            await _orderAPIService.UpdateAsync(Entry.Order.Id, new UpdateOrderRequest
            {
                ServiceId = Entry.Order.Service.Id,
                PriorityId = Entry.Order.Priority.Id,
                StateId = Entry.Order.State.Id + 1,
                UserId = Entry.Order.User?.Id
            });
            await Task.Run(async () => await _orderService.Update());
            await LoadServiceDetails(Entry.Order.Id);
        }
    }
}
