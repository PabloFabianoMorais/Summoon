using Microsoft.Xna.Framework.Graphics;

namespace sunmoon.Core.World
{
    public class TileType
    {
        public string Id { get; private set; }
        public Texture2D Texture { get; private set; }

        public TileType(string id, Texture2D texture)
        {
            Id = id;
            Texture = texture;
        }
    }
}