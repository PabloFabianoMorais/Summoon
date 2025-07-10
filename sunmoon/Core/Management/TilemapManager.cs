using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using sunmoon.Core.ECS;
using sunmoon.Core.Factory;
using sunmoon.Core.World;

namespace sunmoon.Core.Management
{
    /// <summary>
    /// Gerencia o mapa, chunks e tiles.
    /// </summary>
    public class TilemapManager
    {
        public const int DEFAULT_TILE_SIZE = 8;

        private readonly Dictionary<Point, Chunk> _chunks = new Dictionary<Point, Chunk>();
        private readonly WorldGenerator _worldGenerator;
        private int _renderedChunksCount;

        public TilemapManager(WorldGenerator worldGenerator)
        {
            _worldGenerator = worldGenerator;
        }

        public int GetRenderedChunksCount()
        {
            return _renderedChunksCount;
        }

        public int GetTotalChunksCount()
        {
            return _chunks.Count;
        }

        public void GenerateChunk(int chunkX, int chunkY)
        {
            var chunkPosition = new Point(chunkX, chunkY);
            if (_chunks.ContainsKey(chunkPosition))
                return;

            var newChunk = new Chunk(chunkPosition, DEFAULT_TILE_SIZE);
            _chunks[chunkPosition] = newChunk;

            for (int y = 0; y < Chunk.CHUNK_HEIGHT; y++)
            {
                for (int x = 0; x < Chunk.CHUNK_WIDTH; x++)
                {
                    int worldX = (chunkX * Chunk.CHUNK_WIDTH) + x;
                    int worldY = (chunkY * Chunk.CHUNK_HEIGHT) + y;

                    TileType tileType = _worldGenerator.GetTileType(worldX, worldY);
                    string prefabName = _worldGenerator.GetPrefabNameFor(tileType);

                    var tilePosition = new Vector2(worldX * DEFAULT_TILE_SIZE, worldY * DEFAULT_TILE_SIZE);
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
                    newChunk.SetTile(x, y, tileObject);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Rectangle cameraBounds = camera.GetVisibleArea();
            _renderedChunksCount = 0;
            foreach (var chunk in _chunks.Values)
            {
                if (!cameraBounds.Intersects(chunk.BoundingBox)) continue;

                chunk.Draw(spriteBatch);
                _renderedChunksCount++;
            }
        }
    }
}