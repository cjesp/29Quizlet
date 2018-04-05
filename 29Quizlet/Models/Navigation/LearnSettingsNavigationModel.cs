using _29Quizlet.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.Navigation
{
    public class LearnSettingsNavigationModel
    {
        public Set Set { get; set; }
        public ObservableCollection<TermViewModel> Terms { get; set; }
    }
}
