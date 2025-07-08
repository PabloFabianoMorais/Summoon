using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sunmoon.utils
{
    public static class GraphicsUtils
    {
        private static Texture2D _pixelTexture;

        public static Texture2D GetPixelTexture(GraphicsDevice graphicsDevice)
        {
            if (_pixelTexture == null)
            {
                _pixelTexture = new Texture2D(graphicsDevice, 1, 1);
                _pixelTexture.SetData(new[] { Color.White });
            }
            return _pixelTexture;
        }
    }
}