using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using sunmoon.Components;
using sunmoon.Core.ECS;

namespace sunmoon.Core.Management
{
    public class GameObjectManager
    {
        private readonly List<GameObject> _gameObjects = new List<GameObject>();

        private readonly Dictionary<long, GameObject> _gameObjectsById = new Dictionary<long, GameObject>();

        private readonly List<GameObject> _addedGameObjects = new List<GameObject>();
        private readonly List<GameObject> _removedGameObjects = new List<GameObject>();

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

            ProcessRemoval();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Draw(spriteBatch);
            }
        }

        private void ProcessAdditions()
        {
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