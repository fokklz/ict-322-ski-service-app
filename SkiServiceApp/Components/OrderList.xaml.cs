using PropertyChanged;
using SkiServiceApp.Common;
using SkiServiceApp.Common.Extensions;
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

    private readonly OrderService _orderService;
    private readonly IOrderAPIService _orderAPIService;
    private readonly DialogService _dialogService;

    public static readonly BindableProperty LocationProperty =
        BindableProperty.Create(nameof(Location), typeof(string), typeof(OrderList), "Dashboard", propertyChanged: OnLocationPropertyChanged);

    public string Location
    {
        get => (string)GetValue(LocationProperty);
        set
        {
            SetValue(LocationProperty, value);
        }
    }

    private static void OnLocationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (OrderList)bindable;
        string newLocationValue = (string)newValue;

        control.Location = newLocationValue;
    }

    public BatchObservableCollection<CustomListItem> OrdersList { get; set; }

    public ICommand ApplyCommand => new Command<int>(async (id) => await Apply(id));
    public ICommand ModifyCommand => new Command<int>(async (id) => await Modify(id));

    public ICommand CancelCommand => new Command<int>(async (id) => await Cancel(id));

    public ICommand NextStateCommand => new Command<int>(async (id) => await NextState(id));

    public OrderList()
    {
        _orderService = ServiceLocator.GetService<OrderService>();
        _orderAPIService = ServiceLocator.GetService<IOrderAPIService>();
        _dialogService = ServiceLocator.GetService<DialogService>();

        InitializeComponent();

        _orderService.PropertyChanged += (sender, e) =>
        {
            Debug.WriteLine($"Property changed: {e.PropertyName}");
            OrdersList = Location switch
            {
                "UserList" => _orderService.ApplyUserListFilter(),
                "List" => _orderService.AppyListFilter(),
                _ => _orderService.ApplyDashboardFilter(),
            };
        };
    }


    public void Update()
    {
        _ = Task.Run(_orderService.Update);
    }

    public async Task NextState(int id)
    {
        var orderItem = OrdersList.Where(x => x.Order.Id == id).First();
        await _orderAPIService.UpdateAsync(id, new UpdateOrderRequest
        {
            ServiceId = orderItem.Order.Service.Id,
            PriorityId = orderItem.Order.Priority.Id,
            StateId = orderItem.Order.State.Id + 1,
            UserId = orderItem.Order.User?.Id
        });
        _ = Task.Run(_orderService.Update);

        if (Location.Equals("Dashboard"))
        {
            DashboardChartViewModel.Update.Invoke();
        }
    }

    public async Task Apply(int id)
    {
        var orderItem = OrdersList.Where(x => x.Order.Id == id).First();
        await _orderAPIService.UpdateAsync(id, new UpdateOrderRequest
        {
            ServiceId = orderItem.Order.Service.Id,
            PriorityId = orderItem.Order.Priority.Id,
            StateId = orderItem.Order.State.Id,
            UserId = AuthManager.UserId
        });
        _ = Task.Run(_orderService.Update);

        if (Location.Equals("Dashboard"))
        {
            DashboardChartViewModel.Update.Invoke();
        }
    }

    public async Task Cancel(int id)
    {
        var orderItem = OrdersList.Where(x => x.Order.Id == id).First();
        await DialogService.ShowDialog(new CancelDialog(orderItem), async (result) =>
        {
            if (result)
            {
                await _orderAPIService.DeleteAsync(id);
                _ = Task.Run(_orderService.Update);
            }
        },
        submitText: Localization.Instance.CancelDialog_Submit,
        titleText: Localization.Instance.CancelDialog_Title) ;
    }

    public async Task Modify(int id)
    {
        await Shell.Current.GoToAsync($"{nameof(OrderDetailPage)}?OrderId={id}");
    }
}