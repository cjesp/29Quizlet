using _29Quizlet.Models;
using _29Quizlet.Models.QuizletTypes.User;
using _29Quizlet.Models.Settings;
using _29Quizlet.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Template10.Common;
using Template10.Mvvm;
using Template10.Utils;
using Windows.Storage;
using Windows.UI.Xaml;

namespace _29Quizlet.Services.SettingsServices
{
    public class SettingsService
    {
        private ApplicationDataContainer LocalSettings;
        public static SettingsService Instance { get; }
        static SettingsService()
        {
            // implement singleton pattern
            Instance = Instance ?? new SettingsService();
        }

        Template10.Services.SettingsService.ISettingsHelper _helper;
        private SettingsService()
        {
            _helper = new Template10.Services.SettingsService.SettingsHelper();
            LocalSettings = ApplicationData.Current.LocalSettings;
            
        }

        public bool UseShellBackButton
        {
            get { return _helper.Read<bool>(nameof(UseShellBackButton), true); }
            set
            {
                _helper.Write(nameof(UseShellBackButton), value);
                BootStrapper.Current.NavigationService.Dispatcher.Dispatch(() =>
                {
                    BootStrapper.Current.ShowShellBackButton = value;
                    BootStrapper.Current.UpdateShellBackButton();
                    BootStrapper.Current.NavigationService.Refresh();
                });
            }
        }

        public SetsSettings SetSettings
        {
            get
            {
                var value = _helper.Read<SetsSettings>(nameof(SetSettings), SetsSettings.GetDefaults());
                return value;
            }
            set
            {
                _helper.Write(nameof(SetSettings), value);
            }
        }

        public ApplicationTheme AppTheme
        {
            get
            {
                var theme = ApplicationTheme.Dark;
                var value = _helper.Read<string>(nameof(AppTheme), theme.ToString());
                return Enum.TryParse<ApplicationTheme>(value, out theme) ? theme : ApplicationTheme.Dark;
            }
            set
            {
                _helper.Write(nameof(AppTheme), value.ToString());
                (Window.Current.Content as FrameworkElement).RequestedTheme = value.ToElementTheme();
                Views.Shell.HamburgerMenu.RefreshStyles(value);
            }
        }

        public TimeSpan CacheMaxDuration
        {
            get { return _helper.Read<TimeSpan>(nameof(CacheMaxDuration), TimeSpan.FromDays(2)); }
            set
            {
                _helper.Write(nameof(CacheMaxDuration), value);
                BootStrapper.Current.CacheMaxDuration = value;
            }
        }

        public User AuthenticatedUser
        {
            get { return _helper.Read<User>(nameof(AuthenticatedUser), null); }
            set
            {
                _helper.Write(nameof(AuthenticatedUser), value);
                //BootStrapper.Current.CacheMaxDuration = value;
            }
        }


        UserSettings _userSettings = default(UserSettings);
        public UserSettings UserSettings
        {
            get
            {
                if (_userSettings == null)
                {
                    var savedUserSettings = (string)LocalSettings.Values["UserSettings"];
                    try
                    {
                        var settings = JsonConvert.DeserializeObject<UserSettings>(savedUserSettings);
                        _userSettings = settings;
                    }
                    catch (ArgumentNullException)
                    {
                        // means we havent anything stored
                    }
                }

                return _userSettings;
            }
            set
            {
                var json = JsonConvert.SerializeObject(value);

                _userSettings = value;
                LocalSettings.Values["UserSettings"] = json;
                Views.Shell.Instance.LoggedIn();
            }
        }

        public bool ShowHamburgerButton
        {
            get { return _helper.Read<bool>(nameof(ShowHamburgerButton), true); }
            set
            {
                _helper.Write(nameof(ShowHamburgerButton), value);
                Views.Shell.HamburgerMenu.HamburgerButtonVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public bool IsFullScreen
        {
            get { return _helper.Read<bool>(nameof(IsFullScreen), false); }
            set
            {
                _helper.Write(nameof(IsFullScreen), value);
                Views.Shell.HamburgerMenu.IsFullScreen = value;
            }
        }

        public IList<int> UserClasses
        {
            get { return _helper.Read<IList<int>>(nameof(UserClasses), new List<int>()); }

            set
            {
                _helper.Write(nameof(UserClasses), value);
            }
        }
    }
}

