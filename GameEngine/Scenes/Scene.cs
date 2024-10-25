using GameEngine.GameObjects;
using GameEngine.GameObjects.List;
using GameEngine.Renders;
using GameEngine.Resources;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Scenes
{
    public class Scene
    {
        private List<GameObject> gameObjects;
        public string Name { get; set; } = "MainScen";

        public Scene()
        {
            gameObjects = new List<GameObject>();
        }

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
            foreach (var gameObject in gameObjects)
            {
                gameObject.Start();
            }
        }
        public void Update(float deltaTime)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Update(deltaTime);
            }
        }

        public  void Draw()
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Draw();
            }
        }
    }
}
