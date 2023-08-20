using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Terrain
{
    public class TerrainNoisePerlin
    {
        public static BaseTerrain InitTerrain(int size)
        {
            return new BaseTerrain(size, GenerateData(size));
        }

        private static float[,] GenerateData(int size)
        {
            Perlin2D perlin = new Perlin2D();

            float[,] data = new float[size, size];

            float yScale = 24f;
            float yShift = 16;

            for (int x = 0; x < size; x++)
            {
                for (int z = 0; z < size; z++)
                {
                    float average = perlin.Noise(z * 0.01f, x * 0.01f) * yScale + yShift;

                    data[z, x] = average;
                }
            }

            return data;
        }
    }
}
