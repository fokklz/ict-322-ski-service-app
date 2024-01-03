using SkiServiceApp.ViewModels;

namespace SkiServiceApp.Views;

public partial class ListPage : ContentPage
{
	public ListPage(ListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    /// <summary>
    /// Ensure the order list is updated when the page is shown.
    /// </summary>
    protected override void OnAppearing()
    {
        var context = BindingContext as ListViewModel;

        base.OnAppearing();
        
        context?.Orders.Update();
    }
}