using GameEngine.Core.Essentials;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameEngine.Renders
{
    public class Mesh
    {
        [JsonInclude]
        private VertexArray vertexArray;

        [JsonInclude]
        private Material material;

        [JsonIgnore]
        public int Id = 0;
        [JsonIgnore]
        public string Name = "Mesh";

        public Mesh() { }

        public Mesh(string name)
        {
            Name = name;
        }

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
            if (vertexArray != null)
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
                        GL.ActiveTexture(TextureUnit.Texture0 + i);
                        shader.SetInt($"material.{material.textures[i].Type}", i);
                        GL.BindTexture(TextureTarget.Texture2D, material.textures[i].Id);
                    }

                    GL.ActiveTexture(TextureUnit.Texture0);
                }
                else
                {
                    shader.SetInt("material.useTexture", 0);
                }

                shader.SetVector3("material.color", material.Color);
                shader.SetFloat("material.shininess", material.Shininess);
            }
            if (vertexArray != null)
                vertexArray!.Draw(type);
        }

        public void InitBuffers()
        {
            vertexArray.Init(null);
        }
    }
}
