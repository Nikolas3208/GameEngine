using GameEngine.Core;
using GameEngine.GameObjects.Components.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameObjects.Components
{
    public abstract class Component
    {
        public GameObject gameObject;

        public string Name = "Component";

        public abstract void Start();
        public abstract void Update(float deltaTime);
        public abstract void Draw(Shader shader);
    }
}
