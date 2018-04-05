using _29Quizlet.Models.QuizletTypes.Uploading;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Helpers
{
    public class SetVisibilityConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Visibility);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            var value = reader.Value as string;
            if (value == "public")
            {
                return Visibility.Everyone;
            }
            else
            {
                return Visibility.OnlyMe;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var visibility = (Visibility)value;
            switch (visibility)
            {
                case Visibility.Everyone:
                    writer.WriteRawValue("\"public\"");
                    break;
                case Visibility.OnlyMe:
                    writer.WriteRawValue("\"only_me\"");
                    break;
                default:
                    writer.WriteRawValue("\"public\"");
                    break;
            }
        }
    }
}
