using _29Quizlet.Helpers;
using _29Quizlet.Helpers.Enums;
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
    

    public class CardSettingsPageViewModel : ViewModelBase
    {
        public DefinitionsOrTerms DefinitionsOrTerm { get; set; }
        public SortingModes SortingMode { get; set; }
        public CardSettingsNavigationModel NavigationParameter { get; set; }

        public string SelectedMode { get; set; }

        public List<string> PossibleModes { get; set; }
        public List<string> PossibleSortingModes { get; set; }

        private DelegateCommand _startGameCommand;
        public DelegateCommand StartGameCommand
        {
            get
            {
                if (_startGameCommand == null)
                {
                    _startGameCommand = new DelegateCommand(() =>
                    {
                        var parameter = new CardGameNavigationModel()
                        {
                            Set = NavigationParameter.Set,
                            Terms = NavigationParameter.Terms,
                            DefinitionOrTerms = DefinitionsOrTerm,
                            SortingMode = SortingMode
                        };

                        NavigationService.Navigate(typeof(Views.CardsPage), parameter);
                    });

                }

                return _startGameCommand;

            }
        }

        public CardSettingsPageViewModel()
        {
            PossibleModes = Constants.TermsGameModes;
            PossibleSortingModes = Constants.PossibleSortingModes;
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.ContainsKey(nameof(NavigationParameter)))
            {
                var navParameter = suspensionState[nameof(NavigationParameter)] as CardSettingsNavigationModel;
                NavigationParameter = navParameter;
            }
            if (parameter != null)
            {
                var navParameter = parameter as CardSettingsNavigationModel;
                NavigationParameter = navParameter;
            }

            await Task.CompletedTask;
        }

        public void SelectMode()
        {
            return;
        }

    }
}
    