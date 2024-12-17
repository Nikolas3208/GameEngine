using GameEngine.Resources;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameEngine.Scenes
{
    public class SceneSerialize
    {
        public static void Serialize(Scene scene)
        {
            var options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true, ReferenceHandler = ReferenceHandler.Preserve };

            string filePath = $"{AssetManager.basePath}Scens\\{scene.Name}.scen";

            if (File.Exists(filePath))
            {
                string json = JsonSerializer.Serialize(scene, options);

                File.WriteAllText(filePath, json);
            }
        }

        public static async Task<Scene> Deserialize()
        {

            var options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true, ReferenceHandler = ReferenceHandler.Preserve };
            string filePath = $"{AssetManager.basePath}Scens\\MainScen.scen";

            using FileStream createStream = new FileStream(filePath, FileMode.Open);

           return JsonSerializer.Deserialize<Scene>(createStream, options);
        }
    }
}
