using SkiServiceApp.ViewModels;

namespace SkiServiceApp.Views;

public partial class UserListPage : ContentPage
{
	public UserListPage(UserListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        OrderList.Update();
    }

}