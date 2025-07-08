using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sunmoon.Components;
using sunmoon.Core.ECS;
using sunmoon.Core.Serialization;

namespace sunmoon.Core.Factory
{
    public static class GameObjectFactory
    {
        private static ContentManager _content;
        private static JObject _prefabsData;

        private static JsonSerializerSettings _serializerSettings;

        public static void Initialize(ContentManager content)
        {
            _content = content;

            _serializerSettings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new Vector2Converter()
                }
            };

            // Ler o prefabs para fabricação dos objetos
            _prefabsData = new JObject();

            string prefabsRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/data/prefabs");

            if (Directory.Exists(prefabsRootPath))
            {
                string[] prefabFiles = Directory.GetFiles(prefabsRootPath, "*.json", SearchOption.AllDirectories);

                foreach (string filePath in prefabFiles)
                {
                    string prefabName = Path.GetFileNameWithoutExtension(filePath);

                    string jsonText = File.ReadAllText(filePath);

                    JToken prefabJson = JToken.Parse(jsonText);

                    _prefabsData.Add(prefabName, prefabJson);
                }
            }
        }

        public static GameObject Create(string prefabName, JObject overrides = null)
        {
            // Encontra a definição do prefab JSON
            if (!_prefabsData.TryGetValue(prefabName, out JToken prefabToken))
                throw new ArgumentException($"Prefab com nome {prefabName} não encontrado");

            JObject finalPrefaData = GetMergedPrefabData(prefabName);

            if (overrides != null)
            {
                finalPrefaData.Merge(overrides, new JsonMergeSettings
                {
                    MergeArrayHandling = MergeArrayHandling.Replace,
                    MergeNullValueHandling = MergeNullValueHandling.Ignore
                });
            }

            var prefabData = (JObject)prefabToken;
            var gameObject = new GameObject();

            var componentsData = (JObject)finalPrefaData["components"];


            foreach (var prop in componentsData.Properties())
            {
                string componentTypeName = prop.Name;
                JObject componentProperties = (JObject)prop.Value;

                Type type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == componentTypeName);
                if (type == null)
                {
                    throw new Exception($"Componente {componentTypeName} não encontrado");
                }

                var component = gameObject.AddComponent(type) as Component;

                JsonConvert.PopulateObject(componentProperties.ToString(), component, _serializerSettings);

                if (component is IContentLoadable loadableComponent)
                {
                    loadableComponent.LoadContent(_content);
                }
            }

            gameObject.Initialize();
            return gameObject;
        }

        private static JObject GetMergedPrefabData(string prefabName)
        {
            if (!_prefabsData.TryGetValue(prefabName, out JToken prefabToken))
                throw new ArgumentException($"Prefab com nome '{prefabName}' não encontrado.");

            var prefabData = (JObject)prefabToken.DeepClone();

            if (prefabData.TryGetValue("inherits", out JToken inheritsToken))
            {
                JObject parentData = new JObject();

                if (inheritsToken.Type == JTokenType.String)
                {
                    string parentName = inheritsToken.ToString();
                    parentData = GetMergedPrefabData(parentName);
                }
                else if (inheritsToken.Type == JTokenType.Array)
                {
                    foreach (var parentNameToken in inheritsToken.Children<JValue>())
                    {
                        string parentName = parentNameToken.Value.ToString();
                        JObject partialParentData = GetMergedPrefabData(parentName);
                        parentData.Merge(partialParentData);
                    }
                }

                parentData.Merge(prefabData, new JsonMergeSettings
                {
                    MergeArrayHandling = MergeArrayHandling.Union,
                    MergeNullValueHandling = MergeNullValueHandling.Ignore
                });

                return parentData;
            }

            return prefabData;
        }
    }
}