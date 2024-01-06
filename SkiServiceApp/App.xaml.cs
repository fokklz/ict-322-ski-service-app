using SkiServiceApp.Common;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Interfaces.API;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SkiServiceApp
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public AppShell MainAppShell { get; private set; }

        public AppLogin MainAppLogin { get; private set; }

        public App(IServiceProvider serviceProvider, IStorageService storageService)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;

            MainAppShell = serviceProvider.GetService<AppShell>();
            MainAppLogin = new AppLogin();
            // ensure the login page will only turn visible with the animation
            // setting opacity to 0 will hide it until the animation is called
            MainPage = MainAppLogin;

            Task.Run(async () => await InitializeApplicationAsync(storageService));
        }

        private async Task InitializeApplicationAsync(IStorageService storageService)
        {
            await storageService.InitializeAsync();
            // Ensure UI operations are performed on the main thread
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await SwitchToLogin();
            });
        }

        /// <summary>
        /// Switch to the main app
        /// </summary>
        /// <returns>Nothing</returns>
        public async Task SwitchToMainApp()
        {
            await _animatePageTransition(MainAppShell, isAppearing: true);
            SettingsManager.LoadSettings();
            SettingsManager.ApplySettings();
            MainPage = MainAppShell;
            await _animatePageTransition(MainAppShell, isAppearing: false);
        }

        /// <summary>
        /// Switch to the login page
        /// </summary>
        /// <returns>Nothing</returns>
        public async Task SwitchToLogin()
        {
            MainAppLogin = new AppLogin();
            SettingsManager.LoadSettings();
            SettingsManager.ApplySettings();
            await _animatePageTransition(MainAppLogin, isAppearing: true);
            MainPage = MainAppLogin;
            await _animatePageTransition(MainAppLogin, isAppearing: false);
        }

        

        /// <summary>
        /// Helper to animate the page transition
        /// </summary>
        /// <param name="newPage">The page that should appear next</param>
        /// <param name="isAppearing">If it already has been changed (MainPage)</param>
        /// <returns>Nothing</returns>
        private async Task _animatePageTransition(Page newPage, bool isAppearing)
        {

#if ANDROID || IOS
            if (isAppearing)
            {
                // Fade out and scale down the current page
                if (MainPage != null)
                {
                    await Task.WhenAll(
                        MainPage.FadeTo(0, 250),
                        MainPage.ScaleTo(0.9, 250)
                    );
                }
            }
            else
            {
                newPage.AnchorX = 0.52;
                newPage.AnchorY = 0.52;

                // Reset scale and fade in the new page
                newPage.Opacity = 0;
                newPage.Scale = 0.9;

                await Task.WhenAll(
                    newPage.FadeTo(1, 250),
                    newPage.ScaleTo(1, 250)
                );
            }
#endif
        }
    }
}
