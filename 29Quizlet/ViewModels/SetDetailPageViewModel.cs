using _29Quizlet.Commands;
using _29Quizlet.Models;
using _29Quizlet.Models.Navigation;
using _29Quizlet.Models.ViewModels;
using _29Quizlet.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources;
using _29Quizlet.Services.SettingsServices;

namespace _29Quizlet.ViewModels
{
    public class SetDetailPageViewModel : ViewModelBase
    {
        private SettingsService _settingsService;
        private IQuizletRESTApi _quizApi;
        private ResourceLoader _loader;
        public ObservableCollection<TermViewModel> Terms { get; set; }
        public List<TermViewModel> AllTerms { get; set; }
        public int TermSelectedCounter { get; set; }
        public Set Set { get; set; }
        public string Title { get; set; }
        public int TermCount { get; set; }
        public string CreatedBy { get; set; }
        
        bool _Editable = default(bool);
        public bool Editable { get { return _Editable; } set { Set(ref _Editable, value); } }

        private bool _studyHeaderVisibility;
        public bool StudyHeaderVisibility {
            get
            {
                return _studyHeaderVisibility;    
            }
            set
            {
                Set(ref _studyHeaderVisibility, value);
            }
        }

        private string _studyNumber;
        public string StudyNumber {
            get
            {
                return _studyNumber;
            }
            set
            {
                Set(ref _studyNumber, value);
            }
        }

        private bool _studyStarredTerms = false;
        public bool StudyStarredTerms
        {
            get { return _studyStarredTerms; }
            set
            {
                if (value)
                {
                    var tempTerms = Terms.Where(x => !x.Favorite).ToList();
                    foreach (var term in tempTerms)
                    {
                        Terms.Remove(term);
                    }
                    
                }
                else
                {
                    RefreshTerms();
                }

                Set(ref _studyStarredTerms, value);

            }   
        }

        private void RefreshTerms()
        {
            Terms.Clear();
            foreach (var term in AllTerms)
            {
                Terms.Add(term);
            }
        }

        public SetDetailPageViewModel()
        {
            Terms = new ObservableCollection<TermViewModel>();
            AllTerms = new List<TermViewModel>();
            _settingsService = SettingsService.Instance;
            TermSelectedCounter = 0;
            StudyHeaderVisibility = false;
            StudyStarredTerms = false;
            _quizApi = new QuizletRemoteRESTApi();
            _loader = new ResourceLoader();
            Editable = false;
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            Set = null;

            if (suspensionState.ContainsKey(nameof(Set)))
            {
                Set = suspensionState[nameof(Set)] as Set;
            }
            if (parameter != null)
            {
                Set = parameter as Set;
            }

            if (_settingsService.AuthenticatedUser.Username == Set.CreatedBy)
            {
                Editable = true;
            }

            Title = Set.Title;
            TermCount = Set.TermCount;
            CreatedBy = Set.CreatedBy;

            foreach (var term in Set.Terms)
            {
                var tvm = TermViewModel.Create(term);
                Terms.Add(tvm);
                AllTerms.Add(tvm);
            }

            await Task.CompletedTask;
        }

        private DelegateCommander _setFavoriteCommand;
        public DelegateCommander SetFavoriteCommand
        {
            get
            {
                if (_setFavoriteCommand == null)
                {
                    _setFavoriteCommand = new DelegateCommander((input) =>
                    {
                        var tvm = (TermViewModel)input;
                        if (tvm.Favorite)
                        {
                            TermSelectedCounter -= 1;
                            tvm.Favorite = !tvm.Favorite;
                            //SelectedTerms.Remove(tvm);
                        }
                        else
                        {
                            TermSelectedCounter += 1;
                            tvm.Favorite = !tvm.Favorite;
                            //SelectedTerms.Add(tvm);
                        }

                        if (TermSelectedCounter > 0)
                        {
                            StudyHeaderVisibility = true;
                            StudyNumber = $"Study {TermSelectedCounter} starred";
                        }
                        else
                        {
                            StudyHeaderVisibility = false;
                            StudyStarredTerms = false;
                        }
                    });

                }

                return _setFavoriteCommand;

            }
        }

        public void MatchClick()
        {
            var parameter = new MatchPageNavigationModel()
            {
                Set = Set,
                Terms = Terms
            };
            NavigationService.Navigate(typeof(Views.MatchPage), parameter);
        }

        public void CardClick()
        {
            var parameter = new CardSettingsNavigationModel()
            {
                Set = Set,
                Terms = Terms
            };
            NavigationService.Navigate(typeof(Views.CardsSettingsPage), parameter);
        }

        public void LearnClick()
        {
            var parameter = new LearnSettingsNavigationModel()
            {
                Set = Set,
                Terms = Terms
            };
            NavigationService.Navigate(typeof(Views.LearnSettingsPage), parameter);
        }

        public async void ClickAuthor(object sender, RoutedEventArgs e)
        {
            try
            {
                Views.Busy.SetBusy(true, _loader.GetString("HttpLoading"));
                var user = await _quizApi.GetUser(CreatedBy);
                Views.Busy.SetBusy(false, null);
                NavigationService.Navigate(typeof(Views.UserPage), user);
            }
            catch (Exception k)
            {
                var dialog = new MessageDialog($"Error: {k.Message}");
                await dialog.ShowAsync();
                return;
            }
        }

        public async void DeleteSet(object sender, RoutedEventArgs e)
        {

            var confirmDialog = new MessageDialog("Are you sure want to permanently delete this set?", "Delete Set");
            confirmDialog.Options = MessageDialogOptions.None;

            var yesCommand = new UICommand("Yes");
            var noCommand = new UICommand("No");

            confirmDialog.Commands.Add(yesCommand);
            confirmDialog.Commands.Add(noCommand);
            confirmDialog.DefaultCommandIndex = 0;
            confirmDialog.CancelCommandIndex = 0;

            var command = await confirmDialog.ShowAsync();

            if (command == yesCommand)
            {
                var result = await _quizApi.DeleteSet(Set.Id);

                if (result)
                {
                    NavigationService.Navigate(typeof(Views.MySetsPage));
                }
                else
                {
                    var dialog = new MessageDialog("Error: couldn't delete the set.");
                    await dialog.ShowAsync();
                }
            }
            else
            {
                return;
            }

            
        }

        public void EditSet(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(Views.NewSetPage), Set);
        }

    }
}
