using GameEngine.Bufers;
using GameEngine.Resources.Shaders;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Resources.Meshes
{
    public abstract class BaseMesh
    {
        protected BaseBuffer buffers;

        protected Vertex[] vertices;
        protected uint[] indices;

        public string MaterialName;
        public string Name;

        public abstract void Init(BaseShader shader, Vertex[] vertices);
        public abstract void Init(BaseShader shader, Vertex[] vertices, uint[] indices);

        public virtual void Draw(PrimitiveType type)
        {
            buffers.Draw(type);
        }
    }
}
