using _29Quizlet.Models.QuizletTypes.Feeds;
using _29Quizlet.Models.QuizletTypes.Feeds.Home;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Helpers
{
    public class JsonHomeFeedConverter : CustomCreationConverter<IHomeItem>
    {
        public override IHomeItem Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        public IHomeItem Create(Type objectType, JObject jObject)
        {
            var type = (string)jObject.Property("item_type");

            switch (type)
            {
                case "session":
                    return new StudySessionItem();
                case "group_set":
                    return new ClassFeed();
                case "set":
                    return new Item();
            }

            throw new Exception(String.Format("The quizlet type {0} is not supported!", type));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream 
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject 
            var target = Create(objectType, jObject);

            // Populate the object properties 
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }
}
