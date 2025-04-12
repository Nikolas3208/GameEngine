using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameEngine.Core.Serializer
{
    public class SceneSerializer
    {
        public static void Save(string path, Scene scene)
        {
            var sceneData = new SceneData
            {
                GameObjects = scene.GetAllGameObjects().Select(go => new GameObjectData(go)).ToList(),
                Lights = scene.GetAllLights().Select(l => new LightData(l)).ToList()
            };

            var json = JsonSerializer.Serialize(sceneData, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
            File.WriteAllText(path + $"\\{scene.Name}.scene", json);
        }

        public static void Load(Scene scene, string path)
        {
            if (!File.Exists(path))
                return;

            var json = File.ReadAllText(path);
            var options = new JsonSerializerOptions
            {
                IncludeFields = true
            };

            var data = JsonSerializer.Deserialize<SceneData>(json, options);

            scene.OnClear();

            foreach (var goData in data!.GameObjects)
                scene.AddGameObject(goData.ToGameObject(scene));

            foreach (var lightData in data.Lights)
                scene.AddLight(lightData.ToLight());

            scene.Start();
        }

        public static Scene? Load<T>(string path) where T : Scene, new()
        {
            if (!File.Exists(path))
                return null;

            var json = File.ReadAllText(path);
            var options = new JsonSerializerOptions
            {
                IncludeFields = true
            };

            var data = JsonSerializer.Deserialize<SceneData>(json, options);

            var scene = new T();
            scene.OnClear();

            foreach (var goData in data!.GameObjects)
                scene.AddGameObject(goData.ToGameObject(scene));

            foreach (var lightData in data.Lights)
                scene.AddLight(lightData.ToLight());

            return scene;
        }
    }
}
