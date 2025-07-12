using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sunmoon.Components.Core;
using sunmoon.Core.ECS;
using sunmoon.Core.World;

namespace sunmoon.Core.Management
{
    /// <summary>
    /// Gerencia e organiza GameObjects
    /// </summary>
    public class GameObjectManager
    {
        private readonly List<GameObject> _gameObjects = new List<GameObject>();

        private readonly Dictionary<long, GameObject> _gameObjectsById = new Dictionary<long, GameObject>();

        private readonly List<GameObject> _addedGameObjects = new List<GameObject>();
        private readonly List<GameObject> _removedGameObjects = new List<GameObject>();
        private int _renderedObjectsCount = 0;

        public void Add(GameObject gameObject)
        {
            _addedGameObjects.Add(gameObject);
        }

        public void Remove(GameObject gameObject)
        {
            _removedGameObjects.Add(gameObject);
        }

        public int GetObjectsCount()
        {
            return _gameObjects.Count;
        }

        public int GetRenderedObjectsCount()
        {
            return _renderedObjectsCount;
        }

        public GameObject Find(long id)
        {
            _gameObjectsById.TryGetValue(id, out GameObject foundObject);
            return foundObject;
        }

        public void Update(GameTime gameTime)
        {
            ProcessAdditions();
            ProcessRemoval();

            foreach (var gameObject in _gameObjects)
            {
                gameObject.Update(gameTime);
            }

            ProcessRemoval(); // Garante que todos os objetos pendentes para remoção foram removidos
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Rectangle cameraBounds = camera.GetVisibleArea();
            _renderedObjectsCount = 0;

            foreach (var gameObject in _gameObjects)
            {
                if (cameraBounds.Intersects(gameObject.BoundingBox))
                {
                    gameObject.Draw(spriteBatch);
                    _renderedObjectsCount++;
                }
            }
        }

        private void ProcessAdditions()
        {
            // Adiciona todos os objetos pendentes para serem adicionados
            if (_addedGameObjects.Count == 0) return;

            foreach (var gameObject in _addedGameObjects)
            {
                _gameObjects.Add(gameObject);
                var entityComponent = gameObject.GetComponent<EntityComponent>();

                if (entityComponent != null)
                {
                    _gameObjectsById[entityComponent.Id] = gameObject;
                }
            }
            _addedGameObjects.Clear();
        }

        private void ProcessRemoval()
        {
            // Remove todos os objetos pendentes para serem removidos
            if (_removedGameObjects.Count == 0) return;

            foreach (var gameObject in _removedGameObjects)
            {
                _gameObjects.Remove(gameObject);
                var entityComponent = gameObject.GetComponent<EntityComponent>();
                if (entityComponent != null)
                {
                    _gameObjectsById.Remove(entityComponent.Id);
                }
            }

            _removedGameObjects.Clear();
        }
    }
}