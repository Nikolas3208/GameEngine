using GameEngine.Core.Renders;
using GameEngine.Resources;
using GameEngine.Scens;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.LevelEditor
{
    public class Editor
    {
        private BaseWindow window;
        private BaseScen scen;

        public Editor() 
        {
            NativeWindowSettings windowSettings = new NativeWindowSettings();
            windowSettings.Size = new Vector2i(1920, 1080);

            window = new BaseWindow(GameWindowSettings.Default, windowSettings);

            scen = new EditorScen();
            window.AddScen(scen);

            using (window)
            {
                window.Run();
            }
        }

    }
}
