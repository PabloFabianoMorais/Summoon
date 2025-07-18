using Microsoft.Xna.Framework;
using sunmoon.Core.Management;
using sunmoon.Components.Core;
using sunmoon.Core.ECS;
using sunmoon.Components.Combat;

namespace sunmoon.Core.Services
{
    /// <summary>
    /// O serviço estático para rastrear e oferecer métricas de depuração globais
    /// </summary>
    public static class DebugService
    {
        public static float Fps { get; private set; }
        private const float FPS_UPDATE_INTERVAL = 1.0f;

        private static float _fpsTimer = 0f;
        private static int _frameCount = 0;
        private static TimeManager _timeManager;
        public static int DayCount;
        public static float CurrentTime { get; set; }
        private static TransformComponent _playerTransform { get; set; }
        public static Vector2 PlayerPosition { get; set; }
        public static HealthComponent _playerHealth { get; set; }
        public static float PlayerCurrentHealth { get; set; }
        private static GameObjectManager _gameObjectManager { get; set; }
        public static int ObjectsCount { get; set; }
        public static int RenderedObjects { get; set; }
        private static TilemapManager _tilemapManager { get; set; }
        public static int RenderedChunks { get; set; }

        public static void Initialize(
            TimeManager timeManager,
            TransformComponent playerTransform,
            HealthComponent healthComponent,
            GameObjectManager gameObjectManager,
            TilemapManager tilemapManager
        )
        {
            _timeManager = timeManager;
            _playerTransform = playerTransform;
            _playerHealth = healthComponent;
            _gameObjectManager = gameObjectManager;
            _tilemapManager = tilemapManager;
        }

        public static void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _fpsTimer += elapsed;
            _frameCount++;

            if (_fpsTimer >= FPS_UPDATE_INTERVAL)
            {
                Fps = _frameCount / _fpsTimer;
                _fpsTimer = 0f;
                _frameCount = 0;
            }

            if (_timeManager != null)
            {
                DayCount = _timeManager.DayCount;
                CurrentTime = _timeManager.CurrentTime;
            }
            if (_playerTransform != null)
                PlayerPosition = _playerTransform.Position;
            if (_playerHealth != null)
                PlayerCurrentHealth = _playerHealth.CurrentHealth;
            if (_gameObjectManager != null)
                {
                    ObjectsCount = _gameObjectManager.GetObjectsCount();
                    RenderedObjects = _gameObjectManager.GetRenderedObjectsCount();
                }
            if (_tilemapManager != null)
                RenderedChunks = _tilemapManager.GetRenderedChunksCount();
        } 
    }
}