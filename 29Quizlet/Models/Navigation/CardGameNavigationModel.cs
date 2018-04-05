using _29Quizlet.Helpers.Enums;
using _29Quizlet.Models.ViewModels;
using _29Quizlet.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.Navigation
{
    public class CardGameNavigationModel
    {
        public Set Set { get; set; }
        public ObservableCollection<TermViewModel> Terms { get; set; }
        public DefinitionsOrTerms DefinitionOrTerms { get; set; }
        public SortingModes SortingMode { get; set; }
    }
}
