using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace GameEngine.Graphics
{
    public class Material
    {
        public Guid Id { get; }
        public string Name { get; } = string.Empty;

        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }

        public float Shininess { get; set; }

        public bool DiffuseTextureUse { get; set; } = false;
        public bool SpecularTextureUse { get; set; } = false;
        public bool NormalTextureUse { get; set; } = false;

        public Texture? TexDiff { get; set; }
        public Texture? TexSpec { get; set; }
        public Texture? TexNorm { get; set; }

        public Vector4 Color { get; set; } = Vector4.One;

        public Material()
        {
            Id = Guid.NewGuid();
        }

        public Material(string name)
        {
            Name = name;

            Id = Guid.NewGuid();
        }

        public Material(Material material)
        {
            Name = material.Name;

            Ambient = material.Ambient;
            Diffuse = material.Diffuse;
            Specular = material.Specular;

            Shininess = material.Shininess;

            TexDiff = material.TexDiff;
            TexSpec = material.TexSpec;
            TexNorm = material.TexNorm;

            Color = material.Color;

            Id = Guid.NewGuid();
        }

        public Material(string name, Vector3 ambient, Vector3 diffuse, Vector3 specular, float shininess)
        {
            Id = Guid.NewGuid();
            Name = name;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
        }

        public Material(string name, Vector3 ambient, Vector3 diffuse, Vector3 specular, float shininess, Texture txDiffuse, Texture txSpecular, Texture txNormal)
        {
            Id = Guid.NewGuid();
            Name = name;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
            TexDiff = txDiffuse;
            TexSpec = txSpecular;
            TexNorm = txNormal;
        }

        public void Draw(Shader shader)
        {
            shader.SetVector3("material.ambient", Ambient);
            shader.SetFloat("material.shininess", Shininess);
            shader.SetVector4("material.color", Color);

            if (TexDiff != null)
            {
                shader.SetInt("material.texDiffuse", 0);
                TexDiff.Use(TextureUnit.Texture0);
            }
            else
            {
                shader.SetVector3("material.diffuse", Diffuse);
            }

            if (TexSpec != null)
            {
                TexSpec.Use(TextureUnit.Texture1);
                shader.SetInt("material.texSpecular", 1);
            }
            else
            {
                shader.SetVector3("material.specular", Specular);
            }

            if (TexNorm != null)
            {
                TexNorm.Use(TextureUnit.Texture2);
                shader.SetInt("material.texNormal", 2);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
        }

        public Material UpdateColor(Vector4 color)
        {
            Color = color;

            return this;
        }

        public static Material Default => new Material("Default", new Vector3(1f), new Vector3(0.5f), new Vector3(0.2f), 22f);
    }
}
