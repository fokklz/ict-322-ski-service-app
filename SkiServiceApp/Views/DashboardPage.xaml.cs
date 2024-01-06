using SkiServiceApp.Common;
using SkiServiceApp.Interfaces;
using SkiServiceApp.ViewModels;
using SkiServiceApp.ViewModels.Charts;

namespace SkiServiceApp.Views;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }

	/// <summary>
	/// Ensure the chart is updated when the page is shown as well as the order list.
	/// </summary>
	protected override void OnAppearing()
    {
        var context = BindingContext as DashboardViewModel;

        base.OnAppearing();
        context?.Orders.Update();
        ServiceLocator.GetService<ISearchService>().ClearSearch();
        DashboardChartViewModel.Update?.Invoke();
    }
}   