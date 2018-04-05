using _29Quizlet.Models.QuizletTypes.Feeds.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.QuizletTypes.Feeds
{
    [DataContract]
    public class ClassFeed : IHomeItem
    {
        [DataMember]
        public int timestamp { get; set; }

        [DataMember]
        public int set_ids { get; set; }

        [DataMember]
        public string item_type { get; set; }

        [DataMember]
        public string origin_feed { get; set; }

        [DataMember]
        public ClassFeedItemData item_data { get; set; }

        [DataMember]
        public string display_timestamp { get; set; }
    }

    public class ClassFeedVM : IHomeItemVM
    {
        public int ClassId { get; set; }
        public string ClassTitle { get; set; }
        public string SetTitle { get; set; }
        public string AddedBy { get; set; }
        public string AddedByText { get; set; }
        public string Timestamp { get; set; }

        public ClassFeedVM(ClassFeed feed) 
        {
            ClassId = feed.item_data.group.id;
            ClassTitle = feed.item_data.group.name;
            SetTitle = $"Added: {feed.item_data.set.title}";
            var timestamp = feed.display_timestamp.ToLower() == "false" ? "by" : $"{feed.display_timestamp} by";
            AddedByText = $"Added {timestamp.ToLower()}";
            AddedBy = feed.item_data.added_by.username;
            //Timestamp = feed.display_timestamp.ToLower() == "false" ? string.Empty : feed.display_timestamp;
        }
    }

    [DataContract]
    public class GroupItem
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string set_count { get; set; }

        [DataMember]
        public string user_count { get; set; }

        [DataMember]
        public int created_date { get; set; }

        [DataMember]
        public bool has_access { get; set; }

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
    }

    [DataContract]
    public class SetFeed
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

        [DataMember]
        public int created_date { get; set; }

        [DataMember]
        public int modified_date { get; set; }

        [DataMember]
        public bool has_images { get; set; }

        [DataMember]
        public List<string> subjects { get; set; }

        [DataMember]
        public string visibility { get; set; }

        [DataMember]
        public string editable { get; set; }

        [DataMember]
        public bool has_access { get; set; }
    }

    [DataContract]
    public class AddedBy
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

    [DataContract]
    public class ClassFeedItemData
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public GroupItem group { get; set; }

        [DataMember]
        public SetFeed set { get; set; }
        
        [DataMember]
        public AddedBy added_by { get; set; }
    }

}
