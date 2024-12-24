using GameEngine.GameObjects;
using GameEngine.GameObjects.Components;
using GameEngine.GameObjects.Components.List;
using GameEngine.Resources.Meshes;
using GameEngine.Resources;
using ImGuiNET;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Widgets;
using GameEngine.Core;
using GameEngine.Core.Structs;
using GameEngine.Renders;
using OpenTK.Windowing.Desktop;
using GameEngine.Scenes;
using OpenTK.Graphics.OpenGL4;
using GameEngine.Core.Enums;

namespace GameEngine.LevelEditor.Interface
{
    public class EditorInterface
    {
        public ImGuiController _controller;

        private Scene scene;

        private int textureScenView = 0;

        public int currentGameObject = -1;
        private int currentProjection = 0;
        private int currentComponent = 0;
        private int currendLightType = 0;
        private int currentSelectMesh = 0;

        private float cutOff = 12.5f;
        private float outCutOff = 17.5f;

        private string[] cameraProjections = { "Perspective", "Orthografic" };
        private string[] lightType = { "Point", "Direction", "Spot" };
        private string[] gameObjectComponents = { "Transform", "Camera render", "Light", "Mesh render" };
        private string[] meshs3dObject = { "Cube", "Sphere", "Cylinder", "Plane" };

        private bool renameGameObject;

        public bool IsScenViewSelected = false;
        public bool IsListObjectsSelected = false;
        public System.Numerics.Vector2 Size;

        public System.Numerics.Vector2 SizeMin;

        public EditorInterface(int width, int height, Scene scene)
        {
            _controller = new ImGuiController(width, height);
            this.scene = scene;
        }

        public void Update(GameWindow baseWindow, float deltaTime)
        {
            _controller.Update(baseWindow, deltaTime);
        }

        public void Draw()
        {
            GL.Viewport(0, 0, _controller.WindowWidth, _controller.WindowHeight);

            GameObjectsListView(scene.GetGameObjects());
            ScenView();
            PropertisObjectView(scene.GetGameObjects());
            AssetView();

            _controller.Render();
        }

        private void AssetView()
        {
            ImGui.Begin("Assets viwer");


            ImGui.End();
        }

