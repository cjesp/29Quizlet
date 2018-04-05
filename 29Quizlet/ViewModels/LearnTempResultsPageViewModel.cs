using _29Quizlet.Models.Navigation;
using _29Quizlet.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace _29Quizlet.ViewModels
{
    public class LearnTempResultsPageViewModel : ViewModelBase
    {
        public LearnGameNavigationModel NavigationParameter { get; set; }
        public ObservableCollection<LearnResultListViewBase> TermResults { get; set; }
        public string GameRound { get; set; }

        int _ProgressValue = default(int);
        public int ProgressValue { get { return _ProgressValue; } set { Set(ref _ProgressValue, value); } }
        
        int _Minimum = 0;
        public int Minimum { get { return _Minimum; } set { Set(ref _Minimum, value); } }
        
        int _Maximum = 7;
        public int Maximum { get { return _Maximum; } set { Set(ref _Maximum, value); } }

        public LearnTempResultsPageViewModel()
        {
            TermResults = new ObservableCollection<LearnResultListViewBase>();
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.ContainsKey(nameof(NavigationParameter)))
            {
                var navParameter = suspensionState[nameof(NavigationParameter)] as LearnGameNavigationModel;
                NavigationParameter = navParameter;
            }
            if (parameter != null)
            {
                var navParameter = parameter as LearnGameNavigationModel;
                NavigationParameter = navParameter;
            }

            TermResults.Add(new ResultHeader()
            {
                Title = "Missed",
                BackgroundColor = Colors.Firebrick
            });

            foreach (var term in NavigationParameter.ErrorTerms)
            {
                var errorStat = NavigationParameter.Stats
                    .Where(x => x.Term.Id == term.Id)
                    .Single();

                TermResults.Add(new TermResultViewModel()
                {
                    Term = term,
                    Errors = errorStat.Errors
                });
            }

            TermResults.Add(new ResultHeader()
            {
                Title = "Correct",
                BackgroundColor = Colors.LimeGreen
            });

            foreach (var term in NavigationParameter.SuccesfulTerms)
            {
                ProgressValue += 1;

                TermResults.Add(new TermResultViewModel()
                {
                    Term = term,
                    Errors = 0
                });
            }

            // Add temp to total successfull 
            NavigationParameter.TotalSuccessfulTerms.AddRange(NavigationParameter.SuccesfulTerms);

            ProgressValue = NavigationParameter.TotalSuccessfulTerms.Count();
            Maximum = NavigationParameter.Terms.Count();

            await Task.CompletedTask; 
        }

        private DelegateCommand _nextRoundCommand;
        public DelegateCommand NextRoundCommand
        {
            get
            {
                if (_nextRoundCommand == null)
                {
                    _nextRoundCommand = new DelegateCommand(() =>
                    {
                        
                        var gameTerms = new List<TermViewModel>();
                        foreach (var term in NavigationParameter.ErrorTerms)
                        {
                            gameTerms.Add(term);
                        }

                        var residualTerms = NavigationParameter.Terms
                            .Where(x => !(NavigationParameter.TotalSuccessfulTerms.Any(y => y.Id == x.Id) || NavigationParameter.ErrorTerms.Any(y => y.Id == x.Id)))
                            .ToList();

                        GetRandomTerms(residualTerms, gameTerms);

                        var parameter = new LearnGameNavigationModel()
                        {
                            GameRound = NavigationParameter.GameRound + 1,
                            ErrorTerms = new List<TermViewModel>(),
                            SuccesfulTerms = new List<TermViewModel>(),
                            TotalSuccessfulTerms = NavigationParameter.TotalSuccessfulTerms,
                            Mode = NavigationParameter.Mode,
                            Set = NavigationParameter.Set,
                            Stats = NavigationParameter.Stats,
                            Terms = NavigationParameter.Terms,
                            GameTerms = gameTerms
                        };

                        NavigationService.Navigate(typeof(Views.LearnPage), parameter);
                    });

                }

                return _nextRoundCommand;

            }
        }

        private void GetRandomTerms(List<TermViewModel> residualTerms, List<TermViewModel> gameTerms)
        {
            var random = new Random();
            var amount = 7 - gameTerms.Count;

            var randoms = residualTerms
                .Select(x => new { term = x, rand = random.Next() })
                .OrderBy(x => x.rand)
                .Select(x => x.term)
                .Take(amount);

            foreach (var term in randoms)
            {
                gameTerms.Add(term);
            }
        }
    }

}
