using GameEngine.Core;
using GameEngine.Core.Structs;
using GameEngine.Renders.Bufers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameEngine.GameObjects.Components.List
{
    public class LightRender : Component
    {
        public Light Light;

        public bool IsShadowUse = true;

        public override void Start()
        {
            Name = "Light";
            Light = new Light(gameObject.GetComponent<TransformComponet>().Position, new Vector3f(0.5f), new Vector3f(0.5f), new Vector3f(0.5f), false);
        }

        public void SetLight(Light light) => Light = light;
        public Light GetLight() => Light;

        public override void Update(float deltaTime)
        {
            Light.Position = gameObject.GetComponent<TransformComponet>().Position;
        }

        public override void Draw(Shader shader)
        {
            switch(Light.Type)
            {
                case LightType.Point:
                    PointLightUse(shader);
                    break;
                case LightType.Directional:
                    DirectionLightUse(shader);
                    break;
                case LightType.Spot:
                    SpotLightUse(shader);
                    break;
            }

            shader.SetInt("lightType", (int)Light.Type);
        }

        private void SpotLightUse(Shader shader)
        {
            shader.Use();

            shader.SetVector3("spotLight.position", Light.Position);
            shader.SetVector3("spotLight.direction", Light.Direction);

            shader.SetVector3("spotLight.ambient", Light.Ambient);
            shader.SetVector3("spotLight.diffuse", Light.Diffuse);
            shader.SetVector3("spotLight.specular", Light.Specular);

            shader.SetFloat("spotLight.constant", Light.Constant);
            shader.SetFloat("spotLight.linear", Light.Linear);

            shader.SetFloat("spotLight.cutOff", Light.CutOff);
            shader.SetFloat("spotLight.outerCutOff", Light.OuterCutOff);
        }

        private void DirectionLightUse(Shader shader)
        {
            shader.Use();

            shader.SetVector3("dirLight.direction", Light.Direction);

            shader.SetVector3("dirLight.ambient", Light.Ambient);
            shader.SetVector3("dirLight.diffuse", Light.Diffuse);
            shader.SetVector3("dirLight.specular", Light.Specular);

        }

        private void PointLightUse(Shader shader)
        {
            shader.Use();

            shader.SetVector3("pointLight.position", Light.Position);

            shader.SetVector3("pointLight.ambient", Light.Ambient);
            shader.SetVector3("pointLight.diffuse", Light.Diffuse);
            shader.SetVector3("pointLight.specular", Light.Specular);

            shader.SetFloat("pointLight.constant", Light.Constant);
            shader.SetFloat("pointLight.linear", Light.Linear);
            shader.SetFloat("pointLight.quadratic", Light.Quadratic);
        }
    }
}
