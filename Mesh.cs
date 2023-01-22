using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using GameEngine.ResourceLoad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{

    public class Mesh
    {
        private Shader shader;
        private Texture texture;

        private int VAO;

        public List<float> vertices;
        public List<float> textureVertices;
        public List<float> normals;
        public List<uint> vertexIndices;
        public List<uint> textureIndices;
        public List<uint> normalIndices;

        public Mesh(List<float> vertices, List<float> textureVertices, List<float> normals,
                    List<uint> vertexIndices, List<uint> textureIndices, List<uint> normalIndices)
        {
            this.vertices = vertices;
            this.textureVertices = textureVertices;
            this.normals = normals;
            this.vertexIndices = vertexIndices;
            this.textureIndices = textureIndices;
            this.normalIndices = normalIndices;
        }

        public void Init(Shader shader, Texture texture)
        {
            this.shader = shader;
            this.texture = texture;

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            int vboVerices = CreateVBO(vertices.ToArray());
            int vboTexture = CreateVBO(textureVertices.ToArray());

            int ebo = CreateEBO(vertexIndices.ToArray());

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboVerices);

            var vertexLocation = shader.GetAttribLocation("aPos");
            var normalLocation = shader.GetAttribLocation("aNormal");
            var texturelLocation = shader.GetAttribLocation("aTexCoord");

            GL.EnableVertexAttribArray(vertexLocation);
            GL.EnableVertexAttribArray(normalLocation);
            GL.EnableVertexAttribArray(texturelLocation);

            //Vertices
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            //Normals
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            //Texture
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboTexture);
            GL.VertexAttribPointer(texturelLocation, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0 * sizeof(float));

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.DisableVertexAttribArray(vertexLocation);
            GL.DisableVertexAttribArray(normalLocation);
            GL.DisableVertexAttribArray(texturelLocation);
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

        public void Draw(Camera camera)
        {
            GL.BindVertexArray(VAO);

            texture.Use(TextureUnit.Texture0);

            shader.Use();

            shader.SetMatrix4("model", Matrix4.Identity);
            shader.SetMatrix4("view", camera.GetViewMatrix());
            shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            GL.DrawElements(PrimitiveType.Triangles, vertexIndices.Count, DrawElementsType.UnsignedInt, 0);
        }
    }
}
