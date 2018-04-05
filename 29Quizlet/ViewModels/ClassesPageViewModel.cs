using _29Quizlet.Models.QuizletTypes.User;
using _29Quizlet.Repositories;
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

namespace _29Quizlet.ViewModels
{
    public class ClassesPageViewModel : ViewModelBase
    {
        public ObservableCollection<ClassesViewModel> Classes { get; set; }
        private IQuizletRESTApi _quizApi;

        public string Message { get; set; } = "No classes found!";

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

        string _SchoolText = default(string);
        public string SchoolText
        {
            get { return _SchoolText; }
            set { Set(ref _SchoolText, value); }
        }

        public ClassesPageViewModel()
        {
            Classes = new ObservableCollection<ClassesViewModel>();
            _quizApi = new QuizletRemoteRESTApi();
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter != null)
            {
                var classes = parameter as IEnumerable<Classes>;
                if (classes != null && classes.Any())
                {
                    ShowNothingHere = false;
                    ContainsData = true;

                    foreach (var quizClass in classes)
                    {
                        Classes.Add(new ClassesViewModel(quizClass));
                    }
                }
            }

            await Task.CompletedTask;
        }

        public async Task ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var classVm = (ClassesViewModel)((ListView)sender).SelectedItem;
            try
            {
                var quizClass = await _quizApi.GetClass(classVm.Id); 
                NavigationService.Navigate(typeof(Views.ClassDetailPage), quizClass);
            }
            catch (Exception k)
            {
                var dialog = new MessageDialog($"Error: {k.Message}");
                await dialog.ShowAsync();
                return;
            }

        }
    }
}
