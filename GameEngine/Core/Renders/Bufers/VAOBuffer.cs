using GameEngine.Core.Structs;
using GameEngine.Resources.Shaders;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Renders.Bufers
{
    public class VAOBuffer : BaseBuffer
    {
        public override void Init(Shader shader, Vertex[] vertices)
        {
            count = vertices.Length;

            vao = InitVAOBuffer();

            verticesSize = Marshal.SizeOf(typeof(Vertex));

            vbo = CreateVBOBuffer(vertices);

            var vertexLocation = shader.GetAttribLocation("aPos");
            var normalLocation = shader.GetAttribLocation("aNormal");
            var textureLocation = shader.GetAttribLocation("aTexCoords");

            GL.EnableVertexAttribArray(vertexLocation);
            GL.EnableVertexAttribArray(normalLocation);
            GL.EnableVertexAttribArray(textureLocation);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            //Vertices
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, verticesSize, 0 * Marshal.SizeOf(typeof(Vector3)));

            //Normals
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, verticesSize, 3 * Marshal.SizeOf(typeof(Vector3)));

            //TexCoords
            GL.VertexAttribPointer(textureLocation, 2, VertexAttribPointerType.Float, false, verticesSize, 6 * Marshal.SizeOf(typeof(Vector3)));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.DisableVertexAttribArray(vertexLocation);
            GL.DisableVertexAttribArray(normalLocation);
            GL.DisableVertexAttribArray(textureLocation);

            shader.Use();
        }

        public override void Init(Shader shader, float[] vertices)
        {
            count = vertices.Length;

            verticesSize = sizeof(float);

            vao = InitVAOBuffer();
            GL.BindVertexArray(vao);

            vbo = CreateVBOBuffer(vertices);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            var vertexLocation = shader.GetAttribLocation("aPos");
            var normalLocation = shader.GetAttribLocation("aNormal");
            var textureLocation = shader.GetAttribLocation("aTexCoords");

            GL.EnableVertexAttribArray(vertexLocation);
            GL.EnableVertexAttribArray(normalLocation);
            GL.EnableVertexAttribArray(textureLocation);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            //Vertices
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 9 * verticesSize, 0 * sizeof(float));

            //Normals
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 9 * verticesSize, 3 * sizeof(float));

            //TexCoords
            GL.VertexAttribPointer(textureLocation, 2, VertexAttribPointerType.Float, false, 9 * verticesSize, 6 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.DisableVertexAttribArray(vertexLocation);
            GL.DisableVertexAttribArray(normalLocation);
            GL.DisableVertexAttribArray(textureLocation);
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
    }
}
