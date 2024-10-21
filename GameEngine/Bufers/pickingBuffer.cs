using GameEngine.Core.Structs;
using GameEngine.Resources.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Bufers
{
    public class pickingBuffer : BaseBuffer
    {
        private int pickingTex, depthTex;
        public int Width, Height;

        public override void Init(int width, int height)
        {
            Width = width; Height = height;

            fbo = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);

            GL.GenTextures(1, out pickingTex);
            GL.BindTexture(TextureTarget.Texture2D, pickingTex);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb32f, width, height, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (uint)All.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (uint)All.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (uint)All.ClampToEdge);


            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, pickingTex, 0);

            GL.CreateTextures(TextureTarget.Texture2D, 1, out depthTex);
            GL.BindTexture(TextureTarget.Texture2D, depthTex);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, width, height, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthTex, 0);
            
            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

            //DrawBuffersEnum[] draws = { DrawBuffersEnum.ColorAttachment0 };
            //GL.DrawBuffers(pickingTex, draws);

            Unbind();
        }

        public override void Init(BaseShader shader, Vertex[] vertices)
        {
            
        }

        public override void Init(BaseShader shader, Vertex[] vertices, uint[] indices)
        {
            
        }

        public override void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo); 
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public override void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public override void Unbind(int width, int height)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Viewport(0, 0, width, height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public int GetTexture()
        {
            return pickingTex;
        }

        public Vector4 ReadPixel(int x, int y, BaseShader shader)
        {
            Vector4 id = new Vector4(-1);
            shader.Use();


            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, fbo);

            GL.ReadBuffer(ReadBufferMode.ColorAttachment0);


            GL.ReadPixels(x, y, 1, 1, PixelFormat.Rgba, PixelType.Float, ref id);
            GL.ReadBuffer(ReadBufferMode.None);

            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, 0);

            return id;
        }

        public override void Draw(PrimitiveType type)
        {
            throw new NotImplementedException();
        }
    }
}
