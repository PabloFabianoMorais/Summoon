using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using sunmoon.Core.Management;
using sunmoon.Core.Factory;
using sunmoon.Core.ECS;
using sunmoon.Core.World;
using System;
using sunmoon.Components;
using sunmoon.UI;
using sunmoon.utils;


namespace sunmoon.Scenes
{
    public class GamePlayScene : Scene
    {
        public TilemapManager TilemapManager;
        private Camera _camera;
        private TimeManager _timeManager;
        private Color _overlayColor;
        private GameObject _player;
        public UIManager uiManager;
        private UIPanel _debugPanel;
        private const int CHUNK_LOAD_RADIUS = 2;


        private TransformComponent _playerTransform;

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
            GameObjectManager.Add(_player);

            uiManager = new UIManager();

            var font = content.Load<SpriteFont>("Fonts/DebugFont");

            _debugPanel = new DebugPanel(font, _player.GetComponent<TransformComponent>(), GameObjectManager, TilemapManager);
            uiManager.AddElement(_debugPanel);
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

            // Renderização de sobreposição de cor
            spriteBatch.Begin();
            var pixelTexture = GraphicsUtils.GetPixelTexture(spriteBatch.GraphicsDevice);
            var screenRectangle = new Rectangle(0, 0, spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height);
            spriteBatch.Draw(pixelTexture, screenRectangle, _overlayColor);
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
                else if (!_debugPanel.IsVisible)
                {
                    _debugPanel.IsVisible = true;
                }
            }

            _timeManager.Update(gameTime);
            UpdateOverlayColor();

            GameObjectManager.Update(gameTime);

            if (_player != null)
            {
                var playerTransform = _player.GetComponent<TransformComponent>();
                _camera.Position = playerTransform.Position;
            }



            RequestChunksAroundCamera();
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

        private void UpdateOverlayColor()
        {
            Color dayColor = Color.Transparent;
            Color duskColor = new Color(120, 60, 0, 50);
            Color nightColor = new Color(0, 0, 50, 100);
            Color dawnColor = new Color(130, 60, 20, 50);

            switch (_timeManager.CurrentTimeOfDay)
            {
                case TimeOfDay.Day:
                    float dayProgrss = (_timeManager.CurrentTime - 0.75f) / 0.10f;
                    _overlayColor = Color.Lerp(dawnColor, dayColor, dayProgrss);
                    _overlayColor = dayColor;
                    break;
                case TimeOfDay.Dusk:
                    float duskProgress = (_timeManager.CurrentTime - 0.75f) / 0.10f;
                    _overlayColor = Color.Lerp(dayColor, duskColor, duskProgress);
                    break;
                case TimeOfDay.Night:
                    float nightProgress = (_timeManager.CurrentTime - 0.15f) / 0.10f;
                    _overlayColor = Color.Lerp(duskColor, nightColor, nightProgress);
                    break;
                case TimeOfDay.Dawn:
                    float dawnProgress = (_timeManager.CurrentTime - 0.15f) / 0.10f;
                    _overlayColor = Color.Lerp(nightColor, dawnColor, dawnProgress);
                    break;
            }
        }
    }
}