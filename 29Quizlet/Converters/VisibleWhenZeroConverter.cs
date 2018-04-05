using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace _29Quizlet.Converters
{
    public class VisibleWhenZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) =>
            Equals(0, (int)value) ? Visibility.Visible : Visibility.Collapsed;     

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
