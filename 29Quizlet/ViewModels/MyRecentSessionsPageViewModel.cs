using _29Quizlet.Repositories;
using _29Quizlet.Services.SettingsServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using _29Quizlet.Models.QuizletTypes.Feeds;
using Windows.UI.Xaml;
using Windows.ApplicationModel.Resources;
using Microsoft.Practices.Unity;

namespace _29Quizlet.ViewModels
{
    public class MyRecentSessionsPageViewModel : ViewModelBase
    {
        public ObservableCollection<StudySessionItemVM> Sessions { get; set; }
        private readonly IQuizletRESTApi _quizApi;
        private readonly SettingsService _settingsService;
        private readonly ILocalRecentStudySessions _localSessions;
        private readonly ResourceLoader _loader;
        private readonly ISetFetcher _setFetcher;

        public MyRecentSessionsPageViewModel()
        {
            Sessions = new ObservableCollection<StudySessionItemVM>();
            _quizApi = new QuizletRemoteRESTApi();
            _settingsService = SettingsService.Instance;
            _localSessions = App.Container.Resolve<ILocalRecentStudySessions>(); 
            _loader = new ResourceLoader();
            _setFetcher = App.Container.Resolve<ISetFetcher>();
        }

        public string Message { get { return "No recent study sessions found!"; } set { } }

        bool _ShowNothingHere = false;
        public bool ShowNothingHere
        {
            get { return _ShowNothingHere; }
            set { Set(ref _ShowNothingHere, value); }
        }

        bool _ContainsData = false;

        public bool ContainsData
        {
            get { return _ContainsData; }
            set { Set(ref _ContainsData, value); }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var localSessions = await _localSessions.GetAllSessions();

            if (localSessions?.Count() > 0)
            {
                ContainsData = true;
                foreach (var ses in localSessions)
                {
                    Sessions.Add(new StudySessionItemVM(ses));
                }
                return;
            }

            ShowNothingHere = true;

            if (state.Any())
            {
                Sessions = state[nameof(Sessions)] as ObservableCollection<StudySessionItemVM>;
            }
            else
            {
                await UpdateRecentSessions();
            }

        }

        public async Task Selected(object sender, SelectionChangedEventArgs e)
        {
            Views.Busy.SetBusy(true, _loader.GetString("HttpLoading"));
            var session = (StudySessionItemVM)((ListView)sender).SelectedItem;

            var set = await _setFetcher.GetSet(session.SetId);
            
    
            Views.Busy.SetBusy(false, null);
            NavigationService.Navigate(typeof(Views.SetDetailPage), set);
        }

        public async Task Refresh(object sender, RoutedEventArgs e)
        {
            Views.Busy.SetBusy(true, _loader.GetString("HttpLoading"));
            await UpdateRecentSessions();
            Views.Busy.SetBusy(false, null);
        }

        private async Task UpdateRecentSessions()
        {
            try
            {
                if (_settingsService.AuthenticatedUser != null)
                {
                    var latestSessions = await _quizApi.GetPublicStudySessions(_settingsService.AuthenticatedUser);
                    var last = await _quizApi.GetFeedStudySessions();

                    if (last.Items.Any())
                    {
                        ShowNothingHere = false;
                        ContainsData = true;

                        Sessions.Clear();
                        foreach (var session in last.Items)
                        {
                            var studySessionItem = new StudySessionItemVM(session);
                            Sessions.Add(studySessionItem);
                        }
                        await _localSessions.AddStudySessionRange(last.Items);

                    }

                }
            }
            catch (Exception e)
            {
                var dialog = new MessageDialog($"Error: {e.Message}");
                await dialog.ShowAsync();
                return;
            }
        }
    }
}
