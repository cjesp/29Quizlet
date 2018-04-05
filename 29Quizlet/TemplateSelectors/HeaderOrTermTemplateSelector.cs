using _29Quizlet.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace _29Quizlet.TemplateSelectors
{
    public class HeaderOrTermTemplateSelector : DataTemplateSelector
    {
        public DataTemplate HeaderTemplate { get; set; }
        public DataTemplate TermTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var test1 = item as ResultHeader;
            if (test1 != null)
            {
                return HeaderTemplate;
            }

            var test2 = item as TermResultViewModel;
            if (test2 != null)
            {
                return TermTemplate;
            }

            return TermTemplate;
        }
    }
}
