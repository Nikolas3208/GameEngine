using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Terrain
{
    public class TerrainHeightMap
    {
        public static BaseTerrain InitTerrain(string path)
        {
            Image heightMap = new Image(path);

            return new BaseTerrain((int)heightMap.Size.X, GenerateData(heightMap));
        }

        private static float[,] GenerateData(Image heightMap)
        {
            float[,] data = new float[(int)heightMap.Size.X, (int)heightMap.Size.Y];

            float yScale = 24f / 256f;
            float yShift = 16;

            for (int x = 0; x < heightMap.Size.Y; x++)
            {
                for (int z = 0; z < heightMap.Size.X; z++)
                {
                    float average = (heightMap.GetPixel((uint)z, (uint)x).R + heightMap.GetPixel((uint)z, (uint)x).G + heightMap.GetPixel((uint)z, (uint)x).B) / 3 * yScale + yShift;

                    data[z, x] = average;
                }
            }

            return data;
        }
    }
}
