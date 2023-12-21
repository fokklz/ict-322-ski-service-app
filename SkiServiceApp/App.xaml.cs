using PropertyChanged;
using SkiServiceApp.Common;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Interfaces.API;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SkiServiceApp
{
    public partial class App : Application, INotifyPropertyChanged
    {
        private readonly IServiceProvider _serviceProvider;

        public event PropertyChangedEventHandler? PropertyChanged;
        public bool IsLoggedIn => !string.IsNullOrEmpty(Token);

        [AlsoNotifyFor(nameof(IsLoggedIn))]
        public string? Token { get; set; }

        public AppShell MainAppShell { get; private set; }

        public AppLogin MainAppLogin { get; private set; }

        public App(IServiceProvider serviceProvider, IStorageService storageService)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;

            MainAppShell = serviceProvider.GetService<AppShell>();
            MainAppLogin = serviceProvider.GetService<AppLogin>();
            // ensure the login page will only turn visible with the animation
            // setting opacity to 0 will hide it until the animation is called
# if ANDROID || IOS
            MainAppLogin.Opacity = 0;
#endif

            MainPage = MainAppLogin;

            InitializeApplicationAsync(storageService);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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
        /// Login the user with the given token
        /// </summary>
        /// <param name="token">The token to apply to the application</param>
        public void Login(string token)
        {
            Token = token;
            OnPropertyChanged(nameof(IsLoggedIn));
            _updateAuthorizationHeaderForAllServices(token);
        }

        /// <summary>
        /// Logout the user
        /// </summary>
        public void Logout()
        {
            Token = null;
            OnPropertyChanged(nameof(IsLoggedIn));
            _updateAuthorizationHeaderForAllServices(null);
        }

        /// <summary>
        /// Switch to the main app
        /// </summary>
        /// <returns>Nothing</returns>
        public async Task SwitchToMainApp()
        {
            await _animatePageTransition(MainAppShell, isAppearing: true);
            MainPage = MainAppShell;
            await _animatePageTransition(MainAppShell, isAppearing: false);
        }

        /// <summary>
        /// Switch to the login page
        /// </summary>
        /// <returns>Nothing</returns>
        public async Task SwitchToLogin()
        {
            await _animatePageTransition(MainAppLogin, isAppearing: true);
            MainPage = MainAppLogin;
            await _animatePageTransition(MainAppLogin, isAppearing: false);
        }

        /// <summary>
        /// Update the authorization header for all services that are assignable from BaseApiService
        /// </summary>
        /// <param name="token">The token to set for the services</param>
        private void _updateAuthorizationHeaderForAllServices(string? token)
        {
            Type[] apiServices = {
                typeof(IOrderAPIService),
                typeof(IPriorityAPIService),
                typeof(IServiceAPIService),
                typeof(IStateAPIService),
                typeof(IUserAPIService)
            };

            foreach (var type in apiServices)
            {
                var service = _serviceProvider.GetService(type) as IBaseAPIServiceBase;
                if (service != null)
                {
                    service.SetAuthorizationHeader(token);
                }
            }
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
