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
    public sealed partial class FlashCardControl : UserControl
    {
        //public event TappedEventHandler Tap;
        private bool _isOpen;

        public FlashCardControl()
        {
            this.InitializeComponent();
            _isOpen = true;
            //this.Tapped += CardTapped;
        }

        private void CardTapped(object sender, TappedRoutedEventArgs e)
        {
            if (_isOpen)
            {
                FlipOpen.Begin();
                _isOpen = !_isOpen;
            }
            else
            {
                FlipClose.Begin();
                _isOpen = !_isOpen;
            }
            

            //if (Tap != null)
            //{
            //    Tap(this, e);
            //}
        }
        public string FrontText
        {
            get { return (string)GetValue(BlinkProperty); }
            set { SetValue(BlinkProperty, value); }
        }

        // Using a DependencyProperty enables animation, styling, binding, etc.
        public static readonly DependencyProperty BlinkProperty =
            DependencyProperty.Register(
                "FrontText",                  // The name of the DependencyProperty
                typeof(string),             // The type of the DependencyProperty
                typeof(FlashCardControl),       // The type of the owner of the DependencyProperty
                new PropertyMetadata(     // OnBlinkChanged will be called when Blink changes
                    false
                )
            );

        public string BackText
        {
            get { return (string)GetValue(BackTextProperty); }
            set { SetValue(BackTextProperty, value); }
        }

        // Using a DependencyProperty enables animation, styling, binding, etc.
        public static readonly DependencyProperty BackTextProperty =
            DependencyProperty.Register(
                "BackText",                  // The name of the DependencyProperty
                typeof(string),             // The type of the DependencyProperty
                typeof(FlashCardControl),       // The type of the owner of the DependencyProperty
                new PropertyMetadata(     // OnBlinkChanged will be called when Blink changes
                    false
                )
            );
    }
}
