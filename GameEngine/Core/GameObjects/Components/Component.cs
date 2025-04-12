using GameEngine.Core.Serializer;

namespace GameEngine.Core.GameObjects.Components
{
    public abstract class Component
    {
        protected GameObject? gameObject { get; private set; }

        public Guid Id { get; }
        public string Name { get; private set; } = string.Empty;

        public Component()
        {
            Id = Guid.NewGuid();
        }

        public void SetGameObject(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public abstract void Start();
        public abstract void Update(float deltaTime);
        public abstract void Draw(float deltaTime);

        public abstract ComponentData Serialize();
        public abstract Component Deserialize(ComponentData data);
    }
}
