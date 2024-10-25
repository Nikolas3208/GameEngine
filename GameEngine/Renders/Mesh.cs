using GameEngine.Core;
using GameEngine.Core.Structs;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Renders
{
    public class Mesh
    {
        private VertexArray? vertexArray;
        private Material material;

        public int Id = 0;
        public string Name = "Mesh";

        public Mesh() { }

        public Mesh(VertexArray vertexArray)
        {
            this.vertexArray = vertexArray;
        }

        public Mesh(VertexArray vertexArray, Material material)
        {
            this.vertexArray = vertexArray;
            this.material = material;
        }

        public Material GetMaterial() => material;
        public void SetMaterial(Material material) => this.material = material;

        public void Draw(PrimitiveType type)
        {
            vertexArray!.Draw(type);
        }

        public void Draw(PrimitiveType type, Shader shader)
        {
            if (shader != null && vertexArray != null)
            {
                shader.Use();

                if (material.textures != null && material.textures.Count > 0)
                {
                    shader.SetInt("material.useTexture", 1);
                    for (int i = 0; i < material.textures.Count; i++)
                    {
                        material.textures[i].Use(TextureUnit.Texture0 + i);
                    }
                }
                else
                {
                    shader.SetInt("material.useTexture", 0);
                }

                shader.SetVector3("material.color", material.Color);
                shader.SetFloat("material.shininess", material.Shininess);
            }

            vertexArray!.Draw(type);
        }
    }
}
