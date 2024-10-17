using GameEngine.Resources.Meshes;
using GameEngine.Resources.Shaders;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Bufers
{
    public abstract class BaseBuffer
    {
        protected int vao, vbo, ebo, fbo;
        protected int sizeVertex;
        protected int count;

        public abstract void Init(int width, int height);
        public abstract void Init(BaseShader shader, Vertex[] vertices);
        public abstract void Init(BaseShader shader, Vertex[] vertices, uint[] indices);
        public virtual void Init(BaseShader shader, float[] vertices, uint[] indices)
        {

        }
        public virtual void Init(BaseShader shader, float[] vertices)
        {

        }
        protected int CreateVBOBuffer(Vertex[] vertices)
        {
            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeVertex, vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            return vbo;
        }

        protected int CreateVBOBuffer(float[] vertices)
        {
            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeVertex, vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            return vbo;
        }

        protected int InitVAOBuffer()
        {
            int vao = GL.GenVertexArray();
            GL.BindVertexArray(0);

            return vao;
        }

        protected int CreateEBOBuffer(uint[] date)
        {
            int ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, date.Length * sizeof(uint), date, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            return ebo;
        }

        public abstract void Bind();
        public abstract void Unbind();
        public virtual void Unbind(int width, int height)
        {

        }
        public abstract void Draw(PrimitiveType type);
    }
}
