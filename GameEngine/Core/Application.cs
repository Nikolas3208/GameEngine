using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core
{
    public class Application
    {
        private GameWindow window;
        private Layer layer;

        public bool IsOpen = false;

        protected Application(NativeWindowSettings windowSettings)
        {
            window = new GameWindow(GameWindowSettings.Default, windowSettings);

            window.Resize += Window_Resize;
            window.Load += Window_Load;
            window.UpdateFrame += Window_UpdateFrame;
            window.RenderFrame += Window_RenderFrame;
        }

        public GameWindow GetWindow() => window;

        public static Application CreateApplication(NativeWindowSettings windowSettings)
        {
            return new Application(windowSettings);
        }


        public void AddLayer(Layer layer) => this.layer = layer;

        public void Run()
        {
            IsOpen = true;
            using(window)
            {
                window.Run();
            }
        }

        public void Cloae()
        {
            IsOpen = false;
            window.Close();
        }

        private void Window_Load()
        {
            layer.Init(window);
        }

        private void Window_UpdateFrame(OpenTK.Windowing.Common.FrameEventArgs obj)
        {
            layer.Update((float)obj.Time);
        }

        private void Window_RenderFrame(OpenTK.Windowing.Common.FrameEventArgs obj)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            layer.Draw();
            layer.ImGuiDraw(window, (float)obj.Time);

            window.SwapBuffers();
        }

        private void Window_Resize(OpenTK.Windowing.Common.ResizeEventArgs obj)
        {
            GL.Viewport(0, 0, obj.Width, obj.Height);

            layer.Resize(window.ClientSize.X, window.ClientSize.Y);
        }
    }
}
