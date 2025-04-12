using GameEngine.Graphics;
using System.Numerics;

namespace GameEngine.Core.Serializer
{
    public class LightData
    {
        public LightType Type { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }

        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }
        public Vector3 Ambient { get; set; }

        public Vector4 Color { get; set; } = Vector4.One;

        public float Linear { get; set; }
        public float Constant { get; set; }
        public float Quadratic { get; set; }

        public float CutOff { get; set; }
        public float OuterCutOff { get; set; }

        public LightData() { }
        public LightData(LightType type, Vector3 position, Vector3 direction, Vector3 diffuse, Vector3 specular, Vector3 ambient, float linear, float constant, float quadratic, float cutOff, float outerCutOff)
        {
            Type = type;
            Position = position;
            Direction = direction;
            Diffuse = diffuse;
            Specular = specular;
            Ambient = ambient;
            Linear = linear;
            Constant = constant;
            Quadratic = quadratic;
            CutOff = cutOff;
            OuterCutOff = outerCutOff;
        }
        public LightData(Light light)
        {
            Type = light.Type;

            Position = VectorConverter.ToVector3System(light.Position);
            Direction = VectorConverter.ToVector3System(light.Direction);

            Ambient = VectorConverter.ToVector3System(light.Ambient);
            Diffuse = VectorConverter.ToVector3System(light.Diffuse);
            Specular = VectorConverter.ToVector3System(light.Specular);

            Color = VectorConverter.ToVector4System(light.Color);

            Linear = light.Linear;
            Constant = light.Constant;
            Quadratic = light.Quadratic;

            CutOff = light.CutOff;
            OuterCutOff = light.OuterCutOff;
        }

        public Light ToLight()
        {
            var light = new Light(Type);
            light.Ambient = VectorConverter.ToVector3OpenTK(Ambient);
            light.Diffuse = VectorConverter.ToVector3OpenTK(Diffuse);
            light.Specular = VectorConverter.ToVector3OpenTK(Specular);

            light.Color = VectorConverter.ToVector4OpenTk(Color);

            light.Linear = Linear;
            light.Constant = Constant;
            light.Quadratic = Quadratic;

            light.CutOff = CutOff;
            light.OuterCutOff = OuterCutOff;

            return light;
        }
    }
}
