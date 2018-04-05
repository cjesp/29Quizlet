using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using _29Quizlet.ViewModels;
using _29Quizlet.Helpers.Enums;

namespace _29Quizlet.Converters
{
    public class FlashCardSortingModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var enumVal = (SortingModes)Enum.Parse(typeof(SortingModes), value.ToString());
            switch (enumVal)
            {
                case SortingModes.Alphabetical:
                    return "Alphabetically";
                case SortingModes.Random:
                    return "Randomized";
                default:
                    break;
            }

            return enumVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var str = value as string;
            if (!string.IsNullOrEmpty(str))
            {
                if (str == "Alphabetically")
                {
                    return SortingModes.Alphabetical;
                }
                if (str == "Randomized")
                {
                    return SortingModes.Random;
                }
            }
            return null; // will not happen
        }
    }
}
