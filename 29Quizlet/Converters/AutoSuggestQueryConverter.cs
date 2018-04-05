using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace _29Quizlet.Converters
{
    public class AutoSuggestQueryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // cast value to whatever EventArgs class you are expecting here
            var args = (AutoSuggestBoxQuerySubmittedEventArgs)value;
            // return what you need from the args
            return (string)args.ChosenSuggestion ?? (string)args.QueryText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
