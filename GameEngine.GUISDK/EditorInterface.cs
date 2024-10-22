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
using GameEngine.Scens;
using GameEngine.Widgets;
using GameEngine.Core.Structs;
using GameEngine.Core.Renders;

namespace GameEngine.LevelEditor
{
    public class EditorInterface
    {
        public ImGuiController _controller;

        private EditorScen scen;

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

        public bool IsScenViewSelected = false;
        public bool IsListObjectsSelected = false;
        public System.Numerics.Vector2 Size;

        public System.Numerics.Vector2 SizeMin;


        public EditorInterface(int width, int height, EditorScen scen) 
        {
            _controller = new ImGuiController(width, height);
            this.scen = scen;
        }

        public void Update(BaseWindow baseWindow, float deltaTime)
        {
            _controller.Update(baseWindow, deltaTime);
        }

        public void Draw(List<GameObject> gameObjects)
        {
            GameObjectsListView(gameObjects);
            ScenView();
            PropertisObjectView(gameObjects);
            AssetView();

            _controller.Render();
        }

        private void AssetView()
        {
            ImGui.Begin("Asset");
            ImGui.End();
        }

        public void GameObjectsListView(List<GameObject> gameObjects)
        {
            ImGui.Begin("List object");

            if(ImGui.TreeNodeEx("Gameobjects", ImGuiTreeNodeFlags.DefaultOpen))
            {
                foreach (var gameObject in gameObjects)
                {
                    if (ImGui.Selectable(gameObject.Name))
                        currentGameObject = gameObject.Id;
                }
                ImGui.TreePop();

            }

            IsListObjectsSelected = ImGui.IsWindowFocused();
            if(ImGui.IsMouseClicked(ImGuiMouseButton.Right) && IsListObjectsSelected)
            {
                ImGui.OpenPopup("Add object");
            }
            if(ImGui.BeginPopup("Add object"))
            {
                GameObject gameObject = new GameObject();
                if (ImGui.Selectable("Add empty object"))
                {
                    scen.AddGameObject(gameObject);

                    ImGui.CloseCurrentPopup();
                }
                ImGui.Spacing();
                ImGui.Separator();
                if (ImGui.BeginMenu("3d objects"))
                {
                    MeshRender meshRender = new MeshRender();
                    meshRender.Start();

                    for (int i = 0; i < meshs3dObject.Length; i++)
                    {
                        if (ImGui.MenuItem(meshs3dObject[i]))
                        {
                            meshRender.AddMeshRange(MeshLoader.LoadMesh(AssetManager.GetMesh(meshs3dObject[i]), scen.shader));
                            gameObject.AddComponent(meshRender);
                            gameObject.Name = meshs3dObject[i];

                            scen.AddGameObject(gameObject);
                        }
                    }
                }
            }

            ImGui.End();
        }

        public void SetTextureScenView(int texture) => textureScenView = texture;

        public void ScenView()
        {
            ImGui.Begin("Scen");
            IsScenViewSelected = ImGui.IsWindowFocused();

            ImGui.Image(textureScenView, ImGui.GetContentRegionAvail(), CreateVector2(0, 1), CreateVector2(1, 0));

            Size = ImGui.GetItemRectSize();
            SizeMin = ImGui.GetItemRectMin();

            ImGui.End();
        }

