using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using sunmoon.Core;
using sunmoon.Core.ECS;

namespace sunmoon.Components
{
    public class SpriteRendererComponent : Component, IDrawableComponent, IContentLoadable
    {
        private TransformComponent _transformComponent;

        public Texture2D Texture { get; set; }
        public string TexturePath { get; set; }

        public Rectangle? SourceRectangle { get; set; } = null;
        public Color Color { get; set; } = Color.White;
        public Vector2 Origin { get; set; } = Vector2.Zero;
        public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;
        public float LayerDepth { get; set; } = 0f;

        public override void Initialize()
        {
            _transformComponent = GameObject?.GetComponent<TransformComponent>();
            base.Initialize();
        }

        public void LoadContent(ContentManager content)
        {
            if (!string.IsNullOrEmpty(TexturePath))
            {
                Texture = content.Load<Texture2D>(TexturePath);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: this.Texture,
                position: this._transformComponent.Position,
                sourceRectangle: this.SourceRectangle,
                color: this.Color,
                rotation: _transformComponent.Rotation,
                origin: this.Origin,
                scale: _transformComponent.Scale,
                effects: this.SpriteEffects,
                layerDepth: this.LayerDepth
            );
        }
    }
}