using Microsoft.Xna.Framework;
using sunmoon.Core.ECS;

namespace sunmoon.Components
{
    public class TransformComponent : Component, IUpdatableComponent
    {
        public Vector2 Position;
        public float Rotation = 0f;
        public Vector2 Scale { get; set; } = Vector2.One;

        public override void Initialize()
        {
            base.Initialize();
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}