using Assimp;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Structs
{
    public struct Vector3f
    {
        public float X; public float Y; public float Z;

        public Vector3f(Vector3 vector)
        {
            X = vector.X; Y = vector.Y; Z = vector.Z;
        }

        public Vector3f(Vector3f vector)
        {
            X = vector.X; Y = vector.Y; Z = vector.Z;
        }
        public Vector3f(float v)
        {
            X = v; Y = v; Z = v;
        }
        public Vector3f(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 Vector3(Vector3f vector)
        {
            return new Vector3 (vector.X, vector.Y, vector.Z);
        }

        public static Vector3f UnitX => new Vector3f(1, 0, 0);
        public static Vector3f UnitY => new Vector3f(0, 1, 0);
        public static Vector3f UnitZ => new Vector3f(0, 0, 1);
        public static Vector3f One => new Vector3f(1, 1, 1);
        public static Vector3f Zero => new Vector3f(0, 0, 0);

        public static Vector3f operator +(Vector3f v1, Vector3f v2) => new Vector3f(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        public static Vector3f operator -(Vector3f v1, Vector3f v2) => new Vector3f(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        public static Vector3f operator -(Vector3f v1) => new Vector3f(-v1.X, -v1.Y, -v1.Z);

        public static Vector3f operator *(Vector3f v1, float v2) => new Vector3f(v1.X * v2, v1.Y * v2, v1.Z * v2);

        public static Vector3f Vector3F(Vector3 vector) => new Vector3f(vector.X, vector.Y, vector.Z);
        public static bool operator ==(Vector3f v1, Vector3f v2)
        {
            if (v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z)
                return true;

            return false;
        }

        public static bool operator !=(Vector3f v1, Vector3f v2)
        {
            if(v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z) 
                return true; 
            
            return false;
        }

        public static float GetDistance(Vector3f vec)   // Определяет длину вектора
        {
            return (float)Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
        }

        public static Vector3f Normalize(Vector3f vec)
        {
            float len = GetDistance(vec);
            vec.X /= len;
            vec.Y /= len;
            return vec;
        }

        public static Vector3f Cross(Vector3f v1, Vector3f v2)
        {
            Vector3f returnValue;
            returnValue.X = v1.Y * v2.X - v1.X * v2.Y;
            returnValue.Y = v1.Z * v2.X - v1.X * v2.Z;
            returnValue.Z = v1.X * v2.Y - v1.Y * v2.X;

            return returnValue;
        }

    }
}
