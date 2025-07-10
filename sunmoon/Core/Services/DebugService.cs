
using System;
using Microsoft.Xna.Framework;

namespace sunmoon.Core.Services
{
    /// <summary>
    /// O serviço estático para rastrear e oferecer métricas de depuração globais
    /// </summary>
    public static class DebugService
    {
        public static float Fps { get; set; }
        private const float FPS_UPDATE_INTERVAL = 1.0f;

        private static float _fpsTimer = 0f;
        private static int _frameCount = 0;

        public static void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _fpsTimer += elapsed;
            _frameCount++;

            if (_fpsTimer >= FPS_UPDATE_INTERVAL)
            {
                Fps = _frameCount / _fpsTimer;
                _fpsTimer = 0f;
                _frameCount = 0;
            }
        } 
    }
}