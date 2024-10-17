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
        private ImGuiController controller;
        private BaseWindow window;
        private BaseTexture texture;
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
            controller = new ImGuiController(window.Size.X, window.Size.Y);

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

            controller.Update(window, deltaTime);

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

            if (keyboard.IsKeyDown(Keys.W))
                camera.CameraMove(Direction.Front, deltaTime);
            if (keyboard.IsKeyDown(Keys.S))
                camera.CameraMove(Direction.Breack, deltaTime);
            if (keyboard.IsKeyDown(Keys.A))
                camera.CameraMove(Direction.Left, deltaTime);
            if (keyboard.IsKeyDown(Keys.D))
                camera.CameraMove(Direction.Right, deltaTime);

            if (keyboard.IsKeyDown(Keys.F))
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

            //if (!window.KeyboardState.IsKeyDown(Keys.LeftControl) && scenSelect)
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

                    if (mouse.IsButtonDown(MouseButton.Right) && scenSelect)
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
            GL.ClearColor(Color4.Cyan);


            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.CullFace(CullFaceMode.Back);

            // axisRender.SetModel(gameObject.Model);

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

                float mouseX = (window.MouseState.Position.X - sizeMin.X);
                float mouseY = (window.MouseState.Position.Y - sizeMin.Y);

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
                        if(scenSelect)
                            current_gameObject = (int)id.X;
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
            ImGui.BeginMainMenuBar();
            ImGui.Text("Hello");
            ImGui.EndMainMenuBar();
            ScenViwe();
            ListObjectView();
            PropertisObject();
            FileList();

            controller.Render();
        }
        System.Numerics.Vector2 size;
        System.Numerics.Vector2 sizeMin;
        private void ScenViwe()
        {
            ImGui.Begin("Scen");
            scenSelect = ImGui.IsWindowFocused();

            ImGui.Image(bufferManager.GetTexture(), ImGui.GetContentRegionAvail(), new System.Numerics.Vector2(0, 1), new System.Numerics.Vector2(1, 0));
            size = ImGui.GetItemRectSize();
            sizeMin = ImGui.GetItemRectMin();

            if (pickingBuffer.Width != size.X && pickingBuffer.Height != size.Y)
                pickingBuffer.Init((int)size.X, (int)size.Y);

            //var render = (CameraRender)camera.GetComponent(ComponentType.Camera);
            //if(render.Aspect != size.X / size.Y)
               // render.Aspect = size.X / size.Y;
            ImGui.End();
        }

        private int current_gameObject = 0;
        private bool isPopupObjects = false;
        private System.Numerics.Vector2 menuPos;

        private string[] object3DName = { "Cube", "Sphere", "Cylinder", "Plane" };
        private void ListObjectView()
        {
            ImGui.Begin("List gameObjects");
            if (ImGui.TreeNode(this.ScenName))
            {
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    var item = gameObjects.ToArray()[i];

                    ImGui.Selectable(item.Name);
                    {
                        if (ImGui.IsItemClicked(ImGuiMouseButton.Left))
                        {
                            current_gameObject = i;
                        }
                        if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                        {
                            ImGui.OpenPopup("Add object");
                            current_gameObject = i;
                        }
                    }
                }
                ImGui.TreePop();
            }

            if (ImGui.IsMouseDown(ImGuiMouseButton.Right) && ImGui.IsWindowFocused())
            {
                menuPos = ImGui.GetMousePos();
                ImGui.OpenPopup("Add object");
            }

            ImGui.SetWindowPos(menuPos);
            if (ImGui.BeginPopup("Add object", ImGuiWindowFlags.AlwaysAutoResize))
            {
                ImGui.Spacing();
                ImGui.Separator();
                if (ImGui.Selectable("Rename"))
                {

                }
                else if (ImGui.Selectable("Duplicate"))
                {

                }
                else if (ImGui.Selectable("Delate"))
                {
                    RemoveGameObjectById(current_gameObject);
                    current_gameObject = 0;
                    ImGui.CloseCurrentPopup();
                }
                ImGui.Spacing();
                ImGui.Separator();

                GameObject gameObject = new GameObject();
                if (ImGui.Selectable("Add empty object"))
                {
                    AddGameObject(gameObject);

                    ImGui.CloseCurrentPopup();
                }
                ImGui.Spacing();
                ImGui.Separator();
                if (ImGui.BeginMenu("3d objects"))
                {
                    MeshRender meshRender = new MeshRender();
                    meshRender.Start();

                    for (int i = 0; i < object3DName.Length; i++)
                    {
                        if (ImGui.MenuItem(object3DName[i]))
                        {
                            meshRender.AddMeshRange(MeshLoader.LoadMesh(AssetManager.GetMesh(object3DName[i]), shader));
                            meshRender.AddMaterialRange(MaterialLoader.LoadMaterial(AssetManager.GetMesh(object3DName[i])));
                            gameObject.AddComponent(meshRender);
                            gameObject.Name = object3DName[i];

                            AddGameObject(gameObject);
                        }
                    }
                }
            }

            if (ImGui.IsMouseDown(ImGuiMouseButton.Left) && ImGui.IsWindowFocused())
                ImGui.EndPopup();
            //ImGui.End();

            //ImGui.ListBox("Gameobjects", ref current_gameObject, GetNameGameObjects(), GetNameGameObjects().Length);
            ImGui.End();
        }

        System.Numerics.Vector3 rot;
        System.Numerics.Vector3 pos;
        float scale = 1;

        private bool addComponentView = false;

        private void PropertisObject()
        {
            var gameObject = GetGameObjectById(current_gameObject);

            

            ImGui.Begin("Propertis", ImGuiWindowFlags.HorizontalScrollbar);
            if (gameObject == null)
                return;
            ImGui.Spacing();
            ImGui.Text(gameObject.Name);
            ImGui.Spacing();
            TransformView(gameObject);
            ImGui.Separator();
            ImGui.Spacing();
            VertexRenderView(gameObject.GetComponent<MeshRender>());
            ImGui.Spacing();
            CameraView(gameObject.GetComponent<CameraRender>());
            ImGui.Spacing();
            LightView(gameObject.GetComponent<Light>());
            ImGui.SetCursorPos(new System.Numerics.Vector2(ImGui.GetWindowSize().X / 3, ImGui.GetCursorPosY()));
            if (ImGui.Button("Add component"))
            {
                ImGui.OpenPopup("Add component", ImGuiPopupFlags.AnyPopup);
            }
            if (ImGui.BeginPopup("Add component"))
                AddCompoentView(gameObject);

            ImGui.End();
        }

        private void LightView(Light light)
        {
            if (light == null)
                return;

            ImGui.Text("Light");

            ImGui.Separator();
        }

        private string[] camProjections = { ProjectionType.Perspective.ToString(), ProjectionType.Orthgrafic.ToString() };
        private int currentProjectionType = 0;
        private float cameraFOV;

        private void CameraView(CameraRender cameraRender)
        {
            if (cameraRender == null)
                return;

            if (ImGui.TreeNode("Camera"))
            {
                ImGui.Combo("Projection", ref currentProjectionType, camProjections, camProjections.Length);
                cameraRender.ProjectionType = (ProjectionType)Enum.Parse(typeof(ProjectionType), camProjections[currentProjectionType]);
                if (currentProjectionType == 0)
                {
                    ImGui.Spacing();
                    cameraFOV = cameraRender.Fov;
                    ImGui.SliderFloat("FOV", ref cameraFOV, 3, 179);
                    cameraRender.Fov = cameraFOV;
                    ImGui.Spacing();
                }
                else if (currentProjectionType == 1)
                {
                    ImGui.Spacing();
                    ImGui.DragFloat("size", ref cameraRender.size);
                    ImGui.Spacing();
                }
                ImGui.Text("Clipping Planes");
                ImGui.DragFloat("Near", ref cameraRender.depthNear, 0.1f, 0.01f, 100);
                ImGui.Spacing();
                ImGui.DragFloat("Far", ref cameraRender.depthFar, 0.1f, 0.01f, 100);
                ImGui.Spacing();

                ImGui.TreePop();
            }

            ImGui.Separator();
            ImGui.Spacing();
        }

        private string[] componentNames = { "Camera", "Mesh Render", "Light" };
        private int currentComponent = 0;


        private void AddCompoentView(GameObject gameObject)
        {
            ImGui.Spacing();
            ImGui.ListBox("Components", ref currentComponent, componentNames, componentNames.Length);
            ImGui.Spacing();
            if (ImGui.Button("Add component"))
            {
                //ComponentType type = (ComponentType)Enum.Parse(typeof(ComponentType), componentNames[currentComponent]);

                gameObject.AddComponent(new Component());

                ImGui.CloseCurrentPopup();
            }
            else if (ImGui.Button("Cansel"))
                ImGui.CloseCurrentPopup();
        }

        private bool textureEditView = false;
        private bool textureAddView = false;

        private TextureType texType;

        private void VertexRenderView(MeshRender render)
        {
            if (render == null)
                return;

            ImGui.Text("Mesh Render");
            ImGui.Spacing();
            ImGui.Text("Meshes");
            ImGui.Spacing();
            foreach (var mesh in render.meshes)
            {
                //if (ImGui.TreeNodeEx(mesh.Name))
                {
                    ImGui.Selectable(mesh.Name);
                }
            }
            ImGui.Spacing();
            ImGui.Text("Materials");
            ImGui.Spacing();

            foreach (var material in render.materials)
            {
                if (ImGui.TreeNodeEx(material.Key))
                {
                    MaterialView(material.Value);
                    ImGui.TreePop();

                }
            }
            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();
        }

        private void MaterialView(BaseMaterial material)
        {
            if (material == null)
                return;

            {
                ImGui.Text("Textures");
                foreach (var item in material.GetTextures())
                {
                    ImGui.Text(item.Key.ToString());
                    ImGui.Spacing();
                    if (ImGui.ImageButton(item.Key.ToString(), item.Value.Handle, new System.Numerics.Vector2(64, 64)))
                    {
                        textureEditView = true;
                        texType = item.Key;
                    }
                }
                ImGui.Spacing();
                if (ImGui.Button("Add texture"))
                {
                    textureAddView = true;
                }


                ImGui.Spacing();
                ImGui.DragFloat("texture scale", ref material.textureScale, 0.1f, 1, 100);
                //ImGui.Spacing();
                //ImGui.ColorEdit3("color", ref material.Color);
                ImGui.Spacing();
                ImGui.DragFloat("shininess", ref material.shininess, 0.01f, 0);
                ImGui.Spacing();
                ImGui.DragFloat("ambient", ref material.ambient, 0.01f, 0, 1);
                ImGui.Spacing();
                ImGui.DragFloat("diffuse", ref material.diffuse, 0.01f, 0);
                ImGui.Spacing();
                ImGui.DragFloat("lightSpecular", ref material.lightSpecular, 0.01f, 0);
                ImGui.Spacing();

                if (textureAddView)
                {
                    AddTexture(material);
                }
                EditTexture(material);
            }

            ImGui.Separator();
            ImGui.Spacing();
        }

        private int current_TextureToAddType = 0;
        private int current_TextureToAdd = 0;

        private string[] textureTypeNames = { TextureType.Diffuse.ToString(), TextureType.Specular.ToString(), TextureType.Normal.ToString(), TextureType.DepthMap.ToString() };

        private void AddTexture(BaseMaterial material)
        {
            ImGui.Begin("Add textures", ImGuiWindowFlags.AlwaysAutoResize);
            ImGui.Spacing();
            ImGui.Text("Texture type");
            ImGui.Spacing();
            ImGui.Combo("Texture type", ref current_TextureToAddType, textureTypeNames, textureTypeNames.Length);
            ImGui.Spacing();
            ImGui.ListBox("Textures", ref current_TextureToAdd, AssetManager.GetTextures().Keys.ToArray(), AssetManager.GetTextures().Count);
            ImGui.Spacing();
            if (ImGui.Button("Add texture"))
            {
                textureAddView = false;

                TextureType type = (TextureType)Enum.Parse(typeof(TextureType), textureTypeNames[current_TextureToAddType]);

                string texKey = AssetManager.GetTextures().Keys.ToArray()[current_TextureToAdd];
                material.AddTexture(type, TextureLoader.LoadTexture(AssetManager.GetTexture(texKey)));

                current_TextureToAdd = 0;
                current_TextureToAddType = 0;
            }
            else if (ImGui.Button("Cansel"))
            {
                textureAddView = false;
            }
            ImGui.End();
        }

        private int current_texture = 0;

        private void EditTexture(BaseMaterial material)
        {
            if (textureEditView)
            {
                ImGui.Begin("Texture list  ", ImGuiWindowFlags.AlwaysAutoResize);
                ImGui.Spacing();
                ImGui.ListBox("Textures", ref current_texture, AssetManager.GetTextures().Keys.ToArray(), AssetManager.GetTextures().Count);
                ImGui.Spacing();
                if (ImGui.Button("Set texture"))
                {
                    textureEditView = false;
                    material.RemoveTexture(texType);
                    string texKey = AssetManager.GetTextures().Keys.ToArray()[current_texture];
                    material.AddTexture(texType, TextureLoader.LoadTexture(AssetManager.GetTexture(texKey)));

                    current_texture = 0;
                }
                else if (ImGui.Button("Cansel"))
                {
                    textureEditView = false;
                }
                ImGui.End();
            }
        }

        private void TransformView(GameObject gameObject)
        {
            ImGui.LabelText("Transform", "");

            var transform = gameObject.GetComponent<TransformComponet>();

            pos = new System.Numerics.Vector3(transform.Transform.X, transform.Transform.Y, transform.Transform.Z);

            ImGui.DragFloat3("Position", ref pos, 0.01f);
            ImGui.Spacing();


            if (transform.Transform != new Vector3(pos.X, pos.Y, pos.Z))
                transform.Transform = (new Vector3(pos.X, pos.Y, pos.Z));

            rot = new System.Numerics.Vector3(transform.Rotation.X, transform.Rotation.Y, transform.Rotation.Z);

            ImGui.DragFloat3("Rotation", ref rot, 0.01f);
            ImGui.Spacing();

            if (transform.Rotation != new Vector3(rot.X, rot.Y, rot.Z))
                transform.Rotation = new Vector3(rot.X, rot.Y, rot.Z);

            ImGui.DragFloat("Scale", ref scale, 0.01f);
            ImGui.Spacing();

            if (transform.Scale.X != scale)
                transform.Scale = new Vector3(scale);

            gameObject.Update((float)window.UpdateTime, shader);
        }

        private void FileList()
        {
            ImGui.Begin("Assets");
            ImGui.End();
        }

        private void Window_Resize(ResizeEventArgs obj)
        {
            controller.WindowResized(window.ClientSize.X, window.ClientSize.Y);
            bufferManager.Init(window.ClientSize.X, window.ClientSize.Y);
            pickingBuffer.Init(window.ClientSize.X, window.ClientSize.Y);

            var render = camera.GetComponent<CameraRender>();

            render.Aspect = window.ClientSize.X / (float)window.ClientSize.Y;
        }

        private void Window_TextInput(TextInputEventArgs obj)
        {
            controller.PressChar((char)obj.Unicode);
        }

        private void Window_MouseWheel(MouseWheelEventArgs obj)
        {
            controller.MouseScroll(obj.Offset);

            var render = camera.GetComponent<CameraRender>();

            render.Fov -= obj.OffsetY;
        }
    }
}
