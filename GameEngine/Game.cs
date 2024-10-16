using GameEngine.Models;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

//using SFML.Graphics;
using System;
using System.Diagnostics;
using System.Reflection;

namespace GameEngine
{
    public class Game : GameWindow
    {
        //private ImGuiController controller;

        //private List<Model> models;


        //private Camera camera;
        //private Model ModelHouse;
       // private Model ModelHouse2;
        //private Model ModelLight;

        //private Vector3 lightPos = new Vector3(10, 30, 10);
       // Shader shader1;
       // public float Delta;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        /*protected override void OnLoad()
        {
            base.OnLoad();

            //VSync = VSyncMode.On;

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            models = new List<Model>();

            controller = new ImGuiController(ClientSize.X, ClientSize.Y);

            shader1 = new Shader("..\\..\\..\\Shaders\\shader.vert", "..\\..\\..\\Shaders\\shader.frag", geomPath: "..\\..\\..\\Shaders\\shader.geom");
            shader1.Use();
            shader2 = new Shader("..\\..\\..\\Shaders\\depth.vert", "..\\..\\..\\Shaders\\depth.frag");
            shader2.Use();


            light = Lights.Light.DirectLight(new Vector3());

            Shadow = new Shadow();
            Shadow.Init(4096, 4096);

            ModelHouse = new Model("..\\..\\..\\Content\\Models\\Cube.obj", shader1);
            ModelHouse.Scale = 3f;

            ModelHouse.Position = new Vector3(0, 5, 0);
            models.Add(ModelHouse);

            models.Add(new Model("..\\..\\..\\Content\\Models\\Cube.obj", shader1));
            models[models.Count - 1].Position = new Vector3(10, 3, 2);

            ModelHouse2 = new Model("..\\..\\..\\Content\\Models\\Cube2.obj", shader1);
            ModelHouse2.Scale = 3;
            ModelHouse2.Position = new Vector3(0, 0, 0);
            models.Add(ModelHouse2);

            ModelLight = new Model("..\\..\\..\\Content\\Models\\Cube.obj", shader1);

            directionalLight.direction = lightPos;

            camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

            //CursorState = CursorState.Grabbed;
        }
        Shader shader2;

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            Delta = (float)args.Time;

            controller.Update(this, Delta);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);

            GL.CullFace(CullFaceMode.Back);


            Shadow.BindDepthShader(ref shader2, light.direction, light.position, camera.Up);

            ModelHouse.Rotation = new Vector3(ModelHouse.Rotation.X, 0.04f, ModelHouse.Rotation.Z);
            
            foreach (var model in models)
            {
                model.Render(shader2);
            }

            GL.Viewport(0, 0, 1920, 1080);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.CullFace(CullFaceMode.Back);

            Shadow.BindShader(ref shader1, light.direction, light.position, ModelHouse.Position, camera.Up, camera.Position, camera.GetProjectionMatrix(), camera.GetViewMatrix());
            light.UseShader(shader1);

            ModelRender(shader1);

            ImGuiRender();
            LightSetingsImGui();

            controller.Render();

            ImGuiController.CheckGLError("End of frame");

