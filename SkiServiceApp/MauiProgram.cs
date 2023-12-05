using Microsoft.Extensions.Logging;
using SkiServiceApp.Common;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Services;
using SkiServiceApp.Services.API;
using SkiServiceApp.ViewModels;
using SkiServiceApp.Views;
using Syncfusion.Maui.Core.Hosting;

namespace SkiServiceApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            RessourceManager.RegisterSyncfusionLicense();

            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Font Awesome 6 Free-Regular-400.otf", "FARegular");
                    fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FASolid");
                    fonts.AddFont("Font Awesome 6 Brands-Regular-400.otf", "FABrands");
                });


            builder.Services.AddSingleton<IStorageService, StorageService>();
            // all api services need the auth service
            builder.Services.AddSingleton<IAuthService, AuthService>();

            // services without special dependencies
            builder.Services.AddSingleton<IServiceAPIService, ServiceAPIService>();
            builder.Services.AddSingleton<IPriorityAPIService, PriorityAPIService>();
            builder.Services.AddSingleton<IStateAPIService, StateAPIService>();

            // service with dependencies on the above services
            builder.Services.AddSingleton<IUserAPIService, UserAPIService>();
            builder.Services.AddSingleton<IOrderAPIService, OrderAPIService>();

            builder.Services.AddSingleton<AppShellViewModel>();
            builder.Services.AddSingleton<AppShell>();

            builder.Services.AddSingleton<AppLoginViewModel>();
            builder.Services.AddSingleton<AppLogin>();

            builder.Services.AddSingleton<DashboardViewModel>();
            builder.Services.AddSingleton<DashboardPage>();

            builder.Services.AddSingleton<ListViewModel>();
            builder.Services.AddSingleton<ListPage>();

            builder.Services.AddSingleton<UserListViewModel>();
            builder.Services.AddSingleton<UserListPage>();

            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<SettingsPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
