using PropertyChanged;
using SkiServiceApp.Common;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SkiServiceApp.Components.Dialogs;

public partial class ModifyDialog : ContentView, IDialog, INotifyPropertyChanged
{

	private readonly IPriorityAPIService _priorityAPIService;
	private readonly IStateAPIService _stateAPIService;
	private readonly IServiceAPIService _serviceAPIService;



    public CustomListItem Entry { get; set; }

	public ObservableCollection<PickerItem<int>> Priorities { get; set; } = new ObservableCollection<PickerItem<int>>();

	public PickerItem<int> SelectedPriority { get; set; }

	public ObservableCollection<PickerItem<int>> States { get; set; } = new ObservableCollection<PickerItem<int>>();

	public PickerItem<int> SelectedState { get; set; }

	public ObservableCollection<PickerItem<int>> Services { get; set; } = new ObservableCollection<PickerItem<int>>();

	public PickerItem<int> SelectedService { get; set; }

    public ModifyDialog(CustomListItem entry)
	{
		InitializeComponent();

		_priorityAPIService = ServiceLocator.GetService<IPriorityAPIService>();
		_stateAPIService = ServiceLocator.GetService<IStateAPIService>();
		_serviceAPIService = ServiceLocator.GetService<IServiceAPIService>();

		Task.Run(async () =>
		{
			var priorities = await _priorityAPIService.GetAllAsync();
			if (priorities.IsSuccess)
			{
				var parsed = await priorities.ParseSuccess();
                Priorities.Clear();
				foreach (var item in parsed)
				{
					var newItem = new PickerItem<int>
					{
						BackgroundValue = item.Id,
						DisplayText = Localization.Instance.GetResource($"Backend.Priority.{item.Id}")
					};

                    if (item.Id == entry.Order.Priority.Id)
					{
						SelectedPriority = newItem;
					}
                    Priorities.Add(newItem);
				}
				OnPropertyChanged(nameof(Priorities));
			}

			var states = await _stateAPIService.GetAllAsync();
			if (states.IsSuccess)
			{
                var parsed = await states.ParseSuccess();
				States.Clear();
                foreach (var item in parsed)
				{
					var newItem = new PickerItem<int>
					{
						BackgroundValue = item.Id,
						DisplayText = Localization.Instance.GetResource($"Backend.State.{item.Id}")
					};

                    if (item.Id == entry.Order.State.Id)
                    {
                        SelectedState = newItem;
                    }
                    States.Add(newItem);
                }
                OnPropertyChanged(nameof(States));
            }

			var services = await _serviceAPIService.GetAllAsync();
			if (services.IsSuccess)
			{
                var parsed = await services.ParseSuccess();
                Services.Clear();
                foreach (var item in parsed)
				{
                    var newItem = new PickerItem<int>
					{
                        BackgroundValue = item.Id,
                        DisplayText = Localization.Instance.GetResource($"Backend.Service.{item.Id}")
                    };

                    if (item.Id == entry.Order.Service.Id)
					{
                        SelectedService = newItem;
                    }
                    Services.Add(newItem);
                }
                OnPropertyChanged(nameof(Services));
            }
		});

		Entry = entry;
	}
}