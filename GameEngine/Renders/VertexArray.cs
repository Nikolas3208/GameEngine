using GameEngine.Core.Structs;
using GameEngine.Renders.Bufers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Renders
{
    public class VertexArray
    {
        private int VertexArrayId;

        private VertexBuffer? vertexBuffer;
        private IndexBuffer? indexBuffer;

        public VertexArray() { }

        public VertexArray(VertexBuffer vertexBuffer, IndexBuffer indexBuffer)
        {
            this.vertexBuffer = vertexBuffer;
            this.indexBuffer = indexBuffer;
        }

        public void Init()
        {
            int size = Marshal.SizeOf(typeof(Vertex));

            VertexArrayId = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayId);

            vertexBuffer!.Bind();
            indexBuffer!.Bind();

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, size, 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, size, 0);

            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, size, 3 * Marshal.SizeOf(typeof(Vector2)));

            indexBuffer!.Unbind();
            vertexBuffer!.Unbind();
            Unbind();

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);
        }

        public void Bind()
        {
            GL.BindVertexArray(VertexArrayId);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void Draw(PrimitiveType type)
        {
            Bind();

            GL.DrawElements(type, indexBuffer!.Count, DrawElementsType.UnsignedInt, 0);
        }

        public void SetVertexBuffer(VertexBuffer vertexBuffer) => this.vertexBuffer = vertexBuffer;
        public void SetIndexBuffer(IndexBuffer indexBuffer) => this.indexBuffer = indexBuffer;

        public VertexBuffer? GetVertexBuffer() => vertexBuffer;
        public IndexBuffer? GetIndexBuffer() => indexBuffer;
    }
}
