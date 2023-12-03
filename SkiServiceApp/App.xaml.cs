namespace SkiServiceApp
{
    public partial class App : Application
    {
        public AppShell MainAppShell { get; private set; }

        public AppLogin MainAppLogin { get; private set; }

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            MainAppShell = serviceProvider.GetService<AppShell>();
            MainAppLogin = serviceProvider.GetService<AppLogin>();

            MainPage = MainAppLogin;
        }
        
        /// <summary>
        /// Switch the MainPage to the AppShell
        /// </summary>
        public void SwitchToMainApp()
        {
            MainPage = MainAppShell;
        }

        /// <summary>
        /// Switch the MainPage to the AppLogin
        /// </summary>
        public void SwitchToLogin()
        {
            MainPage = MainAppLogin;
        }
    }
}
