using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sunmoon.UI
{
    public class UILabel : UIElement
    {
        public Func<string> GetText { get; set; }
        public SpriteFont Font { get; set; }
        public Color Color { get; set; }

        private string _chachedString;

        public UILabel(SpriteFont font, Func<string> getTextDelegate, Vector2 position, Color color)
        {
            Font = font;
            GetText = getTextDelegate;
            LocalPosition = position;
            Color = color;
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsVisible) return;

            _chachedString = GetText();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            // TODO: Implementar lódica de quebra de linha (word-wrapping) para textos longos.
            spriteBatch.DrawString(Font, _chachedString, AbsolutePosition, Color);
        }
    }
}