using OpenTK.Graphics.OpenGL4;

namespace GameEngine.Graphics.Bufers
{
    public class IndexBuffer
    {
        /// <summary>
        /// Массив индексов
        /// </summary>
        private uint[] indices;

        /// <summary>
        /// Идентефикатор буфера
        /// </summary>
        public int HandleIBO;

        /// <summary>
        /// Колличество индексов
        /// </summary>
        public int Count;

        /// <summary>
        /// Буфер индексов
        /// </summary>
        /// <param name="indices"> Масив индексов </param>
        public IndexBuffer(uint[] indices)
        {
            this.indices = indices;
            Count = indices.Length;

            Init();
        }

        /// <summary>
        /// Инициализация буфера
        /// </summary>
        public void Init()
        {
            HandleIBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, HandleIBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        /// <summary>
        /// Активация буфера
        /// </summary>
        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, HandleIBO);
        }

        /// <summary>
        /// Диактивация буфера
        /// </summary>
        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
    }
}
