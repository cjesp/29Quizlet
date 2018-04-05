using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.ViewModels
{
    public class TermResultViewModel : LearnResultListViewBase
    {
        public TermViewModel Term { get; set; }
        public int Errors { get; set; }
    }
}
