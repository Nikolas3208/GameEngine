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
    public class GameObject : Transform
    {
        private Dictionary<ComponentType, Component> components;

        public string Name { get; set; } = "gameObject";
        public int Id;

        public GameObject()
        {
            components = new Dictionary<ComponentType, Component>();
        }

        public virtual void Start()
        {
            if (components == null)
                components = new Dictionary<ComponentType, Component>();

            foreach (var component in components)
            {
                component.Value.Start();
            }
        }

        public virtual void Update(float deltaTime, BaseShader shader = null)
        {
            shader.Use();

            shader.SetMatrix4("model", Model);
            shader.SetMatrix4("scale", ScaleMat);
            shader.SetMatrix4("rotation", RotationMat);

            foreach (var component in components)
            {
                component.Value.Update(shader, deltaTime);
            }
        }

        public virtual void Draw(BaseShader shader = null)
        {
            shader.SetInt("gameObjectId", Id);
            if (GetComponent(ComponentType.MeshRender) != null)
            {
                shader.Use();

                shader.SetMatrix4("model", Model);
                shader.SetMatrix4("scale", ScaleMat);
                shader.SetMatrix4("rotation", RotationMat);
            }

            foreach (var component in components)
            {
                component.Value.Draw(shader);
            }
        }

        public void AddComponent(ComponentType type)
        {
            if(!components.ContainsKey(type))
                components[type] = ComponentList.GetComponent(type);
        }
        public void AddComponent(Component component)
        {
            if (!components.ContainsKey(component.Type))
                components[component.Type] = component;
        }

        public Component GetComponent(ComponentType type)
        {
            if(components.ContainsKey(type)) return components[type];

            return null;
        }
        public Dictionary<ComponentType, Component> GetComponents()
        {
            return components;
        }
        public void RemoveComponent(ComponentType type) => components.Remove(type);
    }
}
