using _29Quizlet.Helpers;
using _29Quizlet.Helpers.Enums;
using _29Quizlet.Models.Navigation;
using _29Quizlet.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace _29Quizlet.ViewModels
{
    public class LearnSettingsPageViewModel : ViewModelBase
    {
        public List<string> LearningModes { get; set; }
        public DefinitionsOrTerms DefinitionOrTerm { get; set; }
        public LearnSettingsNavigationModel NavigationParameter { get; set; }

        public LearnSettingsPageViewModel()
        {
            LearningModes = Constants.TermsGameModes;
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.ContainsKey(nameof(NavigationParameter)))
            {
                var navParameter = suspensionState[nameof(NavigationParameter)] as LearnSettingsNavigationModel;
                NavigationParameter = navParameter;
            }
            if (parameter != null)
            {
                var navParameter = parameter as LearnSettingsNavigationModel;
                NavigationParameter = navParameter;
            }

            await Task.CompletedTask;
        }

        private DelegateCommand _startGameCommand;
        public DelegateCommand StartGameCommand
        {
            get
            {
                if (_startGameCommand == null)
                {
                    _startGameCommand = new DelegateCommand(() =>
                    {
                        var parameter = new LearnGameNavigationModel()
                        {
                            Mode = DefinitionOrTerm,
                            Terms = NavigationParameter.Terms.ToList(),
                            Set = NavigationParameter.Set,
                            GameRound = 1,
                            Stats = new List<LearnGameStatClass>(),
                            GameTerms = NavigationParameter.Terms.ToList(),
                            TotalSuccessfulTerms = new List<TermViewModel>(),
                            ErrorTerms = new List<TermViewModel>(),
                            SuccesfulTerms = new List<TermViewModel>()
                        };

                        NavigationService.Navigate(typeof(Views.LearnPage), parameter);
                    });

                }

                return _startGameCommand;

            }
        }
    }

}
