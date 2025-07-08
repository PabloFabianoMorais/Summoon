
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace sunmoon.Scenes
{
    public static class SceneManager
    {
        private static Scene _currentScene;
        private static Scene _nextScene;

        private static ContentManager _content;
        private static bool _isInitialized = false;

        public static void Initialize(ContentManager content)
        {
            if (_isInitialized) return;
            _content = new ContentManager(content.ServiceProvider, content.RootDirectory);
            _isInitialized = true;
        }

        public static void LoadScene(Scene scene)
        {
            if (!_isInitialized)
                throw new InvalidOperationException("SceneManger não foi inicializado. Chame Initialize() primeiro");

            _nextScene = scene;
        }

        public static void PerformSceneChange()
        {
            if (_nextScene == null) return;

            _currentScene?.UnloadContent();

            _currentScene = _nextScene;
            _nextScene = null;

            _currentScene.LoadContent(_content);
        }

        public static void Update(GameTime gameTime)
        {
            PerformSceneChange();

            _currentScene?.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            _currentScene?.Draw(spriteBatch);
        }
    }
}