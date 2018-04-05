using _29Quizlet.Models;
using _29Quizlet.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace _29Quizlet.ViewModels
{
    public class PrivateSetPageViewModel : ViewModelBase
    {
        private IQuizletRESTApi _quizApi;
        public long SetId { get; set; }

        string _SetTitle = default(string);
        public string SetTitle { get { return _SetTitle; } set { Set(ref _SetTitle, value); } }

        string _Author = default(string);
        public string Author { get { return _Author; } set { Set(ref _Author, value); } }

        string _Password = default(string);
        public string Password { get { return _Password; } set { Set(ref _Password, value); } }
        
        bool _ShowStatus = default(bool);
        public bool ShowStatus { get { return _ShowStatus; } set { Set(ref _ShowStatus, value); } }

        string _StatusText = default(string);
        public string StatusText { get { return _StatusText; } set { Set(ref _StatusText, value); } }

        public PrivateSetPageViewModel()
        {
            _quizApi = new QuizletRemoteRESTApi();
            ShowStatus = false;
            StatusText = "The password provided wasn't valid";
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var setVm = parameter as SetViewModel;

            if (setVm != null)
            {
                SetTitle = setVm.Title;
                Author = setVm.Author;
                SetId = setVm.Id;
            }

            return Task.CompletedTask;
        }

        public async void SubmitClicked(object sender, RoutedEventArgs e)
        {
            var tempDialog = new MessageDialog("Sorry, this feature isn't available yet due to Quizlet API limitations");
            await tempDialog.ShowAsync();
            return;

            //todo: get scope from Quizlet to allow this...
            if (string.IsNullOrEmpty(Password))
            {
                var dialog = new MessageDialog("Please type in a password.");
                await dialog.ShowAsync();
            }
            else
            {
                var result = await _quizApi.GetPrivateSet(SetId, Password);
                if (result)
                {
                    var set = _quizApi.GetPublicSet(SetId);
                }
                else
                {
                    Password = string.Empty;
                }
            }
        }
    }
}
