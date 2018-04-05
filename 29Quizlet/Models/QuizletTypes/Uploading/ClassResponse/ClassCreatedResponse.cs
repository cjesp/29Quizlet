using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.QuizletTypes.Uploading.ClassResponse
{

    [DataContract]
    public class ClassCreatedResponse
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "class_id")]
        public int ClassId { get; set; }
    }
}
