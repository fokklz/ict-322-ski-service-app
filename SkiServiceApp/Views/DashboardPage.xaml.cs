using SkiServiceApp.Common;
using SkiServiceApp.Services;
using SkiServiceApp.ViewModels;
using SkiServiceApp.ViewModels.Charts;

namespace SkiServiceApp.Views;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        OrderList.Update();
    }

	protected override void OnAppearing()
	{
        base.OnAppearing();
		DashboardChartViewModel.Update.Invoke();
    }
}