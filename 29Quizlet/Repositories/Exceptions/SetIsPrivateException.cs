using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Repositories.Exceptions
{
    
    public class SetIsPrivateException : Exception
    {
        public SetIsPrivateException() { }
        public SetIsPrivateException(string message) : base(message) { }
        public SetIsPrivateException(string message, Exception inner) : base(message, inner) { }
    }
}
