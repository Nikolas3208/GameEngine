using GameEngine.Resources.Meshes;
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

namespace GameEngine.Bufers
{
    public class VAOBuffer : BaseBuffer
    {
        public override void Init(BaseShader shader, Vertex[] vertices)
        {
            count = vertices.Length;

            vao = InitVAOBuffer();

            sizeVertex = Marshal.SizeOf(typeof(Vertex));

            vbo = CreateVBOBuffer(vertices);

            InitVertices(shader, vbo);

            shader.Use();
        }

        private void InitVertices(BaseShader shader, int vbo)
        {
            var vertexLocation = shader.GetAttribLocation("aPos");
            var normalLocation = shader.GetAttribLocation("aNormal");
            var textureLocation = shader.GetAttribLocation("aTexCoords");

            GL.EnableVertexAttribArray(vertexLocation);
            GL.EnableVertexAttribArray(normalLocation);
            GL.EnableVertexAttribArray(textureLocation);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            //Vertices
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, sizeVertex, 0 * Marshal.SizeOf(typeof(Vector3)));

            //Normals
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, sizeVertex, 3 * Marshal.SizeOf(typeof(Vector3)));

            //TexCoords
            GL.VertexAttribPointer(textureLocation, 2, VertexAttribPointerType.Float, false, sizeVertex, 6 * Marshal.SizeOf(typeof(Vector3)));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.DisableVertexAttribArray(vertexLocation);
            GL.DisableVertexAttribArray(normalLocation);
            GL.DisableVertexAttribArray(textureLocation);
        }

        public void Init(BaseShader shader, Vector3[] vertices)
        {
            vao = InitVAOBuffer();
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

        public override void Init(BaseShader shader, Vertex[] vertices, uint[] indices)
        {
            
        }
    }
}
