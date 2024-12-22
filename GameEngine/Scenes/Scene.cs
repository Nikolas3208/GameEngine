using GameEngine.Core;
using GameEngine.Core.Structs;
using GameEngine.GameObjects;
using GameEngine.GameObjects.Components.List;
using GameEngine.GameObjects.List;
using GameEngine.Renders;
using GameEngine.Resources;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameEngine.Scenes
{
    public class Scene
    {
        [JsonInclude]
        private List<GameObject> gameObjects;

        [JsonIgnore]
        private GameObject ScenCamera = null;
        public string Name { get; set; } = "MainScen";

        public Scene()
        {
            gameObjects = new List<GameObject>();

            GL.Enable(EnableCap.DepthTest);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.CullFace);
            //GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.Texture2D);
        }

        public void SetCamera(Camera camera) { if (ScenCamera != null) { RemoveGameObject(ScenCamera); } ScenCamera = camera; }
        public Camera GetCamera() => (Camera)ScenCamera;

        public void AddGameObject(GameObject gameObject)
        {
            int index = 2;
            foreach (GameObject go in gameObjects)
            {
                if(go.Name == gameObject.Name)
                {
                    if (index == 2)
                    {
                        gameObject.Name += $" ({index})";
                    }
                    else
                    {
                        gameObject.Name = gameObject.Name.Replace($"({index - 1})", $"({index})");
                    }
                    index++;
                }
            }
            gameObject.Id = gameObjects.Count;
            gameObjects.Add(gameObject);
        }
        public T GetGameObject<T>() where T : GameObject
        {
            return (T)gameObjects.Where(g => g.GetType() == typeof(T));
        }
        public GameObject GetGameObjectById(int id)
        {
            if (gameObjects.Where(x => x.Id == id).Count() > 0)
                return gameObjects[id];

            return null;
        }

        public List<GameObject> GetGameObjects() { return gameObjects; }

        public void RemoveGameObject(GameObject gameObject)
        {
            gameObjects.Remove(gameObject);
        }
        public void RemoveGameObjectById(int id)
        {
            gameObjects.RemoveAt(id);
        }

        public void Start()
        {
            if (ScenCamera != null)
                ScenCamera.Start();
            foreach (var gameObject in gameObjects)
            {
                if (ScenCamera == null && gameObject.GetComponent<CameraRender>() != null)
                { 
                    var camera = gameObject.GetComponent<CameraRender>();

                    ScenCamera = new Camera(Vector3f.UnitZ * 3, camera.Aspect);
                }

                gameObject.Start();
            }
        }

        [JsonIgnore]
        public GameObject light = null;
        public void Update(float deltaTime)
        {
            if (ScenCamera != null)
                ScenCamera.Update(deltaTime);
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.GetComponent<LightRender>() != null && light == null)
                {
                    light = gameObject;
                }
                gameObject.Update(deltaTime);
            }
        }

        public void Draw(Shader shader = null)
        {
            if (shader == null)
            {
                if (ScenCamera != null)
                    ScenCamera.Draw();
                foreach (var gameObject in gameObjects)
                {
                    if (ScenCamera != null)
                        ScenCamera.GetComponent<CameraRender>().Draw(gameObject.GetShader());
                    if (light != null)
                        light.GetComponent<LightRender>().Draw(gameObject.GetShader());
                    gameObject.GetShader().SetInt("useShadow", 0);
                    gameObject.Draw();
                }
            }
            else
            {
                if (ScenCamera != null)
                    ScenCamera.Draw();
                foreach (var gameObject in gameObjects)
                {
                    if (ScenCamera != null)
                        ScenCamera.GetComponent<CameraRender>().Draw(shader);
                    gameObject.Draw(shader);
                }
            }
        }
    }
}
