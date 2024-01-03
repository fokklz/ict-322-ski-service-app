using SkiServiceApp.ViewModels;

namespace SkiServiceApp.Views;

public partial class UserListPage : ContentPage
{
	public UserListPage(UserListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }

    /// <summary>
    /// Ensure the order list is updated when the page is shown.
    /// </summary>
    protected override void OnAppearing()
    {
        var context = BindingContext as UserListViewModel;

        base.OnAppearing();

        context?.Orders.Update();
    }

}