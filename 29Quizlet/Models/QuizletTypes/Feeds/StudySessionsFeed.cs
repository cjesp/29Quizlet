using _29Quizlet.Models.QuizletTypes.Feeds.Home;
using _29Quizlet.Models.QuizletTypes.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.QuizletTypes.Feeds
{
    [DataContract]
    public class StudySessionsFeed
    {
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "finished")]
        public bool Finished { get; set; }

        [DataMember(Name = "items")]
        public List<StudySessionItem> Items { get; set; }
    }

    [DataContract]
    public class StudySessionItem : IHomeItem
    {
        [DataMember(Name = "timestamp")]
        public int Timestamp { get; set; }

        [DataMember(Name = "set_ids")]
        public string SetIds { get; set; }

        [DataMember(Name = "item_type")]
        public string ItemType { get; set; }

        [DataMember(Name = "origin_feed")]
        public string OriginFeed { get; set; }

        [DataMember(Name = "item_data")]
        public StudySession ItemData { get; set; }

        [DataMember(Name = "display_timestamp")]
        public string DisplayTimestamp { get; set; }
        
    }

    public class StudySessionItemVM : IHomeItemVM
    {

        public long SetId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ModeAndTermCount { get; set; } = string.Empty;
        public string Timestamp { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;

        public StudySessionItemVM()
        {

        }

        public StudySessionItemVM(StudySessionItem item)
        {
            SetId = item.ItemData.Set.Id;
            Title = item.ItemData.Set.Title;
            var mode = ModeHelper(item.ItemData.Mode);
            ModeAndTermCount = $"{mode}, {item.ItemData.Set.TermCount} terms";

            Timestamp = $"{item.DisplayTimestamp}, by";

            if (!string.IsNullOrEmpty(item.ItemData.Set.CreatedBy))
            {
                CreatedBy = item.ItemData.Set.CreatedBy;
            }

        }

        private string ModeHelper(string input)
        {
            if (input.Contains("_"))
            {
                var mode = input.Split('_');
                string output = string.Empty;
                for (int i = 0; i < mode.Length; i++)
                {
                    if (i == 0)
                    {
                        var firstWord = char.ToUpper(mode[i][0]) + mode[i].Substring(1);
                        output = $"{firstWord}";
                    }
                    else
                    {
                        output = $"{output} {mode[i]}";
                    }
                }

                return output;
            }
            else
            {
                var output = $"{char.ToUpper(input[0]) + input.Substring(1)}";
                return output;
            }
        }
    }
}
