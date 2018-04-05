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
using Windows.UI.Xaml.Navigation;

namespace _29Quizlet.ViewModels
{
    public class LearnEndResultsPageViewModel : ViewModelBase
    {
        public string GameRound { get; set; }
        public LearnGameNavigationModel NavParameter { get; set; }
        public ObservableCollection<LearnResultListViewBase> TermResults { get; set; }

        public LearnEndResultsPageViewModel()
        {
            TermResults = new ObservableCollection<LearnResultListViewBase>();
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.ContainsKey(nameof(NavParameter)))
            {
                NavParameter = suspensionState[nameof(NavParameter)] as LearnGameNavigationModel;
            }
            if (parameter != null)
            {
                NavParameter = parameter as LearnGameNavigationModel;
            }

            var headers = NavParameter.Stats
                .GroupBy(x => x.Errors)
                .OrderByDescending(x => x.Key);

            foreach (var elem in headers)
            {
                if (elem.Key == 1)
                {
                    TermResults.Add(new ResultHeader()
                    {
                        Title = $"Missed {elem.Key} time",
                        BackgroundColor = Colors.Firebrick
                    });
                }
                else 
                {
                    // for pluralization
                    TermResults.Add(new ResultHeader()
                    {
                        Title = $"Missed {elem.Key} times",
                        BackgroundColor = Colors.Firebrick
                    });
                }

                foreach (var stat in elem)
                {
                    TermResults.Add(new TermResultViewModel()
                    {
                        Term = stat.Term,
                        Errors = stat.Errors
                    });
                }
            }

            var succesfullWithOutErrors = NavParameter.TotalSuccessfulTerms
                .Where(x => !NavParameter.Stats.Any(y => y.Term.Id == x.Id));

            TermResults.Add(new ResultHeader()
            {
                Title = "Correct",
                BackgroundColor = Colors.LimeGreen
            });

            foreach (var term in succesfullWithOutErrors)
            {
                TermResults.Add(new TermResultViewModel()
                {
                    Term = term,
                    Errors = 0
                });
            }

            await Task.CompletedTask;
        }

        private DelegateCommand _goBackToSet;
        public DelegateCommand GoBackToSet
        {
            get
            {
                if (_goBackToSet == null)
                {
                    _goBackToSet = new DelegateCommand(() =>
                    {
                        NavigationService.Navigate(typeof(Views.SetDetailPage), NavParameter.Set);
                    });
                }

                return _goBackToSet;
            }
        }

        private DelegateCommand _startOver;
        public DelegateCommand StartOver
        {
            get
            {
                if (_startOver == null)
                {
                    _startOver = new DelegateCommand(() =>
                    {
                        var parameter = new LearnGameNavigationModel()
                        {
                            ErrorTerms = new List<TermViewModel>(),
                            GameRound = 1,
                            Terms = NavParameter.Terms,
                            GameTerms = NavParameter.Terms.ToList(),
                            Mode = NavParameter.Mode,
                            Set = NavParameter.Set,
                            Stats = new List<LearnGameStatClass>(),
                            SuccesfulTerms = new List<TermViewModel>(),
                            TotalSuccessfulTerms = new List<TermViewModel>()
                        };

                        NavigationService.Navigate(typeof(Views.LearnPage), parameter);
                    });
                }

                return _startOver;
            }
        }
    }
}
