using Assimp;
using OpenTK.Mathematics;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Terrain
{
    public class ArraysTerrain
    {
        private List<Vertex> vertices = new List<Vertex>();

        private List<uint> indices = new List<uint>();

        public void InitArrays(ref List<Vertex> vertices, ref List<uint> indices, int size, float[,] data)
        {
            vertices = InitVertices(size, data);
            indices = InitIndices(size);
        }

        private List<uint> InitIndices(int size)
        {
            for (int x = 0; x < size; x++)
            {
                for (int z = 0; z < size; z++)
                {
                    indices.Add((uint)(z * size + x));
                    indices.Add((uint)((z + 1) * size + x));
                    indices.Add((uint)((z + 1) * size + x + 1));
                
                    indices.Add((uint)(z * size + x));
                    indices.Add((uint)((z + 1) * size + x + 1));
                    indices.Add((uint)(z * size + x + 1));
                }
            }

            return indices;
        }

        private List<Vertex> InitVertices(int size, float[,] data)
        {
            float scale = (float)(size / (float)(size * size));

            for (int x = 0; x < size; x++)
            {
                for (int z = 0; z < size; z++)
                {
                    float y = data[z, x];

                    vertices.Add(new Vertex(new Vector3(x, y, z), new Vector2(x * scale, z * scale), new Vector3()));
                }
            }

            return vertices;
        }
    }
}
