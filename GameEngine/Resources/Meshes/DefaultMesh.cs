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
    public class DefaultMesh : BaseMesh
    {
        public DefaultMesh(BaseShader shader, Vertex[] vertices, uint[] indices = null, string name = "Mesh") 
        {
            Name = name;
            this.vertices = vertices;
            this.indices = indices;

            if (indices == null)
                Init(shader, vertices);
            else
                Init(shader, vertices, indices);
        }
        public override void Init(BaseShader shader, Vertex[] vertices)
        {
            buffers = new VAOBuffer();
            buffers.Init(shader, vertices);
        }

        public override void Init(BaseShader shader, Vertex[] vertices, uint[] indices)
        {
            buffers = new EBOBuffer();
            buffers.Init(shader, vertices, indices);
        }

        public override void Draw(PrimitiveType type)
        {
            base.Draw(type); 
        }
    }
}
