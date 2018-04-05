using _29Quizlet.Models;
using _29Quizlet.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Template10.Services.NavigationService;
using Windows.UI.Popups;
using _29Quizlet.Repositories.Exceptions;
using Windows.ApplicationModel.Resources;
using Microsoft.Practices.Unity;

namespace _29Quizlet.ViewModels
{
    public class SetsPageViewModel : ViewModelBase
    {
        public ObservableCollection<SetViewModel> Sets { get; set; }
        public ISetsLocalStorage SetStorage { get; set; }
        private IQuizletRESTApi _quizApi;
        private ResourceLoader _loader;

        public Set SelectedSet { get; set; }

        public string Message { get; set; } = "No sets found!";

        bool _ShowNothingHere = true;
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

        public SetsPageViewModel()
        {
            _quizApi = App.Container.Resolve<IQuizletRESTApi>();
            SetStorage = App.Container.Resolve<ISetsLocalStorage>();
            Sets = new ObservableCollection<SetViewModel>();
            _loader = new ResourceLoader();
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {

            if (suspensionState.Any())
            {
                Sets = suspensionState[nameof(Sets)] as ObservableCollection<SetViewModel>;
            }

            if (parameter != null)
            {
                var sets = parameter as IEnumerable<Set>;
                if (sets != null && sets.Any())
                {
                    ShowNothingHere = false;
                    ContainsData = true;

                    foreach (var set in sets)
                    {
                        Sets.Add(new SetViewModel(set));
                    }
                }
            }
            else
            {
                var allSetsFromStorage = await SetStorage.GetAllSets();
                Sets.Clear();
                foreach (var set in allSetsFromStorage)
                {
                    Sets.Add(new SetViewModel(set));
                }
            }

            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            await Task.CompletedTask;
        }

        public async Task ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var setVm = (SetViewModel)((ListView)sender).SelectedItem;

            try
            {
                Views.Busy.SetBusy(true, _loader.GetString("HttpLoading"));
                var set = await _quizApi.GetPublicSet(setVm.Id);
                Views.Busy.SetBusy(false, null);
                NavigationService.Navigate(typeof(Views.SetDetailPage), set);
            }
            catch (SetIsPrivateException)
            {
                // handle password protected case.
                Views.Busy.SetBusy(false, null);
                NavigationService.Navigate(typeof(Views.PrivateSetPage), setVm);
            }
            catch (Exception k)
            {
                Views.Busy.SetBusy(false, null);
                var dialog = new MessageDialog($"Error: {k.Message}");
                await dialog.ShowAsync();
                return;
            }
        }
    }
}
