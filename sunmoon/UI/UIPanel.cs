using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sunmoon.UI
{
    public class UIPanel : UIElement
    {
        public Color BackgroundColor { get; set; }
        private Texture2D _texture;

        public UIPanel(Point size, Vector2 position, Color color)
        {
            Size = size;
            LocalPosition = position;
            BackgroundColor = color;
        }

        public void CreateTexture(GraphicsDevice graphicsDevice)
        {
            if (_texture != null) return;

            _texture = new Texture2D(graphicsDevice, 1, 1);
            _texture.SetData(new[] { Color.White });
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsVisible) return;

            foreach (var child in Children)
            {
                child.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            CreateTexture(spriteBatch.GraphicsDevice);

            spriteBatch.Draw(_texture, new Rectangle(AbsolutePosition.ToPoint(), Size), BackgroundColor);

            foreach (var child in Children) {
                child.Draw(spriteBatch);
            }
        }
    }
}