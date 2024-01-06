using System.Diagnostics;
using System.Windows.Input;

namespace SkiServiceApp.Components;

public partial class CustomFlyoutItem : ContentView
{
    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(string), typeof(CustomFlyoutItem));

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomFlyoutItem));

    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(CustomFlyoutItem), defaultValue: "FASolid");

    public static readonly BindableProperty RouteProperty =
        BindableProperty.Create(nameof(Route), typeof(string), typeof(CustomFlyoutItem), defaultValue: null);

    public static readonly BindableProperty CustomCommandProperty =
        BindableProperty.Create(nameof(CustomCommand), typeof(ICommand), typeof(CustomFlyoutItem), defaultValue: null, propertyChanged: OnIsCustomCommandChanged);

    static void OnIsCustomCommandChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue != null)
        {
            var control = (CustomFlyoutItem)bindable;
            control.NavigateCommand = (ICommand)newValue;
        }
    }

    public ICommand NavigateCommand;

    public ICommand CustomCommand
    {
        get => (ICommand)GetValue(CustomCommandProperty);
        set => SetValue(CustomCommandProperty, value);
    }

    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public string Route
    {
        get => (string)GetValue(RouteProperty);
        set => SetValue(RouteProperty, value);
    }

    public CustomFlyoutItem()
	{
        InitializeComponent();
        NavigateCommand = new Command(Navigate);
    }

    private async void Navigate()
    {
        if (!string.IsNullOrEmpty(Route))
        {
            try
            {
                // '//' sets to root of navigation stack
                await Shell.Current.GoToAsync($"//{Route}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        // keep UI work on main thread
        MainThread.BeginInvokeOnMainThread(() => {
            Shell.Current.FlyoutIsPresented = false;
        });
        NavigateCommand.Execute(null);
    }
}