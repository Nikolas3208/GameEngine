using GameEngine.Core;
using GameEngine.Resources;
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
    public class EditorApplication : Application
    {
        protected EditorApplication(NativeWindowSettings windowSettings) : base(windowSettings)
        {
        }

        public static new EditorApplication CreateApplication(NativeWindowSettings windowSettings)
        {
            return new EditorApplication(windowSettings);
        }
    }
}
