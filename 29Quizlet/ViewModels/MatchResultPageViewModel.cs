using _29Quizlet.Commands;
using _29Quizlet.Models.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace _29Quizlet.ViewModels
{
    public class MatchResultPageViewModel : ViewModelBase
    {
        public MatchResultPageNavigationModel NavigationParameter { get; set; }
        public string TimeResult { get; set; }
        public string Errors { get; set; }

        public MatchResultPageViewModel()
        {

        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.ContainsKey(nameof(NavigationParameter)))
            {
                NavigationParameter = suspensionState[nameof(NavigationParameter)] as MatchResultPageNavigationModel;
                TimeResult = $"{NavigationParameter.TimeResult} seconds";
                Errors = $"with {NavigationParameter.Errors} errors.";
            }

            if (parameter != null)
            {
                NavigationParameter = parameter as MatchResultPageNavigationModel;
                TimeResult = $"{NavigationParameter.TimeResult} seconds";
                Errors = $"with {NavigationParameter.Errors} errors.";
            }


            await Task.CompletedTask;
        }

        private DelegateCommand _playAgainCommand;
        public DelegateCommand PlayAgainCommand
        {
            get
            {
                if (_playAgainCommand == null)
                {
                    _playAgainCommand = new DelegateCommand(() =>
                    {
                        if (NavigationService.CanGoBack)
                        {
                            NavigationService.GoBack();
                        }
                    });

                }

                return _playAgainCommand;

            }
        }

        private DelegateCommand _goBackCommand;
        public DelegateCommand GoBackCommand
        {
            get
            {
                if (_goBackCommand == null)
                {
                    _goBackCommand = new DelegateCommand(() =>
                    {
                        NavigationService.Navigate(typeof(Views.SetDetailPage), NavigationParameter.Set);
                        NavigationService.Frame.BackStack.Clear();
                    });

                }

                return _goBackCommand;

            }
        }
    }
}
