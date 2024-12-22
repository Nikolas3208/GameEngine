using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core
{
    public abstract class Layer
    {
        protected string Name;

        public string GetName() => Name;

        public abstract void Init(GameWindow window);
        public abstract void Update(float deltaTime);
        public abstract void ImGuiDraw(GameWindow window, float deltaTime);
        public abstract void Draw();

        public abstract void OnResize(ResizeEventArgs e);
        public abstract void OnTextInput(TextInputEventArgs e);
        public abstract void OnMouseWheel(MouseWheelEventArgs e);
    }
}
