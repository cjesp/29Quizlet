using _29Quizlet.Models;
using _29Quizlet.Repositories;
using _29Quizlet.Services.SettingsServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources;
using _29Quizlet.Repositories.Exceptions;
using Windows.UI.Xaml;
using Microsoft.Practices.Unity;

namespace _29Quizlet.ViewModels
{
    public class MySetsPageViewModel : ViewModelBase
    {
        public ObservableCollection<SetViewModel> Sets { get; set; }
        public ISetsLocalStorage _setStorage;
        private readonly ISetFetcher _setFetcher;
        private IQuizletRESTApi _quizApi;
        private SettingsService _settingsService;
        private ResourceLoader _loader;

        public string Message { get { return "No sets found!"; } set { } }

        bool _ShowNothingHere = false;
        public bool ShowNothingHere
        {
            get { return _ShowNothingHere; }
            set { Set(ref _ShowNothingHere, value); }
        }

        bool _ContainsData = false;
        public bool ContainsData
        {
            get { return _ContainsData; }
            set { Set(ref _ContainsData, value); }
        }

        public MySetsPageViewModel()
        {
            _loader = new ResourceLoader();
            Sets = new ObservableCollection<SetViewModel>();
            _setStorage = App.Container.Resolve<ISetsLocalStorage>();
            _setFetcher = App.Container.Resolve<ISetFetcher>();
            _quizApi = App.Container.Resolve<IQuizletRESTApi>();
            _settingsService = SettingsService.Instance;
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            var localSets = await _setStorage.GetAllSets();

            if (localSets?.Count() > 0)
            {
                ContainsData = true;

                foreach (var set in localSets)
                {
                    Sets.Add(new SetViewModel(set));
                }

                return;
            }

            ShowNothingHere = true;


            if (suspensionState.Any())
            {
                Sets = suspensionState[nameof(Sets)] as ObservableCollection<SetViewModel>;
            }
            else
            {
                await UpdateSets();
            }

            await Task.CompletedTask;
        }

        private bool SetIsPrivate(string visibility)
        {
            return visibility == "password" || visibility == "classes";
        }

        public void NewSet(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(Views.NewSetPage));
        }

        public async void SelectedSet(object sender, SelectionChangedEventArgs e)
        {
            var setVm = (SetViewModel)((ListView)sender).SelectedItem;
            
            try
            {
                var set = await _setFetcher.GetSet(setVm.Id);
                NavigationService.Navigate(typeof(Views.SetDetailPage), set);
            }
            catch (SetIsPrivateException)
            {
                // handle password protected case.
                NavigationService.Navigate(typeof(Views.PrivateSetPage), setVm);
            }
            catch (Exception k)
            {
                var dialog = new MessageDialog($"Error: {k.Message}");
                await dialog.ShowAsync();
                return;
            }
        }

        public async Task Refresh(object sender, RoutedEventArgs e)
        {
            Views.Busy.SetBusy(true, _loader.GetString("HttpLoading"));
            await UpdateSets();
            Views.Busy.SetBusy(false, null);
        }

        private async Task UpdateSets()
        {
            try
            {
                if (_settingsService.AuthenticatedUser != null)
                {
                    var remoteSets = await _quizApi.GetCreatedSets();

                    var sets = new List<Set>();
                    foreach (var set in remoteSets.Items)
                    {
                        Set remoteSet = null;

                        if (SetIsPrivate(set.ItemData.Visibility))
                        {
                            remoteSet = await _quizApi.GetPrivateSet(set.ItemData.Id);
                        }
                        else
                        {
                            remoteSet = await _quizApi.GetPublicSet(set.ItemData.Id);
                        }

                        sets.Add(remoteSet);
                        await _setStorage.AddSet(remoteSet);
                    }
                    await _setStorage.SaveSets();

                    if (sets.Any())
                    {
                        Sets.Clear();
                        ContainsData = true;
                        ShowNothingHere = false;

                        foreach (var set in sets)
                        {
                            Sets.Add(new SetViewModel(set));
                        }
                    }

                }
            }
            catch (Exception e)
            {
                var dialog = new MessageDialog($"Error: {e.Message}");
                await dialog.ShowAsync();
                return;
            }
        }
    }
}
