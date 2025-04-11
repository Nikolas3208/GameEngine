using OpenTK.Mathematics;
using System.Globalization;

namespace GameEngine.Core.Resources.Loaders.MeshLoaders
{
    public class ObjLoader
    {
        public static ObjModel Load(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            var model = new ObjModel();
            string currentMaterial = string.Empty;
            string baseDir = Path.GetDirectoryName(path)!;

            foreach (var line in File.ReadLines(path))
            {
                var parts = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) continue;

                switch (parts[0])
                {
                    case "o":
                        model.Name = parts[1];
                        break;

                    case "v":
                        model.Vertices.Add(ParseVector3(parts));
                        break;

                    case "vt":
                        model.TexCoords.Add(ParseVector2(parts));
                        break;

                    case "vn":
                        model.Normals.Add(ParseVector3(parts));
                        break;

                    case "f":
                        var face = ParceFace(parts);
                        face.MaterialName = currentMaterial;

                        model.Faces.Add(face);
                        break;

                    case "mtllib":
                        string mtlPath = Path.Combine(baseDir, parts[1]);
                        MtlLoader.Load(mtlPath, model);
                        break;

                    case "usemtl":
                        currentMaterial = parts[1];
                        break;
                }
            }

            return model;
        }

        private static Vector3 ParseVector3(string[] parts)
        {
            return new Vector3(
                float.Parse(parts[1], CultureInfo.InvariantCulture),
                float.Parse(parts[2], CultureInfo.InvariantCulture),
                float.Parse(parts[3], CultureInfo.InvariantCulture)
            );
        }

        private static Vector2 ParseVector2(string[] parts)
        {
            return new Vector2(
                float.Parse(parts[1], CultureInfo.InvariantCulture),
                float.Parse(parts[2], CultureInfo.InvariantCulture)
            );
        }

        private static Face ParceFace(string[] parts)
        {
            var face = new Face();

            for (int i = 1; i < parts.Length; i++)
            {
                var indices = parts[i].Split('/');
                var vertexIndex = int.Parse(indices[0]) - 1;
                var texIndex = indices.Length > 1 && indices[1] != "" ? int.Parse(indices[1]) - 1 : -1;
                var normalIndex = indices.Length > 2 ? int.Parse(indices[2]) - 1 : -1;

                face.Vertices.Add(new FaceVertex(vertexIndex, texIndex, normalIndex));
            }

            return face;
        }
    }
}
