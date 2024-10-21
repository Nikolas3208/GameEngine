using GameEngine.Resources.Textures;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Structs
{
    public struct Material
    {
        public int Id;
        public float Shininess = 32.0f;

        public int Diffuse = 0;
        public int Specular = 1;

        public BaseTexture DiffuseTexture;
        public BaseTexture SpecularTexture;

        public Vector3 Color = new Vector3(1, 1, 1);

        public Material()
        {

        }
    }
}
