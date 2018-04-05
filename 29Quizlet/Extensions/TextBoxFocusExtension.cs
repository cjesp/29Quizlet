using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace _29Quizlet.Extensions
{
    public class TextBoxFocusExtension
    {
        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }

        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.RegisterAttached(
                "IsFocused", typeof(bool), typeof(TextBoxFocusExtension),
                new PropertyMetadata(false, OnIsFocusedPropertyChanged));

        private static void OnIsFocusedPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox)
            {
                //Unbox.
                TextBox t = d as TextBox;

                //If setting is true.
                if ((bool)e.NewValue)
                {
                    //Set the Focus
                    t.Focus(FocusState.Pointer);

                    //Attach handling for lost focus.
                    t.LostFocus += textbox_LostFocus;
                }
                else
                {
                    //Remove handling.
                    t.LostFocus -= textbox_LostFocus;
                }
            }
        }

        private static void textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox t = sender as TextBox;

            t.SetValue(IsFocusedProperty, false);
        }
    }
}
