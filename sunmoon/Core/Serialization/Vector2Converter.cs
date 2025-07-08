using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace sunmoon.Core.Serialization
{
    public class Vector2Converter : JsonConverter<Vector2>
    {
        public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingvalue, JsonSerializer jsonSerializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return Vector2.Zero;
            }

            JObject obj = JObject.Load(reader);
            float x = obj["X"]?.Value<float>() ?? 0f;
            float y = obj["Y"]?.Value<float>() ?? 0f;

            return new Vector2(x, y);
        }

        public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
        {
            JObject obj = new JObject
            {
                {"X", value.X},
                {"Y", value.Y}
            };
            obj.WriteTo(writer);
        }
    }
}