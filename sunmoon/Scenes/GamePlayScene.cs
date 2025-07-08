using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using sunmoon.Core.Management;
using sunmoon.Core.Factory;
using Microsoft.Xna.Framework.Input;
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
        private UIPanel _debugPanel = new UIPanel(new Point(300, 400), new Vector2(0, 0), new Color(0, 0, 0, 100));


        private TransformComponent _playerTransform;

        public override void LoadContent(ContentManager content)
        {
            var graphicsDeviceService = (IGraphicsDeviceService)content.ServiceProvider.GetService(typeof(IGraphicsDeviceService));

            if (graphicsDeviceService == null)
                throw new InvalidOperationException($"IGraphicsDevice não encontrado. O serviço não foi registrado corretamente.");


            _camera = new Camera(graphicsDeviceService.GraphicsDevice.Viewport);
            _camera.Zoom = 3;


            GameObjectManager = new GameObjectManager();
            TilemapManager = new TilemapManager();
            TilemapManager.LoadMapFromFile("Content/data/maps/level_01.map.json");


            _player = GameObjectFactory.Create("Player");
            _playerTransform = _player.GetComponent<TransformComponent>();
            GameObjectManager.Add(_player);

            uiManager = new UIManager();

            var font = content.Load<SpriteFont>("Fonts/DebugFont");

            _debugPanel = new UIPanel(new Point(300, 400), new Vector2(0, 0), new Color(0, 0, 0, 100));
            var stackPanel = new UIStackPanel(new Vector2(0, 0), 2f);

            var playerPosLabel = new UILabel(font, () => $"Posição: X={Math.Floor(_playerTransform.Position.X)}, Y={Math.Floor(_playerTransform.Position.Y)}", Vector2.Zero, Color.White);
            var objectsNumberLabel = new UILabel(font, () => $"Número de objetos: {GameObjectManager.GetObjectsCount()}", Vector2.Zero, Color.White);


            stackPanel.AddChild(playerPosLabel);
            stackPanel.AddChild(objectsNumberLabel);
            _debugPanel.AddChild(stackPanel);
            _debugPanel.IsVisible = false;
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

            TilemapManager.Draw(spriteBatch);
            GameObjectManager.Draw(spriteBatch);

            spriteBatch.End();

            // Renderização da interface gráfica
            spriteBatch.Begin();

            uiManager.Draw(spriteBatch);

            spriteBatch.End();

        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.IsActionReleased("ReloadMap"))
                TilemapManager.LoadMapFromFile("Content/data/maps/level_01.map.json");
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