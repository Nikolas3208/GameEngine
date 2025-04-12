using System.Numerics;

namespace GameEngine.Core
{
    public class VectorConverter
    {
        public static Vector4 ToVector4System(OpenTK.Mathematics.Vector4 v)
        {
            return new Vector4(v.X, v.Y, v.Z, v.W);
        }
        public static Vector3 ToVector3System(OpenTK.Mathematics.Vector3 v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        public static Vector2 ToVector2System(OpenTK.Mathematics.Vector2 v)
        {
            return new Vector2(v.X, v.Y);
        }

        public static OpenTK.Mathematics.Vector4 ToVector4OpenTk(Vector4 v)
        {
            return new OpenTK.Mathematics.Vector4(v.X, v.Y, v.Z, v.W);
        }

        public static OpenTK.Mathematics.Vector3 ToVector3OpenTK(Vector3 v)
        {
            return new OpenTK.Mathematics.Vector3(v.X, v.Y, v.Z);
        }

        public static OpenTK.Mathematics.Vector2 ToVector2OpenTK(Vector2 v)
        {
            return new OpenTK.Mathematics.Vector2(v.X, v.Y);
        }
    }
}
