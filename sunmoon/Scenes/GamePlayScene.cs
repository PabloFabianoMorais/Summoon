using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using sunmoon.Core.Management;
using sunmoon.Core.Factory;
using sunmoon.Core.ECS;
using sunmoon.Core.World;
using System;
using sunmoon.Components.Core;
using sunmoon.UI;
using sunmoon.utils;
using sunmoon.Core.Services;
using sunmoon.Components.Items;
using sunmoon.Components.Combat;


namespace sunmoon.Scenes
{
    public class GamePlayScene : Scene
    {
        public TilemapManager TilemapManager;
        private Camera _camera;
        private TimeManager _timeManager;
        private GameObject _player;
        private TransformComponent _playerTransform;
        private HealthComponent _playerHealth;
        public UIManager uiManager;
        private UIPanel _debugPanel;
        private UIPanel _inventoryPanel;
        private const int CHUNK_LOAD_RADIUS = 2;
        private const int CHUNK_UNLOAD_RADIUS = 4;



        public override void LoadContent(ContentManager content)
        {
            var graphicsDeviceService = (IGraphicsDeviceService)content.ServiceProvider.GetService(typeof(IGraphicsDeviceService));

            if (graphicsDeviceService == null)
                throw new InvalidOperationException($"IGraphicsDevice não encontrado. O serviço não foi registrado corretamente.");

            var worldGenerator = new WorldGenerator(0);
            TilemapManager = new TilemapManager(worldGenerator);

            _camera = new Camera(graphicsDeviceService.GraphicsDevice.Viewport);
            _camera.Zoom = 3;

            _timeManager = new TimeManager();


            GameObjectManager = new GameObjectManager();

            _player = GameObjectFactory.Create("Player");
            _playerTransform = _player.GetComponent<TransformComponent>();
            _playerHealth = _player.GetComponent<HealthComponent>();
            var playerEquipment = _player.GetComponent<EquipmentComponent>();
            playerEquipment.EquipItem(GameObjectFactory.Create("NoodlesGlove"));
            Console.WriteLine(playerEquipment.LeftHand);
            GameObjectManager.Add(_player);

            uiManager = new UIManager();

            var font = content.Load<SpriteFont>("Fonts/DebugFont");

            DebugService.Initialize(_timeManager, _playerTransform, _playerHealth, GameObjectManager, TilemapManager);

            _debugPanel = new DebugPanel(font);
            _inventoryPanel = new PlayerEquipmentPanel(font, playerEquipment);

            uiManager.AddElement(_debugPanel);
            uiManager.AddElement(_inventoryPanel);
        }

        public override void UnloadContent()
        {
            TilemapManager?.StopGenerationThread();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Renderização dos elentos in-game
            spriteBatch.Begin(
                transformMatrix: _camera.GetViewMatrix(),
                samplerState: SamplerState.PointClamp
            );

            TilemapManager.Draw(spriteBatch, _camera);
            GameObjectManager.Draw(spriteBatch, _camera);

            spriteBatch.End();

            // Renderização da interface gráfica
            spriteBatch.Begin();

            uiManager.Draw(spriteBatch);

            spriteBatch.End();

        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.IsActionPressed("ToggleDebug"))
            {
                if (_debugPanel.IsVisible)
                    _debugPanel.IsVisible = false;
                else
                {
                    _debugPanel.IsVisible = true;
                }
            }
            if (InputManager.IsActionPressed("ToggleInventory"))
            {
                if (_inventoryPanel.IsVisible)
                    _inventoryPanel.IsVisible = false;
                else
                {
                    _inventoryPanel.IsVisible = true;
                }
            }

            if (InputManager.IsActionPressed("TakeDemage")) _playerHealth.TakeDemage(10);

            _timeManager.Update(gameTime);

            GameObjectManager.Update(gameTime);

            if (_player != null)
            {
                var playerTransform = _player.GetComponent<TransformComponent>();
                _camera.Position = playerTransform.Position;
            }



            RequestChunksAroundCamera();
            ManageChunkLifetime();
            TilemapManager.ProcessGeneratedChunks();

            uiManager.Update(gameTime);

        }

        private void RequestChunksAroundCamera()
        {
            int cameraChunkX = MathUtils.FloorDiv((int)_camera.Position.X, Chunk.CHUNK_WIDTH * TilemapManager.DEFAULT_TILE_SIZE);
            int cameraChunkY = MathUtils.FloorDiv((int)_camera.Position.Y, Chunk.CHUNK_HEIGHT * TilemapManager.DEFAULT_TILE_SIZE);

            for (int y = cameraChunkY - CHUNK_LOAD_RADIUS; y <= cameraChunkY + CHUNK_LOAD_RADIUS; y++)
            {
                for (int x = cameraChunkX - CHUNK_LOAD_RADIUS; x <= cameraChunkX + CHUNK_LOAD_RADIUS; x++)
                {
                    if (!TilemapManager.IsChunkRequestedOrExists(x, y))
                    {
                        TilemapManager.RequestChunkGeneration(x, y);
                    }
                }
            }
        }

        private void ManageChunkLifetime()
        {
            int cameraChunkX = MathUtils.FloorDiv((int)_camera.Position.X, Chunk.CHUNK_WIDTH * TilemapManager.DEFAULT_TILE_SIZE);
            int cameraChunkY = MathUtils.FloorDiv((int)_camera.Position.Y, Chunk.CHUNK_HEIGHT * TilemapManager.DEFAULT_TILE_SIZE);

            TilemapManager.UnloadDistantChunks(cameraChunkX, cameraChunkY, CHUNK_UNLOAD_RADIUS);
        }
    }
}