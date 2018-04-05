using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.QuizletTypes
{
    public class Authentication
    {
        // if the authentication was a success
        public class Success
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string token_type { get; set; }
            public string scope { get; set; }
            public string user_id { get; set; }
        }

        // otherwise error
        public class Error
        {
            public string error { get; set; }
            public string error_description { get; set; }
        }
    }
}
