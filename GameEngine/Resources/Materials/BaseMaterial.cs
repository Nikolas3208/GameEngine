using Assimp.Unmanaged;
using GameEngine.Resources.Shaders;
using GameEngine.Resources.Textures;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Resources.Materials
{
    public enum TextureType
    {
        Diffuse = 0,
        Specular = 1,
        Normal = 2,
        ShadowMap = 3,
        DepthMap = 4
    }
    public abstract class BaseMaterial
    {
        protected Dictionary<TextureType, BaseTexture> textures;

        public string Name = "Material";

        public Vector3 color = new Vector3(1);

        public float specular = 0.1f;
        public float shininess = 5.0f;

        public float ambient = 0.2f;
        public float diffuse = 0.5f;
        public float lightSpecular = 0.1f;
        public float textureScale = 1;

        public abstract void Draw(BaseShader shader);

        public void AddTexture(TextureType type, BaseTexture texture)
        {
            if (!textures.ContainsKey(type))
                textures.Add(type, texture);
            else
                textures[type] = texture;
        }
        public BaseTexture GetTexture(TextureType type) => textures[type];
        public Dictionary<TextureType, BaseTexture> GetTextures()
        {
            return textures;
        }
        public void RemoveTexture(TextureType type) => textures.Remove(type);
    }
}
