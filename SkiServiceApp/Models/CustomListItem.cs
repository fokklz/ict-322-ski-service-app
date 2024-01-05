using PropertyChanged;
using SkiServiceApp.Common;
using SkiServiceApp.Common.Events;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Services;
using SkiServiceModels.DTOs.Requests;
using SkiServiceModels.DTOs.Responses;
using System.Diagnostics;

namespace SkiServiceApp.Models
{
    /// <summary>
    /// Item used for the OrderList containing all the displaying lokic on a per order basis
    /// </summary>
    public class CustomListItem : BaseNotifyHandler
    {
        private readonly IOrderAPIService _orderAPIService;
        private readonly IMainThreadInvoker _mainThreadInvoker;
        /// <summary>
        /// The raw order data received from the API
        /// </summary>
        public OrderResponseAdmin Order { get; private set; } 

        /// <summary>
        /// The amount of days left before the order is due to be finished
        /// </summary>
        public int DaysLeft { get; set; }

        /// <summary>
        /// The priority of the order in the current language
        /// </summary>
        public string Priority { get; set; }

        /// <summary>
        /// The state of the order in the current language
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The next state of the order in the current language as verb
        /// </summary>
        public string NextState { get; set; }

        /// <summary>
        /// The service of the order in the current language
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// A pretty title for the order, containing the name and the service also in the current language
        /// </summary>
        public string PrettyTitle { get; set;}

        /// <summary>
        /// Whether the order is assigned to a user or not
        /// </summary>
        public bool IsAssigned => Order.User != null;

        /// <summary>
        /// Show the assign button if the order is not assigned, so the user can take it
        /// </summary>
        [DependsOn(nameof(IsAssigned))]
        public bool ShowAssignButton => !IsAssigned;

        /// <summary>
        /// Show cancel button if the order is assigned and the setting is enabled, for quick canceling
        /// </summary>
        [DependsOn(nameof(IsAssigned))]
        public bool ShowCancelButtonInList => IsAssigned && SettingsService.CancelInListView && Order.State.Id < 3;

        /// <summary>
        /// Show the cancel button if the order is assigned, so the user can cancel it (will only apply to the detail view)
        /// </summary>
        [DependsOn(nameof(IsAssigned))]
        public bool ShowCancelButton => IsAssigned && Order.State.Id < 3;

        /// <summary>
        /// Show the next state button if the order is assigned and the setting is enabled, for quick state changing
        /// </summary>
        [DependsOn(nameof(IsAssigned))]
        public bool ShowNextStateButton => IsAssigned && Order.State.Id < 3;

        public CustomListItem(OrderResponseAdmin orderResponse)
        {
            Order = orderResponse;

            _orderAPIService = ServiceLocator.GetService<IOrderAPIService>();
            _mainThreadInvoker = ServiceLocator.GetService<IMainThreadInvoker>();

            Update();
            Localization.LanguageChanged += UpdateLanguage;
        }

        /// <summary>
        /// Update all the volatile properties of the CustomListItem
        /// </summary>
        public void Update()
        {
            UpdateTimeLeft();
            UpdateLanguage(null, null);
        }

        /// <summary>
        /// Update the time left calculation for the order
        /// </summary>
        public void UpdateTimeLeft()
        {
            DateTime deadline = Order.Created.AddDays(Order.Priority.Days);
            TimeSpan timeLeft = deadline - DateTime.Now;
            var days = timeLeft.Days;
            DaysLeft = days > 0 ? days : 0;
        }

        /// <summary>
        /// Event handler for the language changed event, ensures that the language is updated in the UI
        /// </summary>
        /// <param name="sender">The sender of the Event</param>
        /// <param name="args">The args relevant to the current language</param>
        public void UpdateLanguage(object? sender, LanguageChangedEventArgs? args)
        {
            Priority = Localization.Instance.GetResource($"Backend.Priority.{Order.Priority.Id}");
            State = Localization.Instance.GetResource(Order.User == null ? "CustomListItem.Unassigned" : $"Backend.State.{Order.State.Id}");
            Service = Localization.Instance.GetResource($"Backend.Service.{Order.Service.Id}");
            PrettyTitle = $"{Order.Name}  -  {Service}";
            NextState = Localization.Instance.GetResource($"Backend.NextState.{Order.State.Id + 1}");
        }

        /// <summary>
        /// Apply for the order, will update the order in the backend and then update the UI with the new data aswell
        /// Allows for a done callback to be passed, which will be called after the order has been updated (allowing to hook a resorting)
        /// </summary>
        /// <param name="done">The action that is called when the update is done</param>
        public async Task Apply(Action? done = null)
        {
            var newOrderResponse = await _orderAPIService.UpdateAsync(Order.Id, new UpdateOrderRequest
            {
                ServiceId = Order.Service.Id,
                PriorityId = Order.Priority.Id,
                StateId = Order.State.Id,
                UserId = AuthManager.UserId
            });

            if (newOrderResponse.IsSuccess)
            {
                var parsed = await newOrderResponse.ParseSuccess();
                _mainThreadInvoker.BeginInvokeOnMainThread(() =>
                {
                    // update the order with the new data
                    Order = parsed;
                    Update();

                    done?.Invoke();
                });
            }
        }

        /// <summary>
        /// Go to the next state for the order, will update the order in the backend and then update the UI with the new data aswell
        /// </summary>
        /// <param name="done">The action that should be performed when the update to the backend and the Item has been done</param>
        public async Task GoNextState(Action? done = null)
        {
            var newOrderResponse = await _orderAPIService.UpdateAsync(Order.Id, new UpdateOrderRequest
            {
                ServiceId = Order.Service.Id,
                PriorityId = Order.Priority.Id,
                StateId = Order.State.Id + 1,
                UserId = Order.User?.Id
            });

            if (newOrderResponse.IsSuccess)
            {
                var parsed = await newOrderResponse.ParseSuccess();
                _mainThreadInvoker.BeginInvokeOnMainThread(() =>
                {
                    // update the order with the new data
                    Order = parsed;
                    Update();

                    done?.Invoke();
                });
            }
        }

        /// <summary>
        /// Simplify the canceling of an order, will update the order 
        /// in the backend and then call a action with the deleted order id
        /// </summary>
        /// <param name="done">Action to call when the canelation has been processed</param>
        public async Task Cancel(Action done)
        {
            var deleteResponse = await _orderAPIService.DeleteAsync(Order.Id);
            if (deleteResponse.IsSuccess)
            {
                var parsed = await deleteResponse.ParseSuccess();
                if (parsed != null && parsed.Id > 0)
                {
                    _mainThreadInvoker.BeginInvokeOnMainThread(() =>
                    {
                        // mark the order as deleted, so it will be removed from the list
                        Order.IsDeleted = true;
                        Update();

                        done.Invoke();
                    });
                }
            }
        }

        /// <summary>
        /// Update the details for a order, will send the model to the backend and then update the UI with the new data aswell
        /// </summary>
        /// <param name="update">The model to use for the Update containing all data</param>
        public async Task UpdateDetails(UpdateOrderRequest update, Action? done = null)
        {
            var updateOrderResponse = await _orderAPIService.UpdateAsync(Order.Id, update);
            if (updateOrderResponse.IsSuccess)
            {
                var parsed = await updateOrderResponse.ParseSuccess();
                if (parsed != null)
                {
                    _mainThreadInvoker.BeginInvokeOnMainThread(() =>
                    {
                        Order = parsed;
                        Update();

                        done?.Invoke();
                    });
                }
            }
        }
    }
}
