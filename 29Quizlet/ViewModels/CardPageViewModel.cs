using _29Quizlet.Helpers.Enums;
using _29Quizlet.Models.Navigation;
using _29Quizlet.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace _29Quizlet.ViewModels
{
    public class CardPageViewModel : ViewModelBase
    {
        public ObservableCollection<CardViewModel> Cards { get; set; }
        public List<CardViewModel> InitCards { get; set; }
        public CardGameNavigationModel NavigationParameter { get; set; }
        public int MaxProgressValue { get; set; }
        public Random Rand { get; set; }

        private string _progressText;
        public string ProgressText
        {
            get
            {
                return _progressText;
            }
            set
            {
                Set(ref _progressText, value);
            }
        }

        private int _progressValue;
        public int ProgressValue
        {
            get
            {
                return _progressValue;
            }
            set
            {
                Set(ref _progressValue, value);
            }
        }


        public CardPageViewModel()
        {
            Cards = new ObservableCollection<CardViewModel>();
            InitCards = new List<CardViewModel>();
            ProgressValue = 0;
            Rand = new Random();
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            CardGameNavigationModel navParameter = null;

            if (suspensionState.ContainsKey(nameof(NavigationParameter)))
            {
                navParameter = suspensionState[nameof(NavigationParameter)] as CardGameNavigationModel;
            }

            if (parameter != null)
            {
                navParameter = parameter as CardGameNavigationModel;
            }

            NavigationParameter = navParameter;
            MaxProgressValue = NavigationParameter.Terms.Count;

            SetupCards();

            foreach (var card in InitCards)
            {
                Cards.Add(card);
            }

            await Task.CompletedTask;
        }

        public void Flip(object sender, SelectionChangedEventArgs e)
        {
            var flipView = sender as FlipView;
            ProgressValue = flipView.SelectedIndex + 1; // we start from 1...


            ProgressText = $"{ProgressValue} of {MaxProgressValue} Cards";


        }

        private void SetupCards()
        {
            var mode = NavigationParameter.DefinitionOrTerms;
            switch (mode)
            {
                case DefinitionsOrTerms.Definitions:
                    DefinitionsFirst();
                    break;
                case DefinitionsOrTerms.Terms:
                    TermsFirst();
                    break;
                case DefinitionsOrTerms.RandomMix:
                    RandomMix();
                    break;
                default:
                    break;
            }

            var sort = NavigationParameter.SortingMode;
            switch (sort)
            {
                case SortingModes.Alphabetical:
                    SortAlphabetical();
                    break;
                case SortingModes.Random:
                    SortRandom();
                    break;
                default:
                    break;
            }
        }

        private void SortRandom()
        {

            var tempList = InitCards
                .Select(x => new { card = x, rand = Rand.Next() })
                .OrderBy(x => x.rand)
                .Select(x => x.card)
                .ToList();

            InitCards = tempList;
        }

        private void SortAlphabetical()
        {
            InitCards.Sort((card1, card2) => 
                string.Compare(card1.FrontText, card2.FrontText, true));
            
        }

        private void RandomMix()
        {
            var vms = NavigationParameter.Terms;

            foreach (var vm in vms)
            {
                CardViewModel card = null;

                if (NextRandomBool())
                {
                    card = new CardViewModel()
                    {
                        FrontText = vm.TermText,
                        BackText = vm.Definition,
                    };
                }
                else
                {
                    card = new CardViewModel()
                    {
                        FrontText = vm.Definition,
                        BackText = vm.TermText,
                    };
                }

                InitCards.Add(card);
            }
            
        }

        private bool NextRandomBool()
        {
            return 50 > Rand.Next(100);
        }

        private void TermsFirst()
        {
            var vms = NavigationParameter.Terms;

            foreach (var vm in vms)
            {
                var card = new CardViewModel()
                {
                    FrontText = vm.TermText,
                    BackText = vm.Definition,
                };

                InitCards.Add(card);
            }
        }

        private void DefinitionsFirst()
        {
            var vms = NavigationParameter.Terms;

            foreach (var vm in vms)
            {
                var card = new CardViewModel()
                {
                    FrontText = vm.Definition,
                    BackText = vm.TermText,
                };

                InitCards.Add(card);
            }
        }
    }
}
