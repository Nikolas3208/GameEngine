using GameEngine.Core.Renders.Bufers;
using GameEngine.Core.Structs;
using GameEngine.Resources.Shaders;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Renders
{
    public class Mesh
    {
        private BaseBuffer buffer;
        private Material material;

        public Mesh() { }

        public Mesh(BaseBuffer buffer, Material material)
        {
            this.buffer = buffer;
            this.material = material;
        }

        public Mesh(Shader shader, Vertex[] vertices, Material material)
        {
            this.material = material;

            buffer = new VAOBuffer();
            buffer.Init(shader, vertices);
        }

        public Mesh(Shader shader, Vertex[] vertices, Material material, uint[] indices)
        {
            this.material = material;

            buffer = new EBOBuffer();
            buffer.Init(shader, vertices, indices);
        }

        public Mesh(Shader shader, Vertex[] vertices, uint[] indices)
        {
            buffer = new EBOBuffer();
            buffer.Init(shader, vertices, indices);
        }

        public Mesh(Shader shader, float[] vertices, Material material)
        {
            this.material = material;

            buffer = new VAOBuffer();
            buffer.Init(shader, vertices);
        }

        public Mesh(Shader shader, float[] vertices, Material material, uint[] indices)
        {
            this.material = material;

            buffer = new EBOBuffer();
            buffer.Init(shader, vertices, indices);
        }

        public Mesh(Shader shader, float[] vertices, uint[] indices)
        {
            buffer = new EBOBuffer();
            buffer.Init(shader, vertices, indices);
        }

        public Material GetMaterial() => material;
        public void SetMaterial(Material material) => this.material = material;

        public void Draw(PrimitiveType type)
        {
            if (buffer != null)
                buffer.Draw(type);
        }

        public void Draw(PrimitiveType type, Shader shader)
        {
            if(shader != null && buffer != null)
            {
                shader.Use();

                if (material.textures != null)
                {
                    for (int i = 0; i < material.textures.Count; i++)
                    {
                        material.textures[i].Use(TextureUnit.Texture0 + i);
                    }
                }

                shader.SetVector3("material.color", material.Color);
                shader.SetFloat("material.shininess", material.Shininess);
            }

            if (buffer != null)
                buffer.Draw(type);
        }
    }
}
