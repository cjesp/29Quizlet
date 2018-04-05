﻿using _29Quizlet.ViewModels;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace _29Quizlet.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LearnPage : Page
    {
        private readonly LearnPageViewModel _vm;

        public LearnPage()
        {
            this.InitializeComponent();
            _vm = DataContext as LearnPageViewModel;
            _vm.InputTextBoxFocusEvent += SetFocusOnInputTextbox;
        }

        private void SetFocusOnInputTextbox()
        {
            InputTextBox.Focus(FocusState.Pointer);
        }
    }
}
