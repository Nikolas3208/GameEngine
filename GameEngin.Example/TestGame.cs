using GameEngine.Core;
using GameEngine.Core.Serializer;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameEngine.Example
{
    public class TestGame : Game
    {
        public TestGame(GameWindow window, Scene scene) : base(window, scene)
        {
        }

        public override void OnUpdate(FrameEventArgs e)
        {
            base.OnUpdate(e);
        }
    }
}
