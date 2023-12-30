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
    }

	protected override void OnAppearing()
	{
        base.OnAppearing();
        OrderList.Update();
        DashboardChartViewModel.Update.Invoke();
    }
}