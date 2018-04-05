using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace _29Quizlet.Models
{
    public class UserSettings : BindableBase
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string TokenType { get; set; }
        public string Scope { get; set; }
        public int Expiration { get; set; }
        public DateTime Created { get; set; }
    }
}
