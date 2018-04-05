using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml;
using System.Diagnostics;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using _29Quizlet.Models.ViewModels;
using _29Quizlet.Commands;
using _29Quizlet.Models.Navigation;

namespace _29Quizlet.ViewModels
{
    public class MatchPageViewModel : ViewModelBase
    {
        public List<TermViewModel> Terms { get; set; }
        public MatchButtonViewModel[] Buttons { get; set; }
        public List<int> PositionsAtPlay { get; set; }
        public MatchPageNavigationModel NavigationParameter { get; set; }
        private int _gameSize;
        private int _errors;
        private DispatcherTimer _dispatcherTimer;
        private Stopwatch _stopwatch;
        private bool _firstButtonPressed;
        private int _firstButtonPressedIndex;


        int _FontSize = 12;
        public int FontSize
        {
            get { return _FontSize; }
            set { Set(ref _FontSize, value); }
        }

        private string _counter;
        public string Counter {
            get
            {
                return _counter;
            }
            set
            {
                Set(ref _counter, value);
            }
        }

        public MatchPageViewModel()
        {
            _stopwatch = new Stopwatch();
            _dispatcherTimer = new DispatcherTimer();
            Terms = new List<TermViewModel>();
            Buttons = new MatchButtonViewModel[100];
            PositionsAtPlay = new List<int>();
            SetupButtons();
            _firstButtonPressed = false;
            _firstButtonPressedIndex = -1;
            _errors = 0;
        }

        private void SetupButtons()
        {
            var matchButton01 = new MatchButtonViewModel();
            MatchButton01 = matchButton01;
            var matchButton02 = new MatchButtonViewModel();
            MatchButton02 = matchButton02;
            var matchButton03 = new MatchButtonViewModel();
            MatchButton03 = matchButton03;
            var matchButton04 = new MatchButtonViewModel();
            MatchButton04 = matchButton04;
            var matchButton05 = new MatchButtonViewModel();
            MatchButton05 = matchButton05;
            var matchButton06 = new MatchButtonViewModel();
            MatchButton06 = matchButton06;
            var matchButton07 = new MatchButtonViewModel();
            MatchButton07 = matchButton07;
            var matchButton08 = new MatchButtonViewModel();
            MatchButton08 = matchButton08;
            var matchButton09 = new MatchButtonViewModel();
            MatchButton09 = matchButton09;
            var matchButton10 = new MatchButtonViewModel();
            MatchButton10 = matchButton10;
            var matchButton11 = new MatchButtonViewModel();
            MatchButton11 = matchButton11;
            var matchButton12 = new MatchButtonViewModel();
            MatchButton12 = matchButton12;
            var matchButton13 = new MatchButtonViewModel();
            MatchButton13 = matchButton13;
            var matchButton14 = new MatchButtonViewModel();
            MatchButton14 = matchButton14;

            Buttons[0] = matchButton01;
            Buttons[1] = matchButton02;
            Buttons[2] = matchButton03;
            Buttons[3] = matchButton04;
            Buttons[4] = matchButton05;
            Buttons[5] = matchButton06;
            Buttons[6] = matchButton07;
            Buttons[7] = matchButton08;
            Buttons[8] = matchButton09;
            Buttons[9] = matchButton10;
            Buttons[10] = matchButton11;
            Buttons[11] = matchButton12;
            Buttons[12] = matchButton13;
            Buttons[13] = matchButton14;
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.ContainsKey(nameof(NavigationParameter)))
            {
                NavigationParameter = suspensionState[nameof(NavigationParameter)] as MatchPageNavigationModel;

                foreach (var term in NavigationParameter.Terms)
                {
                    Terms.Add(term);
                }
            }

            if (parameter != null)
            {
                NavigationParameter = parameter as MatchPageNavigationModel;
                var observableCollection = NavigationParameter.Terms;
                foreach (var term in observableCollection)
                {
                    Terms.Add(term);
                }

                _gameSize = observableCollection.Count > 7 ? 7 : observableCollection.Count;
            }

            BuildGame();
            DispatcherTimerSetup();

