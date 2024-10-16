using GameEngine.GameObjects.Components.List;
using GameEngine.Resources;
using GameEngine.Resources.Shaders;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameObjects.List
{
    public enum LightType
    {
        PointLight = 1,
        DirectionalLight = 2,
        SpotLight = 3
    }
    public abstract class BaseLight : GameObject
    {
        //Base
        protected Vector3 position;
        protected Vector3 direction;
        protected LightType type;
        protected BaseShadow shadow;
        protected BaseShader shader;
        //PointLight
        protected float constant = 1;
        protected float linear = 0.027f;
        protected float quadratic = 0.0028f;
        protected float ambient = 0.002f;

        //SpotLight
        protected float degrees = 12f;
        protected float cutOff;


        public void AddShadow(BaseShadow shadow) => this.shadow = shadow;
        public void RemoveShadow() => shadow = null;
        public BaseShadow GetShadow() => shadow;

        public void ShadowInit(int width, int height)
        {
            if (shadow != null)
                shadow.Init(width, height);
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update(float deltaTime, BaseShader shader = null)
        {
            base.Update(deltaTime, shader);
        }

        public override void Draw(BaseShader shader)
        {
            base.Draw(shader);
        }
    }
}
