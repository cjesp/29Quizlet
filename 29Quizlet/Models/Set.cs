using _29Quizlet.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models
{
    [DataContract]
    public class Set
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "created_by")]
        public string CreatedBy { get; set; }

        [DataMember(Name = "term_count")]
        public int TermCount { get; set; }

        [JsonConverter(typeof(MicrosecondEpochConverter))]
        [DataMember(Name = "created_date")]
        public DateTime? CreatedDate { get; set; }

        [DataMember(Name = "modified_date")]
        public int ModifiedDate { get; set; }

        [DataMember(Name = "has_images")]
        public bool HasImages { get; set; }

        [DataMember(Name = "subjects")]
        public List<string> Subjects { get; set; }

        [DataMember(Name = "visibility")]
        public string Visibility { get; set; }

        [DataMember(Name = "editable")]
        public string Editable { get; set; }

        [DataMember(Name = "has_access")]
        public bool HasAccess { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "lang_terms")]
        public string LangTerms { get; set; }

        [DataMember(Name = "lang_definitions")]
        public string LangDefinitions { get; set; }

        [DataMember(Name = "terms")]
        public List<Term> Terms { get; set; }
    }

    public class SetViewModel
    {
        public string Title { get; set; }
        public string Date { get; set; }
        public string Author { get; set; }
        public string TermCount { get; set; }
        public string HasImages { get; set; }
        public long Id { get; set; }
        public string Description { get; set; }
        public bool HasDescription { get; set; }
        public bool IsPrivate { get; set; } = false;
        //public Set OriginSet { get; set; }

        public SetViewModel()
        {

        }

        public SetViewModel(Set set)
        {
            Id = set.Id;

            //OriginSet = set;

            if (!string.IsNullOrEmpty(set.Title))
            {
                Title = set.Title;
            }
            else
                Title = "No title for this set";

            if (!string.IsNullOrEmpty(set.Description))
            {
                Description = set.Description.Replace("\n", "").Replace("\r", "");
                HasDescription = true;
            }
            else
            {
                HasDescription = false;
            }

            if (!string.IsNullOrEmpty(set.CreatedBy))
            {
                Author = set.CreatedBy;
            }
            if (set.CreatedDate != null)
            {
                Date = string.Format("{0:d}", set.CreatedDate);
            }
            if (set.HasImages)
            {
                HasImages = " \uE114";
            }
            else
                HasImages = "";

            if (set.Visibility == "password")
            {
                IsPrivate = true;
            }

            TermCount = $"{set.TermCount} terms";
        }
    }
}
