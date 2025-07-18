
using Microsoft.Xna.Framework;

namespace sunmoon.Core.World
{
    /// <summary>
    /// Contém os dados brutos necessários para construir um Chunk,
    /// gerado em um thread segundário para ser processado no thread prícipal
    /// </summary>
    public class ChunkBluePrint
    {
        public Point ChunkPosition { get; }
        public TileData[,] Tiles { get; }

        public ChunkBluePrint(Point chunkPosition)
        {
            ChunkPosition = chunkPosition;
            Tiles = new TileData[Chunk.CHUNK_WIDTH, Chunk.CHUNK_HEIGHT];
        }
    }
}