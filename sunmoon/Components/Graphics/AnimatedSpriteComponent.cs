using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using sunmoon.Core.ECS;
using sunmoon.Core.Graphics;
using sunmoon.Components.Core;

namespace sunmoon.Components.Graphics
{
    public class AnimatedSpriteComponent : Component, IUpdatableComponent, IDrawableComponent, IContentLoadable
    {
        public Dictionary<string, Animation> Animations { get; set; } = new Dictionary<string, Animation>();
        public string CurrentAnimationName { get; set; }
        public Color Color { get; set; } = Color.White;
        public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;
        public float LayerDepth { get; set; } = 0f;

        private TransformComponent _transformComponent;
        private Animation _currentAnimation;
        private int _currentFrameIndex;
        private float _timer;

        public override void Initialize()
        {
            base.Initialize();
            _transformComponent = GameObject.GetComponent<TransformComponent>();

            if (!string.IsNullOrEmpty(CurrentAnimationName))
            {
                Play(CurrentAnimationName);
            }
            else if (Animations.Any())
            {
                Play(Animations.Keys.First());
            }

        }

        public void LoadContent(ContentManager content)
        {
            foreach (var anim in Animations.Values)
            {
                anim.Texture = content.Load<Texture2D>(anim.TexturePath);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_currentAnimation == null) return;

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= _currentAnimation.FrameDuration)
            {
                _timer -= _currentAnimation.FrameDuration;
                _currentFrameIndex++;

                if (_currentFrameIndex >= _currentAnimation.FrameCount)
                {
                    // Se a animação não deve repetir, nós a "congelamos" no último frame
                    // Decrementando o índice de volta para o valor máximo válido.
                    _currentFrameIndex = 0;
                }
                else if (!_currentAnimation.IsLooping)
                {
                    _currentFrameIndex = _currentFrameIndex - 1;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_currentAnimation == null || _currentAnimation.Texture == null) return;

            var sourceRect = new Rectangle(
                _currentFrameIndex * _currentAnimation.FrameWidth,
                0,
                _currentAnimation.FrameWidth,
                _currentAnimation.FrameHeight
            );

            spriteBatch.Draw(
                texture: _currentAnimation.Texture,
                position: _transformComponent.Position,
                sourceRectangle: sourceRect,
                color: this.Color,
                rotation: _transformComponent.Rotation,
                origin: Vector2.Zero,
                scale: _transformComponent.Scale,
                effects: this.SpriteEffects,
                layerDepth: this.LayerDepth
            );
        }


        public void Play(string animationName)
        {
            if (!Animations.ContainsKey(animationName) || _currentAnimation == Animations[animationName])
                return;

            _currentAnimation = Animations[animationName];
            CurrentAnimationName = animationName;
            _currentFrameIndex = 0;
            _timer = 0f;
        }

        public Animation GetCurrentAnimation()
        {
            return _currentAnimation;
        }
    }
}