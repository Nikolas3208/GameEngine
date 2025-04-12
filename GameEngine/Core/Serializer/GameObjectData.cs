using GameEngine.Core.GameObjects;
using GameEngine.Core.GameObjects.Components;
using GameEngine.Graphics;
using System.Numerics;
using System.Xml.Linq;

namespace GameEngine.Core.Serializer
{
    public class GameObjectData
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public List<ComponentData> Components { get; set; } = new();

        public GameObjectData() { }
        public GameObjectData(string name, Vector3 position, Vector3 rotation, Vector3 scale, List<ComponentData> components)
        {
            Name = name;
            Position = position;
            Rotation = rotation;
            Scale = scale;
            Components = components;
        }

        public GameObjectData(GameObject go)
        {
            Name = go.Name;

            Position = VectorConverter.ToVector3System(go.Position);
            Rotation = VectorConverter.ToVector3System(go.Rotation);
            Scale = VectorConverter.ToVector3System(go.Scale);

            int componentCount = go.GetAllComponents().Count;

            for (int i = 0; i < componentCount; i++)
            {
                go.GetAllComponents().ForEach(component => { Components.Add(component.Serialize()); });
            }
        }

        public GameObject ToGameObject(Scene scene)
        {
            var go = new GameObject(scene);
            go.Position = VectorConverter.ToVector3OpenTK(Position);
            go.Rotation = VectorConverter.ToVector3OpenTK(Rotation);
            go.Scale = VectorConverter.ToVector3OpenTK(Scale);

            foreach (var compData in Components)
            {
                var type = Type.GetType(compData.Type);
                if (type == null) continue;

                var comp = (Component)Activator.CreateInstance(type)!;

                comp.Deserialize(compData);

                go.AddComponent(comp);
            }

            return go;
        }
    }
}
