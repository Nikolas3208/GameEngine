using OpenTK.Mathematics;

namespace GameEngine.Graphics
{
    public enum LightType
    {
        Direction = 0,
        Point = 1,
        Spot = 2
    }
    public class Light
    {
        public Guid Id { get; }
        public LightType Type { get; }

        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }

        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }
        public Vector3 Ambient { get; set; }

        public Vector4 Color { get; set; } = Vector4.One;

        public float Linear { get; set; }
        public float Constant {  get; set; }
        public float Quadratic { get; set; }

        public float CutOff { get; set; }
        public float OuterCutOff { get; set; }

        public Light(LightType type)
        {
            Id = Guid.NewGuid();
            Type = type;
        }

        public Light(Vector3 diffuse, Vector3 specular, Vector3 ambient)
        {
            Diffuse = diffuse;
            Specular = specular;
            Ambient = ambient;

            Id = Guid.NewGuid();
            Type = LightType.Direction;
        }

        public Light(Vector3 diffuse, Vector3 specular, Vector3 ambient, float linear, float quadratic, float constant)
        {
            Diffuse = diffuse;
            Specular = specular;
            Ambient = ambient;

            Linear = linear;
            Quadratic = quadratic;
            Constant = constant;

            Id = Guid.NewGuid();
            Type = LightType.Point;
        }

        public Light(Vector3 diffuse, Vector3 specular, Vector3 ambient, float linear, float quadratic, float constant, float cutOff, float outerCutOff)
        {
            Diffuse = diffuse;
            Specular = specular;
            Ambient = ambient;

            Linear = linear;
            Quadratic = quadratic;
            Constant = constant;

            CutOff = cutOff;
            OuterCutOff = outerCutOff;

            Id = Guid.NewGuid();
            Type = LightType.Spot;
        }

        public void Draw(Shader shader)
        {
            shader.SetVector3("light.position", Position);
            shader.SetVector3("light.direction", Direction);

            shader.SetVector3("light.diffuse", Diffuse);
            shader.SetVector3("light.specular", Specular);
            shader.SetVector3("light.ambient", Ambient);

            shader.SetVector4("light.color", Color);

            shader.SetFloat("light.linear", Linear);
            shader.SetFloat("light.constant", Constant);
            shader.SetFloat("light.quadratic", Quadratic);

            shader.SetFloat("light.cutOff", CutOff);
            shader.SetFloat("light.outerCutOff", OuterCutOff);

            shader.SetInt("light.type", (int)Type);
        }

        public void Draw(Shader shader, int index)
        {
            shader.SetVector3($"lights[{index}].position", Position);
            shader.SetVector3($"lights[{index}].direction", Direction);

            shader.SetVector3($"lights[{index}].diffuse", Diffuse);
            shader.SetVector3($"lights[{index}].specular", Specular);
            shader.SetVector3($"lights[{index}].ambient", Ambient);

            shader.SetVector4($"lights[{index}].color", Color);

            shader.SetFloat($"lights[{index}].linear", Linear);
            shader.SetFloat($"lights[{index}].constant", Constant);
            shader.SetFloat($"lights[{index}].quadratic", Quadratic);

            shader.SetFloat($"lights[{index}].cutOff", CutOff);
            shader.SetFloat($"lights[{index}].outerCutOff", OuterCutOff);

            shader.SetInt($"lights[{index}].type", (int)Type);
        }

        public static Light DefaultDirection => new Light(new Vector3(0.5f), new Vector3(0.2f), new Vector3(0.05f));
        public static Light DefaultPoint => new Light(new Vector3(0.8f), new Vector3(1.0f), new Vector3(0.5f), 0.09f, 0.032f, 1f);
        public static Light DefaultSpot => new Light(new Vector3(1.0f), new Vector3(1.0f), new Vector3(0.0f), 0.09f, 0.032f, 1f, MathF.Cos(MathHelper.DegreesToRadians(12.5f)), MathF.Cos(MathHelper.DegreesToRadians(17.5f)));
    }
}
