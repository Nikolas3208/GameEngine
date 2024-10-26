using GameEngine.Core;
using GameEngine.GameObjects.Components;
using GameEngine.GameObjects.Components.List;
using GameEngine.Resources;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameObjects
{
    public class GameObject
    {
        private List<Component> components;

        protected Shader shader;

        public string Name { get; set; } = "GameObject";
        public int Id { get; set; }

        public bool IsSelected = false;

        public GameObject()
        {
            if(shader == null)
                shader = Shader.LoadFromFile(AssetManager.GetShader("multipleLights"));

            components = new List<Component>();
            AddComponent(new TransformComponet());
        }

        public virtual void Start()
        {
            foreach (var component in components)
            {
                component.Start();
            }
        }

        public virtual void Update(float deltaTime)
        {
            foreach (var component in components)
            {
                component.Update(deltaTime);
            }
        }

        public virtual void Draw()
        {
            foreach (var component in components)
            {
                component.Draw(shader);
            }
        }

        public Shader GetShader() { return shader; }
        public void SetShader(Shader shader) { this.shader = shader; }

        public void AddComponent(Component component)
        {
            components.Add(component);
            component.gameObject = this;
        }

        public T GetComponent<T>() where T : Component
        {
            foreach (Component comp in components)
            {
                if(comp.GetType().Equals(typeof(T)))
                    return (T)comp;
            }

            return null;
        }

        public List<Component> GetComponents() => components;
        public void RemoveComponent(Component component) => components.Remove(component);
    }
}
