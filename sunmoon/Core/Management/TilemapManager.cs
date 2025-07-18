using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly ConcurrentQueue<Point> _generationQueue = new ConcurrentQueue<Point>();
        private readonly ConcurrentQueue<ChunkBluePrint> _generatedBluePrintsQueue = new ConcurrentQueue<ChunkBluePrint>();
        private readonly ConcurrentDictionary<Point, bool> _requestedChunks = new ConcurrentDictionary<Point, bool>();
        private Task _generationTask;
        private CancellationTokenSource _cancellationTokenSource;

        private ChunkBluePrint _currentBlueprintToBuild = null;
        private int _buildCoordX = 0;
        private int _buildCoordY = 0;
        private const int TILES_TO_BUILD_PER_FRAME = 32;

        private JObject _tileOverrides = new JObject { ["components"] = new JObject { ["TransformComponent"] = new JObject { ["Position"] = new JObject() } } };

        public TilemapManager(WorldGenerator worldGenerator)
        {
            _worldGenerator = worldGenerator;
            StartGenerationThread();
        }

        private void StartGenerationThread()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            _generationTask = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (_generationQueue.TryDequeue(out Point chunkPosition))
                    {
                        var newBluePrint = CreateChunkBluePrint(chunkPosition.X, chunkPosition.Y);

                        _generatedBluePrintsQueue.Enqueue(newBluePrint);
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
            }, token);
        }

        /// <summary>
        /// Pede para um chunk ser gerado, adicionando-o à fila de geração.
        /// </summary>
        public void RequestChunkGeneration(int chunkX, int chunkY)
        {
            var chunkPos = new Point(chunkX, chunkY);

            if (_requestedChunks.TryAdd(chunkPos, true))
            {
                _generationQueue.Enqueue(new Point(chunkX, chunkY));
            }
        }

        public bool IsChunkRequestedOrExists(int chunkX, int chunkY)
        {
            var chunkPos = new Point(chunkX, chunkY);
            return _chunks.ContainsKey(chunkPos) || _requestedChunks.ContainsKey(chunkPos);
        }

        /// <summary>
        /// Processa os chunks que foram gerados no thread em segundo plano e os adiciona ao mundo.
        /// Deve ser chamado no thread principal (Update)
        /// </summary>
        public void ProcessGeneratedChunks()
        {
            if (_currentBlueprintToBuild == null)
            {
                if (!_generatedBluePrintsQueue.TryDequeue(out _currentBlueprintToBuild))
                    return;

                var newChunk = new Chunk(_currentBlueprintToBuild.ChunkPosition, DEFAULT_TILE_SIZE);
                _chunks[_currentBlueprintToBuild.ChunkPosition] = newChunk;
                _buildCoordX = 0;
                _buildCoordY = 0;
            }

            var chunkBeingBuilt = _chunks[_currentBlueprintToBuild.ChunkPosition];
            var positionJson = (JObject)_tileOverrides["components"]["TransformComponent"]["Position"];

            for (int i = 0; i <= TILES_TO_BUILD_PER_FRAME; i++)
            {
                if (_buildCoordY >= Chunk.CHUNK_HEIGHT)
                {
                    _currentBlueprintToBuild = null;
                    return;
                }

                TileData tileData = _currentBlueprintToBuild.Tiles[_buildCoordX, _buildCoordY];

                if (!string.IsNullOrEmpty(tileData.PrefabName))
                {

                    var tilePosition = new Vector2(
                        (_currentBlueprintToBuild.ChunkPosition.X * Chunk.CHUNK_WIDTH + _buildCoordX) * DEFAULT_TILE_SIZE,
                        (_currentBlueprintToBuild.ChunkPosition.Y * Chunk.CHUNK_HEIGHT + _buildCoordY) * DEFAULT_TILE_SIZE
                    );

                    positionJson["X"] = tilePosition.X;
                    positionJson["Y"] = tilePosition.Y;

                    GameObject tileObject = GameObjectFactory.Create(tileData.PrefabName, _tileOverrides);
                    chunkBeingBuilt.SetTile(_buildCoordX, _buildCoordY, tileObject);
                }
                _buildCoordX++;
                if (_buildCoordX >= Chunk.CHUNK_WIDTH)
                {
                    _buildCoordX = 0;
                    _buildCoordY++;
                }
            }
        }

        public void UnloadDistantChunks(int centerChunkX, int centerChunkY, int unloadRadius)
        {
            var chunksToRemove = new List<Point>();

            foreach (var chunkPos in _chunks.Keys)
            {
                int dist_x = Math.Abs(chunkPos.X - centerChunkX);
                int dist_y = Math.Abs(chunkPos.Y - centerChunkY);

                if (dist_x > unloadRadius || dist_y > unloadRadius)
                {
                    chunksToRemove.Add(chunkPos);
                }

                foreach (var pos in chunksToRemove)
                {
                    _chunks.Remove(pos);
                    _requestedChunks.TryRemove(pos, out _);
                }
            }
        }

        /// <summary>
        /// Verifica se o chunk nas coordenadas de chunks especificadas já foi gerado.
        /// </summary>
        /// <param name="chunkX">Posição X nas coordenadas de chunk.</param>
        /// <param name="chunkY">Posição Y nas coordenadas de chunk.</param>
        /// <returns>True se chunk existe, senão false.</returns>
        public bool HasChunk(int chunkX, int chunkY)
        {
            return _chunks.ContainsKey(new Point(chunkX, chunkY));
        }

        public int GetRenderedChunksCount()
        {
            return _renderedChunksCount;
        }

        public int GetTotalChunksCount()
        {
            return _chunks.Count;
        }

        /// <summary>
        /// Lógica de geração de um único chunk. Esta é a função que roda no thread secundário
        /// </summary>
        private ChunkBluePrint CreateChunkBluePrint(int chunkX, int chunkY)
        {
            var chunkPosition = new Point(chunkX, chunkY);
            var newBluePrint = new ChunkBluePrint(chunkPosition);

            for (int y = 0; y < Chunk.CHUNK_HEIGHT; y++)
            {
                for (int x = 0; x < Chunk.CHUNK_WIDTH; x++)
                {
                    int worldX = (chunkX * Chunk.CHUNK_WIDTH) + x;
                    int worldY = (chunkY * Chunk.CHUNK_HEIGHT) + y;

                    string prefabName = _worldGenerator.GetTilePrefabName(worldX, worldY);
                    newBluePrint.Tiles[x, y] = new TileData { PrefabName = prefabName };
                }
            }

            return newBluePrint;
        }

        /// <summary>
        /// Para a tarefa de geração de forma segura
        /// </summary>
        public void StopGenerationThread()
        {
            _cancellationTokenSource?.Cancel();
            _generationTask?.Wait();
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