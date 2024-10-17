using GameEngine.Resources.Shaders;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameObjects.Components.List
{
    public enum LightType
    {
        Point = 1,
        Directional = 2,
        Spot = 3
    }
    public class Light : Component
    {
        public LightType LightType { get; set; } = LightType.Point;
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }

        public override void Start()
        {
            Name = "Light";
        }

        public override void Update(BaseShader shader, float deltaTime)
        {
            
        }

        protected float constant = 1;
        protected float linear = 0.027f;
        protected float quadratic = 0.0028f;
        protected float ambient = 0.002f;

        public override void Draw(BaseShader shader)
        {
            base.Draw(shader);
            shader.Use();

            shader.SetVector3("light.position", Position);
            shader.SetVector3("light.direction", Position);

            shader.SetFloat("light.constant", constant);
            shader.SetFloat("light.linear", linear);
            shader.SetFloat("light.quadratic", quadratic);

            shader.SetInt("light.type", (int)LightType);
        }
    }
}
