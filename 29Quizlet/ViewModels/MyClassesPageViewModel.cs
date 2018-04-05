using _29Quizlet.Models.QuizletTypes.User;
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
using Windows.UI.Xaml;
using Microsoft.Practices.Unity;
using _29Quizlet.Helpers.Enums;

namespace _29Quizlet.ViewModels
{
    public class MyClassesPageViewModel : ViewModelBase
    {
        public ObservableCollection<ClassesViewModel> Classes { get; set; }
        private IQuizletRESTApi _quizApi;
        private SettingsService _settingsService;
        private ResourceLoader _loader;
        private readonly ILocalClassesStorage _classesStorage;

        public string Message { get { return "No classes found!"; }  set { } }

        string _SchoolText = default(string);
        public string SchoolText
        {
            get { return _SchoolText; }
            set { Set(ref _SchoolText, value); }
        }

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

        public MyClassesPageViewModel()
        {
            Classes = new ObservableCollection<ClassesViewModel>();
            _quizApi = App.Container.Resolve<IQuizletRESTApi>();
            _classesStorage = App.Container.Resolve<ILocalClassesStorage>();
            _settingsService = SettingsService.Instance;
            _loader = new ResourceLoader();
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var returnVal = NewClassNavigation.None;
            if (parameter != null)
            {
                returnVal = (NewClassNavigation)parameter;
            }

            if (returnVal == NewClassNavigation.None || parameter == null)
            {
                var localClasses = await _classesStorage.GetAllClasses();

                if (localClasses?.Count() > 0)
                {
                    ContainsData = true;
                    foreach (var clazz in localClasses)
                    {
                        Classes.Add(new ClassesViewModel(clazz));
                    }

                    return;
                }
            }
            else
            {
                ShowNothingHere = true;

                if (_settingsService.UserSettings != null)
                {
                    await UpdateClasses();
                }
            }
            

            await Task.CompletedTask;
        }

        public async Task ClassSelected(object sender, SelectionChangedEventArgs e)
        {
            var classVm = (ClassesViewModel)((ListView)sender).SelectedItem;
            try
            {
                var clazz = await _classesStorage.GetClassById(classVm.Id);
                if (clazz != null)
                {
                    NavigationService.Navigate(typeof(Views.ClassDetailPage), clazz);
                }
                else
                {
                    Views.Busy.SetBusy(true, _loader.GetString("HttpLoading"));
                    var quizClass = await _quizApi.GetClass(classVm.Id);
                    Views.Busy.SetBusy(false, null);
                    NavigationService.Navigate(typeof(Views.ClassDetailPage), quizClass);
                }
            }
            catch (Exception k)
            {
                var dialog = new MessageDialog($"Error: {k.Message}");
                await dialog.ShowAsync();
                return;
            }
        }

        public void NewClass(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(Views.NewClassPage));
        }

        public async Task Refresh(object sender, RoutedEventArgs e)
        {
            Views.Busy.SetBusy(true, _loader.GetString("HttpLoading"));
            await UpdateClasses();
            Views.Busy.SetBusy(false, null);
        }

        public async Task UpdateClasses()
        {
            try
            {
                var classes = await _quizApi.GetClasses(_settingsService.AuthenticatedUser);

                if (classes.Any())
                {
                    ShowNothingHere = false;
                    ContainsData = true;
                    await _classesStorage.DeleteAllClasses();
                    foreach (var clazz in classes)
                    {
                        await _classesStorage.AddClass(clazz);
                    }

                    await _classesStorage.SaveClasses();

                }

                var classIds = new List<int>();
                Classes.Clear();
                foreach (var quizClass in classes)
                {
                    classIds.Add(quizClass.Id);
                    Classes.Add(new ClassesViewModel(quizClass));
                }

                _settingsService.UserClasses = classIds;
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
