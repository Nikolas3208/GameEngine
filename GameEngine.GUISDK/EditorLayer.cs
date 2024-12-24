using GameEngine.Core;
using GameEngine.Core.Essentials;
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
using System.Threading;
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
        private Shader Shader;

        bool firstMove = true;
        bool scenSelect = false;
        private float _camSpeed = 45f;
        Vector2 lastPos = new Vector2();

        public override void Init(GameWindow window)
        {
            Window = window;

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

            Shader = Shader.LoadFromFile(AssetManager.GetShader("multipleLights"));

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
            var ColorTexture = new FrameBufferTextureSpecification { TextureTarget = TextureTarget.Texture2D, CreateTexture = true, PixelInternalFormat = PixelInternalFormat.Rgb, PixelFormat = PixelFormat.Rgba, PixelType = PixelType.Float, Attachment = FramebufferAttachment.ColorAttachment0 };
            var DepthTexture = new FrameBufferTextureSpecification { TextureTarget = TextureTarget.Texture2D, CreateTexture = true, SizedInternalFormat = SizedInternalFormat.Depth24Stencil8, Attachment = FramebufferAttachment.DepthStencilAttachment };

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
            DepthTexture = new FrameBufferTextureSpecification { TextureTarget = TextureTarget.Texture2D, PixelInternalFormat = PixelInternalFormat.DepthComponent, PixelFormat = PixelFormat.DepthComponent, PixelType = PixelType.Float, Attachment = FramebufferAttachment.DepthAttachment };

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
            if (Window.KeyboardState.IsKeyDown(Keys.LeftControl) && Window.IsKeyDown(Keys.S))
            {
                SceneSerialize.Serialize(scene);
            }
            else if (Window.KeyboardState.IsKeyDown(Keys.LeftControl) && Window.IsKeyDown(Keys.O))
            {
                scene = SceneSerialize.Deserialize().Result;
                scene.SetCamera(SceneCamera);
                scene.Start();
                editorInterface.SetScene(scene);
                return;
            }

            else if (Window.KeyboardState.IsKeyDown(Keys.LeftControl) && Window.IsKeyDown(Keys.LeftShift) && Window.IsKeyDown(Keys.N))
            {
                Init(Window);
                return;
            }

            if (pickingBuffer.frameBufferSpecification.Width != editorInterface.Size.X && pickingBuffer.frameBufferSpecification.Height != editorInterface.Size.Y)
            {
                pickingBuffer.frameBufferSpecification.Width = (int)editorInterface.Size.X; pickingBuffer.frameBufferSpecification.Height = (int)editorInterface.Size.Y;
                pickingBuffer.Init();
            }
            if (scene.GetCamera().GetComponent<CameraRender>().Aspect != editorInterface.Size.X / editorInterface.Size.Y)
                scene.GetCamera().GetComponent<CameraRender>().Aspect = editorInterface.Size.X / editorInterface.Size.Y;

            if (Window.KeyboardState.IsKeyDown(Keys.LeftShift))
            {
                _camSpeed = 20;
            }
            else if (Window.KeyboardState.IsKeyDown(Keys.LeftControl))
            {
                _camSpeed = 90;
            }
            else
            {
                _camSpeed = 45;
            }



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
                    if (Window.KeyboardState.IsKeyDown(Keys.W))
                        scene.GetCamera().CameraMove(Direction.Front, deltaTime, _camSpeed);
                    else if (Window.KeyboardState.IsKeyDown(Keys.S))
                        scene.GetCamera().CameraMove(Direction.Breack, deltaTime, _camSpeed);
                    else if (Window.KeyboardState.IsKeyDown(Keys.A))
                        scene.GetCamera().CameraMove(Direction.Left, deltaTime, _camSpeed);
                    else if (Window.KeyboardState.IsKeyDown(Keys.D))
                        scene.GetCamera().CameraMove(Direction.Right, deltaTime, _camSpeed);

                    Window.CursorState = CursorState.Grabbed;
                    scene.GetCamera().CameraRotation(new Vector2(deltaX, deltaY) * 0.4f);
                }
                else
                    Window.CursorState = CursorState.Normal;


            }
            if (scene != null)
                scene.Update(deltaTime);
        }


        public Vector4 pick;
        float mouseX = 0;
        float mouseY = 0;

        public override void Draw()
        {
            GL.ClearColor(new Color4(42, 61, 93, 1));

            if (scene != null)
            {
                if (Window.IsMouseButtonPressed(MouseButton.Left))
                {
                    pickingBuffer.Bind();
                    Shader.Use();
                    Shader.SetInt("usePicking", 1);

                    scene.Draw(Shader);

                    pickingBuffer.Unbind(Window.ClientSize.X, Window.ClientSize.Y);

                    mouseX = (Window.MouseState.Position.X - editorInterface.SizeMin.X);
                    mouseY = (Window.ClientSize.Y) - (Window.MouseState.Position.Y + (Window.ClientSize.Y - editorInterface.Size.Y) - editorInterface.SizeMin.Y);

                    pick = pickingBuffer.ReadPixel((int)mouseX, (int)mouseY, 0);

                    if(mouseX > 0 && mouseY > 0 && mouseX < editorInterface.Size.X && mouseY < editorInterface.Size.Y)
                        editorInterface.currentGameObject = (int)pick.X;
                }
                Shader.SetInt("usePicking", 0);
                
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

            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if(ImGui.MenuItem("New", "Ctrl + Shift + N"))
                    {
                        Init(Window);
                        return;
                    }
                    else if(ImGui.MenuItem("Open", "Ctrl + O"))
                    {
                        scene = SceneSerialize.Deserialize().Result;
                        scene.SetCamera(SceneCamera);
                        scene.Start();
                        editorInterface.SetScene(scene);
                    }
                    else if(ImGui.MenuItem("Save", "Ctrl + S"))
                    {
                        SceneSerialize.Serialize(scene);
                    }
                    else if(ImGui.MenuItem("Save as"))
                    {

                    }

                    ImGui.EndMenu();
                }
            }
            
            ImGui.EndMainMenuBar();

            editorInterface.Draw();
            editorInterface.SetTextureScenView(frameBuffer.GetTexture(0));
        }
        public Scene GetScene() => scene;

        public override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);

            frameBuffer.frameBufferSpecification.Width = e.Width;
            frameBuffer.frameBufferSpecification.Height = e.Height;
            frameBuffer.Init();

            editorInterface.SetWindowResize(e.Width, e.Height);

            if (scene != null && scene.GetCamera() != null && scene.GetCamera().GetComponent<CameraRender>() != null && e.Height > 0)
                scene.GetCamera().GetComponent<CameraRender>().Aspect = editorInterface.Size.X/ editorInterface.Size.Y;
        }

        public override void OnTextInput(TextInputEventArgs e)
        {
            editorInterface._controller.PressChar((char)e.Unicode);
        }

        public override void OnMouseWheel(MouseWheelEventArgs e)
        {
            editorInterface._controller.MouseScroll(e.Offset);

            if (editorInterface.IsScenViewSelected && scene != null)
                scene.GetCamera().GetComponent<CameraRender>().Fov -= e.OffsetY;
        }
    }
}
