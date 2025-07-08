using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sunmoon.utils;

namespace sunmoon.UI
{
    public class UIPanel : UIElement
    {
        public Color BackgroundColor { get; set; }

        public UIPanel(Point size, Vector2 position, Color color)
        {
            Size = size;
            LocalPosition = position;
            BackgroundColor = color;
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

            spriteBatch.Draw(GraphicsUtils.GetPixelTexture(spriteBatch.GraphicsDevice), new Rectangle(AbsolutePosition.ToPoint(), Size), BackgroundColor);

            foreach (var child in Children) {
                child.Draw(spriteBatch);
            }
        }
    }
}