using GameEngine.Core;
using GameEngine.Core.Structs;
using GameEngine.GameObjects;
using GameEngine.GameObjects.Components.List;
using GameEngine.GameObjects.List;
using GameEngine.LevelEditor.Interface;
using GameEngine.Renders.Bufers;
using GameEngine.Resources;
using GameEngine.Scenes;
using GameEngine.Widgets;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameEngine.LevelEditor
{
    public class EditorLayer : Layer
    {
        private EditorInterface editorInterface;
        private FrameBuffer frameBuffer;
        private FrameBuffer pickingBuffer;
        private Scene scene;
        private GameWindow Window;
        private Camera SceneCamera;

        bool firstMove = true;
        bool scenSelect = false;
        Vector2 lastPos = new Vector2();

        public override void Init(GameWindow window)
        {
            Window = window;
            window.MouseWheel += Window_MouseWheel;

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.Texture2D);

            BufferInit();


            AssetManager assetManager = new AssetManager("Assets\\");
            AssetManager.basePath = "Assets\\";

            scene = new Scene();

            editorInterface = new EditorInterface(window.ClientSize.X, window.ClientSize.Y, scene);

            

            SceneCamera = new Camera(Vector3f.UnitZ * 3, window.ClientSize.X / window.ClientSize.Y);

            scene.SetCamera(SceneCamera);
            scene.AddGameObject(new Grid());
            
            GameObject gameObject = new GameObject();
            gameObject.Name = "Light";
            LightRender lightRender = new LightRender();
            lightRender.IsShadowUse = false;
            gameObject.AddComponent(lightRender);

            scene.AddGameObject(gameObject);

            scene.Start();
        }

        private void BufferInit()
        {
            //Texture view scen
            var ColorTexture = new FrameBufferTextureSpecification { TextureTarget = TextureTarget.Texture2D, PixelInternalFormat = PixelInternalFormat.Rgb, PixelFormat = PixelFormat.Rgba, PixelType = PixelType.UnsignedByte, Attachment = FramebufferAttachment.ColorAttachment0 };
            var DepthTexture = new FrameBufferTextureSpecification { TextureTarget = TextureTarget.Texture2D, SizedInternalFormat = SizedInternalFormat.Depth24Stencil8, Attachment = FramebufferAttachment.DepthStencilAttachment };

            var frameBufferSpecification = new FrameBufferSpecification();
            frameBufferSpecification.textureSpecifications = new List<FrameBufferTextureSpecification>();
            frameBufferSpecification.textureSpecifications.Add(ColorTexture);
            frameBufferSpecification.textureSpecifications.Add(DepthTexture);
            frameBufferSpecification.Width = Window.ClientSize.X;
            frameBufferSpecification.Height = Window.ClientSize.Y;

            frameBuffer = new FrameBuffer(frameBufferSpecification);
            frameBuffer.Init();

            //Mouse picking texture
            ColorTexture = new FrameBufferTextureSpecification { TextureTarget = TextureTarget.Texture2D, PixelInternalFormat = PixelInternalFormat.Rgb32f, PixelFormat = PixelFormat.Rgba, PixelType = PixelType.Float, Attachment = FramebufferAttachment.ColorAttachment0 };
            DepthTexture = new FrameBufferTextureSpecification { TextureTarget = TextureTarget.Texture2D, SizedInternalFormat = SizedInternalFormat.DepthComponent32f, Attachment = FramebufferAttachment.DepthAttachment };

            var fbSpec = new FrameBufferSpecification();
            fbSpec.textureSpecifications = new List<FrameBufferTextureSpecification>();
            fbSpec.textureSpecifications.Add(ColorTexture);
            fbSpec.textureSpecifications.Add(DepthTexture);
            fbSpec.Width = Window.ClientSize.X;
            fbSpec.Height = Window.ClientSize.Y;

            pickingBuffer = new FrameBuffer(fbSpec);
            pickingBuffer.Init();
        }

        public override void Update(float deltaTime)
        {
            if (Window.KeyboardState.IsKeyDown(Keys.W))
                scene.GetCamera().CameraMove(Direction.Front, deltaTime);
            else if (Window.KeyboardState.IsKeyDown(Keys.S))
                scene.GetCamera().CameraMove(Direction.Breack, deltaTime);
            else if (Window.KeyboardState.IsKeyDown(Keys.A))
                scene.GetCamera().CameraMove(Direction.Left, deltaTime);
            else if (Window.KeyboardState.IsKeyDown(Keys.D))
                scene.GetCamera().CameraMove(Direction.Right, deltaTime);

            if (firstMove) // this bool variable is initially set to true
            {
                lastPos = new Vector2(Window.MouseState.X, Window.MouseState.Y);
                firstMove = false;
            }
            else
            {
                //Calculate the offset of the mouse position
                float deltaX = Window.MouseState.X - lastPos.X;
                float deltaY = Window.MouseState.Y - lastPos.Y;
                lastPos = new Vector2(Window.MouseState.X, Window.MouseState.Y);

                if (Window.MouseState.IsButtonDown(MouseButton.Right) && editorInterface.IsScenViewSelected && scene != null)
                {
                    Window.CursorState = CursorState.Grabbed;
                    scene.GetCamera().CameraRotation(new Vector2(deltaX, deltaY) * 0.4f);
                }
                else
                    Window.CursorState = CursorState.Normal;


            }
            if (scene != null)
                scene.Update(deltaTime);
        }

        public override void Draw()
        {
            GL.ClearColor(Color4.White);

            if (scene != null)
            {
                pickingBuffer.Bind();
                scene.Draw();
                pickingBuffer.Unbind();

                frameBuffer.Bind();
                scene.Draw();
                frameBuffer.Unbind();
            }
        }

        public override void ImGuiDraw(GameWindow window, float deltaTime)
        {
            window.Title = (1 / deltaTime).ToString();

            editorInterface.Update(window, deltaTime);

            ImGui.DockSpaceOverViewport();

            ImGui.Text(pickingBuffer.ReadPixel((int)ImGui.GetMousePos().X, (int)ImGui.GetMousePos().Y, 0).ToString());

            ImGui.BeginMainMenuBar();
            if(ImGui.MenuItem("Save scen", "Ctrl + S"))
            {
                //Task.Run(async () => await SceneSerialize.Serialize(scene));
                SceneSerialize.Serialize(scene);
            }
            if(ImGui.MenuItem("Open scen", "Ctrl + O"))
            {
                scene = null;
                try
                {
                    scene = SceneSerialize.Deserialize().Result;
                    scene.SetCamera(SceneCamera);
                    scene.Start();
                    editorInterface.SetScene(scene);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if(scene == null)
                {
                    throw new Exception();
                }
            }
            ImGui.EndMainMenuBar();

            editorInterface.Draw();
            editorInterface.SetTextureScenView(frameBuffer.GetTexture(0));
        }

        private void Window_MouseWheel(MouseWheelEventArgs obj)
        {
            if(editorInterface.IsScenViewSelected && scene != null)
                scene.GetCamera().GetComponent<CameraRender>().Fov -= obj.OffsetY;
        }

        public override void Resize(int width, int height)
        {
            editorInterface.SetWindowResize(width, height);

            if(scene != null && scene.GetCamera() != null && scene.GetCamera().GetComponent<CameraRender>() != null && height > 0)
                scene.GetCamera().GetComponent<CameraRender>().Aspect = width/ height;
        }

        public Scene GetScene() => scene;
    }
}
