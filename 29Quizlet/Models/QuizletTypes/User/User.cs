using _29Quizlet.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.QuizletTypes.User
{
    [DataContract]
    public class User
    {
        [DataMember(Name = "username")]
        public string Username { get; set; }

        [DataMember(Name = "account_type")]
        public string AccountType { get; set; }

        [DataMember(Name = "profile_image")]
        public string Profile_Image { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "statistics")]
        public Statistics Statistics { get; set; }

        [DataMember(Name = "sign_up_date")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime? sign_up_date { get; set; }

        //todo:
        //[DataMember(Name = "sets")]
        //public List<SetRemote> Sets { get; set; }

        //[DataMember(Name = "studied")]
        //public List<Studied> Studied { get; set; }

        //[DataMember(Name = "groups")]
        //public List<ClassRemote> Classes { get; set; }

        //[DataMember(Name = "favorites")]
        //public List<FavoriteSet> Favorites { get; set; }
    }

    [DataContract]
    public class SetRemote
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "created_by")]
        public string CreatedBy { get; set; }

        [DataMember(Name = "term_count")]
        public int TermCount { get; set; }

        [DataMember(Name = "created_date")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime? created_date { get; set; }

        [DataMember(Name = "modified_date")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime? modified_date { get; set; }

        [DataMember(Name = "has_images")]
        public bool HasImages { get; set; }
        //public List<object> subjects { get; set; }
    }

    [DataContract]
    public class FavoriteSet
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "created_by")]
        public string CreatedBy { get; set; }

        [DataMember(Name = "term_count")]
        public int TermCount { get; set; }

        [DataMember(Name = "created_date")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime? created_date { get; set; }

        [DataMember(Name = "modified_date")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime? modified_date { get; set; }

        [DataMember(Name = "has_images")]
        public bool HasImages { get; set; }
        //public List<object> subjects { get; set; }
    }

    [DataContract]
    public class Studied
    {
        [DataMember(Name = "mode")]
        public string Mode { get; set; }

        [DataMember(Name = "last_studied_date")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime? LastStudiedDate { get; set; }
        
        [DataMember(Name = "finished_date")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime? FinishedDate { get; set; }

        [DataMember(Name = "score")]
        public int Score { get; set; }

        [DataMember(Name = "set")]
        public SetRemote Set { get; set; }
    }

    [DataContract]
    public class ClassRemote
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "set_count")]
        public int SetCount { get; set; }

        [DataMember(Name = "user_count")]
        public int UserCount { get; set; }

        [DataMember(Name = "created_date")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime? created_date { get; set; }

        [DataMember(Name = "is_public")]
        public bool IsPublic { get; set; }
    }

    


}
