using _29Quizlet.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace _29Quizlet.Converters
{
    public class ComboBoxLanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value as LanguageViewModel; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value as LanguageViewModel;
        }
    }
}
