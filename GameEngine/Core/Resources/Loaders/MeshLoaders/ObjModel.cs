using GameEngine.Graphics;
using OpenTK.Mathematics;

namespace GameEngine.Core.Resources.Loaders.MeshLoaders
{
    public class ObjModel
    {
        public string Name { get; set; } = string.Empty;

        public List<Vector3> Vertices = new();
        public List<Vector2> TexCoords = new();
        public List<Vector3> Normals = new();

        public List<Face> Faces = new();

        public Dictionary<string, Material> Materials = new();
    }

    public class Face
    {
        public List<FaceVertex> Vertices = new();
        public string MaterialName { get; set; } = string.Empty;
    }

    public struct FaceVertex
    {
        public int VertexIndex;
        public int TexCoordIndex;
        public int NormalIndex;

        public FaceVertex(int v, int t, int n)
        {
            VertexIndex = v;
            TexCoordIndex = t;
            NormalIndex = n;
        }
    }
}
