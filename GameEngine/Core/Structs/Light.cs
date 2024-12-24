using GameEngine.Core.Enums;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Structs
{
    public struct Light
    {
        public int Id;
        public LightType Type;

        public Vector3f Position;
        public Vector3f Direction;

        public Vector3f Ambient;// = new Vector3f(0.2f);
        public Vector3f Diffuse;// = new Vector3f(0.8f);
        public Vector3f Specular;// = new Vector3f(1.0f);

        public float Constant = 1.0f;
        public float Linear = 0.09f;
        public float Quadratic = 0.032f;

        public float CutOff = 12.5f;
        public float OuterCutOff = 17.5f;

        public Light() { }

        public Light(Vector3f position, Vector3f ambient, Vector3f diffuse, Vector3f specular, bool isPointLight)
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

        public Light(Vector3f position, Vector3f direction, Vector3f ambient, Vector3f diffuse, Vector3f specular)
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
