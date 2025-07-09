using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sunmoon.Core.ECS;
using sunmoon.Core.Factory;
using sunmoon.Core.World;
using sunmoon.utils;

namespace sunmoon.Core.Management
{
    /// <summary>
    /// Gerencia o mapa, chunks e tiles.
    /// </summary>
    public class TilemapManager
    {
        public const int DEFAULT_TILE_SIZE = 8;

        public MapData CurrentMapData { get; private set; }

        private readonly Dictionary<Point, Chunk> _chunks = new Dictionary<Point, Chunk>();

        /// Carrega o mapa de um arquivo JSON especificado.
        /// <param name="filePath">Caminho absoluto ou relativo para o arquivo JSON do mapa.</param>
        /// <exception cref="System.Exception">Herro ao abrir ou desserializar o arquivo.</exception>
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

        /// Cria um tile com posição e nome do prefab específicados adicionando o tile à um chunk correspondente
        /// <param name="tileX">Coordenada X na grade de tiles.</param>
        /// <param name="tileX">Coordenada Y na grade de tiles.</param>
        /// <param name="prefabName">Nome do prefab que deve corresponder à um arquivo JSON válido.</param>
        /// <exception cref="System.ArgumentException">Lançada se o prefab com o nome especificado não for encontrado pela GameObjectFactory.</exception>
        public void SetTile(int tileX, int tileY, string prefabName)
        {

            int chunkX = MathUtils.FloorDiv(tileX, Chunk.CHUNK_WIDTH);
            int chunkY = MathUtils.FloorDiv(tileY, Chunk.CHUNK_HEIGHT);

            var chunkPosition = new Point(chunkX, chunkY);

            if (!_chunks.TryGetValue(chunkPosition, out Chunk chunk))
            {
                chunk = new Chunk(chunkPosition);
                _chunks[chunkPosition] = chunk;
            }


            int localX = MathUtils.Mod(tileX, Chunk.CHUNK_WIDTH);
            int localY = MathUtils.Mod(tileY, Chunk.CHUNK_HEIGHT);

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