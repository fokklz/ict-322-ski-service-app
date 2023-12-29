using SkiServiceApp.ViewModels;

namespace SkiServiceApp.Views;

public partial class ListPage : ContentPage
{
	public ListPage(ListViewModel viewModel)
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