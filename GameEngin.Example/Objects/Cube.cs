using GameEngine.Core.Essentials;
using GameEngine.Core.Resources;
using GameEngine.GameObjects;
using GameEngine.GameObjects.Components;
using GameEngine.GameObjects.Components.List;
using GameEngine.ResourceLoad;
using GameEngine.Resources;
using GameEngine.Resources.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Example.Objects
{
    public class Cube : GameObject
    {
        public Shader shader;

        public Cube(Shader shader = null)
        {
            if (shader != null)
                this.shader = shader;
            else
                this.shader = ShaderLoad.Load(AssetManager.GetShader("shader"));
            Name = "Cube";
        }

        public override void Start()
        {
            Material material = new Material();
            material.AddTexture(TextureType.Diffuse, TextureLoader.LoadTexture(AssetManager.GetTexture("container2")));

            AddComponent(material);
            AddComponent(new VertexRender());

            base.Start();

            VertexRender? vertexRender = (VertexRender)GetComponent(ComponentType.VertexRender);

            vertexRender.AddMesh(AssetManager.GetMesh("Cube"), ShaderLoad.Load(AssetManager.GetShader("shader")));
        }

        public override void Update(float deltaTime, Shader shader = null)
        {
            if(shader != null)
                base.Update(deltaTime, shader);
            else
                base.Update(deltaTime, this.shader);
        }

        public override void Draw(Shader shader)
        {
            if(shader != null)
                base.Draw(shader);
            else
                base.Draw(this.shader);
        }

        public void SetShader(Shader shader) => this.shader = shader;
        public Shader GetShader() => this.shader;
    }
}
