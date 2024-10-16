using GameEngine.Resources.Meshes;
using GameEngine.Resources.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Bufers
{
    public class EBOSkyboxBuffer : BaseBuffer
    {
        public override void Init(BaseShader shader, Vertex[] vertices, uint[] indices)
        {
            count = indices.Length;

            vao = InitVAOBuffer();
            GL.BindVertexArray(vao);

            sizeVertex = Marshal.SizeOf(typeof(Vertex));

            vbo = CreateVBOBuffer(vertices);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            ebo = CreateEBOBuffer(indices);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            var vertexLocation = shader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, sizeVertex, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public override void Init(BaseShader shader, float[] vertices, uint[] indices)
        {
            count = indices.Length;

            vao = InitVAOBuffer();
            GL.BindVertexArray(vao);

            sizeVertex = sizeof(float);

            vbo = CreateVBOBuffer(vertices);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            ebo = CreateEBOBuffer(indices);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            var vertexLocation = shader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeVertex, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }


        public override void Bind()
        {
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        }

        public override void Unbind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public override void Draw(PrimitiveType type)
        {
            Bind();

            GL.DrawElements(type, count, DrawElementsType.UnsignedInt, sizeof(int) * 0);

            Unbind();
        }

        public override void Init(int width, int height)
        {

        }

        public override void Init(BaseShader shader, Vertex[] vertices)
        {

        }
    }
}