        public void GameObjectsListView(List<GameObject> gameObjects)
        {
            ImGui.Begin("List object");

            if (gameObjects == null)
                return;

            if (ImGui.TreeNodeEx("Gameobjects", ImGuiTreeNodeFlags.DefaultOpen))
            {
                foreach (var gameObject in gameObjects)
                {
                    if(gameObject.Id == currentGameObject && renameGameObject)
                    {
                        string name = gameObject.Name;
                        ImGui.InputText("", input: ref name, maxLength: 30);
                        gameObject.Name = name;
                    }
                    else if (ImGui.Selectable(gameObject.Name, false,ImGuiSelectableFlags.AllowDoubleClick))
                    {
                        if(ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
                            scene.GetCamera().GetComponent<CameraRender>().Position = gameObject.GetComponent<TransformComponet>().Position;

                        currentGameObject = gameObject.Id;
                    }
                }
                ImGui.TreePop();

            }

            IsListObjectsSelected = ImGui.IsWindowFocused();
            if (ImGui.IsMouseClicked(ImGuiMouseButton.Right) && IsListObjectsSelected)
            {
                ImGui.OpenPopup("Add object");
            }
            if (ImGui.BeginPopup("Add object"))
            {
                if (ImGui.Selectable("Add empty object"))
                {
                    GameObject gameObject = new GameObject();
                    scene.AddGameObject(gameObject);

                    ImGui.CloseCurrentPopup();
                }
                ImGui.Spacing();
                ImGui.Separator();
                if (ImGui.BeginMenu("3d objects"))
                {

                    for (int i = 0; i < meshs3dObject.Length; i++)
                    {
                        if (ImGui.MenuItem(meshs3dObject[i]))
                        {
                            GameObject gameObject = new GameObject();
                            MeshRender meshRender = new MeshRender();
                            meshRender.Start();
                            meshRender.AddMeshRange(MeshLoader.LoadMesh(AssetManager.GetMesh(meshs3dObject[i]), gameObject.GetShader()));
                            meshRender.meshPath = AssetManager.GetMesh(meshs3dObject[i]);
                            gameObject.AddComponent(meshRender);
                            gameObject.Name = meshs3dObject[i];

                            scene.AddGameObject(gameObject);
                            return;
                        }
                    }
                    ImGui.EndMenu();
                }
                ImGui.SeparatorText("object");
                if(ImGui.MenuItem("Delete", "Del"))
                {
                    if(currentGameObject >= 0)
                        scene.RemoveGameObjectById(currentGameObject);
                }
                else if(ImGui.MenuItem("Rename", "F2"))
                {

                }
            }

            ImGui.End();
        }

        public void SetTextureScenView(int texture) => textureScenView = texture;

        public void ScenView()
        {
            ImGui.Begin("Scen");
            IsScenViewSelected = ImGui.IsWindowFocused();

            ImGui.Image(textureScenView, ImGui.GetContentRegionAvail(), new System.Numerics.Vector2(0,1), new System.Numerics.Vector2(1, 0));

            Size = ImGui.GetItemRectSize();
            SizeMin = ImGui.GetItemRectMin();

            ImGui.End();
        }

        public void PropertisObjectView(List<GameObject> gameObjects)
        {
            ImGui.Begin("Propertis");
            var size = ImGui.GetWindowSize();

            if (currentGameObject == -1)
                return;

            var gameObject = scene.GetGameObjectById(currentGameObject);

            if (gameObject == null)
                return;

            ImGui.Spacing();
            string name = gameObject.Name;
            ImGui.InputText("", input: ref name, maxLength: 30);
            gameObject.Name = name;
            ImGui.Spacing();

            foreach (var component in gameObject.GetComponents())
            {
                if (ImGui.TreeNodeEx(component.Name, ImGuiTreeNodeFlags.DefaultOpen))
                {
                    if (component.GetType() == typeof(TransformComponet))
                    {
                        var transform = gameObject.GetComponent<TransformComponet>();

                        Vector3f translate = CreateImGuiDragFloat3("Position", transform.Position, 0.05f);

                        if (transform.Position != translate)
                            transform.Position = translate;

                        Vector3f rotation = CreateImGuiDragFloat3("Rotation", transform.Rotation, 0.05f);
                        if (rotation != transform.Rotation)
                            transform.Rotation = rotation;

                        Vector3f scale = CreateImGuiDragFloat3("Scale", transform.Scale, 0.05f);
                        if (scale != transform.Scale)
                            transform.Scale = scale;
                    }
                    else if (component.GetType() == typeof(CameraRender))
                    {
                        var cameraRender = gameObject.GetComponent<CameraRender>();

                        ImGui.Checkbox("Use skybox", ref cameraRender.IsSkyBox);
                        ImGui.Spacing();

                        ImGui.Combo("Projection", ref currentProjection, cameraProjections, cameraProjections.Length);
                        if (currentProjection == 0)
                        {
                            ImGui.Spacing();
                            cameraRender.ProjectionType = ProjectionType.Perspective;
                            cameraRender.Fov = CreateImGuiSloderFloat("FOV", cameraRender.Fov, 3, 179);
                            ImGui.Spacing();
                        }
                        else
                        {
                            cameraRender.ProjectionType = ProjectionType.Orthgrafic;
                            ImGui.Spacing();
                            ImGui.DragFloat("size", ref cameraRender.size);
                            ImGui.Spacing();
                        }
                        ImGui.Text("Clipping Planes");
                        ImGui.DragFloat("Near", ref cameraRender.depthNear, 0.1f, 0.01f, 100);
                        ImGui.Spacing();
                        ImGui.DragFloat("Far", ref cameraRender.depthFar, 0.1f, 0.01f, 100);
                        ImGui.Spacing();
                    }
                    else if (component.GetType() == typeof(LightRender))
                    {
                        var lightRender = gameObject.GetComponent<LightRender>();
                        currendLightType = (int)lightRender.LightType - 1;

                        ImGui.Combo("Light type", ref currendLightType, lightType, lightType.Length);

                        if (currendLightType + 1 == (int)LightType.Point)
                        {
                            gameObject.GetComponent<TransformComponet>().Rotation = new Vector3f(180, 0, 0);
                            gameObject.GetComponent<TransformComponet>().Scale = new Vector3f(0.3f, 0.3f, 0.3f);

                            lightRender.LightType = LightType.Point;
                            ImGui.Spacing();
                            lightRender.Position = gameObject.GetComponent<TransformComponet>().Position;
                            ImGui.Separator();
                            ImGui.Spacing();
                            lightRender.Ambient = new Vector3f(CreateImGuiSloderFloat("Ambient", lightRender.Ambient.X, 0, 1));
                            ImGui.Spacing();
                            lightRender.Diffuse = new Vector3f(CreateImGuiSloderFloat("Diffuse", lightRender.Diffuse.X, 0, 1));
                            ImGui.Spacing();
                            lightRender.Specular = new Vector3f(CreateImGuiSloderFloat("Specular", lightRender.Specular.X, 0, 1));
                            ImGui.Spacing();
                            ImGui.Separator();

                            lightRender.Constant = CreateImGuiSloderFloat("Constant", lightRender.Constant, 0, 1);
                            ImGui.Spacing();
                            lightRender.Linear = CreateImGuiSloderFloat("Linear", lightRender.Linear, 0, 1);
                            ImGui.Spacing();
                            lightRender.Quadratic = CreateImGuiSloderFloat("Quadratic", lightRender.Quadratic, 0, 1);

                            ImGui.Spacing();
                            ImGui.Checkbox("Shadow", ref lightRender.IsShadowUse);
                            ImGui.Spacing();
                            //ImGui.Checkbox("Perspective", ref lightRender.pointShadowBuffer.IsPerspective);
                            {
                            }
                            // ImGui.SliderFloat("Angle", ref lightRender.pointShadowBuffer.angle, 3, 179);
                            ImGui.Spacing();
                            // ImGui.SliderFloat("depthNear", ref lightRender.pointShadowBuffer.depthNear, 1, lightRender.pointShadowBuffer.depthFar - 1);
                            ImGui.Spacing();
                            // ImGui.SliderFloat("depthFar", ref lightRender.pointShadowBuffer.depthFar, lightRender.pointShadowBuffer.depthNear + 1, 100);
                            ImGui.Spacing();
                        }
                        else if (currendLightType + 1 == (int)LightType.Directional)
                        {
                            lightRender.LightType = LightType.Directional;

                            ImGui.Spacing();
                            lightRender.Direction = gameObject.GetComponent<TransformComponet>().Rotation;
                            ImGui.Separator();

                            ImGui.Spacing();
                            lightRender.Ambient = new Vector3f(CreateImGuiSloderFloat("Ambient", lightRender.Ambient.X, 0, 1));
                            ImGui.Spacing();
                            lightRender.Diffuse = new Vector3f(CreateImGuiSloderFloat("Diffuse", lightRender.Diffuse.X, 0, 1));
                            ImGui.Spacing();
                            lightRender.Specular = new Vector3f(CreateImGuiSloderFloat("Specular", lightRender.Specular.X, 0, 1));
                            ImGui.Spacing();

                            ImGui.Spacing();
                            ImGui.Checkbox("Shadow", ref lightRender.IsShadowUse);
                            ImGui.Spacing();
                            //ImGui.SliderFloat("size", ref lightRender.shadowBuffer.size, 0, 8000);
                            ImGui.Spacing();
                            //ImGui.SliderFloat("depthNear", ref lightRender.shadowBuffer.dpthNear, -100, 100);
                            ImGui.Spacing();
                            //ImGui.SliderFloat("depthFar", ref lightRender.shadowBuffer.depthFar, -100, 100);
                            ImGui.Spacing();
                        }
                        else if (currendLightType + 1 == (int)LightType.Spot)
                        {
                            lightRender.LightType = LightType.Spot;

                            ImGui.Spacing();
                            lightRender.Position = gameObject.GetComponent<TransformComponet>().Position;
                            ImGui.Spacing();
                            lightRender.Direction = gameObject.GetComponent<TransformComponet>().Rotation;
                            ImGui.Separator();

                            ImGui.Spacing();
                            lightRender.Ambient = new Vector3f(CreateImGuiSloderFloat("Ambient", lightRender.Ambient.X, 0, 1));
                            ImGui.Spacing();
                            lightRender.Diffuse = new Vector3f(CreateImGuiSloderFloat("Diffuse", lightRender.Diffuse.X, 0, 1));
                            ImGui.Spacing();
                            lightRender.Specular = new Vector3f(CreateImGuiSloderFloat("Specular", lightRender.Specular.X, 0, 1));
                            ImGui.Spacing();
                            ImGui.Separator();

                            lightRender.Constant = CreateImGuiSloderFloat("Constant", lightRender.Constant, 0, 1);
                            ImGui.Spacing();
                            lightRender.Linear = CreateImGuiSloderFloat("Linear", lightRender.Linear, 0, 1);
                            ImGui.Spacing();
                            lightRender.Quadratic = CreateImGuiSloderFloat("Quadratic", lightRender.Quadratic, 0, 1);
                            ImGui.Spacing();
                            cutOff = CreateImGuiSloderFloat("CutOff", cutOff, 0, 100);
                            lightRender.CutOff = MathF.Cos(MathHelper.DegreesToRadians(cutOff));
                            ImGui.Spacing();
                            outCutOff = CreateImGuiSloderFloat("OuterCutOff", outCutOff, 0, 100);
                            lightRender.OuterCutOff = MathF.Cos(MathHelper.DegreesToRadians(outCutOff));

                            ImGui.Spacing();
                            ImGui.Checkbox("Shadow", ref lightRender.IsShadowUse);
                            ImGui.Spacing();
                            //ImGui.SliderFloat("size", ref lightRender.shadowBuffer.size, 0, 8000);
                            ImGui.Spacing();
                            //ImGui.SliderFloat("depthNear", ref lightRender.shadowBuffer.dpthNear, -100, 100);
                            ImGui.Spacing();
                            //ImGui.SliderFloat("depthFar", ref lightRender.shadowBuffer.depthFar, -100, 100);
                            ImGui.Spacing();
                        }
                    }
                    else if (component.GetType() == typeof(MeshRender))
                    {
                        var meshRender = gameObject.GetComponent<MeshRender>();

                        ImGui.Text("Mesh");
                        var buttonPos = ImGui.GetCursorPos();
                        if (ImGui.ArrowButton("Select meshs", ImGuiDir.Right))
                        {
                            ImGui.OpenPopup("Select mesh");
                        }

                        var cursorPos = ImGui.GetCursorPos();

                        ImGui.SetCursorPos(CreateVector2(size.X - 75, buttonPos.Y));
                        ImGui.Text(meshRender.MeshName);

                        if (ImGui.BeginPopup("Select mesh"))
                        {
                            if (ImGui.Combo("Meshes", ref currentSelectMesh, AssetManager.GetMeshes().Values.ToArray(), AssetManager.GetMeshes().Count))
                            {
                                meshRender.meshes = new List<Mesh>();
                                meshRender.AddMeshRange(MeshLoader.LoadMesh(AssetManager.GetMeshes().Values.ToArray()[currentSelectMesh]));
                                meshRender.meshPath = AssetManager.GetMeshes().Values.ToArray()[currentSelectMesh];

                                ImGui.CloseCurrentPopup();
                            }
                        }
                        ImGui.Spacing();
                        ImGui.Separator();
                    }
                    ImGui.Spacing();
                    ImGui.Separator();
                    ImGui.Spacing();
                    ImGui.TreePop();
                }

            }
            if (ImGui.Button("Add component"))
            {
                ImGui.OpenPopup("Add component");
            }

            if (ImGui.BeginPopup("Add component"))
            {
                ImGui.Text("Components");
                ImGui.Spacing();
                if (ImGui.Combo("", ref currentComponent, gameObjectComponents, gameObjectComponents.Length))
                {
                    Component component = ComponentList.GetComponentById(currentComponent);
                    gameObject.AddComponent(component);
                    component.Start();
                    currentComponent = -1;

                    ImGui.CloseCurrentPopup();
                }
            }

            ImGui.End();
        }

        internal void SetWindowResize(int x, int y)
        {
            _controller.WindowResized(x, y);
        }

        public Vector3 CreateImGuiDragFloat3(string lable, Vector3 vector)
        {
            var vector2 = CreateVector3(vector);

            ImGui.DragFloat3(lable, ref vector2);

            return CreateVector3(vector2);
        }

        public Vector3f CreateImGuiDragFloat3(string lable, Vector3f vector, float speed = 1)
        {
            var vector2 = CreateVector3(vector);

            ImGui.DragFloat3(lable, ref vector2, speed);

            return CreateVector3f(vector2);
        }

        public float CreateImGuiSloderFloat(string lable, float value, float min = -1000, float max = 1000)
        {
            ImGui.SliderFloat(lable, ref value, min, max);

            return value;
        }

        public System.Numerics.Vector2 CreateVector2(float x, float y)
        {
            return new System.Numerics.Vector2(x, y);
        }

        public System.Numerics.Vector2 CreateVector2(Vector2 vector2)
        {
            return new System.Numerics.Vector2(vector2.X, vector2.Y);
        }

        public Vector2 CreateVector2(System.Numerics.Vector2 vector2)
        {
            return new Vector2(vector2.X, vector2.Y);
        }

        public System.Numerics.Vector3 CreateVector3(float x, float y, float z)
        {
            return new System.Numerics.Vector3(x, y, z);
        }


        public System.Numerics.Vector3 CreateVector3(Vector3 vector3)
        {
            return new System.Numerics.Vector3(vector3.X, vector3.Y, vector3.Z);
        }
        public System.Numerics.Vector3 CreateVector3(Vector3f vector3)
        {
            return new System.Numerics.Vector3(vector3.X, vector3.Y, vector3.Z);
        }

        public Vector3 CreateVector3(System.Numerics.Vector3 vector3)
        {
            return new Vector3(vector3.X, vector3.Y, vector3.Z);
        }

        public Vector3f CreateVector3f(System.Numerics.Vector3 vector3)
        {
            return new Vector3f(vector3.X, vector3.Y, vector3.Z);
        }

        public void SetScene(Scene scene)
        {
            this.scene = scene;
        }
    }
}
