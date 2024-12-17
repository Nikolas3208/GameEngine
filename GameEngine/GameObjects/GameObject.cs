using GameEngine.Core;
using GameEngine.Core.Structs;
using GameEngine.GameObjects.Components;
using GameEngine.GameObjects.Components.List;
using GameEngine.GameObjects.List;
using GameEngine.Resources;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameEngine.GameObjects
{
    [JsonDerivedType(typeof(GameObject), 0)]
    [JsonDerivedType(typeof(Camera), 1)]
    [JsonDerivedType(typeof(Grid), 2)]
    public class GameObject
    {
        [JsonInclude]
        protected List<Component> components;

        [JsonInclude]
        protected Shader shader = null;

        public string Name { get; set; } = "GameObject";
        public int Id { get; set; }

        public bool IsSelected = false;

        [JsonConstructor]
        public GameObject()
        {
            if(shader == null)
                shader = Shader.LoadFromFile(AssetManager.GetShader("multipleLights"));

            components = new List<Component>();
            if(GetComponent<TransformComponet>() == null)
                AddComponent(new TransformComponet(new Vector3f(), new Vector3f(), new Vector3f(1)));
        }

        public virtual void Start()
        {
            foreach (var component in components)
            {
                component.gameObject = this;
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
            if (!components.Contains(component))
            {
                component.gameObject = this;
                components.Add(component);
            }
            else
                throw new Exception("This component is contains this object");
        }

        public T GetComponent<T>() where T : Component
        {
            if (components.Count > 0)
            {
                return  (T)components.Find(c => c.GetType() == typeof(T));
            }

            return null;
        }
        public List<Component> GetComponents() => components;
        public void RemoveComponent(Component component) => components.Remove(component);
    }
}
