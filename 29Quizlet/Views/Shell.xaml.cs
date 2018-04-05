using System.ComponentModel;
using System.Linq;
using System;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using _29Quizlet.Services.SettingsServices;
using Template10.Mvvm;

namespace _29Quizlet.Views
{
    public sealed partial class Shell : Page, INotifyPropertyChanged
    {
        public static Shell Instance { get; set; }
        public static HamburgerMenu HamburgerMenu { get { return Instance.MyHamburgerMenu; } }

        //private string UserName { get; set; }
        private SettingsService SettingsService;
        public event PropertyChangedEventHandler PropertyChanged;

        Uri _profilePictureSrc = new Uri("ms-appx://29Quizlet/Assets/avatar.png");
        public Uri ProfilePictureSrc
        {
            get { return _profilePictureSrc; }
            set
            {
                _profilePictureSrc = value;
                RaisePropertyChanged("ProfilePictureSrc");
            }
        }


        string _UserName = "Login";
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
                RaisePropertyChanged("UserName");
            }
        }

        //Visibility _loginVisibility = Visibility.Visible;
        //public Visibility LoginVisibility
        //{
        //    get { return _loginVisibility; }
        //    set
        //    {
        //        _loginVisibility = value;
        //        RaisePropertyChanged("LoginVisibility");
        //    }
        //}

        //Visibility _userButtonVisibility = Visibility.Collapsed;
        //public Visibility UserButtonVisibility
        //{
        //    get { return _userButtonVisibility; }
        //    set
        //    {
        //        _userButtonVisibility = value;
        //        RaisePropertyChanged("UserButtonVisibility");
        //    }
        //}

        public void UpdateUserImage()
        {
            if (SettingsService.UserSettings != null)
            {
                UserName = SettingsService.UserSettings.UserId;
                ProfilePictureSrc = new Uri(SettingsService.AuthenticatedUser.Profile_Image);
            }
        }

        public Shell()
        {
            Instance = this;
            InitializeComponent();
            SettingsService = SettingsService.Instance;
            if (SettingsService.UserSettings != null)
            {
                UpdateUserImage();    
            }
            LoginButton.IsEnabled = true;
            LoginButton.PageType = typeof(Views.LoginPage);
        }

        public Shell(INavigationService navigationService) : this()
        {
            SetNavigationService(navigationService);
        }

        public void SetNavigationService(INavigationService navigationService)
        {
            MyHamburgerMenu.NavigationService = navigationService;
        }

        #region Login

        private void LoginTapped(object sender, RoutedEventArgs e)
        {
            LoginButton.PageType = typeof(Views.LoginPage);
            
        }

        

        #endregion

        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public void LoggedIn()
        {
            var user = SettingsService.UserSettings;
            if (user == null)
            {
                ProfilePictureSrc = new Uri("ms-appx://29Quizlet/Assets/avatar.png");
                UserName = "Login";
            }
            else
                UserName = user.UserId;

            LoginButton.PageType = typeof(Views.LoginPage);
        }
    }
}

