using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sunmoon.Core.World
{
    public class Camera
    {
        public Vector2 Position { get; set; }
        public float Zoom { get; set; }
        public float Rotation { get; set; }

        private readonly Viewport _viewport;

        public Camera(Viewport viewport)
        {
            _viewport = viewport;
            Position = Vector2.Zero;
            Zoom = 1.0f;
            Rotation = 0.0f;
        }

        /// <summary>
        /// Gera uma matriz de visualização 2D que leva em conta posição, rotação e zoom.
        /// </summary>
        /// <returns>Uma matriz de visualização para ser usada como perspectiva.</returns>
        public Matrix GetViewMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1.0f) *
                Matrix.CreateTranslation(new Vector3(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0));
        }

        /// <summary>
        /// Converte a matriz de visualização da câmera em um Rectangle.
        /// </summary>
        /// <returns>Área visível da câmera em formato de Rectangle</returns>
        public Rectangle GetVisibleArea()
        {
            var inverseViewMatrix = Matrix.Invert(GetViewMatrix());

            var topLeft = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
            var topRight = Vector2.Transform(new Vector2(_viewport.Width, 0), inverseViewMatrix);
            var bottomLeft = Vector2.Transform(new Vector2(0, _viewport.Height), inverseViewMatrix);
            var bottomRight = Vector2.Transform(new Vector2(_viewport.Width, _viewport.Height), inverseViewMatrix);

            var minX = Math.Min(topLeft.X, Math.Min(topRight.X, Math.Min(bottomLeft.X, bottomRight.X)));
            var maxX = Math.Max(topLeft.X, Math.Max(topRight.X, Math.Max(bottomLeft.X, bottomRight.X)));
            var minY = Math.Min(topLeft.Y, Math.Min(topRight.Y, Math.Min(bottomLeft.Y, bottomRight.Y)));
            var maxY = Math.Max(topLeft.Y, Math.Max(topRight.Y, Math.Max(bottomLeft.Y, bottomRight.Y)));

            return new Rectangle((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
        }
    }
}