            await Task.CompletedTask;
        }


        public void DispatcherTimerSetup()
        {
            
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            _dispatcherTimer.Start();
            _stopwatch.Start();
        }

        public void dispatcherTimer_Tick(object sender, object e)
        {
            var elapsedMinutes = _stopwatch.Elapsed.Minutes;
            var elapsedMinutesStr = _stopwatch.Elapsed.Minutes.ToString();
            var elapsedSecondsStr = _stopwatch.Elapsed.Seconds.ToString();
            var elapsedMilisecondsStr = (_stopwatch.Elapsed.Milliseconds / 100L).ToString();

            string updatedCounter = string.Empty;

            if (elapsedMinutes > 0)
                updatedCounter = $"{elapsedMinutesStr}:{elapsedSecondsStr}.{elapsedMilisecondsStr}";
            else
                updatedCounter = $"{elapsedSecondsStr}.{elapsedMilisecondsStr}";

            Counter = updatedCounter;
        }

        public void BuildGame()
        {
            var random = new Random();
            var randomList = Terms
                .Select(x => new { term = x, number = random.Next(Terms.Count) })
                .OrderBy(x => x.number)
                .Select(x => x.term)
                .Take(_gameSize)
                .ToList();

            var listOfNumbers = new List<int>();
            for (int i = 0; i < _gameSize * 2; i++)
            {
                listOfNumbers.Add(i);
            }

            var listOfRands = listOfNumbers
                .Select(x => new { Id = x, rand = random.Next() })
                .OrderBy(x => x.rand)
                .Select(x => x.Id)
                .ToList();

            SetupButtons();
            PositionsAtPlay.Clear();
            _errors = 0;

            for (int i = 0; i < _gameSize; i++)
            {
                var index = i * 2;
                var rand0 = listOfRands[index];
                var rand1 = listOfRands[index + 1];
                var term = randomList[i];
                PositionsAtPlay.Add(rand0);
                PositionsAtPlay.Add(rand1);

                var menuButton01 = Buttons[rand0];
                var menuButton02 = Buttons[rand1];

                menuButton01.ResetWithTerm(term, MatchButtonType.Answer);
                menuButton02.ResetWithTerm(term, MatchButtonType.Question);

            }

        }

        public void Reset()
        {
            _stopwatch.Stop();
            _dispatcherTimer.Stop();
            _stopwatch.Reset();

            BuildGame();

            _dispatcherTimer.Start();
            _stopwatch.Start();
        }

        private DelegateCommander _buttonPressCommand;
        public DelegateCommander ButtonPressCommand
        {
            get
            {
                if (_buttonPressCommand == null)
                {
                    _buttonPressCommand = new DelegateCommander((input) => 
                    {
                        var inputAsString = input as string;
                        if (inputAsString == null)
                        {
                            return;
                        }

                        int i;
                        if (!int.TryParse(inputAsString, out i))
                        {
                            return;
                        }

                        ButtonPressed(i); 

                    });

                }

                return _buttonPressCommand;

            }
        }

        private void ButtonPressed(int i)
        {
            var index = i;
            if (_firstButtonPressed)
            {
                var termAlreadyPressed = Buttons[_firstButtonPressedIndex];

                if (index == _firstButtonPressedIndex)
                {
                    termAlreadyPressed.Pressed();
                    _firstButtonPressedIndex = -1;
                    _firstButtonPressed = !_firstButtonPressed;

                    return;
                }

                var termJustPressed = Buttons[index];
                termJustPressed.Pressed();

                if (termJustPressed.CheckForCorrectAnswer(termAlreadyPressed))
                {
                    _firstButtonPressed = false;
                    Buttons[index] = null;
                    Buttons[_firstButtonPressedIndex] = null;
                    PositionsAtPlay.Remove(index);
                    PositionsAtPlay.Remove(_firstButtonPressedIndex);
                    IsGameOver();
                }
                else
                {
                    // todo
                    _errors += 1;
                }
                _firstButtonPressed = false;
                _firstButtonPressedIndex = -1;
            }
            else
            {
                if (!PositionsAtPlay.Contains(index))
                {
                    return;
                }
                var termJustPressed = Buttons[index];
                if (termJustPressed == null)
                {
                    return;
                }
                termJustPressed.Pressed();

                _firstButtonPressed = true;
                _firstButtonPressedIndex = index;
            }

        }

