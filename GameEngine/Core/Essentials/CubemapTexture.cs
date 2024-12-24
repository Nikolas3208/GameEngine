using GameEngine.Core.Resources;
using GameEngine.Core.Structs;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Essentials
{
    public class CubemapTexture : TextureLoader
    {
        private static int textureId;
        public static CubemapTexture LoadFromFile(string[] paths)
        {
            GL.GenTextures(1, out textureId);
            GL.BindTexture(TextureTarget.TextureCubeMap, textureId);


            for (int i = 0; i < paths.Length; i++)
            {
                using (Stream stream = File.OpenRead(paths[i]))
                {
                    StbImage.stbi_set_flip_vertically_on_load(0);
                    ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgb, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
                }
            }

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);

            return new CubemapTexture(textureId);
        }

        public CubemapTexture(int handle) : base(handle)
        {
        }

        public new void Use(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.TextureCubeMap, Handle);
        }
    }
}
