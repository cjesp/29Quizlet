using _29Quizlet.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace _29Quizlet.Converters
{
    public class MatchButtonColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var color = (MatchButtonColorEnum)value;

            switch (color)
            {
                case MatchButtonColorEnum.Picked:
                    return new SolidColorBrush(Windows.UI.Colors.LightBlue);
                case MatchButtonColorEnum.Wrong:
                    return new SolidColorBrush(Windows.UI.Colors.LightPink);
                case MatchButtonColorEnum.Correct:
                    return new SolidColorBrush(Windows.UI.Colors.LightGreen);
                case MatchButtonColorEnum.Neutral:
                    return new SolidColorBrush(Windows.UI.Colors.Transparent);
                default:
                    return new SolidColorBrush(Windows.UI.Colors.Transparent);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
