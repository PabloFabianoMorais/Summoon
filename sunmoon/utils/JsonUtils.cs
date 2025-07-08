using System.IO;
using System.Text.Json;

namespace sunmoon.utils
{
    public static class JsonUtils
    {
        public static T LoadJson<T>(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                return JsonSerializer.Deserialize<T>(json);
            }
        }
    }
}