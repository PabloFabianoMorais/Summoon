using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace sunmoon.Core.World
{
    /// <summary>
    /// Estrutura de dados para mapas.
    /// </summary>
    public class MapData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tileSize")]
        public int TileSize { get; set; }

        [JsonProperty("palette")]
        public Dictionary<char, string> Palette { get; set; }

        [JsonProperty("origin")]
        public Point Origin { get; set; }

        [JsonProperty("layout")]
        public List<string> Layout { get; set; }
    }
}