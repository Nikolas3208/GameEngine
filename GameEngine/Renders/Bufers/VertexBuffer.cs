using GameEngine.Core.Structs;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Renders.Bufers
{
    public class VertexBuffer
    {
        private int VertexBufferId;

        private Vertex[] vertices;

        public VertexBuffer(Vertex[] vertices)
        {
            this.vertices = vertices;

            Init();
        }

        public void Init()
        {
            VertexBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Marshal.SizeOf(typeof(Vertex)), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferId);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }
}
