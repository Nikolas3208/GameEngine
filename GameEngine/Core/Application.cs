using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;

namespace GameEngine.Core
{
    public class Application
    {
        public Game Game { get; private set; }

        public Application(Game game)
        {
            Game = game;
        }

        public void Run()
        {
            Game.Run();
        }
    }
}
