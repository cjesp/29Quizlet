using _29Quizlet.Models.QuizletTypes.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.QuizletTypes.Uploading
{

    [DataContract]
    public class CreateClass
    {
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string description { get; set; }

        public Classes ClassForLocal { get; set; }
    }
}
