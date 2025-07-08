
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sunmoon.UI
{
    public class UIManager
    {
        private List<UIElement> _rootElements = new List<UIElement>();

        public void AddElement(UIElement element)
        {
            _rootElements.Add(element);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = _rootElements.Count - 1; i >= 0; i--)
            {
                _rootElements[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var element in _rootElements)
            {
                element.Draw(spriteBatch);
            }
        }
    }
}