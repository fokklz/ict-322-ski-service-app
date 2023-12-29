using PropertyChanged;
using SkiServiceApp.Common;
using SkiServiceApp.Common.Events;
using SkiServiceApp.Services;
using SkiServiceModels.DTOs.Responses;

namespace SkiServiceApp.Models
{
    /// <summary>
    /// Item used for the OrderList containing all the displaying lokic on a per order basis
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class CustomListItem
    {
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
        public bool ShowCancelButtonInList => IsAssigned && SettingsService.CancelInListView && Order.State.Id != 3;

        /// <summary>
        /// Show the cancel button if the order is assigned, so the user can cancel it (will only apply to the detail view)
        /// </summary>
        [DependsOn(nameof(IsAssigned))]
        public bool ShowCancelButton => IsAssigned && Order.State.Id != 3;

        /// <summary>
        /// Show the next state button if the order is assigned and the setting is enabled, for quick state changing
        /// </summary>
        [DependsOn(nameof(IsAssigned))]
        public bool ShowNextStateButton => IsAssigned && Order.State.Id != 3;

        public CustomListItem(OrderResponseAdmin orderResponse)
        {
            Order = orderResponse;

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
    }
}
