using GameEngine.Graphics;
using GameEngine.Graphics.Bufers;
using OpenTK.Mathematics;

namespace GameEngine.Core.Resources.Loaders.MeshLoaders
{
    public class MeshLoader
    {
        public static List<Mesh> LoadMesh(string path)
        {
            var obj = ObjLoader.Load(path);

            var mesh = ObjModelToMesh.Convert(obj);

            return mesh;
        }
    }
}