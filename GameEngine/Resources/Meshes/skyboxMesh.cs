using GameEngine.Bufers;
using GameEngine.Core.Structs;
using GameEngine.Resources.Shaders;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Resources.Meshes
{
    public class skyboxMesh : BaseMesh
    {
        public skyboxMesh(BaseShader shader, Vertex[] vertices, uint[] indices)
        {
            Init(shader, vertices, indices);
        }

        public skyboxMesh(BaseShader shader, float[] vertices, uint[] indices)
        {
            buffers = new EBOSkyboxBuffer();
            buffers.Init(shader, vertices, indices);
        }

        public override void Init(BaseShader shader, Vertex[] vertices)
        {
            
        }

        public override void Init(BaseShader shader, Vertex[] vertices, uint[] indices)
        {
            buffers = new EBOSkyboxBuffer();
            buffers.Init(shader, vertices, indices);
        }

        public void Draw(PrimitiveType type)
        {
            buffers.Draw(type);
        }

        public override void Draw(PrimitiveType type, BaseShader shader)
        {
            base.Draw(type, shader);
        }

        public override void Init(BaseShader shader, Material material, Vertex[] vertices, uint[] indices)
        {
            
        }
    }
}
