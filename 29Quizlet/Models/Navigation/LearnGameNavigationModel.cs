using _29Quizlet.Helpers.Enums;
using _29Quizlet.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.Navigation
{
    public class LearnGameStatClass
    {
        public TermViewModel Term { get; set; }
        public int Errors { get; set; }
    }

    public class LearnGameNavigationModel
    {
        public int GameRound { get; set; }
        public DefinitionsOrTerms Mode { get; set; }
        public List<TermViewModel> Terms { get; set; }
        public Set Set { get; set; }
        public List<TermViewModel> ErrorTerms { get; set; }
        public List<TermViewModel> SuccesfulTerms { get; set; }
        public List<TermViewModel> TotalSuccessfulTerms { get; set; }
        public List<TermViewModel> GameTerms { get; set; }
        public List<LearnGameStatClass> Stats { get; set; }
    }
}
