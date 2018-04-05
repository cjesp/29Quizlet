using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace _29Quizlet.Models.ViewModels
{
    public class TermDefinitionViewModel : ViewModelBase
    {
        string _Term = default(string);
        public string Term { get { return _Term; } set { Set(ref _Term, value); Valid = true; } }

        string _Definition = default(string);
        public string Definition { get { return _Definition; } set { Set(ref _Definition, value); Valid = true; } }

        bool _Valid = default(bool);
        public bool Valid { get { return _Valid; } set { Set(ref _Valid, value); } }
        
    }
}
