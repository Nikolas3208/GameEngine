using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Structs
{
    public struct Vertex
    {
        public Vector3 Position = new Vector3();
        public Vector3 Normal = new Vector3();
        public Vector2 TexCoords = new Vector2();

        public Vertex(Vector3 pos)
        {
            Position = pos;
        }

        public Vertex(Vector3 pos, Vector2 texCoord)
        {
            Position = pos;
            TexCoords = texCoord;
        }

        public Vertex(Vector3 pos, Vector3 normal)
        {
            Position = pos;
            Normal = normal;
        }

        public Vertex(Vector3 pos, Vector2 texCoord, Vector3 normal)
        {
            Position = pos;
            TexCoords = texCoord;
            Normal = normal;
        }
    }
}
