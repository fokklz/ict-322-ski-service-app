using Microsoft.Extensions.DependencyInjection;
using SkiServiceApp.Common;
using SkiServiceApp.ViewModels;

namespace SkiServiceApp
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}