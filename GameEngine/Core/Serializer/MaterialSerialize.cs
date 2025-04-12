using GameEngine.Graphics;
using OpenTK.Mathematics;

namespace GameEngine.Core.Serializer
{
    public class MaterialSerialize
    {
        private static Texture _texDiff;
        private static Texture _texSpec;
        private static Texture _texNorm;
        public static void SaveToFile(Material material, string path)
        {
            using var fs = new FileStream(path, FileMode.Create);
            using var bw = new BinaryWriter(fs);

            bw.Write(material.Name);

            bw.Write(material.Color.X);
            bw.Write(material.Color.Y);
            bw.Write(material.Color.Z);
            bw.Write(material.Color.W);

            bw.Write(material.Ambient.X);
            bw.Write(material.Ambient.Y);
            bw.Write(material.Ambient.Z);

            bw.Write(material.Diffuse.X);
            bw.Write(material.Diffuse.Y);
            bw.Write(material.Diffuse.Z);

            bw.Write(material.Specular.X);
            bw.Write(material.Specular.Y);
            bw.Write(material.Specular.Z);

            bw.Write(material.Shininess);

            if (material.TexDiff != null)
                bw.Write(material.TexDiff.Path ?? "");
            else
                bw.Write(string.Empty);

            if (material.TexSpec != null)
                bw.Write(material.TexSpec.Path ?? "");
            else
                bw.Write(string.Empty);

            if (material.TexNorm != null)
                bw.Write(material.TexNorm.Path ?? "");
            else
                bw.Write(string.Empty);
        }

        public static Material LoadFromFile(string path)
        {
            using var fs = new FileStream(path, FileMode.Open);
            using var br = new BinaryReader(fs);

            var matName = br.ReadString();
            var color = new Vector4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            var ambient = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            var diffuse = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            var specular = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            var shininess = br.ReadSingle();

            var texDiff = br.ReadString();
            var texSpec = br.ReadString();
            var texNorm = br.ReadString();

            if (texDiff != string.Empty)
            {
                _texDiff = Texture.LoadFromFile(texDiff);
            }
            if (texSpec != string.Empty)
            {
                _texSpec = Texture.LoadFromFile(texDiff);
            }
            if (texNorm != string.Empty)
            {
                _texNorm = Texture.LoadFromFile(texDiff);
            }

            var mat = new Material(matName, ambient, diffuse, specular, shininess, _texDiff, _texSpec, _texNorm);

            return mat;
        }
    }
}
