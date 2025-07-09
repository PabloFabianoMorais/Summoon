using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace sunmoon.Core.ECS
{
    /// <summary>
    /// Representa a 'Entidade' na arquitetura Entidade-Componente-Sistema (ECS).
    /// É um contêiner universal para Componentes, que definem seu comportamento e dados.
    /// </summary>
    public class GameObject
    {
        public long Id { get; set; }
        private readonly List<Component> _allComponents = new List<Component>();
        private readonly List<IDrawableComponent> _drawableComponents = new List<IDrawableComponent>();
        private readonly List<IUpdatableComponent> _updatableComponents = new List<IUpdatableComponent>();



        public GameObject() { }

        /// <returns>Retorna todos os componentes do objeto</returns>
        public List<Component> GetAllComponents()
        {
            return _allComponents;
        }

        /// <typeparam name="T">Deve ser do tipo Component</typeparam>
        /// <returns>Componente adicionado</returns>
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

        /// <summary>
        /// Adiciona um componente à lista de componentes e atribui suas heranças.
        /// </summary>
        /// <param name="componentType">Tipo de component que deve ser do tipo Component.</param>
        /// <returns>Componente adicionado.</returns>
        /// <exception cref="ArgumentException">Se o tipo do componente não for Component.</exception>
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


        /// <returns>Procura o componente específicado.</returns>
        public T GetComponent<T>() where T : Component
        {
            return _allComponents.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Remove o componente especificado.
        /// </summary>
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

        public virtual void Initialize()
        {
            foreach (Component component in _allComponents)
            {
                component.Initialize();
            }
        }


        public virtual void Update(GameTime gameTime)
        {
            foreach (IUpdatableComponent component in _updatableComponents)
            {
                component.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (IDrawableComponent component in _drawableComponents)
            {
                component.Draw(spriteBatch);
            }
        }
    }
}