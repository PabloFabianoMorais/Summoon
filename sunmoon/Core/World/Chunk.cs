using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D11;
using sunmoon.Core.ECS;

namespace sunmoon.Core.World
{
    public class Chunk
    {
        public const int CHUNK_WIDTH = 16;
        public const int CHUNK_HEIGHT = 16;

        private readonly GameObject[,] _tiles;

        public Point ChunkPosition { get; set; }

        public Chunk(Point chunkPosition)
        {
            ChunkPosition = chunkPosition;
            _tiles = new GameObject[CHUNK_WIDTH, CHUNK_HEIGHT];
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

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < CHUNK_HEIGHT; y++) {
                for (int x = 0; x < CHUNK_WIDTH; x++)
                {
                    _tiles[y, x]?.Draw(spriteBatch);
                }
            }
        }
    }
}