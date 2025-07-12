using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using sunmoon.Components.Movement;
using sunmoon.Components.Core;
using sunmoon.Components.Graphics;
using sunmoon.Core;
using sunmoon.Core.ECS;
using sunmoon.Core.Management;

namespace sunmoon.Components.Player
{
    // Gerencia os inpus que controlam o player
    public class PlayerInputComponent : Component, IUpdatableComponent
    {
        private MovementComponent _movementComponent;
        private AnimatedSpriteComponent _animatedSpriteComponent;

        public override void Initialize()
        {
            _movementComponent = GameObject.GetComponent<MovementComponent>();
            _animatedSpriteComponent = GameObject.GetComponent<AnimatedSpriteComponent>();
            base.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            var velocity = _movementComponent.Velocity;

            // Movemento WASD
            // FIXME: Se o jogador pressionar duas teclas opostas ao mesmo tempo elas
            //  irão se anular. Dar prioridade a última tecla pressionada
            if (InputManager.IsActionDown("MoveRight"))
            {
                velocity.X += _movementComponent.MovementSpeed;
                _animatedSpriteComponent.SpriteEffects = SpriteEffects.None;
            }
            if (InputManager.IsActionDown("MoveLeft"))
            {
                velocity.X -= _movementComponent.MovementSpeed;
                _animatedSpriteComponent.SpriteEffects = SpriteEffects.FlipHorizontally;
            }
            if (InputManager.IsActionDown("MoveDown"))
                velocity.Y += _movementComponent.MovementSpeed;
            if (InputManager.IsActionDown("MoveUp"))
                velocity.Y -= _movementComponent.MovementSpeed;

            if (velocity.LengthSquared() > 0)
            {
                _animatedSpriteComponent.Play("Running");
            }
            else
            {
                _animatedSpriteComponent.Play("Idle");
            }

            _movementComponent.Velocity = velocity;
        }
    }
}