using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace GameEngine.Example
{
    public class Program
    {
        private static TestGame Game { get; set; }
        public static void Main(string[] args)
        {
            var gameWindowSetings = GameWindowSettings.Default;

            Game = new TestGame(new GameWindow(gameWindowSetings,
                new NativeWindowSettings() { ClientSize = new Vector2i(1920, 1080),
                    Vsync = VSyncMode.On,
                    Title = "Game Engine" }), new TestScene());
            Game.Run();
        }
    }
}
