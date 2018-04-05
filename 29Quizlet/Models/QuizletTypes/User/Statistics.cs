using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.QuizletTypes.User
{
    [DataContract]
    public class Statistics
    {

        [DataMember(Name = "study_session_count")]
        public int StudySessionCount { get; set; }

        [DataMember(Name = "total_answer_count")]
        public int TotalAnswerCount { get; set; }

        [DataMember(Name = "public_sets_created")]
        public int PublicSetsCreated { get; set; }

        [DataMember(Name = "public_terms_entered")]
        public int PublicTermsEntered { get; set; }

        //[DataMember(Name = "total_sets_created")]
        //public int TotalSetsCreated { get; set; }

        //[DataMember(Name = "total_terms_entered")]
        //public int TotalTermsEntered { get; set; }
    }
}
