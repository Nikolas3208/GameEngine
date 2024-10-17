using GameEngine.Bufers;
using GameEngine.Resources.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Resources.Meshes
{
    public class AxesMesh : BaseMesh
    {
        public AxesMesh(BaseShader shader, float[] vertices)
        {
            buffers = new EBOLineBuffer();
            buffers.Init(shader, vertices);
        }
        public override void Init(BaseShader shader, Vertex[] vertices)
        {
            
        }

        public override void Init(BaseShader shader, Vertex[] vertices, uint[] indices)
        {
            
        }
    }
}
