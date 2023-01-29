using GameEngine.ResourceLoad;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Model
    {
        private string name;

        private Vector3 position;
        private Vector3 rotation;
        private float scale;

        private Texture txDeff;
        private Texture txSpec;
        private Mesh mesh;

        public Model(string name, bool ebo, Vector3 position, Vector3 rotation, float scale, Texture txDeff, Texture txSpec, Shader shader, Mesh mesh)
        {
            this.name = name;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.txDeff = txDeff;
            this.txSpec = txSpec;
            this.mesh = mesh;

            if (!ebo)
                mesh.Init(shader);
            else
                mesh.InitEBO(shader);
        }

        public void UpdateShader(Shader shader)
        {
            // Here we specify to the shaders what textures they should refer to when we want to get the positions.
            shader.SetInt("material.diffuse", 0);
            shader.SetInt("material.specular", 1);
            shader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            shader.SetFloat("material.shininess", 32.0f);

            shader.SetVector3("light.position", new Vector3(1.2f, 1.0f, 2.0f));
            shader.SetVector3("light.ambient", new Vector3(0.2f));
            shader.SetVector3("light.diffuse", new Vector3(0.5f));
            shader.SetVector3("light.specular", new Vector3(1.0f));
        }

        private void UniformUpdate(Shader shader)
        {
            Matrix4 matrix = Matrix4.Identity;
            matrix = matrix * Matrix4.CreateTranslation(position);
            matrix = matrix * Matrix4.CreateScale(scale);
            matrix = matrix * Matrix4.CreateRotationX(rotation.X);
            matrix = matrix * Matrix4.CreateRotationY(rotation.Y);
            matrix = matrix * Matrix4.CreateRotationZ(rotation.Z);

            shader.SetMatrix4("model", matrix);
        }

        public void RenderModel(Shader shader)
        {
            UniformUpdate(shader);
            UpdateShader(shader);

            GL.BindVertexArray(mesh.VAO);

            txDeff.Use(TextureUnit.Texture0);
            txSpec.Use(TextureUnit.Texture1);

            mesh.Render();
        }
    }
}
