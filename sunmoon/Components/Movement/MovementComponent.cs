using Microsoft.Xna.Framework;
using sunmoon.Core.ECS;
using sunmoon.Components.Core;

namespace sunmoon.Components.Movement
{
    public class MovementComponent : Component, IUpdatableComponent
    {
        private TransformComponent _transformComponent;

        public Vector2 Velocity = new Vector2(0, 0);
        public float MovementSpeed = 0f;
        public override void Initialize()
        {
            _transformComponent = GameObject?.GetComponent<TransformComponent>();
            base.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            _transformComponent.Position += Velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds);
            Velocity = new Vector2(0, 0);
        }
    }
}