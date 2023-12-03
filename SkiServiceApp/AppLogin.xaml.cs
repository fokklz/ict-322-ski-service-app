using SkiServiceApp.Interfaces.API;
using SkiServiceApp.ViewModels;

namespace SkiServiceApp;

public partial class AppLogin : ContentPage
{
	public AppLogin(IUserAPIService userAPIService)
	{
		InitializeComponent();
		BindingContext = new AppLoginViewModel(userAPIService);
	}

    /// <summary>
    /// Reset the login state when the page appears.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AppLoginViewModel viewModel)
        {
            viewModel.ResetLoginState();
        }
    }
}