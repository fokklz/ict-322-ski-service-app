﻿using SkiServiceApp.Common;
using SkiServiceApp.Common.Helpers;
using System.ComponentModel;
using System.Diagnostics;

namespace SkiServiceApp.Services
{
    public class SettingsService : INotifyPropertyChanged
    {
        /// <summary>
        /// Current language setting
        /// </summary>
        public static string Language { get; set; }

        /// <summary>
        /// Current theme setting
        /// </summary>
        public static string Theme { get; set; }

        /// <summary>
        /// Current cancel in list view setting
        /// </summary>
        public static bool CancelInListView { get; set; }

        /// <summary>
        /// Current always save login setting
        /// </summary>
        public static bool AlwaysSaveLogin { get; set; } 

        /// <summary>
        /// Load settings from local storage (user based)
        /// </summary>
        public static void LoadSettings()
        {
            Language = Preferences.Get($"{SettingsKey.Language}.{AuthManager.UserId}", "Deutsch");
            Theme = Preferences.Get($"{SettingsKey.Theme}.{AuthManager.UserId}", "System");
            CancelInListView = Preferences.Get($"{SettingsKey.CancelInListView}.{AuthManager.UserId}", false);
            AlwaysSaveLogin = Preferences.Get($"{SettingsKey.AlwaysSaveLogin}.{AuthManager.UserId}", false);
        }

        /// <summary>
        /// Set the language setting for the current logged in user
        /// </summary>
        /// <param name="language">The language to set, should be contained in the languageMap in Localization</param>
        public static void SetLanguage(string language)
        {
            Language = language;
            Preferences.Set($"{SettingsKey.Language}.{AuthManager.UserId}", language);
            ApplyLanguage();
        }
        
        /// <summary>
        /// Set the theme setting for the current logged in user
        /// </summary>
        /// <param name="theme">The theme to set, should match the common english Names (Dark, Light, System)</param>
        public static void SetTheme(string theme)
        {
            Theme = theme;
            Debug.WriteLine($"Set theme to {theme} with key {SettingsKey.Theme}.{AuthManager.UserId}");
            Preferences.Set($"{SettingsKey.Theme}.{AuthManager.UserId}", theme);
            ApplyTheme();
        }

        /// <summary>
        /// Set the cancel in list view setting for the current logged in user
        /// </summary>
        /// <param name="cancelInListView">The value to set, should be true when cancel should be shown in the list view</param>
        public static void SetCancelInListView(bool cancelInListView)
        {
            CancelInListView = cancelInListView;
            Preferences.Set($"{SettingsKey.CancelInListView}.{AuthManager.UserId}", cancelInListView);
        }

        /// <summary>
        /// Set the always save login setting for the current logged in user
        /// </summary>
        /// <param name="alwaysSaveLogin">The value to set, should be true to skip the logout dialog</param>
        public static void SetAlwaysSaveLogin(bool alwaysSaveLogin)
        {
            AlwaysSaveLogin = alwaysSaveLogin;
            Preferences.Set($"{SettingsKey.AlwaysSaveLogin}.{AuthManager.UserId}", alwaysSaveLogin);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public static void ApplyTheme()
        {
            var app = Application.Current;
            if (app != null)
            {
                app.UserAppTheme = Theme switch
                {
                    "Dark" => AppTheme.Dark,
                    "Light" => AppTheme.Light,
                    _ => AppTheme.Unspecified
                };
            }
        }

        public static void ApplyLanguage()
        {
            Localization.SetLanguage(Language);
        }

        public static void ApplySettings()
        {
            ApplyTheme();
            ApplyLanguage();
        }
    }
}
