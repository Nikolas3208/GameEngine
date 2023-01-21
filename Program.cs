using System;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;

namespace GameEngine
{
    class Program
    {
        public static void Main()
        {
            using (Game game = new Game(GameWindowSettings.Default, NativeWindowSettings.Default))
            {
                game.Run();
            }
        }
    }
}