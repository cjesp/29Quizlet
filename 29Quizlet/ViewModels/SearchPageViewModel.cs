using _29Quizlet.Commands;
using _29Quizlet.Models.QuizletTypes.Search;
using _29Quizlet.Repositories;
using _29Quizlet.Services.SettingsServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources;
using Microsoft.Practices.Unity;

namespace _29Quizlet.ViewModels
{
    public class SearchPageViewModel : ViewModelBase
    {
        public ObservableCollection<SetQViewModel> _sets;
        public ObservableCollection<UserQViewModel> _users;
        public ObservableCollection<GroupQViewModel> _classes;
        private IQuizletRESTApi _remoteApi;
        private SettingsService _settingsService;
        private string _searchString;
        private ResourceLoader _loader;


        int _PivotNavIndex;
        public int PivotNavIndex
        {
            get { return _PivotNavIndex; }
            set { Set(ref _PivotNavIndex, value); }
        }

        string _Query = default(string);
        public string Query
        {
            get { return _Query; }
            set { Set(ref _Query, value); }
        }

        public SearchPageViewModel()
        {
            _loader = new ResourceLoader();
            _sets = new ObservableCollection<SetQViewModel>();
            _users = new ObservableCollection<UserQViewModel>();
            _classes = new ObservableCollection<GroupQViewModel>();
            _remoteApi = App.Container.Resolve<IQuizletRESTApi>();
            _settingsService = SettingsService.Instance;
            PivotNavIndex = 0; 
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.Any())
            {
                var searchString = suspensionState[nameof(_searchString)] as string;
                if (searchString != null)
                {
                    TextChanged.Execute(searchString);
                    Query = searchString;
                }

                if (suspensionState.ContainsKey(nameof(PivotNavIndex)))
                {
                    PivotNavIndex = (int)suspensionState[nameof(PivotNavIndex)];
                }
            }


            await Task.CompletedTask;
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> pageState, bool suspending)
        {
            pageState[nameof(_searchString)] = _searchString;
            pageState[nameof(PivotNavIndex)] = PivotNavIndex;

            return Task.CompletedTask;
        }

        private DelegateCommander _textChanged;

        public DelegateCommander TextChanged
        {
            get
            {
                if (_textChanged == null)
                {
                    _textChanged = new DelegateCommander(async (input) =>
                    {
                        var inputAsString = input as string;
                        if (inputAsString == null)
                        {
                            return;
                        }

                        _searchString = inputAsString;

                        if (_settingsService.UserSettings == null)
                        {
                            // We can't search if we're not logged in.
                            var dialog = new MessageDialog("You need to be logged in to search.");
                            await dialog.ShowAsync();
                            return;
                        }

                        try
                        {
                            var results = await _remoteApi.SearchUniversal(inputAsString);

                            if (!results.Success)
                            {
                                // means we encountered an error
                                var dialog = new MessageDialog("Couldn't complete the search. Maybe the network is down.");
                                await dialog.ShowAsync();
                                return;
                            }

                            _sets.Clear();
                            _users.Clear();
                            _classes.Clear();

                            foreach (var item in results.items)
                            {
                                if (item is SetQ)
                                {
                                    _sets.Add(new SetQViewModel((SetQ)item));
                                }
                                if (item is UserQ)
                                {
                                    _users.Add(new UserQViewModel((UserQ)item));
                                }
                                if (item is GroupQ)
                                {
                                    _classes.Add(new GroupQViewModel((GroupQ)item));
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            var dialog = new MessageDialog($"Error: {e.Message}");
                            await dialog.ShowAsync();
                            return;
                        }
                    });

                }

                return _textChanged;

            }
        }

        public async Task SelectedSet(object sender, SelectionChangedEventArgs e)
        {
            var set = (SetQViewModel)((ListView)sender).SelectedItem;
            Views.Busy.SetBusy(true, _loader.GetString("HttpLoading"));
            var fecthedSet = await _remoteApi.GetPublicSet(set.Id);
            Views.Busy.SetBusy(false, null);
            PivotNavIndex = 0;
            NavigationService.Navigate(typeof(Views.SetDetailPage), fecthedSet);
        }

        public async Task SelectedUser(object sender, SelectionChangedEventArgs e)
        {
            var user = (UserQViewModel)((ListView)sender).SelectedItem;
            Views.Busy.SetBusy(true, _loader.GetString("HttpLoading"));
            var fetchedUser = await _remoteApi.GetUser(user.Username);
            Views.Busy.SetBusy(false, null);
            PivotNavIndex = 1;
            NavigationService.Navigate(typeof(Views.UserPage), fetchedUser);
        }

        public async Task SelectedClass(object sender, SelectionChangedEventArgs e)
        {
            var quizClass = (GroupQViewModel)((ListView)sender).SelectedItem;
            Views.Busy.SetBusy(true, _loader.GetString("HttpLoading"));
            var fetchedClass = await _remoteApi.GetClass(quizClass.Id);
            Views.Busy.SetBusy(false, null);
            PivotNavIndex = 2;
            NavigationService.Navigate(typeof(Views.ClassDetailPage), fetchedClass);
        }
    }
}
