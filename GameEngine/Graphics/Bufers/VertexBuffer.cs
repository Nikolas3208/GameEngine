using GameEngine.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace GameEngine.Graphics.Bufers
{
    public class VertexBuffer
    {
        /// <summary>
        /// Массив данных модели
        /// </summary>
        private Vertex[] _vertices;

        /// <summary>
        /// Идентефикатор буфера данных
        /// </summary>
        public int HandleVBO {  get; private set; }

        /// <summary>
        /// Количество елментов массива данных
        /// </summary>
        public int Count => _vertices.Length;

        /// <summary>
        /// Буфер данных
        /// </summary>
        /// <param name="vertices"> Данные модели </param>
        public VertexBuffer(Vertex[] vertices)
        {
            _vertices = vertices;

            Init();
        }

        /// <summary>
        /// Инициализация буфера
        /// </summary>
        public void Init()
        {
            HandleVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, HandleVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * Marshal.SizeOf(typeof(Vertex)), _vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        /// <summary>
        /// Активация буфера
        /// </summary>
        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, HandleVBO);
        }

        /// <summary>
        /// Диактивация буфера
        /// </summary>
        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }
}
