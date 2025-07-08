using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sunmoon.UI
{
    public class UIStackPanel : UIElement
    {
        public float Spacing { get; set; }

        public UIStackPanel(Vector2 position, float spacing = 5f)
        {
            LocalPosition = position;
            Spacing = spacing;
        }

        public override void AddChild(UIElement child)
        {
            base.AddChild(child);
            ArrangeChildren();
        }

        public void ArrangeChildren()
        {
            float currentY = 0;
            foreach (var child in Children)
            {
                child.LocalPosition = new Vector2(0, currentY);

                var label = child as UILabel;
                if (label != null)
                {
                    currentY += label.Font.LineSpacing + Spacing;
                }
                else
                {
                    currentY += child.Size.Y + Spacing;
                }
            }
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

            foreach (var child in Children)
            {
                child.Draw(spriteBatch);
            }
        }
    }
}