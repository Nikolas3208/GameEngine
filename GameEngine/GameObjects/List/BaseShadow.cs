using GameEngine.Resources.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameObjects.List
{
    public abstract class BaseShadow
    {
        protected int fbo, textureHandle;
        protected int Width, Height;

        public virtual void BindForWriting()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
            GL.Viewport(0, 0, Width, Height);

            GL.Clear(ClearBufferMask.DepthBufferBit);
        }

        public virtual void BindForReading(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);
        }

        public abstract void Init(int width, int height);

        public abstract void UseDepthShader(Shader shader, Vector3 lightDir);

        public abstract void UseShader(Shader shader, TextureUnit unit = TextureUnit.Texture3);
    }
}
