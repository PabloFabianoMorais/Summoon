using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using sunmoon.Core;
using sunmoon.Core.ECS;
using sunmoon.Core.Management;

namespace sunmoon.Components
{
    // Gerencia os inpus que controlam o player
    public class PlayerInputComponent : Component, IUpdatableComponent
    {
        private MovementComponent _movementComponent;

        public override void Initialize()
        {
            _movementComponent = GameObject.GetComponent<MovementComponent>();
            base.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Movemento WASD
            // FIXME: Se o jogador pressionar duas teclas opostas ao mesmo tempo elas
            //  irão se anular. Dar prioridade a última tecla pressionada
            if (InputManager.IsActionDown("MoveRight"))
                _movementComponent.Velocity.X += _movementComponent.MovementSpeed;
            if (InputManager.IsActionDown("MoveLeft"))
                _movementComponent.Velocity.X -= _movementComponent.MovementSpeed;
            if (InputManager.IsActionDown("MoveDown"))
                _movementComponent.Velocity.Y += _movementComponent.MovementSpeed;
            if (InputManager.IsActionDown("MoveUp"))
                _movementComponent.Velocity.Y -= _movementComponent.MovementSpeed;
        }
    }
}