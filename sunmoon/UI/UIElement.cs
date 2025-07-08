
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sunmoon.UI
{
    public abstract class UIElement
    {
        public bool IsVisible { get; set; } = true;
        public Vector2 LocalPosition { get; set; }

        public Point Size { get; set; }

        public UIElement Parent { get; private set; }

        protected List<UIElement> Children { get; } = new List<UIElement>();

        public Vector2 AbsolutePosition
        {
            get
            {
                if (Parent == null)
                    return LocalPosition;

                return Parent.AbsolutePosition + LocalPosition;
            }
        }

        public virtual void AddChild(UIElement child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}