        private void IsGameOver()
        {
            if (PositionsAtPlay.Count == 0)
            {
                StopGame();
                var parameter = new MatchResultPageNavigationModel()
                {
                    Set = NavigationParameter.Set,
                    TimeResult = Counter,
                    Errors = _errors
                };
                NavigationService.Navigate(typeof(Views.MatchResultPage), parameter);
            }
        }

        private void StopGame()
        {
            _stopwatch.Stop();
            _dispatcherTimer.Stop();
            _stopwatch.Reset();
        }

        #region Game States

        private MatchButtonViewModel _MatchButton01;
        public MatchButtonViewModel MatchButton01 {
            get
            {
                return _MatchButton01;
            }
            set
            {
                Set(ref _MatchButton01, value);
            }
        }

        private MatchButtonViewModel _MatchButton02;
        public MatchButtonViewModel MatchButton02
        {
            get
            {
                return _MatchButton02;
            }
            set
            {
                Set(ref _MatchButton02, value);
            }
        }

        private MatchButtonViewModel _MatchButton03;
        public MatchButtonViewModel MatchButton03
        {
            get
            {
                return _MatchButton03;
            }
            set
            {
                Set(ref _MatchButton03, value);
            }
        }

        private MatchButtonViewModel _MatchButton04;
        public MatchButtonViewModel MatchButton04
        {
            get
            {
                return _MatchButton04;
            }
            set
            {
                Set(ref _MatchButton04, value);
            }
        }

        private MatchButtonViewModel _MatchButton05;
        public MatchButtonViewModel MatchButton05
        {
            get
            {
                return _MatchButton05;
            }
            set
            {
                Set(ref _MatchButton05, value);
            }
        }

        private MatchButtonViewModel _MatchButton06;
        public MatchButtonViewModel MatchButton06
        {
            get
            {
                return _MatchButton06;
            }
            set
            {
                Set(ref _MatchButton06, value);
            }
        }

        private MatchButtonViewModel _MatchButton07;
        public MatchButtonViewModel MatchButton07
        {
            get
            {
                return _MatchButton07;
            }
            set
            {
                Set(ref _MatchButton07, value);
            }
        }

        private MatchButtonViewModel _MatchButton08;
        public MatchButtonViewModel MatchButton08
        {
            get
            {
                return _MatchButton08;
            }
            set
            {
                Set(ref _MatchButton08, value);
            }
        }

        private MatchButtonViewModel _MatchButton09;
        public MatchButtonViewModel MatchButton09
        {
            get
            {
                return _MatchButton09;
            }
            set
            {
                Set(ref _MatchButton09, value);
            }
        }

        private MatchButtonViewModel _MatchButton10;
        public MatchButtonViewModel MatchButton10
        {
            get
            {
                return _MatchButton10;
            }
            set
            {
                Set(ref _MatchButton10, value);
            }
        }

        private MatchButtonViewModel _MatchButton11;
        public MatchButtonViewModel MatchButton11
        {
            get
            {
                return _MatchButton11;
            }
            set
            {
                Set(ref _MatchButton11, value);
            }
        }

        private MatchButtonViewModel _MatchButton12;
        public MatchButtonViewModel MatchButton12
        {
            get
            {
                return _MatchButton12;
            }
            set
            {
                Set(ref _MatchButton12, value);
            }
        }

        private MatchButtonViewModel _MatchButton13;
        public MatchButtonViewModel MatchButton13
        {
            get
            {
                return _MatchButton13;
            }
            set
            {
                Set(ref _MatchButton13, value);
            }
        }

        private MatchButtonViewModel _MatchButton14;
        public MatchButtonViewModel MatchButton14
        {
            get
            {
                return _MatchButton14;
            }
            set
            {
                Set(ref _MatchButton14, value);
            }
        }

        #endregion
    }
}
