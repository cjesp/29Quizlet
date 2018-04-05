using _29Quizlet.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.QuizletTypes.User
{
    [DataContract]
    public class StudySession
    {
        [DataMember(Name = "mode")]
        public string Mode { get; set; }

        [DataMember(Name = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "start_date")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime? start_date { get; set; }

        [JsonProperty(PropertyName = "finish_date")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime? finish_date { get; set; }

        [DataMember(Name = "formatted_score")]
        public string FormattedScore { get; set; }

        [DataMember(Name = "set")]
        public SetStudySession Set { get; set; }

        [DataMember(Name = "image_url")]
        public string ImageUrl { get; set; }
    }

    [DataContract]
    public class SetStudySession
    {
        [DataMember(Name = "id")]
        public Int64 Id { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "created_by")]
        public string CreatedBy { get; set; }

        [DataMember(Name = "term_count")]
        public int TermCount { get; set; }

        [JsonProperty(PropertyName = "created_date")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime? created_date { get; set; }

        [JsonProperty(PropertyName = "modified_date")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime? modified_date { get; set; }

        [JsonProperty(PropertyName = "published_date")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime? published_date { get; set; }

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


    }

    public class StudySessionViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ModeAndTermCount { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;

        public StudySessionViewModel()
        {

        }

        public StudySessionViewModel(StudySession session)
        {
            Id = session.Set.Id;
            Title = session.Set.Title;

            string mode = ModeHelper(session.Mode);
            //switch (GetMode(session.Mode))
            //{
            //    case Modes.Unknown:
            //        mode = "Mode: Unknown";
            //        break;
            //    case Modes.FlashCards:
            //        mode = "Mode: Flash Cards";
            //        break;
            //    case Modes.MobileLearn:
            //        mode = "Mode: Mobile Learn";
            //        break;
            //    case Modes.MobileCards:
            //        mode = "Mode: Mobile Cards";
            //        break;
            //    case Modes.Test:
            //        mode = "Mode: Test";
            //        break;
            //    case Modes.MobileScatter:
            //        mode = "Mode: Mobile Scatter";
            //        break;
            //    default:
            //        mode = "Mode: Unknown";
            //        break;
            //}

            ModeAndTermCount = $"{mode}, {session.Set.TermCount} terms";

            //TermCount = $"{session.Set.TermCount}";
            if (session.start_date != null)
            {
                var startDate = (DateTime)session.start_date;
                StartDate = $"{startDate.ToString("G")}, by ";
            }

            if (!string.IsNullOrEmpty(session.Set.CreatedBy))
            {
                CreatedBy = session.Set.CreatedBy;
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



    //    private Modes GetMode(string mode)
    //    {
    //        switch (mode.ToLower())
    //        {
    //            case "flashcards":
    //                return Modes.FlashCards;
    //            case "mobile_learn":
    //                return Modes.MobileLearn;
    //            case "mobile_cards":
    //                return Modes.MobileCards;
    //            case "test":
    //                return Modes.Test;
    //            case "mobile_scatter":
    //                return Modes.MobileScatter;
    //            default:
    //                return Modes.Unknown;
    //        }
    //    }
    //}

    //enum Modes
    //{
    //    Unknown,
    //    FlashCards,
    //    MobileLearn,
    //    MobileCards,
    //    Test,
    //    MobileScatter
    //}
}
