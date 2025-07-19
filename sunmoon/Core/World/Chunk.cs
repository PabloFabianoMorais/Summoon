using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sunmoon.Core.ECS;

namespace sunmoon.Core.World
{
    /// <summary>
    /// Representa uma divisão fixa np mapa usada para organizar e renderizar (tiles) de forma eficiênte
    /// </summary>
    public class Chunk
    {
        public const int CHUNK_WIDTH = 16;
        public const int CHUNK_HEIGHT = 16;

        private readonly GameObject[,] _tiles;

        public Point ChunkPosition { get; set; }
        public Rectangle BoundingBox { get; set; }

        public Chunk(Point chunkPosition, int tileSize)
        {
            ChunkPosition = chunkPosition;
            _tiles = new GameObject[CHUNK_WIDTH, CHUNK_HEIGHT];

            int worldX = chunkPosition.X * CHUNK_WIDTH * tileSize;
            int worldY = chunkPosition.Y * CHUNK_HEIGHT * tileSize;
            int width = CHUNK_WIDTH * tileSize;
            int height = CHUNK_HEIGHT * tileSize;

            BoundingBox = new Rectangle(worldX, worldY, width, height);
        }

        /// <summary>
        /// Adiciona um tile se a posição estiver dentro dos limites da chunk.
        /// </summary>
        /// <param name="x">Posição X na grade da chunk.</param>
        /// <param name="y">Posição Y na grade da chunk.</param>
        /// <param name="tile">GameObject a ser adicoinado.</param>
        public void SetTile(int x, int y, GameObject tile)
        {
            if (x < 0 || x >= CHUNK_WIDTH || y < 0 || y >= CHUNK_HEIGHT)
                return;

            _tiles[y, x] = tile;
        }

        public void Update(GameTime gameTime)
        {
            for (int y = 0; y < CHUNK_HEIGHT; y++)
            {
                for (int x = 0; x < CHUNK_WIDTH; x++)
                {
                    _tiles[y, x]?.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < CHUNK_HEIGHT; y++)
            {
                for (int x = 0; x < CHUNK_WIDTH; x++)
                {
                    _tiles[y, x]?.Draw(spriteBatch);
                }
            }
        }
    }
}