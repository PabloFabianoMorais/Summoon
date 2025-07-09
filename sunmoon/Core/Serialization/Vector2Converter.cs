using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace sunmoon.Core.Serialization
{
    /// <summary>
    /// Conversor entre estrutura de posição do prefab e Vector2.
    /// </summary>
    public class Vector2Converter : JsonConverter<Vector2>
    {
        /// Carrega o arquivo JSON e converte a estrutura de posição do prefab
        /// para Vector2
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

        /// Converte valor Vector2 para estrutura de posição prefab.
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