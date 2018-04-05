using _29Quizlet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace _29Quizlet.TemplateSelectors
{
    public class TermViewSelector : DataTemplateSelector
    {
        public DataTemplate TermWithImage { get; set; }
        public DataTemplate TermWithoutImage { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {

            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is Term)
            {
                Term term = item as Term;

                if (term.Image != null)
                {
                    return TermWithImage;
                }
                else
                {
                    return TermWithoutImage;
                }
            }

            return null;
        }
    }
}
