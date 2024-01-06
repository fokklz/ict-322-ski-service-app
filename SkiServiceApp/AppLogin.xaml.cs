using SkiServiceApp.Common;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.ViewModels;

namespace SkiServiceApp;

public partial class AppLogin : ContentPage
{
	public AppLogin()
	{
		InitializeComponent();
        BindingContextChanged += OnBindingContextChanged;
        BindingContext = new AppLoginViewModel();
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

    /// <summary>
    /// Unfocus the control when the binding context changes.
    /// </summary>
    /// <param name="sender">The sender of the Event</param>
    /// <param name="e">The params of the Event</param>
    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        UnfocusMe.Unfocus();
    }
}