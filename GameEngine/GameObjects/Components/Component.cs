using GameEngine.Resources.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameObjects.Components
{
    public enum ComponentType
    {
        MeshRender = 0,
        Camera = 1,
        Light = 2,
    }
    public abstract class Component
    {
        public ComponentType Type { get; set; }
        public abstract void Start();
        public abstract void Update(BaseShader shader, float deltaTime);
        public abstract void Draw(BaseShader shader);
    }
}
