using Microsoft.Extensions.DependencyInjection;
using SkiServiceApp.Common;
using SkiServiceApp.ViewModels;
using SkiServiceApp.Views;

namespace SkiServiceApp
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel viewModel)
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(OrderDetailPage), typeof(OrderDetailPage));
            BindingContext = viewModel;
        }
    }
}