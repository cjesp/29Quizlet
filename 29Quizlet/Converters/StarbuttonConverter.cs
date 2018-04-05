using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace _29Quizlet.Converters
{
    public class StarbuttonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var favorite = (bool)value;
            if (favorite)
            {
                return "\uE735";
            }
            else
            {
                return "\uE734";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
