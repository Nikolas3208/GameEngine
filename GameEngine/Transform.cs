using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Transform
    {
        public Matrix4 Model = Matrix4.Identity;
        protected Matrix4 RotationMat = Matrix4.Identity;
        protected Matrix4 ScaleMat = Matrix4.Identity;

        private Vector3 position;
        private Vector3 rotation;
        private float scale;

        public Vector3 Position { get => position; set { position = value; Model = Matrix4.CreateTranslation(value); } }
        public Vector3 Rotation { get => rotation; set { rotation = value; RotationMat = RotationMat * Matrix4.CreateRotationX(value.X); RotationMat = RotationMat * Matrix4.CreateRotationY(value.Y); RotationMat = RotationMat * Matrix4.CreateRotationZ(value.Z); } }
        public float Scale { get => scale; set { scale = value; ScaleMat = Matrix4.CreateScale(value); } }
    }
}
