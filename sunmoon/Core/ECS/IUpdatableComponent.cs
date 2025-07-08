using Microsoft.Xna.Framework;

namespace sunmoon.Core.ECS
{
    public interface IUpdatableComponent
    {
        void Update(GameTime gameTime);
    }
}