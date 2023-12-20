using System.Reflection;

namespace SkiServiceApp.Common
{
    /// <summary>
    /// Should be to interact with ressources
    /// </summary>
    internal class RessourceManager
    {
        /// <summary>
        /// Register Syncfusion license
        /// </summary>
        public static void RegisterSyncfusionLicense()
        {
            var assembly = Assembly.GetExecutingAssembly();
            // a license can be obtained from https://www.syncfusion.com/products/communitylicense
            var resourceName = "SkiServiceApp.SyncfusionLicense.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string licenseKey = reader.ReadToEnd();
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(licenseKey);
            }
        }

    }
}
