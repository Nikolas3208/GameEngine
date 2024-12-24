using GameEngine.Core.Essentials;
using GameEngine.Core.Structs;
using GameEngine.Renders.Bufers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameEngine.Renders
{
    public class VertexArray
    {
        private int VertexArrayId;

        [JsonInclude]
        private VertexBuffer vertexBuffer;
        [JsonInclude]
        private IndexBuffer indexBuffer;

        public VertexArray()
        {

        }

        public VertexArray(VertexArray vertexArray)
        {

        }

        public VertexArray(VertexBuffer vertexBuffer, IndexBuffer indexBuffer)
        {
            this.vertexBuffer = vertexBuffer;
            this.indexBuffer = indexBuffer;

            Init(null);
        }
        public VertexArray(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, Shader shader = null)
        {
            this.vertexBuffer = vertexBuffer;
            this.indexBuffer = indexBuffer;

            Init(shader);
        }

        private int vertexLocation = 0;
        private int normalLocation = 1;
        private int textureLocation = 2;

        public void Init(Shader shader)
        {
            int size = Marshal.SizeOf(typeof(Vertex));

            VertexArrayId = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayId);

            vertexBuffer!.Bind();
            indexBuffer!.Bind();

            if(shader != null)
            {
                vertexLocation = shader.GetAttribLocation("aPos");
                normalLocation = shader.GetAttribLocation("aNormal");
                textureLocation = shader.GetAttribLocation("aTexCoords");
            }

            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, size, 0);

            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, size, 1 * Marshal.SizeOf(typeof(Vector3)));

            GL.EnableVertexAttribArray(textureLocation);
            GL.VertexAttribPointer(textureLocation, 3, VertexAttribPointerType.Float, false, size, 2 * Marshal.SizeOf(typeof(Vector3)));

            indexBuffer!.Unbind();
            vertexBuffer!.Unbind();
            Unbind();

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);

            if (shader != null)
                shader.Use();
        }

        public void Bind()
        {
            GL.BindVertexArray(VertexArrayId);
            indexBuffer!.Bind();
        }

        public void Unbind()
        {
            indexBuffer!.Unbind();
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
