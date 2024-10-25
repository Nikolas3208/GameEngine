using GameEngine.Core;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameObjects.Components.List
{
    public class TransformComponet : Component
    {
        private Matrix4 positionMatrix = Matrix4.Identity;
        private Matrix4 rotationMatrix = Matrix4.Identity;
        private Matrix4 scaleMatrix = Matrix4.Identity;

        private Vector3 position;
        private Vector3 rotation;
        private Vector3 scale;

        public Vector3 Position 
        {  
            get => position;
            set 
            {
                position = value;
                positionMatrix = Matrix4.CreateTranslation(value); 
            } 
        }
        public Vector3 Rotation { get => rotation; set { rotation = value; RotateMatrix(value); } }
        public Vector3 Scale { get => scale; set { scale = value; scaleMatrix = Matrix4.CreateScale(value.X); } }

        private void RotateMatrix(Vector3 value)
        {
            rotationMatrix = Matrix4.Identity;

            rotationMatrix *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(value.X));
            rotationMatrix *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(value.Y));
            rotationMatrix *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(value.Z));
        }

        public override void Start()
        {
            base.Start();

            Name = "Transform";
        }

        public override void Draw(Shader shader)
        {
            base.Draw(shader);

            shader.Use();
            shader.SetMatrix4("model", positionMatrix);
            shader.SetMatrix4("scale", scaleMatrix);
            shader.SetMatrix4("rotation", rotationMatrix);
        }
    }
}
