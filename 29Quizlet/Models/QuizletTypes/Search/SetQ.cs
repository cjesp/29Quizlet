using _29Quizlet.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.QuizletTypes.Search
{
    [DataContract]
    public class SetQ : ISearchItem
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string url { get; set; }

        [DataMember]
        public string title { get; set; }

        [DataMember]
        public string created_by { get; set; }

        [DataMember]
        public int term_count { get; set; }

        [DataMember(Name= "created_date")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime? created_date { get; set; }

        [DataMember(Name = "modified_date")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime? modified_date { get; set; }

        [DataMember(Name = "published_date")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime? published_date { get; set; }

        [DataMember]
        public bool has_images { get; set; }

        [DataMember]
        public List<object> subjects { get; set; }

        [DataMember]
        public string visibility { get; set; }

        [DataMember]
        public string editable { get; set; }

        [DataMember]
        public bool has_access { get; set; }

        [DataMember]
        public bool can_edit { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string lang_terms { get; set; }

        [DataMember]
        public string lang_definitions { get; set; }

        [DataMember]
        public int password_use { get; set; }

        [DataMember]
        public int password_edit { get; set; }

        [DataMember]
        public int access_type { get; set; }

        [DataMember]
        public int creator_id { get; set; }

        [DataMember]
        public string type { get; set; }
    }

    public class SetQViewModel
    {
        public string Title { get; set; }
        public string Date { get; set; }
        public string Author { get; set; }
        public string TermCount { get; set; }
        public string HasImages { get; set; }
        public long Id { get; set; }
        public string Description { get; set; }
        public bool HasDescription { get; set; }

        public SetQViewModel(SetQ set)
        {
            Id = set.id;

            if (!string.IsNullOrEmpty(set.title))
            {
                Title = set.title;
            }
            else
                Title = "No title for this set";

            if (!string.IsNullOrEmpty(set.description))
            {
                Description = set.description.Replace("\n", "").Replace("\r", "");
                HasDescription = true;
            }
            else
            {
                HasDescription = false;
            }

            if (!string.IsNullOrEmpty(set.created_by))
            {
                Author = set.created_by;
            }
            if (set.created_date != null)
            {
                Date = string.Format("{0:d}", set.created_date);
            }
            if (set.has_images)
            {
                HasImages = " \uE114";
            }
            else
                HasImages = "";


            TermCount = $"{set.term_count} terms";
        }

        //public int Id { get; set; }
        //public string Title { get; set; }
        //public string Description { get; set; }
        //public string Author { get; set; }
        //public string Date { get; set; }
        //public string TermCount { get; set; }

        //public SetQViewModel(SetQ set)
        //{
        //    Id = set.id;

        //    if (!string.IsNullOrEmpty(set.title))
        //    {
        //        Title = set.title;
        //    }
        //    else
        //        Title = "No title for this set";

        //    if (!string.IsNullOrEmpty(set.description))
        //    {
        //        Description = set.description.Replace("\n", "").Replace("\r", "");
        //    }
        //    else
        //        Description = "no description for this set";

        //    if (!string.IsNullOrEmpty(set.created_by))
        //    {
        //        Author = set.created_by;
        //    }
        //    if (set.created_date != null)
        //    {
        //        Date = string.Format("{0:d}", set.created_date);
        //    }

        //    TermCount = $"{set.term_count} terms";


        //}
    }
}
