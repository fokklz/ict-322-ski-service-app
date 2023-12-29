using SkiServiceApp.ViewModels;

namespace SkiServiceApp.Views;

public partial class ListPage : ContentPage
{
	public ListPage(ListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        OrderList.Update();
    }

    /*
     * should be used instead since it will ensure more accurate data
     - because of lags and pretty big performance impact we just do it once atm
    protected override void OnAppearing()
    {
        base.OnAppearing();
        OrderList.Update();
    }*/
}