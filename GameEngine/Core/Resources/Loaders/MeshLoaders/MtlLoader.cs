using GameEngine.Graphics;
using ImGuiNET;
using OpenTK.Mathematics;
using System.Globalization;

namespace GameEngine.Core.Resources.Loaders.MeshLoaders
{
    public static class MtlLoader
    {
        public static void Load(string path, ObjModel model)
        {
            string baseDir = Path.GetDirectoryName(path)!;

            Material current = new Material();

            foreach (var line in File.ReadLines(path))
            {
                var parts = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) continue;

                switch (parts[0])
                {
                    case "newmtl":
                        current = new Material(parts[1]);
                        model.Materials[parts[1]] = current;
                        break;

                    case "Ns":
                        current.Shininess = float.Parse(parts[1]) / 10;  
                        break;

                    case "Kd":
                        current.Diffuse = new Vector3(
                            float.Parse(parts[1], CultureInfo.InvariantCulture),
                            float.Parse(parts[2], CultureInfo.InvariantCulture),
                            float.Parse(parts[3], CultureInfo.InvariantCulture)
                        );
                        break;

                    case "Ka":
                        current.Ambient = new Vector3(
                            float.Parse(parts[1], CultureInfo.InvariantCulture),
                            float.Parse(parts[2], CultureInfo.InvariantCulture),
                            float.Parse(parts[3], CultureInfo.InvariantCulture)
                        );
                        break;

                    case "Ks":
                        current.Specular = new Vector3(
                            float.Parse(parts[1], CultureInfo.InvariantCulture),
                            float.Parse(parts[2], CultureInfo.InvariantCulture),
                            float.Parse(parts[3], CultureInfo.InvariantCulture)
                        );
                        break;

                    case "map_Kd":
                        string texPath = Path.Combine(baseDir, parts[1]);
                        current.TexDiff = Texture.LoadFromFile(texPath);
                        break;

                    case "map_Ks":
                        texPath = Path.Combine(baseDir, parts[1]);
                        current.TexSpec = Texture.LoadFromFile(texPath);
                        break;

                    case "map_Bump":
                        texPath = Path.Combine(baseDir, parts[1]);
                        current.TexNorm = Texture.LoadFromFile(texPath);
                        break;
                }
            }
        }
    }
}
