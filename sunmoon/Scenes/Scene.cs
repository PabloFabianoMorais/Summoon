using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using sunmoon.Core.Management;

namespace sunmoon.Scenes
{
    public abstract class Scene
    {
        public GameObjectManager GameObjectManager { get; set; }
        public ContentManager Content { get; set; }
        public abstract void LoadContent(ContentManager content);
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}