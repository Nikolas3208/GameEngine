using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Runtime.InteropServices;
using GameEngine.Graphics.Bufers;

namespace GameEngine.Graphics
{
    public class VertexArray
    {
        /// <summary>
        /// Буфер даннх
        /// </summary>
        private VertexBuffer _vertexBuffer;

        /// <summary>
        /// Буфер индексов
        /// </summary>
        private IndexBuffer? _indexBuffer;

        /// <summary>
        /// Позиция данных вершин в шейдере
        /// </summary>
        private int vertexLocation = 0;

        /// <summary>
        /// Позиция данных нормали в шейдер
        /// </summary>
        private int normalLocation = 1;

        /// <summary>
        /// Позиция данных текстурных координат в шейдер
        /// </summary>
        private int textureLocation = 2;

        /// <summary>
        /// Идентификатор массива данных
        /// </summary>
        public int HandleVAO { get; private set; }

        public VertexArray(VertexBuffer vertexBuffer)
        {
            _vertexBuffer = vertexBuffer;
        }

        public VertexArray(VertexBuffer vertexBuffer, IndexBuffer indexBuffer)
        {
            _vertexBuffer = vertexBuffer;
            _indexBuffer = indexBuffer;
        }
        public VertexArray(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, Shader shader)
        {
            _vertexBuffer = vertexBuffer;
            _indexBuffer = indexBuffer;

            Init(shader);
        }

        public void Init(Shader shader)
        {
            int size = Marshal.SizeOf(typeof(Vertex));
            int v3size = Marshal.SizeOf(typeof(Vector3));
            int v2size = Marshal.SizeOf(typeof(Vector2));

            HandleVAO = GL.GenVertexArray();
            GL.BindVertexArray(HandleVAO);

            _vertexBuffer!.Bind();
            _indexBuffer!.Bind();

            if (shader != null)
            {
                vertexLocation = shader.GetAttribLocation("aPos");
                normalLocation = shader.GetAttribLocation("aNormal");
                textureLocation = shader.GetAttribLocation("aTexCoords");
            }

            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, size, 0);

            if (normalLocation >= 0)
            {
                GL.EnableVertexAttribArray(normalLocation);
                GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, size, v3size);
            }

            if (textureLocation >= 0)
            {
                GL.EnableVertexAttribArray(textureLocation);
                GL.VertexAttribPointer(textureLocation, 2, VertexAttribPointerType.Float, false, size, 3 * v2size);
            }

            _indexBuffer!.Unbind();
            _vertexBuffer!.Unbind();
            GL.BindVertexArray(0);

            GL.DisableVertexAttribArray(vertexLocation);
            GL.DisableVertexAttribArray(normalLocation);
            GL.DisableVertexAttribArray(textureLocation);

            if (shader != null)
                shader.Use();
        }

        public void BindEBO()
        {
            GL.BindVertexArray(HandleVAO);
            _indexBuffer!.Bind();
        }

        public void UnbindEBO()
        {
            _indexBuffer!.Unbind();
            GL.BindVertexArray(0);

        }

        public void BindVBO()
        {
            GL.BindVertexArray(HandleVAO);
            _vertexBuffer.Bind();
        }

        public void UnbindVBO()
        {
            _vertexBuffer.Unbind();
            GL.BindVertexArray(0);
        }

        public void DrawElements(PrimitiveType type)
        {
            BindEBO();

            GL.DrawElements(type, _indexBuffer!.Count, DrawElementsType.UnsignedInt, 0);

            UnbindEBO();
        }

        public void DrawArrays(PrimitiveType type)
        {
            BindVBO();

            GL.DrawArrays(type, 0, _vertexBuffer.Count);

            UnbindVBO();
        }

        public VertexBuffer GetVertexBuffer()
        {
            return _vertexBuffer;
        }
    }
}
