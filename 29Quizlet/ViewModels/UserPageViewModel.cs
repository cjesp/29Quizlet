using _29Quizlet.Commands;
using _29Quizlet.Models.QuizletTypes.User;
using _29Quizlet.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources;

namespace _29Quizlet.ViewModels
{
    public class UserPageViewModel : ViewModelBase
    {

        private IQuizletRESTApi _quizApi;
        private User _user;
        private ResourceLoader _loader;

        public UserPageViewModel()
        {
            _quizApi = new QuizletRemoteRESTApi();
            _loader = new ResourceLoader();
        }

        Uri _profilePictureSrc = new Uri("ms-appx://29Quizlet/Assets/avatar.png");
        public Uri ProfilePictureSrc
        {
            get { return _profilePictureSrc; }
            set { Set(ref _profilePictureSrc, value); }
        }

        string _AccountType = "N/A";
        public string AccountType { get { return _AccountType; } set { Set(ref _AccountType, value); } }

        string _TermsCreated = "N/A";
        public string TermsCreated { get { return _TermsCreated; } set { Set(ref _TermsCreated, value); } }

        string _SetsCreated = "N/A";
        public string SetsCreated { get { return _SetsCreated; } set { Set(ref _SetsCreated, value); } }

        string _StudySessionCount = "N/A";
        public string StudySessionCount { get { return _StudySessionCount; } set { Set(ref _StudySessionCount, value); } }

        string _TotalAnswerCount = "N/A";
        public string TotalAnswerCount { get { return _TotalAnswerCount; } set { Set(ref _TotalAnswerCount, value); } }

        string _UserName = default(string);
        public string UserName
        {
            get { return _UserName; }
            set { Set(ref _UserName, value); }
        }

        private DelegateCommander _UserNavButton;

        public DelegateCommander UserNavButton
        {
            get
            {
                if (_UserNavButton == null)
                {
                    _UserNavButton = new DelegateCommander(async (input) =>
                    {
                        var index = int.Parse(input as string);
                        
                        try
                        {
                            if (index == 0)
                            {
                                // navigate to recent study sessions
                                Views.Busy.SetBusy(true, _loader.GetString("HttpLoading"));
                                var sessions = await _quizApi.GetPublicStudySessions(_user);
                                Views.Busy.SetBusy(false, null);
                                NavigationService.Navigate(typeof(Views.RecentSessionsPage), sessions);
                            }
                            if (index == 1)
                            {
                                // navigate to users classes
                                Views.Busy.SetBusy(true, _loader.GetString("HttpLoading"));
                                var classes = await _quizApi.GetClasses(_user);
                                Views.Busy.SetBusy(false, null);
                                NavigationService.Navigate(typeof(Views.ClassesPage), classes);
                            }
                            if (index == 2)
                            {
                                Views.Busy.SetBusy(true, _loader.GetString("HttpLoading"));
                                var sets = await _quizApi.GetSets(_user);
                                Views.Busy.SetBusy(false, null);
                                NavigationService.Navigate(typeof(Views.SetsPage), sets);
                            }
                        }
                        catch (Exception e)
                        {
                            var dialog = new MessageDialog($"Error: {e.Message}");
                            await dialog.ShowAsync();
                            return;
                        }
                        return;
                    });

                }
                
                return _UserNavButton;
                
            }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var user = parameter as User;

            if (user != null)
            {
                UserName = user.Username;
                ProfilePictureSrc = new Uri(user.Profile_Image);
                AccountType = user.AccountType ?? "N/A";
                TermsCreated = $"{user.Statistics.PublicTermsEntered}";
                SetsCreated = $"{user.Statistics.PublicSetsCreated}";
                StudySessionCount = $"{user.Statistics.StudySessionCount}";
                TotalAnswerCount = $"{user.Statistics.TotalAnswerCount}";
            }

            _user = user;

            return Task.CompletedTask;
        }
    }
}
