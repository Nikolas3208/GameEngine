using GameEngine.Resources.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.LevelEditor
{
    public class BufferManager
    {
        public int mFbo;
        public int mTexId, mDepthId;
        public int Width, Height;

        public void Init(int width, int height)
        {
            Width = width; Height = height;

            mFbo = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, mFbo);
            GL.CreateTextures(TextureTarget.Texture2D, 1, out mTexId);
            GL.BindTexture(TextureTarget.Texture2D, mTexId);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (uint)All.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (uint)All.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (uint)All.ClampToEdge);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, mTexId, 0);

            GL.CreateTextures(TextureTarget.Texture2D, 1, out mDepthId);
            GL.BindTexture(TextureTarget.Texture2D, mDepthId);
            GL.TexStorage2D(TextureTarget2d.Texture2D, 1, SizedInternalFormat.Depth24Stencil8, width, height);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (uint)All.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (uint)All.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (uint)All.ClampToEdge);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2D, mDepthId, 0);

            DrawBuffersEnum[] bufers = { DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment0 };
            GL.DrawBuffers(mTexId, bufers);

            Unbind();
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, mFbo);
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
        public void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Unbind(int width, int height)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Viewport(0, 0, width, height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public Vector4 ReadPixel(int x, int y, Shader shader)
        {
            Vector4 id = new Vector4(-1);

            shader.Use();
            shader.SetInt("useFbo", 1);

            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, mFbo);

            GL.ReadBuffer(ReadBufferMode.ColorAttachment0);


            GL.ReadPixels(x, y, 1, 1, PixelFormat.Rgba, PixelType.Float, ref id);
            GL.ReadBuffer(ReadBufferMode.None);

            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, 0);

            return id;
        }
        public int GetTexture()
        {
            return mTexId;
        }
    }
}
