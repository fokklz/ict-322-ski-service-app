using PropertyChanged;
using SkiServiceApp.Common.Events;
using SkiServiceApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace SkiServiceApp.Common
{
    public class Localization : INotifyPropertyChanged
    {
        /// <summary>
        /// All supported languages with their corresponding culture code
        /// </summary>
        public static Dictionary<string, string> LanguageMap = new Dictionary<string, string>
        {
            {"العربية", "ar"},
            {"Deutsch", "de"},
            {"English", "en"},
            {"Español", "es"},
            {"Français", "fr"},
            {"Italiano", "it"},
            {"Nederlands", "nl"},
            {"Polski", "pl"},
            {"Português", "pt"},
            {"Русский", "ru"},
            {"Türkçe", "tr"},
            {"한국어", "ko"},
            {"日本語", "ja"},
            {"中文(简体)", "zh-Hans"},
            {"中文(繁體)", "zh-Hant"},
            {"Svenska", "sv"},
            {"Dansk", "da"},
            {"Suomi", "fi"},
            {"Norsk", "no"},
            {"Čeština", "cs"},
            {"Magyar", "hu"},
            {"Ελληνικά", "el"},
            {"עברית", "he"},
            {"ไทย", "th"},
            {"हिंदी", "hi"},
            {"Български", "bg"},
            {"Română", "ro"},
            {"Українська", "uk"},
            {"Hrvatski", "hr"},
            {"Slovenský", "sk"},
            {"Lietuvių", "lt"},
            {"Slovenščina", "sl"},
            {"Eesti", "et"},
            {"Latviešu", "lv"},
            {"Tiếng Việt", "vi"},
            {"Bahasa Indonesia", "id"},
            {"Filipino", "fil"},
            {"Bahasa Melayu", "ms"}
        };

        /// <summary>
        /// Event that is invoked when the language is changed
        /// </summary>
        public static EventHandler<LanguageChangedEventArgs> LanguageChanged;

        /// <summary>
        /// Singleton instance of the Localization class
        /// </summary>
        public static Localization Instance { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Localization()
        {
            Instance = this;
        }

        protected virtual void OnLanguageChanged(string newLanguage, string code)
        {
            LanguageChanged?.Invoke(this, new LanguageChangedEventArgs(newLanguage, code));
        }

        /// <summary>
        /// Allows to set the language of the app
        /// </summary>
        /// <param name="language">The language to use</param>
        public static void SetLanguage(string language)
        {
            string code;
            if (!LanguageMap.TryGetValue(language, out code))
            {
                code = "de";
            }
            var culture = new CultureInfo(code);
            Instance.CurrentCulture = culture;
            LanguageChanged?.Invoke(Instance, new LanguageChangedEventArgs(language, code));
        }

        /// <summary>
        /// Small helper method to update the titles of the flyout items
        /// since they for some reason don't update automatically with INotifyPropertyChanged
        /// we utilize the automation id to get the correct resource key
        /// </summary>
        private void UpdateFlyoutItemTitles()
        {
            if (Shell.Current?.Items is null)
                return;

            foreach (var item in Shell.Current.Items)
            {
                if (item is FlyoutItem flyoutItem)
                {
                    var id = flyoutItem.AutomationId;
                    if (id is null)
                        continue;
                    flyoutItem.Title = GetResource(id.Replace('_', '.'));
                }
            }
        }

        /// <summary>
        /// Get the resource from the resource manager with the given key
        /// Will return the key if the resource is not found or something goes wrong
        /// </summary>
        /// <param name="key">The key of the value to resolve</param>
        /// <returns>The resolved value as string</returns>
        public string GetResource(string key)
        {
            try
            {
                return Resources.Languages.Resources.ResourceManager.GetString(key, CurrentCulture) ?? key;
            }
            catch (Exception)
            {
                return key;
            }
        }

        /// <summary>
        /// Accessor for the current culture of the app
        /// Also updates the flyout item titles when the culture is changed since they don't update automatically
        /// </summary>
        public CultureInfo CurrentCulture {
            get => Thread.CurrentThread.CurrentUICulture;
            set
            {
                Thread.CurrentThread.CurrentUICulture = value;
                Thread.CurrentThread.CurrentCulture = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentCulture)));
                UpdateFlyoutItemTitles();
            }
        }


        [DependsOn(nameof(CurrentCulture))]
        public ObservableCollection<PickerItem<string>> ThemeDropdown => new ObservableCollection<PickerItem<string>> {
            new PickerItem<string> { DisplayText = Instance.SettingsPage_Theme_System, BackgroundValue = "System" },
            new PickerItem<string> { DisplayText = Instance.SettingsPage_Theme_Dark, BackgroundValue = "Dark" },
            new PickerItem<string> { DisplayText = Instance.SettingsPage_Theme_Light, BackgroundValue = "Light" }
        };

        [DependsOn(nameof(CurrentCulture))]
        public string AppLogin_LastLoginsLabel => GetResource("AppLogin.LastLoginsLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string AppLogin_LastUsedLabel => GetResource("AppLogin.LastUsedLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string AppLogin_PasswordLabel => GetResource("AppLogin.PasswordLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string AppLogin_SigninButton => GetResource("AppLogin.SigninButton");
        [DependsOn(nameof(CurrentCulture))]
        public string AppLogin_SigninLabel => GetResource("AppLogin.SigninLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string AppLogin_UsernameLabel => GetResource("AppLogin.UsernameLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string Dashboard_Chart_You => GetResource("Dashboard.Chart.You");
        [DependsOn(nameof(CurrentCulture))]
        public string Dashboard_Chart_Done => GetResource("Dashboard.Chart.Done");
        [DependsOn(nameof(CurrentCulture))]
        public string Dashboard_Chart_Open => GetResource("Dashboard.Chart.Open");
        [DependsOn(nameof(CurrentCulture))]
        public string Dashboard_Summary_Pre => GetResource("Dashboard.Summary.Pre");
        [DependsOn(nameof(CurrentCulture))]
        public string Dashboard_Summary_Middle => GetResource("Dashboard.Summary.Middle");
        [DependsOn(nameof(CurrentCulture))]
        public string Dashboard_Summary_Sub => GetResource("Dashboard.Summary.Sub");
        [DependsOn(nameof(CurrentCulture))]
        public string Dashboard_SecondarySummary_Pre => GetResource("Dashboard.SecondarySummary.Pre");
        [DependsOn(nameof(CurrentCulture))]
        public string Dashboard_SecondarySummary_Sub => GetResource("Dashboard.SecondarySummary.Sub");
        [DependsOn(nameof(CurrentCulture))]
        public string AppShell_Love => GetResource("AppShell.Love");
        [DependsOn(nameof(CurrentCulture))]
        public string AppShell_Title => GetResource("AppShell.Title"); 
        [DependsOn(nameof(CurrentCulture))]
        public string AppShell_FlyoutItem_DashboardButton => GetResource("AppShell.FlyoutItem.DashboardButton");
        [DependsOn(nameof(CurrentCulture))]
        public string AppShell_FlyoutItem_ListButton => GetResource("AppShell.FlyoutItem.ListButton");
        [DependsOn(nameof(CurrentCulture))]
        public string AppShell_FlyoutItem_LogoutButton => GetResource("AppShell.FlyoutItem.LogoutButton");
        [DependsOn(nameof(CurrentCulture))]
        public string AppShell_FlyoutItem_SettingsButton => GetResource("AppShell.FlyoutItem.SettingsButton");
        [DependsOn(nameof(CurrentCulture))]
        public string AppShell_FlyoutItem_UserListButton => GetResource("AppShell.FlyoutItem.UserListButton");
        [DependsOn(nameof(CurrentCulture))]
        public string AppShell_FlyoutItem_DetailsPageButton => GetResource("AppShell.FlyoutItem.DetailsPageButton");
        [DependsOn(nameof(CurrentCulture))]
        public string CustomListItem_ApplyButton => GetResource("CustomListItem.ApplyButton");
        [DependsOn(nameof(CurrentCulture))]
        public string CustomListItem_ChangeButton => GetResource("CustomListItem.ChangeButton");
        [DependsOn(nameof(CurrentCulture))]
        public string CustomListItem_CancelButton => GetResource("CustomListItem.CancelButton"); 
        [DependsOn(nameof(CurrentCulture))] 
        public string CustomListItem_DaysLeft => GetResource("CustomListItem.DaysLeft");
        [DependsOn(nameof(CurrentCulture))]
        public string CustomListItem_Unassigned => GetResource("CustomListItem.Unassigned");
        [DependsOn(nameof(CurrentCulture))]
        public string OrderDetailPage_CancelButton => GetResource("OrderDetailPage.CancelButton");
        [DependsOn(nameof(CurrentCulture))]
        public string OrderDetailPage_ChangeButton => GetResource("OrderDetailPage.ChangeButton");
        [DependsOn(nameof(CurrentCulture))]
        public string OrderDetailPage_CustomerNameLabel => GetResource("OrderDetailPage.CustomerNameLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string OrderDetailPage_DateLabel => GetResource("OrderDetailPage.DateLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string OrderDetailPage_EmailLabel => GetResource("OrderDetailPage.EmailLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string OrderDetailPage_PhoneLabel => GetResource("OrderDetailPage.PhoneLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string OrderDetailPage_PreDaysLeft => GetResource("OrderDetailPage.PreDaysLeft");
        [DependsOn(nameof(CurrentCulture))]
        public string SettingsPage_AlwaysSaveLoginLabel => GetResource("SettingsPage.AlwaysSaveLoginLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string SettingsPage_CancelInListViewLabel => GetResource("SettingsPage.CancelInListViewLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string SettingsPage_LogoutOnAllDevicesButton => GetResource("SettingsPage.LogoutOnAllDevicesButton");

        [DependsOn(nameof(CurrentCulture))]
        public string Dialog_Cancel => GetResource("Dialog.Cancel"); 
        [DependsOn(nameof(CurrentCulture))]
        public string Dialog_Submit => GetResource("Dialog.Submit");
        [DependsOn(nameof(CurrentCulture))]
        public string Dialog_Title => GetResource("Dialog.Title");
        [DependsOn(nameof(CurrentCulture))]
        public string ModifyDialog_Title => GetResource("ModifyDialog.Title");
        [DependsOn(nameof(CurrentCulture))]
        public string ModifyDialog_Submit => GetResource("ModifyDialog.Submit");
        [DependsOn(nameof(CurrentCulture))]
        public string ModifyDialog_NameLabel => GetResource("ModifyDialog.NameLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string ModifyDialog_EmailLabel => GetResource("ModifyDialog.EmailLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string ModifyDialog_PhoneLabel => GetResource("ModifyDialog.PhoneLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string ModifyDialog_PriorityLabel => GetResource("ModifyDialog.PriorityLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string ModifyDialog_StateLabel => GetResource("ModifyDialog.StateLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string ModifyDialog_ServiceLabel => GetResource("ModifyDialog.ServiceLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string CancelDialog_Title => GetResource("CancelDialog.Title");
        [DependsOn(nameof(CurrentCulture))]
        public string CancelDialog_Submit => GetResource("CancelDialog.Submit");
        [DependsOn(nameof(CurrentCulture))]
        public string LogoutDialog_Title => GetResource("LogoutDialog.Title");
        [DependsOn(nameof(CurrentCulture))]
        public string LogoutDialog_Text => GetResource("LogoutDialog.Text");
        [DependsOn(nameof(CurrentCulture))]
        public string LogoutDialog_Submit => GetResource("LogoutDialog.Submit");
        [DependsOn(nameof(CurrentCulture))]
        public string LogoutDialog_Danger => GetResource("LogoutDialog.Danger");
        [DependsOn(nameof(CurrentCulture))]
        public string CancelDialog_SubTitlePre => GetResource("CancelDialog.SubTitlePre");
        [DependsOn(nameof(CurrentCulture))]
        public string CancelDialog_SubTitleMiddle => GetResource("CancelDialog.SubTitleMiddle");
        [DependsOn(nameof(CurrentCulture))]
        public string CancelDialog_SubTitleSub => GetResource("CancelDialog.SubTitleSub");
        [DependsOn(nameof(CurrentCulture))]
        public string SettingsPage_ThemeLabel => GetResource("SettingsPage.ThemeLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string SettingsPage_Theme_System => GetResource("SettingsPage.Theme.System");
        [DependsOn(nameof(CurrentCulture))]
        public string SettingsPage_Theme_Dark => GetResource("SettingsPage.Theme.Dark");
        [DependsOn(nameof(CurrentCulture))]
        public string SettingsPage_Theme_Light => GetResource("SettingsPage.Theme.Light");
        [DependsOn(nameof(CurrentCulture))]
        public string SettingsPage_LanguageLabel => GetResource("SettingsPage.LanguageLabel");
        [DependsOn(nameof(CurrentCulture))]
        public string Backend_CANNOT_UNLOCK_SELF => GetResource("Backend.CANNOT_UNLOCK_SELF");
        [DependsOn(nameof(CurrentCulture))]
        public string Backend_ENTRY_NOT_FOUND => GetResource("Backend.ENTRY_NOT_FOUND");
        [DependsOn(nameof(CurrentCulture))]
        public string Backend_INVALID_CREDENTIALS => GetResource("Backend.INVALID_CREDENTIALS");
        [DependsOn(nameof(CurrentCulture))]
        public string Backend_USER_LOCKED => GetResource("Backend.USER_LOCKED");
        [DependsOn(nameof(CurrentCulture))]
        public string Backend_USER_NOT_LOCKED => GetResource("Backend.USER_NOT_LOCKED");


    }
}
