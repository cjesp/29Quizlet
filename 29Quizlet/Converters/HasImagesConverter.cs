﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace _29Quizlet.Converters
{
    public class HasImagesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var has_image = (bool)value;
            if (has_image)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
