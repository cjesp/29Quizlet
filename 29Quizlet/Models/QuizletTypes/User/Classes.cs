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
    public class School
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "country")]
        public string Country { get; set; }

        [DataMember(Name = "country_code")]
        public string CountryCode { get; set; }

        [DataMember(Name = "latitude")]
        public double latitude { get; set; }

        [DataMember(Name = "longtitude")]
        public double longitude { get; set; }
    }

    [DataContract]
    public class Classes
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "set_count")]
        public int SetCount { get; set; }

        [DataMember(Name = "user_count")]
        public int UserCount { get; set; }

        //[JsonProperty(ItemConverterType = typeof(JavaScriptDateTimeConverter))]
        [JsonProperty(PropertyName = "created_date")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime? created_date { get; set; }

        [DataMember(Name = "has_access")]
        public bool HasAccess { get; set; }

        [DataMember(Name = "access_level")]
        public string AccessLevel { get; set; }

        [DataMember(Name = "role_level")]
        public int RoleLevel { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "admin_only")]
        public bool AdminOnly { get; set; }

        [DataMember(Name = "is_public")]
        public bool IsPublic { get; set; }

        [DataMember(Name = "has_password")]
        public bool HasPassword { get; set; }

        [DataMember(Name = "member_add_sets")]
        public bool MemberAddSets { get; set; }

        [DataMember(Name = "school")]
        public School School { get; set; }
    }

    public class ClassesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool HasDescription { get; set; } = false;
        public string Description { get; set; } = string.Empty;
        public bool HasSchool { get; set; } = false;
        public string SchoolAndCity { get; set; } = string.Empty;
        public string SetCount { get; set; } = string.Empty;
        public string UserCount { get; set; } = string.Empty;

        public ClassesViewModel(Classes quizClass)
        {
            Id = quizClass.Id;
            Name = quizClass.Name;

            if (!string.IsNullOrEmpty(quizClass.Description))
            {
                HasDescription = true;
                Description = quizClass.Description;
            }
            if (quizClass.School != null)
            {
                HasSchool = true;
                SchoolAndCity = $"{quizClass.School.Name}, {quizClass.School.City}";
            }

            SetCount = $"{quizClass.SetCount}";
            UserCount = $"{quizClass.UserCount}";

        }
    }
}
