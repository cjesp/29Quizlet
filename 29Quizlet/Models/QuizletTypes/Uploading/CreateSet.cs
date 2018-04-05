using _29Quizlet.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.QuizletTypes.Uploading
{
    [DataContract]
    public class CreateSet
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "lang_terms")]
        public string LangTerms { get; set; }

        [DataMember(Name = "lang_definitions")]
        public string LangDefinitions { get; set; }

        [DataMember(Name = "terms")]
        public string[] Terms { get; set; }

        [DataMember(Name = "definitions")]
        public string[] Definitions { get; set; }

        [JsonConverter(typeof(SetVisibilityConverter))]
        [DataMember(Name = "visibility")]
        public Visibility Visibility { get; set; }
    }

    public enum Visibility
    {
        Everyone,
        OnlyMe
    }
}
