using GameEngine.Bufers;
using GameEngine.Core.Structs;
using GameEngine.Resources.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Resources.Meshes
{
    public abstract class BaseMesh
    {
        protected BaseBuffer buffers;

        protected Vertex[] vertices;
        protected uint[] indices;

        public Material material;

        public string MaterialName;
        public string Name;

        public abstract void Init(BaseShader shader, Vertex[] vertices);
        public abstract void Init(BaseShader shader, Vertex[] vertices, uint[] indices);
        public abstract void Init(BaseShader shader, Material material, Vertex[] vertices, uint[] indices);

        public virtual void Draw(PrimitiveType type, BaseShader shader)
        {
            if(shader != null)
            {
                shader.Use();

                if (material.DiffuseTexture != null)
                {
                    shader.SetInt("material.diffuse", material.Diffuse);
                    material.DiffuseTexture.Use(TextureUnit.Texture0 + material.Diffuse);
                }
                if (material.SpecularTexture != null)
                {
                    shader.SetInt("material.specular", material.Specular);
                    material.SpecularTexture.Use(TextureUnit.Texture0 + material.Specular);
                }

                shader.SetVector3("material.color", material.Color);
                shader.SetFloat("material.shininess", material.Shininess);
            }

            buffers.Draw(type);
        }
    }
}
