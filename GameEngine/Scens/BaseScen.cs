using GameEngine.Core.Renders;
using GameEngine.GameObjects;
using GameEngine.GameObjects.List;
using GameEngine.Resources;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Scens
{
    public abstract class BaseScen
    {
        protected List<GameObject> gameObjects { get; set; } = new();
        protected AssetManager? AssetManager { get; set; } = null;

        public string ScenName { get; set; } = "MainScen";

        public void AddGameObject(GameObject gameObject)
        {
            gameObject.Id = gameObjects.Count;
            gameObjects.Add(gameObject);
        }
        public T GetGameObject<T>() where T : GameObject
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.GetType().Equals(typeof(T)))
                    return (T)gameObject;
            }

            return null;
        }
        public GameObject GetGameObjectById(int id)
        {
            if (gameObjects.Where(x => x.Id == id).Count() > 0)
                return gameObjects[id];

            return null;
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            gameObjects.Remove(gameObject);
        }
        public void RemoveGameObjectById(int id)
        {
            gameObjects.RemoveAt(id);
        }

        public virtual void Start(BaseWindow window)
        {
            foreach(var gameObject in gameObjects)
            {
                gameObject.Start();
            }
        }
        public virtual void Update(BaseWindow window, float deltaTime)
        {
            OnUpdateKey(window.KeyboardState, deltaTime);
            OnUpdateMouse(window.MouseState, deltaTime);
        }
        public abstract void Render(BaseWindow window, float deltaTime);
        protected virtual void OnUpdateKey(KeyboardState keyboard, float deltaTime)
        {

        }
        protected virtual void OnUpdateMouse(MouseState mouse, float deltaTime)
        {

        }
    }
}
