namespace SkiServiceApp.Common
{
    /// <summary>
    /// A little hacky helper class to get access to the ServiceProvider from anywhere in the app
    /// Simplyfies the access to services from components out of the DI chain
    /// </summary>
    public class ServiceLocator
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public static T? GetService<T>() => ServiceProvider.GetService<T>();
    }
}
