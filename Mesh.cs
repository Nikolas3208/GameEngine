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

    public class Mesh
    {
        public int VAO;
        public int ebo;
        public int eboTexture;

        public float[] vertices;
        public float[] textureVertices;
        public float[] normals;
        public uint[] vertexIndices;
        public uint[] textureIndices;
        public uint[] normalIndices;

        public Mesh(List<float> vertices, List<float> textureVertices, List<float> normals,
                    List<uint> vertexIndices, List<uint> textureIndices, List<uint> normalIndices)
        {
            this.vertices = vertices.ToArray();
            this.textureVertices = textureVertices.ToArray();
            this.normals = normals.ToArray();
            this.vertexIndices = vertexIndices.ToArray();
            this.textureIndices = textureIndices.ToArray();
            this.normalIndices = normalIndices.ToArray();
        }

        public Mesh(float[] vertices)
        {
            this.vertices = vertices;
        }

        public void Init(Shader shader)
        {
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            int vboVerices = CreateVBO(vertices);

            var vertexLocation = shader.GetAttribLocation("aPos");
            var normalLocation = shader.GetAttribLocation("aNormal");
            var textureLocation = shader.GetAttribLocation("aTexCoords");

            GL.EnableVertexAttribArray(vertexLocation);
            GL.EnableVertexAttribArray(normalLocation);
            GL.EnableVertexAttribArray(textureLocation);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboVerices);

            //Vertices
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            //Normals
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            //TexCoords
            GL.VertexAttribPointer(textureLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.DisableVertexAttribArray(vertexLocation);
            GL.DisableVertexAttribArray(normalLocation);
            GL.DisableVertexAttribArray(textureLocation);

            shader.Use();
        }

        public void InitEBO(Shader shader)
        {   
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            int vboVerices = CreateVBO(vertices);
            int vboTexture = CreateVBO(textureVertices);
            int vboNormal = CreateVBO(normals);

            ebo = CreateEBO(vertexIndices.ToArray());
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            var vertexLocation = shader.GetAttribLocation("aPos");
            var normalLocation = shader.GetAttribLocation("aNormal");
            var textureLocation = shader.GetAttribLocation("aTexCoords");

            GL.EnableVertexAttribArray(vertexLocation);
            GL.EnableVertexAttribArray(normalLocation);
            GL.EnableVertexAttribArray(textureLocation);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboVerices);

            //Vertices
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboNormal);

            //Normals
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboTexture);
            
            //TexCoords
            GL.VertexAttribPointer(textureLocation, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0 * sizeof(float));

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.DisableVertexAttribArray(vertexLocation);
            GL.DisableVertexAttribArray(normalLocation);
            GL.DisableVertexAttribArray(textureLocation);

            shader.Use();
        }

        private int CreateVBO(float[] date)
        {
            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, date.Length * sizeof(float), date, BufferUsageHint.StaticDraw);
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

        public void Render()
        {
            if (vertexIndices == null)
                GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
            else
                GL.DrawElements(PrimitiveType.Triangles, vertexIndices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}
