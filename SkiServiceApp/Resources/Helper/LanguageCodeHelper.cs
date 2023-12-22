using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiServiceApp.Resources.Helper
{
    public class LanguageCodeHelper
    {
        public static string GetLanguageCode(string languageName)
        {
            var languageMap = new Dictionary<string, string>
            {
                {"عربي", "ar"},
                {"Deutsch", "de"},
                {"English", "en"},
                {"Español", "es"},
                {"Français", "fr"},
                {"Italiano", "it"},
                {"Nederlands", "nl"},
                {"Polski", "pl"},
                {"Português", "pt"},
                {"Русский", "ru"},
                {"Türkçe", "tr"}
            };

            return languageMap.TryGetValue(languageName, out var code) ? code : "de"; // Default to German if not found
        }
    }
}
