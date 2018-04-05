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
    public class CreatedFeed
    {
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "finished")]
        public bool Finished { get; set; }

        [DataMember(Name = "items")]
        public List<Item> Items { get; set; }
    }

    [DataContract]
    public class Item : IHomeItem
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
        public Set ItemData { get; set; }

        [DataMember(Name = "display_timestamp")]
        public string DisplayTimestamp { get; set; }
    }

    public class ItemVM : IHomeItemVM
    {

        public long SetId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool HasDescription { get; set; }
        public string Timestamp { get; set; }
        public bool IsPrivate { get; set; } = false;
        public string HasImages { get; set; }

        public ItemVM(Item item)
        {
            SetId = item.ItemData.Id;
            Title = item.ItemData.Title;
            Description = item.ItemData.Description;
            HasDescription = string.IsNullOrEmpty(Description) ? false : true;
            var timestamp = item.DisplayTimestamp.ToLower() == "false" ? string.Empty : $" {item.DisplayTimestamp.ToLower()}";

            if (string.IsNullOrEmpty(timestamp))
            {
                Timestamp = $"Set updated";
            }
            else
                Timestamp = $"Created set{timestamp}, {item.ItemData.TermCount} terms";

            if (item.ItemData.HasImages)
            {
                HasImages = " \uE114";
            }
            else
                HasImages = "";

            if (item.ItemData.Visibility == "password")
            {
                IsPrivate = true;
            }
        }
    }

}
