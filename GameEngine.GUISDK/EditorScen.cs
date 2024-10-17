using GameEngine.Bufers;
using GameEngine.GameObjects;
using GameEngine.GameObjects.Components;
using GameEngine.GameObjects.Components.List;
using GameEngine.GameObjects.List;
using GameEngine.Resources;
using GameEngine.Resources.Materials;
using GameEngine.Resources.Meshes;
using GameEngine.Resources.Shaders;
using GameEngine.Resources.Textures;
using GameEngine.Scens;
using GameEngine.Windws;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.LevelEditor
{
    public class EditorScen : BaseScen
    {
        private BaseShader shader;
        private BaseShader pickingShader;
        private BufferManager bufferManager;
        private BaseWindow window;
        private BaseTexture texture;
        private EditorInterface editorInterface;
        GameObject gameObject = new GameObject();
        GameObject gameObject2;
        MeshRender meshRender = new MeshRender();
        Camera camera;
        pickingBuffer pickingBuffer;

        public EditorScen()
        {
            gameObjects = new List<GameObject>();
            AssetManager = new AssetManager("Assets\\");
            ScenName = "Main scen";
        }

        public override void Start(BaseWindow window)
        {

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.CullFace);



            this.window = window;
            window.Resize += Window_Resize;
            window.MouseWheel += Window_MouseWheel;
            window.TextInput += Window_TextInput;

            shader = ShaderLoad.Load(AssetManager.GetShader("shader"));
            pickingShader = ShaderLoad.Load(AssetManager.GetShader("base"));
            bufferManager = new BufferManager();
            bufferManager.Init(window.Size.X, window.Size.Y);
            pickingBuffer = new pickingBuffer();
            pickingBuffer.Init(window.Size.X, window.Size.Y);

            editorInterface = new EditorInterface(window.Size.X, window.Size.Y);

            camera = new Camera(Vector3.UnitZ * 3, window.Size.X / (float)window.Size.Y);
            AddGameObject(camera);

            texture = TextureLoader.LoadTexture(AssetManager.GetTexture("container2"));

            MeshRender meshRender = new MeshRender();
            meshRender.Start();
            meshRender.AddMeshRange(MeshLoader.LoadMesh(AssetManager.GetMesh("house2"), shader));
            meshRender.AddMaterialRange(MaterialLoader.LoadMaterial(AssetManager.GetMesh("house2")));
            gameObject.AddComponent(meshRender);
            gameObject.GetComponent<TransformComponet>().Transform = new Vector3(0, 0, -3);


            AddGameObject(gameObject);

            gameObject2 = new GameObject { Name = "light" };
            gameObject2.AddComponent(new Light());

            AddGameObject(gameObject2);

            base.Start(window);
        }

        public override void Update(BaseWindow window, float deltaTime)
        {
            base.Update(window, deltaTime);
            editorInterface.Update(window, deltaTime);  

            foreach (var obj in gameObjects)
            {
                obj.Update(deltaTime, shader);
            }
        }

        protected override void OnUpdateKey(KeyboardState keyboard, float deltaTime)
        {
            base.OnUpdateKey(keyboard, deltaTime);

            if (keyboard.IsKeyDown(Keys.Escape))
                window.Close();

            if (window.MouseState.IsButtonDown(MouseButton.Right) && editorInterface.IsScenViewSelected)
            {
                if (keyboard.IsKeyDown(Keys.W))
                    camera.CameraMove(Direction.Front, deltaTime);
                if (keyboard.IsKeyDown(Keys.S))
                    camera.CameraMove(Direction.Breack, deltaTime);
                if (keyboard.IsKeyDown(Keys.A))
                    camera.CameraMove(Direction.Left, deltaTime);
                if (keyboard.IsKeyDown(Keys.D))
                    camera.CameraMove(Direction.Right, deltaTime);


            }
            if (keyboard.IsKeyDown(Keys.F) && editorInterface.IsScenViewSelected)
            {
                gameObject2.GetComponent<Light>().Position = camera.GetComponent<TransformComponet>().Transform;
            }

        }

        bool firstMove = true;
        bool scenSelect = false;
        Vector2 lastPos = new Vector2();

        protected override void OnUpdateMouse(MouseState mouse, float deltaTime)
        {
            base.OnUpdateMouse(mouse, deltaTime);

            {
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

                    if (mouse.IsButtonDown(MouseButton.Right) && editorInterface.IsScenViewSelected)
                    {
                        window.CursorState = CursorState.Grabbed;
                        camera.CameraRotation(new Vector2(deltaX, deltaY) * 0.4f);
                    }
                    else
                        window.CursorState = CursorState.Normal;
                }
            }
        }

        Vector4 id = new Vector4();

        public override void Render(BaseWindow window, float deltaTime)
        {
            GL.ClearColor(Color4.Black);


            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.CullFace(CullFaceMode.Back);

            if (window.MouseState.IsButtonPressed(MouseButton.Left))
            {
                shader.Use();
                shader.SetInt("useFbo", 1);

                pickingBuffer.Bind();
                foreach (var obj in gameObjects)
                {
                    obj.Draw(shader);
                }

                pickingBuffer.Unbind(window.Size.X, window.Size.Y);

                float mouseX = (window.MouseState.Position.X);
                float mouseY = (window.MouseState.Position.Y);

                id = pickingBuffer.ReadPixel((int)mouseX, (int)mouseY, shader);
            }

            shader.Use();
            shader.SetInt("useFbo", 0);

            bufferManager.Bind();
            foreach (var obj in gameObjects)
            {
                var render = obj.GetComponent<MeshRender>();

                if (window.MouseState.IsButtonDown(MouseButton.Left))
                {
                    if (id.X == obj.Id)
                    {
                        if (render != null)
                        {
                            foreach (var mat in render.materials)
                                mat.Value.color = new Vector3(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B);
                        }

                    }
                    else
                    {
                        if (render != null)
                        {
                            foreach (var mat in render.materials)
                                mat.Value.color = new Vector3(1);
                        }
                    }
                }
                else
                {
                    if (render != null)
                    {
                        foreach (var mat in render.materials)
                            mat.Value.color = new Vector3(1);
                    }
                }
                obj.Draw(shader);
            }

            bufferManager.Unbind();

            ImGui.DockSpaceOverViewport();

            editorInterface.SetTextureScenView(bufferManager.GetTexture());
            editorInterface.Draw(gameObjects);

        }

        private void Window_Resize(ResizeEventArgs obj)
        {
            editorInterface.SetWindowResize(window.ClientSize.X, window.ClientSize.Y);
            bufferManager.Init(window.ClientSize.X, window.ClientSize.Y);
            pickingBuffer.Init(window.ClientSize.X, window.ClientSize.Y);

            camera.GetComponent<CameraRender>().Aspect = window.ClientSize.X / (float)window.ClientSize.Y;
        }

        private void Window_TextInput(TextInputEventArgs obj)
        {
            editorInterface._controller.PressChar((char)obj.Unicode);
        }

        private void Window_MouseWheel(MouseWheelEventArgs obj)
        {
            editorInterface._controller.MouseScroll(obj.Offset);

            if(scenSelect)
            {
                camera.GetComponent<CameraRender>().Fov -= obj.OffsetY;
            }
        }
    }
}
