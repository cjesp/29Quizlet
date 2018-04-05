using Windows.UI.Xaml;
using System.Threading.Tasks;
using _29Quizlet.Services.SettingsServices;
using Windows.ApplicationModel.Activation;
using Template10.Controls;
using Template10.Common;
using StructureMap;
using _29Quizlet.Repositories;
using _29Quizlet.Models.Settings;
using System.Linq;
using _29Quizlet.Models.Navigation;
using _29Quizlet.Models;
using Windows.Foundation;
using _29Quizlet.Helpers;
using Windows.UI.ViewManagement;
using Microsoft.Practices.Unity;

namespace _29Quizlet
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    sealed partial class App : Template10.Common.BootStrapper
    {
        private SettingsService SettingsService;

        public static Container StructMap { get; set; }
        public static SetsSettings SetSettings { get; set; }
        public static UserSettings UserSettings { get; set; }
        public static UnityContainer Container { get; set; }
        //public static Size ScreenResolutionSize;

        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            Container = new UnityContainer();
            Container.RegisterType<IQuizletRESTApi, QuizletRemoteRESTApi>();
            
            Container.RegisterInstance<IQuizletRESTApi>(new QuizletRemoteRESTApi())
                .RegisterInstance<ILocalRecentStudySessions>(new RecentStudySessionsRepository())
                .RegisterInstance<ISetsLocalStorage>(new SetsLocalStorageRepository())
                .RegisterInstance<ILocalClassesStorage>(new ClassesLocalStorageRepository())
                .RegisterType<ISetFetcher, SetFetcher>();

            


            #region App settings

            SettingsService = SettingsService.Instance;
            RequestedTheme = SettingsService.AppTheme;
            CacheMaxDuration = SettingsService.CacheMaxDuration;
            ShowShellBackButton = SettingsService.UseShellBackButton;
            SetSettings = SettingsService.SetSettings;
            UserSettings = SettingsService.UserSettings;
            

            #endregion
        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {

            // content may already be shell when resuming
            if ((Window.Current.Content as ModalDialog) == null)
            {
                // setup hamburger shell inside a modal dialog
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
                Window.Current.Content = new ModalDialog
                {
                    DisableBackButtonWhenModal = true,
                    Content = new Views.Shell(nav),
                    ModalContent = new Views.Busy(),
                };
            }
            

            await Task.CompletedTask;
        }

        public override Task OnPrelaunchAsync(IActivatedEventArgs args, out bool runOnStartAsync)
        {
            return base.OnPrelaunchAsync(args, out runOnStartAsync);
        }

        public override void OnResuming(object s, object e, AppExecutionState previousExecutionState)
        {
            base.OnResuming(s, e, previousExecutionState);
        }


        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            

            // long-running startup tasks go here
            var activated = false;

            var kind = args as ProtocolActivatedEventArgs;
            if (kind != null)
            {
                activated = true;

                var results = kind.Uri.Query;

                var code = string.Empty;
                var state = string.Empty;
                var expires = string.Empty;
                var error = string.Empty;
                var errorDescription = string.Empty;
                foreach (var arg in results.Split('&'))
                {
                    string[] parts = arg.Replace("?", string.Empty).Split('=');
                    if (parts[0] == "code")
                    {
                        code = parts[1];
                    }
                    if (parts[0] == "state")
                    {
                        state = parts[1];
                    }
                    if (parts[0] == "expires")
                    { 
                        expires = parts[1];
                    }
                    if (parts[0] == "error")
                    {
                        error = parts[1];
                    }
                    if (parts[0] == "errorDescription")
                    {
                        errorDescription = parts[1];
                    }
                }

                var parameter = new LoginPageNavigationModel()
                {
                    Code = code,
                    State = state,
                    Expires = expires,
                    Error = error,
                    ErrorDescription = errorDescription
                };

                SettingsService.IsFullScreen = false;
                SettingsService.ShowHamburgerButton = true;
                NavigationService.Navigate(typeof(Views.LoginPage), parameter);
            }

            if (!activated)
            {
                if (SettingsService.AuthenticatedUser != null)
                {
                    SettingsService.IsFullScreen = false;
                    SettingsService.ShowHamburgerButton = true;
                    NavigationService.Navigate(typeof(Views.MyRecentSessionsPage));
                }
                else
                {
                    SettingsService.IsFullScreen = true;
                    SettingsService.ShowHamburgerButton = false;
                    NavigationService.Navigate(typeof(Views.LoginSignupPage));
                }

            }
            await Task.CompletedTask;
        }
        
    }
}

