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
    public class GroupQ : ISearchItem
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string url { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public int set_count { get; set; }

        [DataMember]
        public int user_count { get; set; }

        [DataMember(Name = "created_date")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime? created_date { get; set; }

        [DataMember]
        public bool has_access { get; set; }

        [DataMember]
        public string access_level { get; set; }

        [DataMember]
        public int role_level { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public bool admin_only { get; set; }

        [DataMember]
        public bool is_public { get; set; }

        [DataMember]
        public bool has_password { get; set; }

        [DataMember]
        public bool member_add_sets { get; set; }

        [DataMember]
        public School school { get; set; }

        [DataMember]
        public List<MembersSample> members_sample { get; set; }

        [DataMember]
        public string type { get; set; }
    }

    [DataContract]
    public class School
    {
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string city { get; set; }

        [DataMember]
        public string state { get; set; }

        [DataMember]
        public string country { get; set; }

        [DataMember]
        public string country_code { get; set; }

        [DataMember]
        public int latitude { get; set; }

        [DataMember]
        public int longitude { get; set; }
    }

    [DataContract]
    public class MembersSample
    {
        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string account_type { get; set; }

        [DataMember]
        public string profile_image { get; set; }

        [DataMember]
        public int id { get; set; }
    }

    public class GroupQViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasDescription { get; set; }
        public string SchoolNameCity { get; set; }
        public bool HasSchool { get; set; }
        public string SetAndUserCount { get; set; }

        public GroupQViewModel(GroupQ group)
        {
            Id = group.id;
            Name = group.name;
            if (!string.IsNullOrEmpty(group.description))
            {
                Description = group.description;
                HasDescription = true;
            }
            else
                HasDescription = false;

            if (group.school != null)
            {
                HasSchool = true;
                SchoolNameCity = $"{group.school.name}, {group.school.city}";
            }
            else
                HasSchool = false;

            SetAndUserCount = $"{group.set_count} set, {group.user_count} members";

        }
    }
}
