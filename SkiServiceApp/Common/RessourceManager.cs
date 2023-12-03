using System.Reflection;

namespace SkiServiceApp.Common
{
    internal class RessourceManager
    {
        public static void RegisterSyncfusionLicense()
        {
            var assembly = Assembly.GetExecutingAssembly();
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
