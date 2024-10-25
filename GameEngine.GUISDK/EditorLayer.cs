using GameEngine.Core;
using GameEngine.LevelEditor.Interface;
using GameEngine.Scenes;
using GameEngine.Widgets;
using ImGuiNET;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.LevelEditor
{
    public class EditorLayer : Layer
    {
        private EditorInterface editorInterface;
        private Scene scene;

        public override void Init(GameWindow window)
        {
            scene = new Scene();

            editorInterface = new EditorInterface(window.ClientSize.X, window.ClientSize.Y, scene);
        }

        public override void Update(float deltaTime)
        {
            
        }

        public override void Draw()
        {
            
        }

        public override void ImGuiDraw(GameWindow window, float deltaTime)
        {
            editorInterface.Update(window, deltaTime);

            ImGui.DockSpaceOverViewport();

            editorInterface.Draw();
        }

    }
}
