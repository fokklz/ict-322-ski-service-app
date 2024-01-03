using PropertyChanged;
using SkiServiceApp.Common;
using SkiServiceApp.Common.Types;
using SkiServiceApp.Components.Dialogs;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Models;
using SkiServiceApp.Services;
using SkiServiceApp.ViewModels.Charts;
using SkiServiceApp.Views;
using SkiServiceModels.DTOs.Requests;
using SkiServiceModels.DTOs.Responses;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace SkiServiceApp.Components;

public partial class OrderList : ContentView, INotifyPropertyChanged
{
    public static readonly BindableProperty OrdersProperty =
        BindableProperty.Create(nameof(Orders), typeof(OrderCollection), typeof(OrderList), propertyChanged: OnOrderPropertyChanged);

    public static readonly BindableProperty LocationProperty =
        BindableProperty.Create(nameof(Location), typeof(string), typeof(OrderList), "Dashboard", propertyChanged: OnLocationPropertyChanged);

    public new event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Collection of orders to display.
    /// </summary>
    public OrderCollection Orders
    {
        get => (OrderCollection)GetValue(OrdersProperty);
        set
        {
            SetValue(OrdersProperty, value);
        }
    }

    /// <summary>
    /// The location where the list is used, usefull for updating the dashboard chart for example.
    /// </summary>
    public string Location
    {
        get => (string)GetValue(LocationProperty);
        set
        {
            SetValue(LocationProperty, value);
        }
    }

    /// <summary>
    /// Hook to call PropertyChanged when the Orders property changes. Since Fody.PropertyChanged is not working on BindableProperties.
    /// </summary>
    /// <param name="bindable">The control</param>
    /// <param name="oldValue">The previous value</param>
    /// <param name="newValue">The current value</param>
    private static void OnOrderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (OrderList)bindable;
        control.PropertyChanged?.Invoke(control, new PropertyChangedEventArgs(nameof(Orders)));
    }

    /// <summary>
    /// Hook to call PropertyChanged when the Location property changes. Since Fody.PropertyChanged is not working on BindableProperties.
    /// </summary>
    /// <param name="bindable">The control</param>
    /// <param name="oldValue">The previous value</param>
    /// <param name="newValue">The current value</param>
    private static void OnLocationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (OrderList)bindable;
        control.PropertyChanged?.Invoke(control, new PropertyChangedEventArgs(nameof(Location)));
    }

    /// <summary>
    /// Apply to a Order. (each item)
    /// </summary>
    public ICommand ApplyCommand => new Command<int>(async (id) => await ExecuteApplyCommand(id));

    /// <summary>
    /// Go to the next state of a Order. (each item)
    /// </summary>
    public ICommand NextStateCommand => new Command<int>(async (id) => await ExecuteNextStateCommand(id));

    /// <summary>
    /// Modify a Order. (each item)
    /// </summary>
    public ICommand ModifyCommand => new Command<int>(async (id) => await ExecuteModifyCommand(id));

    /// <summary>
    /// Cancel a Order. (each item)
    /// </summary>
    public ICommand CancelCommand => new Command<int>(async (id) => await ExecuteCancelCommand(id));

    public OrderList()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Move the order to the next state. & Sort the list.
    /// </summary>
    /// <param name="id">The Id to change the State for</param>
    public async Task ExecuteNextStateCommand(int id)
    {
        var orderItem = Orders.Where(x => x.Order.Id == id).First();
        await Task.Run(async () =>
        {
            await orderItem.GoNextState(() =>
            {
                Orders.SortAndNotify();

                if (Location.Equals("Dashboard"))
                {
                    DashboardChartViewModel.Update?.Invoke();
                }
            });
        });
    }

    /// <summary>
    /// Apply to the order. & Sort the list.
    /// </summary>
    /// <param name="id">The Id of the Order the current user Should be added</param>
    public async Task ExecuteApplyCommand(int id)
    {
        var orderItem = Orders.Where(x => x.Order.Id == id).First();
        await Task.Run(async () =>
        {
            await orderItem.Apply(() =>
            {
                Orders.SortAndNotify();

                if (Location.Equals("Dashboard"))
                {
                    DashboardChartViewModel.Update?.Invoke();
                }
            });
        });
    }

    /// <summary>
    /// Cancel the order. & Sort the list.
    /// </summary>
    /// <param name="id">The Id of the Order that should be canceld</param>
    public async Task ExecuteCancelCommand(int id)
    {
        var orderItem = Orders.Where(x => x.Order.Id == id).First();
        await DialogService.ShowDialog(new CancelDialog(orderItem), async (result) =>
        {
            if (result)
            {
                await orderItem.Cancel(() =>
                {
                    // remove the item from the list when it has been canceled
                    Orders.RemoveAt(Orders.IndexOf(orderItem));
                    Orders.SortAndNotify();

                    if (Location.Equals("Dashboard"))
                    {
                        DashboardChartViewModel.Update?.Invoke();
                    }
                });
            }
        },
        submitText: Localization.Instance.CancelDialog_Submit,
        titleText: Localization.Instance.CancelDialog_Title) ;
    }

    /// <summary>
    /// Redirect to the OrderDetailPage so the user can modify the order.
    /// </summary>
    /// <param name="id">The Id of the Order of interest</param>
    public async Task ExecuteModifyCommand(int id)
    {
        await Shell.Current.GoToAsync($"{nameof(OrderDetailPage)}?OrderId={id}");
    }


    /// <summary>
    /// Allow to update the list from the outside.
    /// Should be used in views inside OnAppearing.
    /// </summary>
    /// <param name="done">a optional action to perform when the update is done</param>
    public void Update(Action? done = null)
    {
        Task.Run(async () =>
        {
            await Orders.Update(done);
            OnPropertyChanged(nameof(Orders));
        }).ConfigureAwait(false);
    }
}