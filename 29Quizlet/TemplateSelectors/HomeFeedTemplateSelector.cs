using _29Quizlet.Models.QuizletTypes.Feeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace _29Quizlet.TemplateSelectors
{
    public class HomeFeedTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ClassFeedTemplate { get; set; }
        public DataTemplate SessionFeedTemplate { get; set; }
        public DataTemplate SetFeedTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null)
            {
                if (item is ClassFeedVM)
                {
                    return ClassFeedTemplate;
                }
                if (item is StudySessionItemVM)
                {
                    return SessionFeedTemplate;
                }
                if (item is ItemVM)
                {
                    return SetFeedTemplate;
                }
                
            }

            return null;
        }
    }
}
