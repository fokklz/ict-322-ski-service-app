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

        public CustomListItem Entry { get; set; }

        public Command ApplyCommand { get; set; }
        public Command CancelCommand { get; set; }
        public Command EditCommand { get; set; }
        public Command NextStateCommand { get; set; }

        public OrderDetailViewModel()
        {
            _orderAPIService = ServiceLocator.GetService<IOrderAPIService>();

            CancelCommand = new Command(async () => await ExecuteCancelCommand());
            EditCommand = new Command(async () => await ExecuteEditCommand());
            NextStateCommand = new Command(async () => await ExecuteNextStateCommand());
            ApplyCommand = new Command(async () => await ExecuteApplyCommand());
        }

        public async Task LoadServiceDetails(int serviceId)
        {
            var data = await _orderAPIService.GetAsync(serviceId);
            if (data != null)
            {
                Entry = new CustomListItem(await data.ParseSuccess());
            }
        }

        private async Task ExecuteCancelCommand()
        {
            await DialogService.ShowDialog(new CancelDialog(Entry), async (result) =>
            {
                if (result)
                {
                    await Entry.Cancel(async () =>
                    {
                        await Shell.Current.GoToAsync("..");
                    });
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
                    await Entry.UpdateDetails(new UpdateOrderRequest
                    {
                        ServiceId = dialogInstance.SelectedService.BackgroundValue,
                        PriorityId = dialogInstance.SelectedPriority.BackgroundValue,
                        StateId = dialogInstance.SelectedState.BackgroundValue,
                        UserId = dialogInstance.Entry.Order.User?.Id,
                        Name = dialogInstance.Entry.Order.Name,
                        Email = dialogInstance.Entry.Order.Email,
                        Phone = dialogInstance.Entry.Order.Phone,
                    });
                }
            },
            submitText: Localization.Instance.ModifyDialog_Submit,
            titleText: Localization.Instance.ModifyDialog_Title);
        }

        private async Task ExecuteNextStateCommand()
        {
            if (Entry.Order.State.Id < 3)
            {
                await Entry.GoNextState();
            }
        }

        private async Task ExecuteApplyCommand()
        {
            await Entry.Apply();
        }
    }
}
