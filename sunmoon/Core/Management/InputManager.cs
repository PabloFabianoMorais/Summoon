
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace sunmoon.Core.Management
{
    /// <summary>
    /// Gerencia, manipula e mapeia as ações de entrada do usuário.
    /// </summary>
    public static class InputManager
    {
        private static KeyboardState _currentKeyboardState;
        private static KeyboardState _previousKeyboardState;
        private static MouseState _currentMouseState;
        private static MouseState _previousMouseState;

        private static readonly Dictionary<string, Keys> _keyMappings = new Dictionary<string, Keys>();

        public static void Initialize()
        {
            _keyMappings["MoveUp"] = Keys.W;
            _keyMappings["MoveDown"] = Keys.S;
            _keyMappings["MoveLeft"] = Keys.A;
            _keyMappings["MoveRight"] = Keys.D;
            _keyMappings["ToggleDebug"] = Keys.P;
            _keyMappings["ReloadMap"] = Keys.R;
        }

        public static void Update()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Retorna true se a tecla for pressionada.
        /// </summary>
        /// <param name="key">Tecla a ser verificada.</param>
        /// <returns>True ou false</returns>
        public static bool IsKeyDown(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Retorna true se a tecla foi pressionada nesse exato frame.
        /// </summary>
        /// <param name="key">Tecla a ser verificada.</param>
        /// <returns>rue ou false</returns>
        public static bool WasKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Retorna true se a tecla foi solta nesse exato frame.
        /// </summary>
        /// <param name="key">Tecla a ser verificada.</param>
        /// <returns>True ou false</returns>
        public static bool WasKeyReleased(Keys key)
        {
            return _currentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Verifica se uma ação foi acionada.
        /// </summary>
        /// <param name="action">O nome da ação a ser verificada</param>
        /// <returns>True ou false</returns>
        public static bool IsActionDown(string action)
        {
            if (_keyMappings.TryGetValue(action, out var key))
            {
                return IsKeyDown(key);
            }
            return false;
        }

        /// <summary>
        /// Verifica se uma ação que foi acionada nesse exato frame.
        /// </summary>
        /// <param name="action">O nome da ação a ser verificada</param>
        /// <returns>True ou false</returns>
        public static bool IsActionPressed(string action)
        {
            if (_keyMappings.TryGetValue(action, out var key))
            {
                return WasKeyPressed(key);
            }
            return false;
        }

        /// <summary>
        /// Verifica se uma ação que foi solta nesse exato frame.
        /// </summary>
        /// <param name="action">O nome da ação a ser verificada</param>
        /// <returns>True ou false</returns>
        public static bool IsActionReleased(string action)
        {
            if (_keyMappings.TryGetValue(action, out var key))
            {
                return WasKeyReleased(key);
            }
            return false;
        }
    }
}