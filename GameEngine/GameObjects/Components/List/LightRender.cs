using GameEngine.Core.Renders.Bufers;
using GameEngine.Core.Structs;
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
    public class LightRender : Component
    {
        public ShadowBuffer shadowBuffer;

        public Light Light;

        public bool IsShadowUse = true;

        public override void Start()
        {
            Name = "Light";
            Light = new Light(gameObject.GetComponent<TransformComponet>().Transform, new Vector3(0.5f), new Vector3(0.5f), new Vector3(0.5f), false);
            shadowBuffer = new ShadowBuffer();
            shadowBuffer.Init(2048 * 4, 2048 * 4);
        }

        public void SetLight(Light light) => Light = light;
        public Light GetLight() => Light;

        public override void Update(Shader shader, float deltaTime)
        {
            base.Update(shader, deltaTime);

            Light.Position = gameObject.GetComponent<TransformComponet>().Transform;
        }

        public override void Draw(Shader shader)
        {
            base.Draw(shader);

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
