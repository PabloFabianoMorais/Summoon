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
        public Vector2 Origin { get; set; } = Vector2.Zero;
        public bool AutoCenterOrigin { get; set; } = false;
        public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;
        public float LayerDepth { get; set; } = 0f;

        private TransformComponent _transformComponent;
        private Animation _currentAnimation;
        private int _currentFrameIndex;

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

            if (_currentAnimation.FrameDuration > 0)
            {
                float totalLoopDuration = _currentAnimation.FrameCount * _currentAnimation.FrameDuration;
                float timeInCurrentLoop = (float)gameTime.TotalGameTime.TotalSeconds % totalLoopDuration;

                _currentFrameIndex = (int)(timeInCurrentLoop / _currentAnimation.FrameDuration);
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

            var originToUse = this.Origin;

            if (this.AutoCenterOrigin)
                originToUse = new Vector2(sourceRect.Width / 2f, sourceRect.Height / 2);

            spriteBatch.Draw(
                texture: _currentAnimation.Texture,
                position: _transformComponent.Position,
                sourceRectangle: sourceRect,
                color: this.Color,
                rotation: _transformComponent.Rotation,
                origin: originToUse,
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
        }

        public Animation GetCurrentAnimation()
        {
            return _currentAnimation;
        }
    }
}