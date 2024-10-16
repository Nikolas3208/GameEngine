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
    public class DefaulMaterial : BaseMaterial
    {
        public DefaulMaterial()
        {
            textures = new Dictionary<TextureType, BaseTexture>();
        }

        public override void Draw(BaseShader shader)
        {
            shader.Use();

            shader.SetInt("material.diffuse", 0);
            shader.SetInt("material.specular", 1);
            shader.SetInt("material.normal", 2);
            shader.SetInt("shadowMap", 3);
            shader.SetInt("material.depthMap", 4);

            shader.SetFloat("material.shininess", shininess);
            shader.SetFloat("material.textureScale", textureScale);

            shader.SetVector3("material.specular", new Vector3(specular));
            shader.SetVector3("material.color", new Vector3(color.X, color.Y, color.Z));

            shader.SetVector3("light.ambient", new Vector3(ambient));
            shader.SetVector3("light.diffuse", new Vector3(diffuse));
            shader.SetVector3("light.specular", new Vector3(lightSpecular));

            foreach (var type in textures)
            {
                type.Value.Use(OpenTK.Graphics.OpenGL4.TextureUnit.Texture0 + (int)type.Key);
            }
        }
    }
}
