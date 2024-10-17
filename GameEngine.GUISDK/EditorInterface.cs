using GameEngine.GameObjects;
using GameEngine.GameObjects.Components;
using GameEngine.GameObjects.Components.List;
using GameEngine.Windws;
using ImGuiNET;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.LevelEditor
{
    public class EditorInterface
    {
        public ImGuiController _controller;

        private int textureScenView = 0;

        private int currentGameObject = 1;
        private int currentProjection = 0;
        private int currentComponent = 0;


        private string[] cameraProjections = { "Perspective", "Orthografic" };
        private string[] gameObjectComponents = { "Transform", "Camera render", "Light", "Mesh render" };

        public bool IsScenViewSelected = false;


        public EditorInterface(int width, int height) 
        {
            _controller = new ImGuiController(width, height);
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

            _controller.Render();
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
        }

        public void SetTextureScenView(int texture) => textureScenView = texture;

        public void ScenView()
        {
            ImGui.Begin("Scen");
            IsScenViewSelected = ImGui.IsWindowFocused();

            ImGui.Image(textureScenView, ImGui.GetContentRegionAvail(), CreateVector2(0, 1), CreateVector2(1, 0));

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
                    if(component.GetType() == typeof(TransformComponet))
                    {
                        var transform = gameObject.GetComponent<TransformComponet>();

                        transform.Transform = CreateImGuiDragFloat3("Position", transform.Transform);
                        transform.Rotation = CreateImGuiDragFloat3("Rotation", transform.Rotation);
                        transform.Scale = CreateImGuiDragFloat3("Scale", transform.Scale);
                    }
                    else if(component.GetType() == typeof(CameraRender))
                    {
                        var cameraRender = gameObject.GetComponent<CameraRender>();

                        ImGui.Checkbox("Use skybox", ref cameraRender.IsSkyBox);
                        ImGui.Spacing();

                        ImGui.Combo("Projection", ref currentProjection, cameraProjections, cameraProjections.Length);
                        if(currentProjection == 0)
                        {
                            ImGui.Spacing();
                            cameraRender.Fov = CreateImGuiSloderFloat("FOV", cameraRender.Fov, 3, 179);
                            ImGui.Spacing();
                        }
                        else
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
                    }
                    else if(component.GetType() == typeof(MeshRender))
                    {

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
