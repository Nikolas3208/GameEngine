using GameEngine.Windws;
using OpenTK.Windowing.Desktop;
using System.Configuration;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.LevelEditor;

namespace GameEngine.Example
{
    public class Game
    {
        private BaseWindow window;
        private MainScen mainScen;

        public Game(string name = "")
        {
            if(File.Exists("Assets\\Config\\" + name))
            {
                StreamReader reader = new StreamReader("Assets\\Config\\" + name);


            }
        }

        public void Run()
        {
            NativeWindowSettings windowSettings = new NativeWindowSettings();
            windowSettings.Size = new OpenTK.Mathematics.Vector2i(1920, 1080);

            window = new BaseWindow(GameWindowSettings.Default, windowSettings);
            window.AddScen(new MainScen());

            using (window)
            {
                window.Run();
            }
        }
    }
}
