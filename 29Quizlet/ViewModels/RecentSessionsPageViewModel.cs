using _29Quizlet.Models.QuizletTypes.User;
using _29Quizlet.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources;

namespace _29Quizlet.ViewModels
{
    public class RecentSessionsPageViewModel : ViewModelBase
    {
        public ObservableCollection<StudySessionViewModel> Sessions { get; set; }
        private IQuizletRESTApi _quizApi;
        private ResourceLoader _loader;

        public RecentSessionsPageViewModel()
        {
            Sessions = new ObservableCollection<StudySessionViewModel>();
            _quizApi = new QuizletRemoteRESTApi();
            _loader = new ResourceLoader();
        }

        public string Message { get { return "No sessions found!"; } set { } }

        bool _ShowNothingHere = true;
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
            var sessions = parameter as List<StudySession>;

            if (sessions != null && sessions.Any())
            {
                ShowNothingHere = false;
                ContainsData = true;

                foreach (var session in sessions)
                {
                    Sessions.Add(new StudySessionViewModel(session));
                }
            }
            

            await Task.CompletedTask;
        }

        public async Task Selected(object sender, SelectionChangedEventArgs e)
        {
            var session = (StudySessionViewModel)((ListView)sender).SelectedItem;
            Views.Busy.SetBusy(true, _loader.GetString("HttpLoading"));
            var set = await _quizApi.GetPublicSet(session.Id);
            Views.Busy.SetBusy(false, null);
            NavigationService.Navigate(typeof(Views.SetDetailPage), set);
        }
    }
}
