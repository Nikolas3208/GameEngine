using GameEngine.LevelEditor;
using GameEngine.Core;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace GameEngin.LevelEditor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            EditorApplication editor = EditorApplication.CreateApplication(new NativeWindowSettings { Title = "Editor", Size = new Vector2i(1920, 1080) });
            editor.AddLayer(new EditorLayer());
            editor.Run();
        }
    }
}
