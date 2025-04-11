using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.ComponentModel;

namespace GameEngine.Core
{
    public abstract class Game
    {
        protected GameWindow gameWindow;
        protected Scene scene;
        public Game(GameWindow window, Scene scene)
        {
            gameWindow = window;
            this.scene = scene;
            this.scene.SetGame(this);

            gameWindow.Load += OnLoad;
            gameWindow.UpdateFrame += OnUpdate;
            gameWindow.RenderFrame += OnRender;

            gameWindow.Resize += OnResize;
            gameWindow.Closing += OnClose;

            gameWindow.MouseWheel += OnMouseWheel;
            gameWindow.TextInput += OnTextInput;
        }


        public void Run()
        {
            using(gameWindow)
            {
                gameWindow.Run();
            }
        }

        public MouseState GetMouseState()
        {
            return gameWindow.MouseState;
        }

        public KeyboardState GetKeyboardState()
        {
            return gameWindow.KeyboardState;
        }

        public virtual void OnLoad()
        {
            scene.Statr();
        }

        public virtual void OnUpdate(FrameEventArgs e)
        {
            scene.Update((float)e.Time);
        }

        public virtual void OnRender(FrameEventArgs e)
        {
            scene.Draw((float)e.Time);

            gameWindow.SwapBuffers();
        }

        public virtual void OnTextInput(TextInputEventArgs e)
        {
            scene.OnTextInput(e);
        }

        public virtual void OnMouseWheel(MouseWheelEventArgs e)
        {
            scene.OnMouseWheel(e);
        }

        public virtual void OnResize(ResizeEventArgs e)
        {
            scene.OnResize(e);
        }

        public virtual void OnClose(CancelEventArgs e)
        {
            scene.OnClose(e);
        }

        public GameWindow GetWindow()
        {
            return gameWindow;
        }
    }
}
