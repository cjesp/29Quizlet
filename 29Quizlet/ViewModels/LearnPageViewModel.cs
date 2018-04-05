using _29Quizlet.Models.Navigation;
using _29Quizlet.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace _29Quizlet.ViewModels
{
    public class QAndA
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public Int64 Id { get; set; }
    }

    public class LearnPageViewModel : ViewModelBase
    {
        public delegate void TextBoxFocus();
        public event TextBoxFocus InputTextBoxFocusEvent;

        public LearnGameNavigationModel NavigationParameter { get; set; }
        public int QuestionCounter { get; set; }
        public List<QAndA> QAndAs { get; set; }
        
        private bool _inCopyMode = false;
        
        string _GameRound = default(string);
        public string GameRound { get { return _GameRound; } set { Set(ref _GameRound, value); } }
      
        private string _answer = default(string);
        public string Answer
        {
            get { return _answer; }
            set { Set(ref _answer, value); }
        }

        string _textBoxPlaceholderText = default(string);
        public string TextBoxPlaceholderText
        {
            get { return _textBoxPlaceholderText; }
            set { Set(ref _textBoxPlaceholderText, value); }
        }

        string _yourAnswer = default(string);
        public string YourAnswer
        {
            get { return _yourAnswer; }
            set { Set(ref _yourAnswer, value); }
        }

        Color _gridBackgroundColor = Colors.Transparent;
        public Color GridBackgroundColor
        {
            get { return _gridBackgroundColor; }
            set { Set(ref _gridBackgroundColor, value); }
        }

        string _proposedAnswer = default(string);
        public string ProposedAnswer
        {
            get { return _proposedAnswer; }
            set { Set(ref _proposedAnswer, value); }
        }

        string _question = default(string);
        public string Question
        {
            get { return _question; }
            set { Set(ref _question, value); }
        }

        
        Visibility _questionVisibility = Visibility.Visible;
        public Visibility QuestionVisibility
        {
            get { return _questionVisibility; }
            set { Set(ref _questionVisibility, value); }
        }

        Visibility _errorRows = Visibility.Collapsed;
        public Visibility ErrorRows
        {
            get { return _errorRows; }
            set { Set(ref _errorRows, value); }
        }

        Visibility _misspelledVisibility = Visibility.Collapsed;
        public Visibility MisspelledVisibility
        {
            get { return _misspelledVisibility; }
            set { Set(ref _misspelledVisibility, value); }
        }

        Visibility _dontKnowVisibility = Visibility.Visible;
        public Visibility DontKnowVisibility
        {
            get { return _dontKnowVisibility; }
            set { Set(ref _dontKnowVisibility, value); }
        }

        Visibility _nextVisibility = Visibility.Collapsed;
        public Visibility NextVisibility
        {
            get { return _nextVisibility; }
            set { Set(ref _nextVisibility, value); }
        }

        private GridLength _rowHeight;
        public GridLength RowHeight
        {
            get
            {
                return _rowHeight;
            }
            set
            {
                Set(ref _rowHeight, value);
            }
        }

        private double _rowHeightInt;
        public double RowHeightInt
        {
            get
            {
                return _rowHeightInt;
            }
            set
            {
                Set(ref _rowHeightInt, value);
            }
        }

        public LearnPageViewModel()
        {
            QAndAs = new List<QAndA>();
            InputPane.GetForCurrentView().Showing += InputPane_Showing;
            InputPane.GetForCurrentView().Hiding += LearnPageViewModel_Hiding;
            RowHeight = new GridLength(1.0, GridUnitType.Star);
            QuestionCounter = -1;
            InputPane.GetForCurrentView().TryShow();
        }

        private void LearnPageViewModel_Hiding(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            //todo
        }

        private void InputPane_Showing(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            ResizeGrid(args.OccludedRect.Height);
        }

        public void ResizeGrid(double height)
        {
            var newHeight = height * 1.04;
            if (newHeight > height)
            {
                RowHeightInt = newHeight; 
            }
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

            if (NavigationParameter.ErrorTerms == null)
            {
                NavigationParameter.ErrorTerms = new List<TermViewModel>();
            }

            if (NavigationParameter.SuccesfulTerms == null)
            {
                NavigationParameter.SuccesfulTerms = new List<TermViewModel>();
            }

            SetupGame();

            await Task.CompletedTask;
        }

        private void SetupGame()
        {
            UpdateGameRoundHeader();
            SetupQAndAs();

            NextQuestion();
        }

        private void SetupQAndAs()
        {
            var terms = NavigationParameter.GameTerms;

            var random = new Random();
            var randomSelected = terms
                .Select(x => new { term = x, rand = random.Next() })
                .OrderBy(x => x.rand)
                .Take(7)
                .Select(x => x.term);

            foreach (var term in randomSelected)
            {
                switch (NavigationParameter.Mode)
                {
                    case Helpers.Enums.DefinitionsOrTerms.Definitions:
                        DefinitionAsQuestion(term);
                        break;
                    case Helpers.Enums.DefinitionsOrTerms.Terms:
                        TermAsQuestion(term);
                        break;
                    case Helpers.Enums.DefinitionsOrTerms.RandomMix:
                        if (random.Next(2) == 0)
                        {
                            TermAsQuestion(term);
                        }
                        else
                            DefinitionAsQuestion(term);
                        break;
                    default:
                        DefinitionAsQuestion(term);
                        break;
                }
            }
        }

        private void TermAsQuestion(TermViewModel term)
        {
            QAndAs.Add(new QAndA()
            {
                Question = term.TermText,
                Answer = term.Definition,
                Id = term.Id
            });
        }

        private void DefinitionAsQuestion(TermViewModel term)
        {
            QAndAs.Add(new QAndA()
            {
                Answer = term.TermText,
                Question = term.Definition,
                Id = term.Id
            });
        }

        private void NextQuestion()
        {
            ProposedAnswer = "";

            QuestionCounter++;

            GameRound = $"{QuestionCounter + 1}/{QAndAs.Count}";

            if (QAndAs.Count > 0)
            {
                if (QAndAs.Count == QuestionCounter)
                {
                    NavigateToResultsPage();
                    return;
                }

                var qAndA = QAndAs[QuestionCounter];
                Question = qAndA.Question;
                Answer = qAndA.Answer;
            }

            InputTextBoxFocusEvent();
            
        }

        private void NavigateToResultsPage()
        {
            if (NavigationParameter.ErrorTerms.Count > 0)
            {
                // go to temp result page
                NavigationService.Navigate(typeof(Views.LearnTempResultsPage), NavigationParameter);
            }
            else
            {
                // go to end result page
                var residuals = NavigationParameter.SuccesfulTerms
                    .Where(x => !NavigationParameter.TotalSuccessfulTerms.Any(y => y.Id == x.Id));
                NavigationParameter.TotalSuccessfulTerms.AddRange(residuals);

                if (NavigationParameter.TotalSuccessfulTerms.Count == NavigationParameter.Terms.Count)
                {
                    // Then we're done with all terms
                    NavigationService.Navigate(typeof(Views.LearnEndResultsPage), NavigationParameter);
                }
                else
                {
                    // go to temp result page since we're still missing some terms
                    NavigationService.Navigate(typeof(Views.LearnTempResultsPage), NavigationParameter);
                }    
            }
        }

        private void UpdateGameRoundHeader()
        {
            GameRound = $"Learn Mode: round {NavigationParameter.GameRound}";
        }

        public void TextChanged(object sender, RoutedEventArgs e)
        {
            var input = sender as TextBox;

            if (_inCopyMode)
            {
                if (string.IsNullOrEmpty(input.Text))
                {
                    return;
                }

                var proposed = input.Text.ToLower().Trim();
                var answer = Answer.ToLower().Trim();

                if (proposed == answer)
                {
                    MisspelledVisibility = Visibility.Collapsed;
                    HandleCorrectAnswer();
                }
            }

            if (string.IsNullOrEmpty(input.Text))
            {
                DontKnowVisibility = Visibility.Visible;
            }
            else
            {
                DontKnowVisibility = Visibility.Collapsed;
                ProposedAnswer = input.Text;
            }

            return;
        }

        public void Enter()
        {
            var proposed = ProposedAnswer.ToLower().Trim();
            var answer = Answer.ToLower().Trim();

            
            if (proposed == answer)
            {
                // correct answer
                HandleCorrectAnswer();
            }
            else 
            {
                // wrong answer
                YourAnswer = ProposedAnswer;
                HandleIncorrectAnswer();
            }
        }

        private void HandleIncorrectAnswer()
        {
            ErrorRows = Visibility.Visible;
            QuestionVisibility = Visibility.Collapsed;
            MisspelledVisibility = Visibility.Visible;
            ProposedAnswer = "";
            TextBoxPlaceholderText = "Type in the correct answer";
            _inCopyMode = true;
        }

        private void HandleCorrectAnswer()
        {
            NextVisibility = Visibility.Visible;
            GridBackgroundColor = Colors.LimeGreen;
        }

        public void NextClick(object sender, RoutedEventArgs e)
        {
            NextVisibility = Visibility.Collapsed;
            ErrorRows = Visibility.Collapsed;
            QuestionVisibility = Visibility.Visible;
            GridBackgroundColor = Colors.Transparent;

            if (_inCopyMode)
            {
                AddAnError();
                _inCopyMode = false;
            }
            else
            {
                AddASuccess();
            }

            NextQuestion();
        }

        private void AddASuccess()
        {
            var qAndA = QAndAs[QuestionCounter];
            var term = NavigationParameter.GameTerms
                .Where(x => x.Id == qAndA.Id)
                .SingleOrDefault();

            if (term != null)
            {
                NavigationParameter.SuccesfulTerms.Add(term);
            }
        }

        private void AddAnError()
        {
            var qAndA = QAndAs[QuestionCounter];
            var term = NavigationParameter.GameTerms
                .Where(x => x.Id == qAndA.Id)
                .SingleOrDefault();

            NavigationParameter.ErrorTerms.Add(term);

            if (NavigationParameter.Stats.Any(x => x.Term.Id == term.Id))
            {
                var stat = NavigationParameter.Stats
                    .Where(x => x.Term.Id == term.Id)
                    .SingleOrDefault();

                if (stat != null)
                {
                    stat.Errors++;
                }
            }
            else
            {
                NavigationParameter.Stats.Add(new LearnGameStatClass()
                {
                    Errors = 1,
                    Term = term
                });
            }

        }

        public void DontKnowClick(object sender, RoutedEventArgs e)
        {
            AddAnError();
            NextQuestion();
        }

        public void MisspelledClick(object sender, RoutedEventArgs e)
        {
            NextVisibility = Visibility.Collapsed;
            ErrorRows = Visibility.Collapsed;
            QuestionVisibility = Visibility.Visible;
            GridBackgroundColor = Colors.Transparent;

            AddASuccess();

            NextQuestion();
        }
    }
}
