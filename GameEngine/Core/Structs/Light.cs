using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Structs
{
    public enum LightType
    {
        Point = 1,
        Directional = 2,
        Spot = 3
    }
    public struct Light
    {
        public int Id;
        public LightType Type;

        public Vector3 Position;
        public Vector3 Direction;

        public Vector3 Ambient = new Vector3(0.2f);
        public Vector3 Diffuse = new Vector3(0.8f);
        public Vector3 Specular = new Vector3(1.0f);

        public float Constant = 1.0f;
        public float Linear = 0.09f;
        public float Quadratic = 0.032f;

        public float CutOff = 12.5f;
        public float OuterCutOff = 17.5f;

        public Light() { }

        public Light(Vector3 position, Vector3 ambient, Vector3 diffuse, Vector3 specular, bool isPointLight)
        {
            if(isPointLight)
            {
                Type = LightType.Point;
                Position = position;
                Ambient = ambient;
                Diffuse = diffuse;
                Specular = specular;
            }
            else
            {
                Type = LightType.Directional;
                Direction = position;
                Ambient = ambient;
                Diffuse = diffuse;
                Specular = specular;
            }

        }

        public Light(Vector3 position, Vector3 direction, Vector3 ambient, Vector3 diffuse, Vector3 specular)
        {
            Type = LightType.Spot;
            Position = position;
            Direction = direction;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
        }
    }
}
