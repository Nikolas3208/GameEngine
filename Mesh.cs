using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using GameEngine.ResourceLoad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenTK.Graphics.OpenGL.GL;

namespace GameEngine
{
    public struct Vertex
    {
        public Vector3 Position = new Vector3();
        public Vector3 Normal = new Vector3(0, 0, 0);
        public Vector2 TexCoords = new Vector2();

        public Vertex(Vector3  pos)
        {
            Position = pos;
        }

        public Vertex(Vector3 pos,  Vector2 texCoord)
        {
            Position = pos;
            TexCoords = texCoord;
        }

        public Vertex(Vector3 pos, Vector3 normal)
        {
            Position = pos;
            Normal = normal;
        }

        public Vertex(Vector3 pos, Vector2 texCoord,  Vector3 normal)
        {
            Position = pos;
            TexCoords = texCoord;
            Normal = normal;
        }
    }

    public class Mesh
    {
        private int VAO;
        private int VBO;
        private int EBO;

        public Vertex[] Vertices;
        public uint[] indices;

        public Mesh(List<Vertex> vertices, List<uint> indices, Shader shader)
        {
            this.Vertices = vertices.ToArray();
            this.indices = indices.ToArray();

            InitEBO(shader);
        }

        public void Init(Shader shader)
        {
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vertex));

            int vboVerices = CreateVBO(Vertices);

            var vertexLocation = shader.GetAttribLocation("aPos");
            var normalLocation = shader.GetAttribLocation("aNormal");
            var textureLocation = shader.GetAttribLocation("aTexCoords");

            GL.EnableVertexAttribArray(vertexLocation);
            GL.EnableVertexAttribArray(normalLocation);
            GL.EnableVertexAttribArray(textureLocation);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboVerices);

            //Vertices
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, size, 0 * System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector3)));

            //Normals
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, size, 3 * System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector3)));

            //TexCoords
            GL.VertexAttribPointer(textureLocation, 2, VertexAttribPointerType.Float, false, size, 6 * System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector3)));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.DisableVertexAttribArray(vertexLocation);
            GL.DisableVertexAttribArray(normalLocation);
            GL.DisableVertexAttribArray(textureLocation);

            shader.Use();
        }

        public void InitEBO(Shader shader)
        {   
            VAO = GL.GenVertexArray();

            GL.BindVertexArray(VAO);

            int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vertex));

            VBO = CreateVBO(Vertices);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            EBO = CreateEBO(indices);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);

            //Vertices
            var vertexLocation = shader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, size, 0);

            //Normals
            var normalLocation = shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, size, 0 * System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector2)));

            //TexCoords
            var textureLocation = shader.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(textureLocation);
            GL.VertexAttribPointer(textureLocation, 3, VertexAttribPointerType.Float, false, size, 3 * System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector2)));

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.DisableVertexAttribArray(vertexLocation);
            GL.DisableVertexAttribArray(normalLocation);
            GL.DisableVertexAttribArray(textureLocation);

            shader.Use();
        }

        private int CreateVBO(Vector3[] date)
        {
            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, date.Length * System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector3)), date, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            return vbo;
        }
        private int CreateVBO(Vertex[] date)
        {
            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, date.Length * System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vertex)), date, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            return vbo;
        }
        private int CreateEBO(uint[] date)
        {
            int ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, date.Length * sizeof(uint), date, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            return ebo;
        }
        public void Draw(Shader shader, PrimitiveType mode)
        {
            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);

            GL.DrawElements(mode, indices.Length, DrawElementsType.UnsignedInt, sizeof(int) * 0);
        }
    }
}
