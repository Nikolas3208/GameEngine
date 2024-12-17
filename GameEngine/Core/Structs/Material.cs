using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameEngine.Core.Structs
{
    public struct Material
    {
        public int Id;
        public string Name;
        public float Shininess = 32.0f;

        public List<Texture> textures;

        public Vector3f Color = Vector3f.One;

        [JsonConstructor]
        public Material(List<Texture> textures)
        {
            this.textures = textures;
        }

        public Material()
        {
            textures = new List<Texture>();
        }
    }
}
