using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpDX.X3DAudio;
using sunmoon.Core.ECS;
using sunmoon.Core.Factory;
using sunmoon.Core.World;

namespace sunmoon.Core.Management
{


    public class TilemapManager
    {
        public const int DEFAULT_TILE_SIZE = 8;

        public MapData CurrentMapData { get; private set; }

        private readonly Dictionary<Point, Chunk> _chunks = new Dictionary<Point, Chunk>();

        private int FloorDiv(int a, int n)
        {
            return (int)Math.Floor((double)a / n);
        }

        private int Mod(int a, int n)
        {
            return ((a % n) + n) % n;
        }

        public void LoadMapFromFile(string filePath)
        {
            _chunks.Clear();

            string jsonText = File.ReadAllText(filePath);
            CurrentMapData = JsonConvert.DeserializeObject<MapData>(jsonText);

            if (CurrentMapData == null)
                throw new System.Exception($"Falha ao carregar o mapa de: {filePath}");

            for (var y = 0; y < CurrentMapData.Layout.Count; y++)
            {
                string row = CurrentMapData.Layout[y];

                for (var x = 0; x < row.Length; x++)
                {
                    char tileChar = row[x];

                    if (CurrentMapData.Palette.TryGetValue(tileChar, out string prefabName))
                    {
                        if (prefabName == null)
                        {
                            continue;
                        }

                        int worldX = CurrentMapData.Origin.X + x;
                        int worldY = CurrentMapData.Origin.Y + y;

                        SetTile(worldX, worldY, prefabName);
                    }
                }
            }
        }

        public void SetTile(int tileX, int tileY, string prefabName)
        {

            int chunkX = FloorDiv(tileX, Chunk.CHUNK_WIDTH);
            int chunkY = FloorDiv(tileY, Chunk.CHUNK_HEIGHT);

            var chunkPosition = new Point(chunkX, chunkY);

            if (!_chunks.TryGetValue(chunkPosition, out Chunk chunk))
            {
                chunk = new Chunk(chunkPosition);
                _chunks[chunkPosition] = chunk;
            }


            int localX = Mod(tileX, Chunk.CHUNK_WIDTH);
            int localY = Mod(tileY, Chunk.CHUNK_HEIGHT);

            int tileSize = CurrentMapData?.TileSize ?? DEFAULT_TILE_SIZE;
            var tilePosition = new Vector2(tileX * tileSize, tileY * tileSize);

            var overrides = new JObject
            {
                ["components"] = new JObject
                {
                    ["TransformComponent"] = new JObject
                    {
                        ["Position"] = new JObject
                        {
                            ["X"] = tilePosition.X,
                            ["Y"] = tilePosition.Y
                        }
                    }
                }
            };

            GameObject tileObject = GameObjectFactory.Create(prefabName, overrides);
            chunk.SetTile(localX, localY, tileObject);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var chunk in _chunks.Values)
            {
                chunk.Draw(spriteBatch);
            }
        }
    }
}