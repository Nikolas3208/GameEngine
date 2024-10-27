using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Renders.Bufers
{
    public struct FrameBufferTextureSpecification
    {
        public int Count;

        public int TextureId;

        public TextureTarget TextureTarget;
        public PixelInternalFormat PixelInternalFormat;
        public SizedInternalFormat SizedInternalFormat;
        public PixelFormat PixelFormat;
        public PixelType PixelType;

        public FramebufferAttachment Attachment;
    }
    public struct FrameBufferSpecification
    {
        public int Width, Height;

        public FramebufferAttachment FrameBufferAttachment;

        public List<FrameBufferTextureSpecification> textureSpecifications; 
    }
    public class FrameBuffer
    {
        private FrameBufferSpecification frameBufferSpecification;

        private int FrameBufferIndex = -1;

        public FrameBuffer(FrameBufferSpecification frameBufferSpecification)
        {
            this.frameBufferSpecification = frameBufferSpecification;
        }

        public void CreateTextures(ref int id)
        {
            GL.CreateTextures(TextureTarget.Texture2D, 1, out id);
        }

        public void BindTexture(TextureTarget target, int id)
        {
            GL.BindTexture(target, id);
        }

        public void AttachColorTexture(PixelInternalFormat pixelInternalFormat, int width, int height, PixelFormat pixelFormat, PixelType pixelType, int id, int index)
        {
            GL.TexImage2D(TextureTarget.Texture2D, 0, pixelInternalFormat, width, height, 0, pixelFormat, pixelType, nint.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (uint)All.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (uint)All.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (uint)All.ClampToEdge);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0 + index, TextureTarget.Texture2D, id, 0);
        }

        public void AttachDepthTexture(TextureTarget2d target, SizedInternalFormat sizedInternalFormat, int width, int height, FramebufferAttachment framebufferAttachment, int id)
        {
            GL.TexStorage2D(TextureTarget2d.Texture2D, 1, sizedInternalFormat, width, height);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (uint)All.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (uint)All.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (uint)All.ClampToEdge);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, framebufferAttachment, TextureTarget.Texture2D, id, 0);
        }

        public void Init()
        {
            GL.CreateFramebuffers(1, out FrameBufferIndex);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameBufferIndex);

            var textureSpecifications = frameBufferSpecification.textureSpecifications;

            for (int i = 0; i < textureSpecifications.Count; i++)
            {
                var textureSpecification = textureSpecifications[i];

                CreateTextures(ref textureSpecification.TextureId);

                BindTexture(TextureTarget.Texture2D, textureSpecification.TextureId);

                if (textureSpecification.PixelInternalFormat != PixelInternalFormat.DepthComponent && textureSpecification.SizedInternalFormat != SizedInternalFormat.Depth24Stencil8)
                {
                    AttachColorTexture(textureSpecification.PixelInternalFormat, frameBufferSpecification.Width, frameBufferSpecification.Height, textureSpecification.PixelFormat, 
                        textureSpecification.PixelType, textureSpecification.TextureId, i);
                }
                else
                {
                    AttachDepthTexture(TextureTarget2d.Texture2D, textureSpecification.SizedInternalFormat, frameBufferSpecification.Width, frameBufferSpecification.Height, 
                        textureSpecification.Attachment, textureSpecification.TextureId);
                }

                textureSpecifications[i] = textureSpecification;
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameBufferIndex);
            GL.Viewport(0, 0, frameBufferSpecification.Width, frameBufferSpecification.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public Vector4 ReadPixel(int x, int y, int attachmentIndex)
        {
            Vector4 pixel = new Vector4(-1);

            Bind();
            GL.ReadBuffer(ReadBufferMode.ColorAttachment0 + attachmentIndex);

            GL.ReadPixels<Vector4>(x, y, frameBufferSpecification.Width, frameBufferSpecification.Height, frameBufferSpecification.textureSpecifications[attachmentIndex].PixelFormat, 
                frameBufferSpecification.textureSpecifications[attachmentIndex].PixelType, ref pixel);

            Unbind();

            return pixel;
        }

        public int GetTexture(int attachmentIndex)
        {
            if(attachmentIndex < frameBufferSpecification.textureSpecifications.Count)
            {
                return frameBufferSpecification.textureSpecifications[attachmentIndex].TextureId;
            }

            return -1;
        }
    }
}
