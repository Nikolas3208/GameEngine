using Assimp;
using OpenTK.Mathematics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Models
{
    public static class MathHelper
    {
        public static Vector3 FromVector(Vector3D vec)
        {
            Vector3 v;
            v.X = vec.X;
            v.Y = vec.Y;
            v.Z = vec.Z;
            return v;
        }

        public static Vector2 FromVector2D(Vector3D vec)
        {
            Vector2 v;
            v.X = vec.X;
            v.Y = vec.Y;
            return v;
        }
    }
}
