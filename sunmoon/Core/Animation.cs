
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace sunmoon.Core
{
    /// <summary>
    /// Estrutura de dados para um animação.
    /// </summary>
    public class Animation
    {
        public string Name { get; set; }
        public string TexturePath { get; set; }
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public int FrameCount { get; set; }
        public float FrameDuration { get; set; }
        public bool IsLooping { get; set; }

        [JsonIgnore]
        public Texture2D Texture { get; set; }
    }
}