using GameEngine.Core.Structs;
using GameEngine.Resources.Shaders;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Bufers
{
    public class EBOLineBuffer : BaseBuffer
    {
        public override void Init(BaseShader shader, float[] vertices)
        {
            base.Init(shader, vertices);

            count = vertices.Length;

            vao = InitVAOBuffer();
            GL.BindVertexArray(vao);

            sizeVertex = sizeof(float);

            vbo = CreateVBOBuffer(vertices);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            var vertexLocation = shader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeVertex, 0);

            var colorLocation = shader.GetAttribLocation("aColor");
            GL.EnableVertexAttribArray(colorLocation);
            GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeVertex, 3 * sizeVertex);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public override void Bind()
        {
            GL.BindVertexArray(vao);
        }

        public override void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public override void Draw(PrimitiveType type)
        {
            Bind();

            GL.DrawArrays(type, 0, count);

            Unbind();
        }

        public override void Init(int width, int height)
        {

        }

        public override void Init(BaseShader shader, Vertex[] vertices)
        {

        }

        public override void Init(BaseShader shader, Vertex[] vertices, uint[] indices)
        {

        }

    }
}
