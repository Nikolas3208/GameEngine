using GameEngine.Core.GameObjects;
using GameEngine.Core.Resources;
using GameEngine.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System.ComponentModel;

namespace GameEngine.Core
{
    public abstract class Scene
    {
        protected List<GameObject> gameObjects;
        protected List<Light> lights;

        protected Game? game;
        protected Shader shader;
        protected Camera camera;

        protected AssetManager assetManager;

        public string Name { get; protected set; } = string.Empty;

        public Scene()
        {
            gameObjects = new List<GameObject>();
            lights = new List<Light>();

            assetManager = AssteLoader.CreateManager("Assets");
            shader = assetManager.GetShader("base");

            Name = "Main Scene";
        }

        public void SetGame(Game game)
        {
            this.game = game;
            camera = new Camera(Vector3.UnitZ * 3, 800 / (float)600);
        }

        public void AddGameObject(GameObject gameObject)
        {
            gameObject.SetShader(shader);
            gameObjects.Add(gameObject);
        }
        public T GetGameObject<T>() where T : GameObject
        {
            return (T)gameObjects.FirstOrDefault(g => g.GetType() == typeof(T))!;
        }
        public T GetGameObjectById<T>(Guid id) where T : GameObject
        {
            return (T)gameObjects.FirstOrDefault(g => g.GetType() == typeof(T) && g.Id == id)!;
        }
        public bool RemoveGameObject(GameObject gameObject)
        {
            return gameObjects.Remove(gameObject);
        }

        public List<GameObject> GetAllGameObjects()
        {
            return gameObjects;
        }

        public void AddLight(Light light)
        {
            lights.Add(light);
        }

        public Light? GetLight(Guid lightId)
        {
            return lights.Find(l => l.Id == lightId);
        }

        public bool RemoveLight(Light light)
        {
            return lights.Remove(light);
        }

        public List<Light> GetAllLights()
        {
            return lights;
        }

        public void OnClear()
        {
            gameObjects.Clear();
            lights.Clear();
        }

        public virtual void Start()
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Start();
            }
        }
        public virtual void Update(float deltaTime)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Update(deltaTime);
            }
        }
        public virtual void Draw(float deltaTime)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.GetShader().Use();
                gameObject.GetShader().SetMatrix4("view", camera.GetViewMatrix());
                gameObject.GetShader().SetMatrix4("projection", camera.GetProjectionMatrix());
                gameObject.GetShader().SetVector3("viewPos", camera.Position);
                gameObject.GetShader().SetInt("lightCount", lights.Count);

                for (int i = 0; i < lights.Count; i++)
                {
                    lights[i].Draw(gameObject.GetShader(), i);
                }
                gameObject.Draw(deltaTime);
            }
        }

        public virtual void OnTextInput(TextInputEventArgs e)
        {
            
        }
        public virtual void OnMouseWheel(MouseWheelEventArgs e)
        {

        }
        public virtual void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);

            camera.AspectRatio = e.Width / (float)e.Height;
        }
        public virtual void OnClose(CancelEventArgs e)
        {
            
        }
    }
}
