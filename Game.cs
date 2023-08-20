using Assimp;
using GameEngine.ResourceLoad;
using GameEngine.Terrain;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Reflection.Emit;

namespace GameEngine
{
    public class Game : GameWindow
    {
        public List<Shader> shaders = new List<Shader>();

        private Camera camera;
        private Model ModelHouse;
        private Model ModelLight;
        private Player Player;

        private Vector3 lightPos = new Vector3(10, 30, 10);

        public BaseTerrain Terrain;

        public float Delta;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            VSync = VSyncMode.On;

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);

            Shader shader1 = new Shader("C:\\Users\\gaste\\source\\repos\\Nikolas3208\\GameEngine\\Shaders\\shader.vert", "C:\\Users\\gaste\\source\\repos\\Nikolas3208\\GameEngine\\Shaders\\shader.frag");

            shaders.Add(shader1);
            shaders.Add(shader1);

            ModelHouse = new Model("C:\\Users\\gaste\\source\\repos\\Nikolas3208\\GameEngine\\Content\\Models\\Cube.obj", shaders[0]);
            ModelLight = new Model("C:\\Users\\gaste\\source\\repos\\Nikolas3208\\GameEngine\\Content\\Models\\Cube.obj", shaders[0]);
            ModelLight.SetPosition(lightPos);

            //Terrain = new BaseTerrain("C:\\Users\\gaste\\source\\repos\\Nikolas3208\\GameEngine\\Content\\Textures\\TerrainTextures\\blendMap.png", shaders[1]);
            Terrain = TerrainHeightMap.InitTerrain(Content.HEIGHTMAP_PATH + "Heightmap.png");
            Terrain = TerrainNoisePerlin.InitTerrain(256);

            camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

            Player = new Player(camera);

            foreach (var shader in shaders)
                shader.Use();


            CursorState = CursorState.Grabbed;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            Title = ((int)(1 / args.Time)).ToString();

            Delta = (float)args.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);

            //ModelLight.SetPosition(Player.GetPositionPlayer());

            updateShader(shaders);

            float i = 0;

            lightPos.X += lightPos.X * 50 * (float)Math.Cos(i) + 0;
            lightPos.Y += 50 * (float)Math.Sin(i) + 20;

            i += 1;

            Terrain.RenderMesh(camera);
            //Player.Draw(shaders[2]);
            ModelHouse.Render(shaders[0], lightPos);
            ModelLight.Render(shaders[0], lightPos);

            SwapBuffers();
        }

        private void updateShader(List<Shader> shaders)
        {
            for (int i = 0; i < shaders.Count; i++)
            {
                shaders[i].Use();

                shaders[i].SetMatrix4("view", camera.GetViewMatrix());
                shaders[i].SetMatrix4("projection", camera.GetProjectionMatrix());
                
                if (shaders[i].GetAttribLocation("viewPos") != -1)
                    shaders[i].SetVector3("viewPos", camera.Position);
            }

        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (!IsFocused)
            {
                return;
            }

            var key = KeyboardState;

            if (key.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            Player.KeyUpdate(key);

            var mouse = MouseState;

            Player.MouseUpdate(mouse);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            camera.Fov -= e.OffsetY;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            camera.AspectRatio = Size.X / (float)Size.Y;
        }

    }
}
