using GameEngine.GameObjects.Components;
using GameEngine.GameObjects.Components.List;
using GameEngine.Resources.Shaders;
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

        public string Name { get; set; } = "GameObject";
        public int Id { get; set; }

        public bool IsSelected = false;

        public GameObject()
        {
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

        public virtual void Update(float deltaTime, BaseShader shader = null)
        {
            foreach (var component in components)
            {
                component.Update(shader, deltaTime);
            }
        }

        public virtual void Draw(BaseShader shader = null)
        {
            foreach (var component in components)
            {
                component.Draw(shader);
            }
        }

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
        public void RemoveComponent(Component component) => components.Remove(component);
    }
}
