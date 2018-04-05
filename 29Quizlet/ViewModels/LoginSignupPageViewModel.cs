using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.System;
using Windows.UI.Xaml.Input;

namespace _29Quizlet.ViewModels
{
    public class LoginSignupPageViewModel : ViewModelBase
    {
        public string LoginText { get; set; } = "Please login";



        public void LoginTapped(object sender, TappedRoutedEventArgs e)
        {
            var guid = Guid.NewGuid().ToString();
            var uri = new Uri($"https://quizlet.com/authorize?response_type=code&client_id=XXXXX&scope=read%20write_set%20write_group&state={guid}");
            Task.Run(() => Launcher.LaunchUriAsync(uri));
        }

        public void SignUpTapped(object sender, TappedRoutedEventArgs e)
        {
            var guid = Guid.NewGuid().ToString();
            var uri = new Uri($"https://quizlet.com/sign-up");
            Task.Run(() => Launcher.LaunchUriAsync(uri));
        }
    }
}
