using GameEngine.Resources.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Renders.Bufers
{
    public class ShadowBuffer : BaseBuffer
    {
        private int depthId;
        public float dpthNear = -1.0f, depthFar = 6.5f, size = 10;
        public Matrix4 projection, view;
        public override void Init(int width, int height)
        {
            Width = width;
            Height = height;

            //projection = Matrix4.Identity;
            //view = Matrix4.Identity;

            GL.GenFramebuffers(1, out fbo);
            GL.GenTextures(1, out depthId);
            GL.BindTexture(TextureTarget.Texture2D, depthId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, width, height, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.ClampToBorder);

            float[] borderColor = { 1.0f, 1.0f, 1.0f, 1.0f };
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthId, 0);
            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void EnableWriting(Shader shader, Vector3 lightPos, Vector3 lightDir)
        {
            Bind();

            projection = Matrix4.CreateOrthographic(size, size, dpthNear, depthFar);
            //projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver2, Width / Height, dpthNear, depthFar);
            view = Matrix4.LookAt(-lightDir, -lightPos - lightDir , new Vector3(0, 1, 0));

            shader.Use();
            shader.SetMatrix4("lightview", view);
            shader.SetMatrix4("lightprojection", projection);
        }

        public int GetTexture() { return depthId; }

        public void EnableReading(Shader shader)
        {
            shader.Use();
            shader.SetMatrix4("lightview", view);
            shader.SetMatrix4("lightprojection", projection);
        }

        public void TextureUse(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, depthId);
        }

        public override void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.DepthBufferBit);
        }

        public override void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public override void Unbind(int width, int heigth)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Viewport(0, 0, width, heigth);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
    }
}
