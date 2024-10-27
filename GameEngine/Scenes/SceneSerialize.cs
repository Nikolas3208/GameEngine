using GameEngine.Resources;
using OpenTK.Mathematics;
using Orleans.Concurrency;
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
            ReferenceHandler handler = ReferenceHandler.Preserve;
            handler.AsImmutable();

            var options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true, ReferenceHandler = handler };

            string filePath = $"{AssetManager.basePath}Scens\\{scene.Name}.scen";
            using FileStream createStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

            //JsonSerializer.SerializeToUtf8Bytes

            //string json = JsonSerializer.Serialize<Scene>(scene, options);
            string json = JsonSerializer.Serialize(scene, options);

            Console.WriteLine(json);

            byte[] test = new byte[json.Length];
            for (int i = 0; i < test.Length; i++)
            {
                test[i] = (byte)json[i];
            }

            createStream.Write(test);

            //await JsonSerializer.SerializeAsync<Scene>(createStream, scene, options);
        }

        public static async Task<Scene> Deserialize()
        {

            var options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true, ReferenceHandler = ReferenceHandler.Preserve };
            string filePath = $"{AssetManager.basePath}Scens\\MainScen.scen";

            using FileStream createStream = new FileStream(filePath, FileMode.Open);

            Scene scene = JsonSerializer.Deserialize<Scene>(createStream, options);

            return scene;
        }
    }
}
