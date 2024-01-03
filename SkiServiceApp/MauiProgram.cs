using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SkiServiceApp.Common;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Services;
using SkiServiceApp.Services.API;
using SkiServiceApp.ViewModels;
using SkiServiceApp.Views;
using Syncfusion.Maui.Core.Hosting;

#if ANDROID
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif

namespace SkiServiceApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            RessourceManager.RegisterSyncfusionLicense();
            builder.UseMauiApp<App>().ConfigureSyncfusionCore().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Font Awesome 6 Free-Regular-400.otf", "FARegular");
                fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FASolid");
                fonts.AddFont("Font Awesome 6 Brands-Regular-400.otf", "FABrands");
            }).UseMauiCommunityToolkit();
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

            builder.Services.AddSingleton<DialogService>();
            builder.Services.AddSingleton<SettingsService>();

            builder.Services.AddSingleton<AppShellViewModel>();
            builder.Services.AddSingleton<AppShell>();

            builder.Services.AddSingleton<AppLoginViewModel>();

            builder.Services.AddSingleton<DashboardViewModel>();
            builder.Services.AddSingleton<DashboardPage>();

            builder.Services.AddSingleton<ListViewModel>();
            builder.Services.AddSingleton<ListPage>();

            builder.Services.AddSingleton<UserListViewModel>();
            builder.Services.AddSingleton<UserListPage>();

            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<SettingsPage>();

            builder.Services.AddSingleton<OrderDetailViewModel>();
            builder.Services.AddSingleton<OrderDetailPage>();

            // to remove the switch labels on win ui since they don't support multi language -> fk microsoft
#if WINDOWS
            Microsoft.Maui.Handlers.SwitchHandler.Mapper.AppendToMapping("NoLabel", (handler, view) =>
            {
                handler.PlatformView.OnContent = null;
                handler.PlatformView.OffContent = null;
                handler.PlatformView.MinWidth = 0;
            });

#endif

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.Background = null;
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
#elif IOS
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });

            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.Background = null;
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
#elif IOS
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            // allow for service out of DI-chain to access the service provider
            ServiceLocator.Initialize(app.Services);

            return app;
        }

    }
}