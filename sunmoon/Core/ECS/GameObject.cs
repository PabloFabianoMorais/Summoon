using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace sunmoon.Core.ECS
{

    public class GameObject
    {
        public long Id { get; set; }

        // Lista de componentes
        private readonly List<Component> _allComponents = new List<Component>();
        private readonly List<IDrawableComponent> _drawableComponents = new List<IDrawableComponent>();
        private readonly List<IUpdatableComponent> _updatableComponents = new List<IUpdatableComponent>();



        public GameObject() { }

        public List<Component> GetAllComponents()
        {
            return _allComponents;
        }

        // Adiciona um componente
        public T AddComponent<T>() where T : Component, new()
        {
            T newComponent = new T();
            newComponent.GameObject = this;
            if (newComponent is IUpdatableComponent updatable)
                _updatableComponents.Add(updatable);

            if (newComponent is IDrawableComponent drawable)
                _drawableComponents.Add(drawable);

            _allComponents.Add(newComponent);

            return newComponent;
        }

        public Component AddComponent(Type componentType)
        {
            if (!typeof(Component).IsAssignableFrom(componentType))
                throw new ArgumentException($"{componentType.Name} não herda de Component");

            Component newComponent = (Component)Activator.CreateInstance(componentType);

            newComponent.GameObject = this;
            if (newComponent is IUpdatableComponent updatable)
                _updatableComponents.Add(updatable);

            if (newComponent is IDrawableComponent drawable)
                _drawableComponents.Add(drawable);

            _allComponents.Add(newComponent);

            return newComponent;
        }


        // Procura um componente
        public T GetComponent<T>() where T : Component
        {
            return _allComponents.OfType<T>().FirstOrDefault();
        }

        public void RemoveComponent<T>() where T : Component
        {
            T component = GetComponent<T>();
            if (component == null) return;

            if (component is IDrawableComponent drawable)
                _drawableComponents.Remove(drawable);
            if (component is IUpdatableComponent updatable)
                _updatableComponents.Remove(updatable);
            _allComponents.Remove(component);
        }

        // Inicializa os componentes
        public virtual void Initialize()
        {
            foreach (Component component in _allComponents)
            {
                component.Initialize();
            }
        }

        // Executa funções de atualização, se houver
        public virtual void Update(GameTime gameTime)
        {
            foreach (IUpdatableComponent component in _updatableComponents)
            {
                component.Update(gameTime);
            }
        }

        // Executa funções de rederização, se houver
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (IDrawableComponent component in _drawableComponents)
            {
                component.Draw(spriteBatch);
            }
        }
    }
}