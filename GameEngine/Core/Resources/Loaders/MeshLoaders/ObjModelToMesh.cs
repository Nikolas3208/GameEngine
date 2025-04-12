using GameEngine.Graphics;
using GameEngine.Graphics.Bufers;
using OpenTK.Mathematics;

namespace GameEngine.Core.Resources.Loaders.MeshLoaders
{
    public class ObjModelToMesh
    {
        public static List<Mesh> Convert(ObjModel objModel)
        {
            var meshes = new Dictionary<string, Mesh>();

            List<Vertex> vertices = new List<Vertex>();
            List<uint> indices = new List<uint>();

            foreach (var face in objModel.Faces)
            {
                string matName = face.MaterialName ?? "default";

                if (!meshes.ContainsKey(matName))
                {
                    meshes[matName] = new Mesh();
                    if (objModel.Materials.TryGetValue(matName, out var material))
                    {
                        meshes[matName].SetMaterial(new Material(material));
                    }
                    else
                    {
                        meshes[matName].SetMaterial(Material.Default);
                    }
                }

                uint baseIndex = meshes[matName].GetVerticesCount();

                foreach (var fv in face.Vertices)
                {
                    var v = objModel.Vertices[fv.VertexIndex];
                    var t = fv.TexCoordIndex >= 0 ? objModel.TexCoords[fv.TexCoordIndex] : new Vector2();
                    var n = fv.NormalIndex >= 0 ? objModel.Normals[fv.NormalIndex] : new Vector3();

                    vertices.Add(new Vertex(v, t, n));
                }

                // Допустим, все грани — треугольники
                for (uint i = 0; i < face.Vertices.Count - 2; i++)
                {
                    indices.Add(baseIndex);
                    indices.Add(baseIndex + i + 1);
                    indices.Add(baseIndex + i + 2);
                }

                VertexBuffer vertexBuffer = new VertexBuffer(vertices.ToArray());
                IndexBuffer indexBuffer = new IndexBuffer(indices.ToArray());
                VertexArray vertexArray = new VertexArray(vertexBuffer, indexBuffer);

                meshes[matName] = new Mesh(vertexArray, meshes[matName].GetMaterial(), objModel.Name);
            }

            return meshes.Values.ToList();
        }
    }
}
