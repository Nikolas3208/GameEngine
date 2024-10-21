using GameEngine.Gizmo;
using GameEngine.GameObjects;
using GameEngine.GameObjects.Components;
using GameEngine.GameObjects.Components.List;
using GameEngine.GameObjects.List;
using GameEngine.Resources;
using GameEngine.Resources.Meshes;
using GameEngine.Resources.Shaders;
using GameEngine.Resources.Textures;
using GameEngine.Scens;
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
using GameEngine.Core.Renders.Bufers;
using GameEngine.Core.Renders;

namespace GameEngine.LevelEditor
{
    public class EditorScen : BaseScen
    {
        public Shader shader;
        private Shader pickingShader;
        private BufferManager bufferManager;
        private BaseWindow window;
        private BaseTexture texture;
        private EditorInterface editorInterface;
        private Gizmo.Gizmo gizmo;
        GameObject gameObject = new GameObject();
        GameObject gameObject2;
        MeshRender meshRender = new MeshRender();
        Camera camera;
        Grid Grid;
        PickingFBOBuffer pickingBuffer;

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

            shader = ShaderLoad.Load(AssetManager.GetShader("multipleLights"));
            pickingShader = ShaderLoad.Load(AssetManager.GetShader("picking"));
            bufferManager = new BufferManager();
            bufferManager.Init(window.ClientSize.X, window.ClientSize.Y);
            pickingBuffer = new PickingFBOBuffer();
            pickingBuffer.Init(window.ClientSize.X, window.ClientSize.Y);
            Grid = new Grid(AssetManager.GetShader("grid"));


            editorInterface = new EditorInterface(window.ClientSize.X, window.ClientSize.Y, this);

            gizmo = new Gizmo.Gizmo();
            gizmo.Init(shader, GizmoType.Translation);
            gizmo.Position = new Vector3(16, 0, 0);

            camera = new Camera(Vector3.UnitZ * 3, window.Size.X / (float)window.Size.Y);
            AddGameObject(camera);

            texture = TextureLoader.LoadTexture(AssetManager.GetTexture("container2"));

            MeshRender meshRender = new MeshRender();
            meshRender.Start();
            meshRender.AddMeshRange(MeshLoader.LoadMesh(AssetManager.GetMesh("house2"), shader));
            gameObject.AddComponent(meshRender);
            gameObject.GetComponent<TransformComponet>().Transform = new Vector3(0, 0, -3);

            AddGameObject(gameObject);

            MeshRender meshRender2 = new MeshRender();
            meshRender2.Start();
            meshRender2.AddMeshRange(MeshLoader.LoadMesh(AssetManager.GetMesh("Cube"), shader));
            gameObject2 = new GameObject { Name = "light" };
            gameObject2.AddComponent(new LightRender());

            AddGameObject(gameObject2);

            base.Start(window);
        }

        public override void Update(BaseWindow window, float deltaTime)
        {
            base.Update(window, deltaTime);
            editorInterface.Update(window, deltaTime);  

            if(pickingBuffer.Width != editorInterface.Size.X && pickingBuffer.Height != editorInterface.Size.Y)
                pickingBuffer.Init((int)editorInterface.Size.X, (int)editorInterface.Size.Y);

            if(camera.GetComponent<CameraRender>().Aspect != editorInterface.Size.X / editorInterface.Size.Y)
                camera.GetComponent<CameraRender>().Aspect = editorInterface.Size.X / editorInterface.Size.Y;

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
            if (keyboard.IsKeyDown(Keys.F))
            {
                gameObject2.GetComponent<TransformComponet>().Transform = camera.GetComponent<TransformComponet>().Transform;
                gameObject2.GetComponent<LightRender>().Light.Direction = camera.GetComponent<CameraRender>().Front;
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

        Vector4 id = new Vector4(-1);

        public override void Render(BaseWindow window, float deltaTime)
        {
            GL.ClearColor(Color4.White);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.CullFace(CullFaceMode.Back);

            if (window.MouseState.IsButtonDown(MouseButton.Left))
            {
                pickingBuffer.Bind();
                pickingShader.Use();

                foreach (var obj in gameObjects)
                { 
                    pickingShader.SetInt("gameObjectId", obj.Id);
                    obj.Draw(pickingShader);
                    if (obj.IsSelected)
                    {
                        //gizmo.Position = obj.GetComponent<TransformComponet>().Transform;
                        //gizmo.Draw(pickingShader);
                    }
                }

                pickingBuffer.Unbind(window.ClientSize.X, window.ClientSize.Y);

                float mouseX = (window.MouseState.Position.X - editorInterface.SizeMin.X);
                float mouseY = (window.ClientSize.Y) - (window.MouseState.Position.Y + (window.ClientSize.Y - editorInterface.Size.Y) - editorInterface.SizeMin.Y);

                id = pickingBuffer.ReadPixel((int)mouseX, (int)mouseY, pickingShader);
            }

            bufferManager.Bind();

            DrawGameObject();
            Grid.Draw(camera.GetComponent<CameraRender>());
            bufferManager.Unbind(window.ClientSize.X, window.ClientSize.Y);

            ImGui.DockSpaceOverViewport();

            editorInterface.SetTextureScenView(bufferManager.GetTexture());
            editorInterface.Draw(gameObjects);
        }

        public void DrawGameObject()
        {
            foreach (var obj in gameObjects)
            {
                if (window.MouseState.IsButtonDown(MouseButton.Left))
                {
                    if (id.X == obj.Id)
                    {
                        editorInterface.currentGameObject = (int)id.X;
                        obj.IsSelected = true;
                    }
                    else
                    {
                        obj.IsSelected = false;
                    }
                }

                obj.Draw(shader);
            }


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
