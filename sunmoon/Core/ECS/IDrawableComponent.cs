using Microsoft.Xna.Framework.Graphics;

namespace sunmoon.Core.ECS
{
    public interface IDrawableComponent
    {
        void Draw(SpriteBatch spriteBatch);
    }
}