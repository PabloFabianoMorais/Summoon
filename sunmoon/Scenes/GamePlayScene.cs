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

namespace sunmoon.Scenes
{
    public class GamePlayScene : Scene
    {
        public TilemapManager TilemapManager;
        private Camera _camera;
        private GameObject _player;
        public UIManager uiManager;
        private UIPanel _debugPanel;


        private TransformComponent _playerTransform;

        public override void LoadContent(ContentManager content)
        {
            var graphicsDeviceService = (IGraphicsDeviceService)content.ServiceProvider.GetService(typeof(IGraphicsDeviceService));

            if (graphicsDeviceService == null)
                throw new InvalidOperationException($"IGraphicsDevice não encontrado. O serviço não foi registrado corretamente.");

            var worldGenerator = new WorldGenerator(0);
            TilemapManager = new TilemapManager(worldGenerator);

            for (int y = -3; y <= 1; y++)
            {
                for (int x = -3; x <= 1; x++)
                {
                    TilemapManager.GenerateChunk(x, y);
                }
            }

            _camera = new Camera(graphicsDeviceService.GraphicsDevice.Viewport);
            _camera.Zoom = 3;


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
                else if (!_debugPanel.IsVisible)
                {
                    _debugPanel.IsVisible = true;
                }
            }

            GameObjectManager.Update(gameTime);

            if (_player != null)
            {
                var playerTransform = _player.GetComponent<TransformComponent>();
                _camera.Position = playerTransform.Position;
            }

            uiManager.Update(gameTime);

        }
    }
}