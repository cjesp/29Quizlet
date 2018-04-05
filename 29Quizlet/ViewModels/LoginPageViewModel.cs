using _29Quizlet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using _29Quizlet.Services.SettingsServices;
using Windows.UI.Xaml.Navigation;
using _29Quizlet.Models.Navigation;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using _29Quizlet.Models.QuizletTypes;
using _29Quizlet.Repositories;
using Windows.UI.Popups;
using _29Quizlet.Helpers;
using Windows.UI.Xaml;
using Windows.System;

namespace _29Quizlet.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private SettingsService _SettingsService;
        private UserSettings _user;
        private LoginPageNavigationModel NavigationParameter;
        private HttpClient _client;
        private IQuizletRESTApi _quizletAPI;


        Uri _profilePictureSrc = new Uri("ms-appx://29Quizlet/Assets/avatar.png");
        public Uri ProfilePictureSrc
        {
            get { return _profilePictureSrc; }
            set { Set(ref _profilePictureSrc, value); }
        }


        string _UserId = default(string);
        public string UserId { get { return _UserId; } set { Set(ref _UserId, value); } }

        string _AccountType = default(string);
        public string AccountType { get { return _AccountType; } set { Set(ref _AccountType, value); } }

        string _TermsCreated = default(string);
        public string TermsCreated { get { return _TermsCreated; } set { Set(ref _TermsCreated, value); } }

        string _SetsCreated = default(string);
        public string SetsCreated { get { return _SetsCreated; } set { Set(ref _SetsCreated, value); } }
        
        string _LogInOutText = default(string);
        public string LogInOutText { get { return _LogInOutText; } set { Set(ref _LogInOutText, value); } }

        public LoginPageViewModel()
        {
            _SettingsService = Services.SettingsServices.SettingsService.Instance;
            _client = new HttpClient();
            _quizletAPI = new QuizletRemoteRESTApi();
            LogInOutText = "Log in";
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.ContainsKey(nameof(NavigationParameter)))
            {
                var navParameter = suspensionState[nameof(NavigationParameter)] as LoginPageNavigationModel;
                NavigationParameter = navParameter;
            }
            if (parameter != null)
            {
                var zero = parameter as string;
                if (zero == null)
                {
                    var navParameter = parameter as LoginPageNavigationModel;
                    NavigationParameter = navParameter;
                    
                }
            }

            if (NavigationParameter != null && (!string.IsNullOrEmpty(NavigationParameter.Error) || !string.IsNullOrEmpty(NavigationParameter.ErrorDescription)))
            {
                // this means that login created an error
            }
            if (NavigationParameter != null && !string.IsNullOrEmpty(NavigationParameter.Code))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "XXX");
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.quizlet.com/oauth/token");

                var formData = new List<KeyValuePair<string, string>>();
                formData.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
                formData.Add(new KeyValuePair<string, string>("code", NavigationParameter.Code));

                request.Content = new FormUrlEncodedContent(formData);

                var response = await _client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonConvert.DeserializeObject<Authentication.Success>(content);
                    var userSettings = new UserSettings()
                    {
                        UserId = json.user_id,
                        Token = json.access_token,
                        Scope = json.scope,
                        TokenType = json.token_type,
                        Expiration = json.expires_in,
                        Created = DateTime.UtcNow
                    };

                    _SettingsService.UserSettings = userSettings;
                    LogInOutText = "Log out";
                    await FetchAndSaveUser(userSettings.UserId);
                    
                }
                else
                {
                    //todo: there was an error
                }
            }

            // load the page with data... 
            if (_SettingsService.UserSettings != null)
            {
                UserId = _SettingsService.UserSettings.UserId;
                LogInOutText = "Log out";
                try
                {
                    var user = await _quizletAPI.GetUser(UserId);

                    if (user.Profile_Image != null)
                        ProfilePictureSrc = new Uri(user.Profile_Image);

                    if (user.AccountType != null)
                        AccountType = user.AccountType;

                    if (user.Statistics != null)
                        TermsCreated = user.Statistics.PublicTermsEntered.ToString();

                    if (user.Statistics != null)
                        SetsCreated = user.Statistics.PublicSetsCreated.ToString();

                    // update the shell to reflect the new authenticated user
                    Views.Shell.Instance.UpdateUserImage();

                }
                catch (Exception e)
                {
                    var dialog = new MessageDialog($"Error: {e.Message}");
                    await dialog.ShowAsync();
                    return;
                }
            }

            if (_SettingsService.AuthenticatedUser != null)
            {
                LogInOutText = "Log out";
            }

            await Task.CompletedTask;
        }

        private async Task FetchAndSaveUser(string userId)
        {
            try
            {
                var user = await _quizletAPI.GetUser(userId);
                _SettingsService.AuthenticatedUser = user;
            }
            catch (Exception e)
            {
                var dialog = new MessageDialog($"Error: {e.Message}");
                await dialog.ShowAsync();
                return;
            }
        }

        public void LogOut(object sender, RoutedEventArgs e)
        {
            if (LogInOutText == "Log out")
            {
                _SettingsService.UserSettings = null;
                _SettingsService.AuthenticatedUser = null;
                LogInOutText = "Log in";
            }
            else
            {
                var guid = Guid.NewGuid().ToString();
                var uri = new Uri($"https://quizlet.com/authorize?response_type=code&client_id=XXXXX&scope=read%20write_set%20write_group&state={guid}");
                Task.Run(() => Launcher.LaunchUriAsync(uri));
            }
        }
    }
}
