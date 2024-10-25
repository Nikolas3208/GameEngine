using GameEngine.Core;
using GameEngine.GameObjects.Components.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameObjects.Components
{
    public  class Component
    {
        public GameObject gameObject;

        public string Name = "Component";

        public virtual void Start()
        {

        }
        public virtual void Update(Shader shader, float deltaTime)
        {

        }
        public virtual void Draw(Shader shader)
        {

        }
    }
}
