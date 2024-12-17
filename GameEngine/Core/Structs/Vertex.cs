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
        public Vector3f Position = new Vector3f();
        public Vector3f Normal = new Vector3f();
        public Vector2f TexCoords = new Vector2f();

        public Vertex(Vector3f pos)
        {
            Position = pos;
        }

        public Vertex(Vector3f pos, Vector2f texCoord)
        {
            Position = pos;
            TexCoords = texCoord;
        }

        public Vertex(Vector3f pos, Vector3f normal)
        {
            Position = pos;
            Normal = normal;
        }

        public Vertex(Vector3f pos, Vector2f texCoord, Vector3f normal)
        {
            Position = pos;
            TexCoords = texCoord;
            Normal = normal;
        }
    }
}
