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

        /// <summary>
        /// Inicializa a fábrica carregando todas as definições de prefab (.json) da pasta de conteúdo da memória
        /// </summary>
        /// <param name="content">O ContentManager principal do jogo, usado para carregar assets para componentes.</param>
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

        /// <summary>
        /// Cria um novo GameObject com base em um definição de prefab, aplicando opcionalmente dados de mesclagem.
        /// </summary>
        /// <param name="prefabName">Nome do prefab válido.</param>
        /// <param name="overrides">Um JObject contendo propriedades para mesclar sobre os dados do prefab base, definindo instâncias únicas.</param>
        /// <returns>Um GameObject totalmente inicializado e construído, pronto para ser adicionado ao mundo do jogo.</returns>
        /// <exception cref="ArgumentException">Lançada caso o nome do especificado não for encontrado em prefabs.</exception>
        /// <exception cref="Exception">Lançada se o nome do componente especificado no prefab não corresponder a nenhuma classe de comopnente existente.</exception>
        public static GameObject Create(string prefabName, JObject overrides = null)
        {
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