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

        public int VBO;
        public int VAO;
        public int EBO;

        public List<Vector3> vertices;
        public List<Vector3> textureVertices;
        public List<Vector3> normals;
        public List<uint> vertexIndices;
        public List<uint> textureIndices;
        public List<uint> normalIndices;

        public Mesh(List<Vector3> vertices, List<Vector3> textureVertices, List<Vector3> normals,
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

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexIndices.Count * sizeof(float), vertices.ToArray(), BufferUsageHint.StaticDraw);

            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, vertexIndices.Count * sizeof(float), vertexIndices.ToArray(), BufferUsageHint.StaticDraw);

            GL.BindVertexArray(VAO);
            {
                var vertexLocation = shader.GetAttribLocation("aPos");
                GL.EnableVertexAttribArray(vertexLocation);
                GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

                var normalLocation = shader.GetAttribLocation("aNormal");
                GL.EnableVertexAttribArray(normalLocation);
                GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

                var texturelLocation = shader.GetAttribLocation("aTexCoords");
                GL.EnableVertexAttribArray(texturelLocation);
                GL.VertexAttribPointer(texturelLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            }
        }

        public void Draw(Shader shader, Camera camera)
        {
            GL.BindVertexArray(VAO);

            texture.Use(TextureUnit.Texture0);

            shader.Use();

            shader.SetMatrix4("model", Matrix4.Identity);
            shader.SetMatrix4("view", camera.GetViewMatrix());
            shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            GL.DrawElements(BeginMode.Triangles, vertexIndices.Count, DrawElementsType.UnsignedInt, 0);
        }
    }
}
