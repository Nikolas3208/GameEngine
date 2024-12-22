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
        public int LightId = 0;

        public LightType LightType = LightType.Directional;

        public Vector3f Position;
        public Vector3f Direction;
        public Vector3f Ambient;
        public Vector3f Diffuse;
        public Vector3f Specular;

        public float Constant = 1.0f;
        public float Linear = 0.09f;
        public float Quadratic = 0.032f;

        public float CutOff = 12.5f;
        public float OuterCutOff = 17.5f;


        [JsonInclude]
        public bool IsShadowUse = true;

        [JsonInclude]
        public Vector3f ambient;

        public LightRender() { }

        public override void Start()
        {
            Name = "Light";
            //if(GameObject != null)
                //Light = new Light(GameObject.GetComponent<TransformComponet>().Position, new Vector3f(0.5f), new Vector3f(0.5f), new Vector3f(0.5f), false);
        }

        public override void Update(float deltaTime)
        {
            Position = GameObject.GetComponent<TransformComponet>().Position;
        }

        public override void Draw(Shader shader)
        {
            switch(LightType)
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

            shader.SetInt("lightType", (int)LightType);
        }

        private void SpotLightUse(Shader shader)
        {
            shader.Use();

            shader.SetVector3("spotLight.position", Position);
            shader.SetVector3("spotLight.direction", Direction);

            shader.SetVector3("spotLight.ambient", Ambient);
            shader.SetVector3("spotLight.diffuse", Diffuse);
            shader.SetVector3("spotLight.specular", Specular);

            shader.SetFloat("spotLight.constant", Constant);
            shader.SetFloat("spotLight.linear", Linear);

            shader.SetFloat("spotLight.cutOff", CutOff);
            shader.SetFloat("spotLight.outerCutOff", OuterCutOff);
        }

        private void DirectionLightUse(Shader shader)
        {
            shader.Use();

            shader.SetVector3("dirLight.direction", Direction);

            shader.SetVector3("dirLight.ambient", Ambient);
            shader.SetVector3("dirLight.diffuse", Diffuse);
            shader.SetVector3("dirLight.specular", Specular);

        }

        private void PointLightUse(Shader shader)
        {
            shader.Use();

            shader.SetVector3("pointLight.position", Position);

            shader.SetVector3("pointLight.ambient", Ambient);
            shader.SetVector3("pointLight.diffuse", Diffuse);
            shader.SetVector3("pointLight.specular", Specular);

            shader.SetFloat("pointLight.constant", Constant);
            shader.SetFloat("pointLight.linear", Linear);
            shader.SetFloat("pointLight.quadratic", Quadratic);
        }
    }
}
