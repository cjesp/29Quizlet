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
    public class FlashCardModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var enumVal = (DefinitionsOrTerms)Enum.Parse(typeof(DefinitionsOrTerms), value.ToString());
            switch (enumVal)
            {
                case DefinitionsOrTerms.Definitions:
                    return "Definitions";
                case DefinitionsOrTerms.Terms:
                    return "Terms";
                case DefinitionsOrTerms.RandomMix:
                    return "Random Mix";
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
                if (str == "Definitions")
                {
                    return DefinitionsOrTerms.Definitions;
                }
                if (str == "Terms")
                {
                    return DefinitionsOrTerms.Terms;
                }
                if (str == "Random Mix")
                {
                    return DefinitionsOrTerms.RandomMix;
                }
            }
            return null; // will not happen
        }
    }
}
