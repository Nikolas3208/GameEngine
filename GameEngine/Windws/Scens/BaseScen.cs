using GameEngine.GameObjects;
using GameEngine.GameObjects.List;
using GameEngine.Resources;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Windws.Scens
{
    public abstract class BaseScen
    {
        protected Dictionary<string, GameObject> gameObjects { get; set; }

        public string Name { get; set; }

        public AssetManager AssetManager { get; set; }


        private int i = 1;
        public void AddGameObject(GameObject gameObject)
        {
            if (!gameObjects.ContainsKey(gameObject.Name))
            {
                gameObject.Id = gameObjects.Count;
                gameObjects.Add(gameObject.Name, gameObject);
            }
            else
            {
                gameObject.Id = gameObjects.Count;
                gameObjects.Add(gameObject.Name + $"({i})", gameObject);
                i++;
            }
        }
        public GameObject GetGameObject(string key)
        {
            if(gameObjects.ContainsKey(key))
                return gameObjects[key];

            return null;
        }
        public void RemoveGameObject(string key)
        {
            gameObjects.Remove(key);
        }

        public virtual void Start(BaseWindow window)
        {
            AssetManager = new AssetManager("Assets\\");
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

        public string[] GetNameGameObjects()
        {
            return gameObjects.Keys.ToArray();
        }
    }
}
