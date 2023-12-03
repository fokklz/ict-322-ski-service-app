using Microsoft.Extensions.Logging;
using SkiServiceApp.Common;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Services;
using SkiServiceApp.Services.API;
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

            // services without special dependencies
            builder.Services.AddSingleton<IServiceAPIService, ServiceAPIService>();
            builder.Services.AddSingleton<IPriorityAPIService, PriorityAPIService>();
            builder.Services.AddSingleton<IStateAPIService, StateAPIService>();

            // service with dependencies on the above services
            builder.Services.AddSingleton<IUserAPIService, UserAPIService>();
            builder.Services.AddSingleton<IOrderAPIService, OrderAPIService>();

            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<AppLogin>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
