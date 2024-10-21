using Assimp;
using GameEngine.Scens;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Renders
{
    public class BaseWindow : GameWindow
    {
        private BaseScen scen;
        public BaseWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        public void AddScen(BaseScen scen) => this.scen = scen;
        public void RemoveScen() => scen = null;
        public BaseScen GetScen() => scen;

        protected override void OnLoad()
        {
            base.OnLoad();

            if (scen != null)
            {
                scen.Start(this);
            }

        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (scen != null)
            {
                scen.Update(this, (float)args.Time);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            if (scen != null)
            {
                scen.Render(this, (float)args.Time);
            }

            SwapBuffers();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        }
    }
}
