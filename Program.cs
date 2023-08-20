using System;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;

namespace GameEngine
{
    class Program
    {
        public static Game Game { get; private set; }

        public static void Main()
        {
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new OpenTK.Mathematics.Vector2i(1920, 1080)
            };

            using (Game = new Game(GameWindowSettings.Default, nativeWindowSettings))
            {
                Game.Run();
            }
        }
    }
}