            SwapBuffers();
        }

        private int model;
        private string[] modelsName = { "Cube", "Cube", "Plain" };

        public void ModelRender(Shader shader)
        {
            ImGui.Begin("Material setings");
            ImGui.Combo("Models", ref model, modelsName, modelsName.Length);
            for(int i = 0; i < models.Count; i++)
            {
                if(i == model)
                {
                    ImGui.DragFloat("MaterialSpecular", ref models[i].Material.MaterialSpecular, 0.001f, 0, 1);
                    ImGui.DragFloat("shininess", ref models[i].Material.shininess, 0.1f, 0, 100);

                    ImGui.DragFloat("Ambient", ref models[i].Material.Ambient, 0.001f, 0, 1);
                    ImGui.DragFloat("Diffuse", ref models[i].Material.Diffuse, 0.001f, 0, 1);
                    ImGui.DragFloat("LightSpecular", ref models[i].Material.LightSpecular, 0.001f, 0, 1);
                    ImGui.DragFloat("height_scale", ref models[i].Material.height_scale, 0.001f, -1, 1);
                }
                models[i].Render(shader);
            }
            ImGui.End();
        }

        public float WidhtHeight = 50;
        float depthNear = -7f, depthFar = 7f;
        bool offCenter = false;

        private string[] lightType = { "point", "directional", "spot" };
        private int type = 1;

        private PointLight pointLight = new PointLight();
        private DirectionalLight directionalLight = new DirectionalLight();
        private SpotLight spotLight = new SpotLight();
        public void LightSetingsImGui()
        {
            ImGui.Begin("Light setings");
            ImGui.Combo("Light type", ref type, lightType, lightType.Length);
            if(type == 0)
            {
                light = pointLight;
                ImGui.DragFloat("constant", ref light.constant);
                ImGui.DragFloat("linear", ref light.linear);
                ImGui.DragFloat("quadratic", ref light.quadratic);
            }
            else if (type == 1)
            {
                light = directionalLight;
                System.Numerics.Vector3 dir = new System.Numerics.Vector3(light.direction.X, light.direction.Y, light.direction.Z);
                ImGui.DragFloat3("direction", ref dir);
                light.direction = new Vector3(dir.X, dir.Y, dir.Z);
            }
            else if (type == 2)
            {
                light = spotLight;
                ImGui.DragFloat("degrees", ref light.degrees);
                ImGui.DragFloat("ambient", ref light.ambient);
            }
            ImGui.End();
        }

        public void ImGuiRender()
        {
            ImGui.Begin("Shadow setings");
            ImGui.Checkbox("OffCenter", ref Shadow.OffCenter);
            ImGui.DragFloat("Widht&Height", ref WidhtHeight, 0.1f);
            Shadow.UpdateSize(WidhtHeight, WidhtHeight);
            ImGui.DragFloat("depthNear", ref Shadow.depthNear, 0.1f);
            ImGui.DragFloat("depthFar", ref Shadow.depthFar, 0.1f);

            ImGui.Checkbox("CamProj?", ref Shadow.b_camProj);
            ImGui.Checkbox("CamView?", ref Shadow.b_camView);

            ImGui.End();
        }

        bool firstMove = true;
        Vector2 lastPos = new Vector2();
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

            if (key.IsKeyDown(Keys.F))
            {
                ModelLight.Position = camera.Position;
                light.position = camera.Position;
                light.direction = camera.Front;
            }

            if (key.IsKeyDown(Keys.W))
                camera.Position += camera.Front * 45 * (float)e.Time; //Forward 
            if (key.IsKeyDown(Keys.S))
                camera.Position -= camera.Front * 45 * (float)e.Time; //Backwards
            if (key.IsKeyDown(Keys.A))
                camera.Position -= camera.Right * 45 * (float)e.Time; //Left
            if (key.IsKeyDown(Keys.D))
                camera.Position += camera.Right * 45 * (float)e.Time; //Right
            if (key.IsKeyDown(Keys.Space))
                camera.Position += camera.Up * 45 * (float)e.Time; //Up 
            if (key.IsKeyDown(Keys.LeftShift))
                camera.Position -= camera.Up * 45 * (float)e.Time; //Down				

            var mouse = MouseState;

            if (key.IsKeyDown(Keys.Q))
            {
                CursorState = CursorState.Grabbed;
                if (firstMove) // this bool variable is initially set to true
                {
                    lastPos = new Vector2(mouse.X, mouse.Y);
                    firstMove = false;
                }
                else
                {
                    //Calculate the offset of the mouse position
                    float deltaX = mouse.X - lastPos.X;
                    float deltaY = mouse.Y - lastPos.Y;
                    lastPos = new Vector2(mouse.X, mouse.Y);

                    //Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                    camera.Yaw += deltaX * 0.4f;
                    camera.Pitch -= deltaY * 0.4f; // reversed since y-coordinates range from bottom to top
                }
            }
            else
                CursorState = CursorState.Normal;

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

    }*/
    }
}
