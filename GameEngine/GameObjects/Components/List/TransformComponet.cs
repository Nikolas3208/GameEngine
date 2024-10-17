using GameEngine.Resources.Shaders;
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
        public Matrix4 tranfsormMatrix = Matrix4.Identity;
        public Matrix4 rotationMatrix = Matrix4.Identity;
        public Matrix4 scaleMatrix = Matrix4.Identity;

        private Vector3 transform;
        private Vector3 rotation;
        private Vector3 scale;

        public Vector3 Transform 
        {  
            get => transform;
            set 
            { 
                transform = value;
                tranfsormMatrix = Matrix4.CreateTranslation(value); 
            } 
        }
        public Vector3 Rotation { get => rotation; set { rotation = value; rotationMatrix *= Matrix4.CreateRotationX(value.X); rotationMatrix *= Matrix4.CreateRotationY(value.Y); rotationMatrix *= Matrix4.CreateRotationZ(value.Z); } }
        public Vector3 Scale { get => scale; set { scale = value; scaleMatrix = Matrix4.CreateScale(value.X); } }

        public override void Start()
        {
            base.Start();
        }

        public override void Draw(BaseShader shader)
        {
            base.Draw(shader);

            shader.Use();
            shader.SetMatrix4("model", tranfsormMatrix);
            shader.SetMatrix4("scale", scaleMatrix);
            shader.SetMatrix4("rotation", rotationMatrix);
        }
    }
}
