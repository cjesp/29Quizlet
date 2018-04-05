using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.QuizletTypes.Feeds.Home
{
    [DataContract]
    public class HomeFeed
    {
        [DataMember]
        public bool success { get; set; }

        [DataMember]
        public bool finished { get; set; }

        [DataMember]
        public List<IHomeItem> items { get; set; }
    }

    [DataContract]
    public class HomeItems
    {
        [DataMember]
        public int timestamp { get; set; }

        [DataMember]
        public object set_ids { get; set; }

        [DataMember]
        public string item_type { get; set; }

        [DataMember]
        public string origin_feed { get; set; }

        [DataMember]
        public IHomeItem item_data { get; set; }

        [DataMember]
        public object display_timestamp { get; set; }
    }

}
