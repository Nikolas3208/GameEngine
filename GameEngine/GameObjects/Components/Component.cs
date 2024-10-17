using GameEngine.GameObjects.Components.List;
using GameEngine.Resources.Shaders;
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

        public string Name;

        public virtual void Start()
        {

        }
        public virtual void Update(BaseShader shader, float deltaTime)
        {

        }
        public virtual void Draw(BaseShader shader)
        {

        }
    }
}
