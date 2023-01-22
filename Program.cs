using System;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;

namespace GameEngine
{
    class Program
    {
        public static void Main()
        {
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new OpenTK.Mathematics.Vector2i(1920, 1080)
            };

            using (Game game = new Game(GameWindowSettings.Default, nativeWindowSettings))
            {
                game.Run();
            }
        }
    }
}