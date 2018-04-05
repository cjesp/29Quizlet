using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class NothingHereControl : UserControl
    {
        public NothingHereControl()
        {
            this.InitializeComponent();
        }

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(
                "Message",                  // The name of the DependencyProperty
                typeof(string),             // The type of the DependencyProperty
                typeof(NothingHereControl), // The type of the owner of the DependencyProperty
                null
                //new PropertyMetadata(     // OnBlinkChanged will be called when Blink changes
                //    false
                //)
            );
    }
}