        public void PropertisObjectView(List<GameObject> gameObjects)
        {
            ImGui.Begin("Propertis");

            if (currentGameObject == -1)
                return;

            var gameObject = gameObjects[currentGameObject];

            foreach (var component in gameObject.GetComponents())
            {
                if (ImGui.TreeNodeEx(component.Name, ImGuiTreeNodeFlags.DefaultOpen))
                {
                    if (component.GetType() == typeof(TransformComponet))
                    {
                        var transform = gameObject.GetComponent<TransformComponet>();

                        Vector3 translate = CreateImGuiDragFloat3("Position", transform.Transform);

                        if (transform.Transform != translate)
                            transform.Transform = translate;

                        Vector3 rotation = CreateImGuiDragFloat3("Rotation", transform.Rotation);
                        if (rotation != transform.Rotation)
                            transform.Rotation = rotation;

                        Vector3 scale = CreateImGuiDragFloat3("Scale", transform.Scale);
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
                        var light = lightRender.GetLight();

                        ImGui.Combo("Light type", ref currendLightType, lightType, lightType.Length);

                        if (currendLightType + 1 == (int)LightType.Point)
                        {
                            light.Type = LightType.Point;
                            ImGui.Spacing();
                            light.Position = CreateImGuiDragFloat3("Position", light.Position);
                            ImGui.Separator();
                            ImGui.Spacing();
                            light.Ambient = new Vector3(CreateImGuiSloderFloat("Ambient", light.Ambient.X, 0, 1));
                            ImGui.Spacing();
                            light.Diffuse = new Vector3(CreateImGuiSloderFloat("Diffuse", light.Diffuse.X, 0, 1));
                            ImGui.Spacing();
                            light.Specular = new Vector3(CreateImGuiSloderFloat("Specular", light.Specular.X, 0, 1));
                            ImGui.Spacing();
                            ImGui.Separator();

                            light.Constant = CreateImGuiSloderFloat("Constant", light.Constant, 0, 1);
                            ImGui.Spacing();
                            light.Linear = CreateImGuiSloderFloat("Linear", light.Linear, 0, 1);
                            ImGui.Spacing();
                            light.Quadratic = CreateImGuiSloderFloat("Quadratic", light.Quadratic, 0, 1);
                        }
                        else if (currendLightType + 1 == (int)LightType.Directional)
                        {
                            light.Type = LightType.Directional;

                            ImGui.Spacing();
                            light.Direction = CreateImGuiDragFloat3("Direction", light.Direction);
                            ImGui.Separator();

                            ImGui.Spacing();
                            light.Ambient = new Vector3(CreateImGuiSloderFloat("Ambient", light.Ambient.X, 0, 1));
                            ImGui.Spacing();
                            light.Diffuse = new Vector3(CreateImGuiSloderFloat("Diffuse", light.Diffuse.X, 0, 1));
                            ImGui.Spacing();
                            light.Specular = new Vector3(CreateImGuiSloderFloat("Specular", light.Specular.X, 0, 1));
                            ImGui.Spacing();
                        }
                        else if (currendLightType + 1 == (int)LightType.Spot)
                        {
                            light.Type = LightType.Spot;

                            ImGui.Spacing();
                            light.Position = CreateImGuiDragFloat3("Position", light.Position);
                            ImGui.Spacing();
                            light.Direction = CreateImGuiDragFloat3("Direction", light.Direction);
                            ImGui.Separator();

                            ImGui.Spacing();
                            light.Ambient = new Vector3(CreateImGuiSloderFloat("Ambient", light.Ambient.X, 0, 1));
                            ImGui.Spacing();
                            light.Diffuse = new Vector3(CreateImGuiSloderFloat("Diffuse", light.Diffuse.X, 0, 1));
                            ImGui.Spacing();
                            light.Specular = new Vector3(CreateImGuiSloderFloat("Specular", light.Specular.X, 0, 1));
                            ImGui.Spacing();
                            ImGui.Separator();

                            light.Constant = CreateImGuiSloderFloat("Constant", light.Constant, 0, 1);
                            ImGui.Spacing();
                            light.Linear = CreateImGuiSloderFloat("Linear", light.Linear, 0, 1);
                            ImGui.Spacing();
                            light.Quadratic = CreateImGuiSloderFloat("Quadratic", light.Quadratic, 0, 1);
                            ImGui.Spacing();
                            cutOff = CreateImGuiSloderFloat("CutOff", cutOff, 0, 100);
                            light.CutOff = MathF.Cos(MathHelper.DegreesToRadians(cutOff));
                            ImGui.Spacing();
                            outCutOff = CreateImGuiSloderFloat("OuterCutOff", outCutOff, 0, 100);
                            light.OuterCutOff = MathF.Cos(MathHelper.DegreesToRadians(outCutOff));
                        }

                        lightRender.SetLight(light);
                    }
                    else if(component.GetType() == typeof(MeshRender))
                    {
                        var meshRender = gameObject.GetComponent<MeshRender>();

                        ImGui.Text("Mesh");
                        if(ImGui.ArrowButton("Button", ImGuiDir.Right))
                        {
                            ImGui.OpenPopup("Select mesh");
                        }

                        if (ImGui.BeginPopup("Select mesh"))
                        {
                            if(ImGui.Combo("Meshes", ref currentSelectMesh, AssetManager.GetMeshes().Values.ToArray(), AssetManager.GetMeshes().Count))
                            {
                                meshRender.meshes = new List<Mesh>();
                                meshRender.AddMeshRange(MeshLoader.LoadMesh(AssetManager.GetMeshes().Values.ToArray()[currentSelectMesh], scen.shader));

                                ImGui.CloseCurrentPopup();
                            }
                        }
                    }
                    ImGui.Spacing();
                    ImGui.Separator();
                    ImGui.Spacing();
                    ImGui.TreePop();
                }

            }
            if(ImGui.Button("Add component"))
            {
                ImGui.OpenPopup("Add component");
            }

            if(ImGui.BeginPopup("Add component"))
            {
                if (ImGui.Combo("", ref currentComponent, gameObjectComponents, gameObjectComponents.Length))
                {
                    Component component = ComponentList.GetComponentById(currentComponent);
                    component.Start();

                    gameObject.AddComponent(component);

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

        public Vector3 CreateVector3(System.Numerics.Vector3 vector3)
        {
            return new Vector3(vector3.X, vector3.Y, vector3.Z);
        }
    }
}
