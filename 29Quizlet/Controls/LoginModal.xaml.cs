using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace _29Quizlet.Controls
{
    public sealed partial class LoginModal : UserControl
    {

        public string Username { get; set; }
        public string Password { get; set; }

        public LoginModal()
        {
            this.InitializeComponent();
        }

        public event EventHandler HideRequested;
        public event EventHandler LoggedIn;

        private void LoginClicked(object sender, RoutedEventArgs e)
        {
            LoggedIn?.Invoke(this, EventArgs.Empty);
            var guid = Guid.NewGuid().ToString();
            var uri = new Uri($"https://quizlet.com/authorize?response_type=code&client_id=XXX&scope=read&state={guid}");
            Task.Run(() => Launcher.LaunchUriAsync(uri));
            LoggedIn?.Invoke(this, EventArgs.Empty);
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            HideRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
