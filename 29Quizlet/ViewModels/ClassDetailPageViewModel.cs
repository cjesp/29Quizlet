using _29Quizlet.Helpers.Enums;
using _29Quizlet.Models.QuizletTypes.User;
using _29Quizlet.Repositories;
using _29Quizlet.Services.SettingsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Unity;

namespace _29Quizlet.ViewModels
{
    public class ClassDetailPageViewModel : ViewModelBase
    {
        private IQuizletRESTApi _quizApi;
        private SettingsService _settingsService;

        Classes _QuizClass;
        public Classes QuizClass
        {
            get { return _QuizClass; }
            set { Set(ref _QuizClass, value); }
        }

        string _CityAndState = default(string);
        public string CityAndState
        {
            get { return _CityAndState; }
            set { Set(ref _CityAndState, value); }
        }
        
        bool _Editable = default(bool);
        public bool Editable { get { return _Editable; } set { Set(ref _Editable, value); } }

        public ClassDetailPageViewModel()
        {
            Editable = false;
            _quizApi = App.Container.Resolve<IQuizletRESTApi>();
            _settingsService = SettingsService.Instance;
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var quizClass = parameter as Classes;

            if (quizClass != null)
            {
                QuizClass = quizClass;
                var classes = _settingsService.UserClasses;

                if (classes.Where(x => x == quizClass.Id).Any())
                {
                    Editable = true;
                }

                if (QuizClass.School != null)
                {
                    var locationOutputStr = string.Empty;

                    if (!string.IsNullOrEmpty(QuizClass.School.City))
                    {
                        locationOutputStr = locationOutputStr + $"{QuizClass.School.City}";
                    }

                    if (!string.IsNullOrEmpty(QuizClass.School.State))
                    {
                        locationOutputStr = locationOutputStr + $", {QuizClass.School.State}";
                    }

                    if (!string.IsNullOrEmpty(QuizClass.School.Country))
                    {
                        locationOutputStr = locationOutputStr + $" ({QuizClass.School.Country})";
                    }

                    CityAndState = locationOutputStr;
                }
            }

            await Task.CompletedTask;
        }
        
        public async void ViewSets(object sender, RoutedEventArgs e)
        {
            try
            {
                var sets = await _quizApi.GetSets(QuizClass);
                NavigationService.Navigate(typeof(Views.SetsPage), sets);
            }
            catch (Exception k)
            {
                var dialog = new MessageDialog($"Error: {k.Message}");
                await dialog.ShowAsync();
                return;
            }
        }

        public async void Enroll(object sender, RoutedEventArgs e)
        {
            //todo: implement enrolling
            var dialog = new MessageDialog($"Feature not implemented yet!");
            await dialog.ShowAsync();
            return;
            
        }

        public async void DeleteClass(object sender, RoutedEventArgs e)
        {

            var confirmDialog = new MessageDialog("Are you sure want to permanently delete this class?", "Delete Class");
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


                var result = await _quizApi.DeleteClass(QuizClass.Id);

                if (result)
                {
                    NavigationService.Navigate(typeof(Views.MyClassesPage), NewClassNavigation.UpdatedOrNewClass);
                }
                else
                {
                    var dialog = new MessageDialog("Error: couldn't delete the class.");
                    await dialog.ShowAsync();
                }
            }
            else
            {
                return;
            }


        }

        public void EditClass(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(Views.NewClassPage), QuizClass);
        }
    }
}
