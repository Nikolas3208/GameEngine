using GameEngine.Core;
using GameEngine.Core.Structs;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameEngine.GameObjects.Components.List
{
    public class TransformComponet : Component
    {
        private Matrix4 positionMatrix = Matrix4.Identity;
        private Matrix4 rotationMatrix = Matrix4.Identity;
        private Matrix4 scaleMatrix = Matrix4.Identity;

        private Vector3f position;
        private Vector3f rotation;
        private Vector3f scale = new Vector3f(1, 1, 1);

        public Vector3f Position 
        {  
            get => position;
            set 
            {
                position = value;
                positionMatrix = Matrix4.CreateTranslation(value.X, value.Y, value.Z);
            } 
        }
        public Vector3f Rotation { get => rotation; set { rotation = value; RotateMatrix(Vector3f.Vector3(value)); } }
        public Vector3f Scale { get => scale; set { scale = value; scaleMatrix = Matrix4.CreateScale(value.X, value.Y, value.Z); } }

        private void RotateMatrix(Vector3 value)
        {
            rotationMatrix = Matrix4.Identity;

            rotationMatrix *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(value.X));
            rotationMatrix *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(value.Y));
            rotationMatrix *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(value.Z));
        }

        public TransformComponet()
        {

        }

        public TransformComponet(Vector3f position, Vector3f rotation, Vector3f scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public override void Start()
        {
            Name = "Transform";
        }

        public override void Update(float deltaTime)
        {
            
        }

        public override void Draw(Shader shader)
        {
            shader.Use();
            shader.SetMatrix4("model", positionMatrix);
            shader.SetMatrix4("scale", scaleMatrix);
            shader.SetMatrix4("rotation", rotationMatrix);
        }
    }
}
