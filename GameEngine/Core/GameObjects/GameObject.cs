using GameEngine.Core.GameObjects.Components;
using GameEngine.Graphics;

namespace GameEngine.Core.GameObjects
{
    public class GameObject : Transform
    {
        protected List<Component> components;
        protected List<GameObject> childrens;

        protected Scene scene;
        protected Shader shader;

        public Guid Id { get; }
        public string Name { get; set; } = nameof(GameObject);

        public GameObject(Scene scene)
        {
            this.scene = scene;

            components = new List<Component>();
            childrens = new List<GameObject>();

            Id = Guid.NewGuid();
        }

        public GameObject AddComponent(Component component)
        {
            if (components.FirstOrDefault(c => c.Id == component.Id) == null
                || components.FirstOrDefault(c => c.GetType() == component.GetType()) == null)
            {
                component.SetGameObject(this);
                components.Add(component);
            }
            else
            {
                throw new Exception("Данный компонент или тип уже в списке.");
            }

            return this;
        }

        public T GetComponent<T>() where T : Component
        {
            return (T)components.FirstOrDefault(c => c.GetType() == typeof(T))!;
        }

        public bool RemoveComponent(Component component)
        {
            return components.Remove(component);
        }

        public List<Component> GetAllComponents()
        {
            return components;
        }

        public virtual void Start()
        {
            foreach (var component in components)
            {
                component.Start();
            }

            foreach (var child in childrens)
            {
                child.Start();
            }
        }

        public virtual void Update(float deltaTime)
        {
            foreach (var component in components)
            {
                component.Update(deltaTime);
            }

            foreach (var child in childrens)
            {
                child.Update(deltaTime);
            }
        }

        public virtual void Draw(float deltaTime)
        {
            shader.SetMatrix4("model", GetModelMatrix());

            foreach (var component in components)
            {
                component.Draw(deltaTime);
            }

            foreach (var child in childrens)
            {
                child.Draw(deltaTime);
            }
        }

        public void SetShader(Shader shader)
        {
            this.shader = shader;
        }

        public Shader GetShader()
        {
            return shader;
        }
    }
}
