using SkiServiceApp.ViewModels;

namespace SkiServiceApp.Views;

public partial class Dashboard : ContentPage
{
	public Dashboard()
	{
		InitializeComponent();
		BindingContext = new DashboardViewModel();
	}
}