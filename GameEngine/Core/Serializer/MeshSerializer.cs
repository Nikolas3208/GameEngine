using GameEngine.Graphics;
using OpenTK.Mathematics;

namespace GameEngine.Core.Serializer
{
    public class MeshSerializer
    {
        public static void SaveToFile(Mesh mesh, string path)
        {
            string baseDir = Path.GetDirectoryName(path)!;

            using var fs = new FileStream(path, FileMode.Create);
            using var bw = new BinaryWriter(fs);

            var verts = mesh.GetVertices();
            var inds = mesh.GetIndices();

            bw.Write(mesh.Name);

            // Вершины
            bw.Write(verts!.Length);
            foreach (var v in verts)
            {
                bw.Write(v.Position.X);
                bw.Write(v.Position.Y);
                bw.Write(v.Position.Z);
                bw.Write(v.Normal.X);
                bw.Write(v.Normal.Y);
                bw.Write(v.Normal.Z);
                bw.Write(v.TexCoords.X);
                bw.Write(v.TexCoords.Y);
            }

            // Индексы
            bw.Write(inds!.Length);
            foreach (var i in inds)
                bw.Write(i);

            if(mesh.GetMaterial() != null)
            {
                bw.Write(mesh.GetMaterial().Name);

                MaterialSerialize.SaveToFile(mesh.GetMaterial(), Path.Combine(baseDir, mesh.GetMaterial().Name + ".mat"));
            }
        }

        public static Mesh LoadFromFile(string path)
        {
            string baseDir = Path.GetDirectoryName(path)!;

            using var fs = new FileStream(path, FileMode.Open);
            using var br = new BinaryReader(fs);

            string meshName = br.ReadString();

            int vCount = br.ReadInt32();
            var vertices = new Vertex[vCount];

            for (int i = 0; i < vCount; i++)
            {
                var pos = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                var norm = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                var uv = new Vector2(br.ReadSingle(), br.ReadSingle());
                vertices[i] = new Vertex { Position = pos, Normal = norm, TexCoords = uv };
            }

            int iCount = br.ReadInt32();
            var indices = new uint[iCount];
            for (int i = 0; i < iCount; i++)
                indices[i] = br.ReadUInt32();

            string materialName = br.ReadString();

            var mat = MaterialSerialize.LoadFromFile(Path.Combine(baseDir, materialName + ".mat"));

            var mesh = new Mesh(vertices, indices, mat, meshName);

            return mesh;
        }
    }
}
