using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.Navigation
{
    public class LoginPageNavigationModel
    {
        public string Code { get; set; }
        public string State { get; set; }
        public string Expires { get; set; }
        public string Error { get; set; }
        public string ErrorDescription { get; set; }
    }
}
