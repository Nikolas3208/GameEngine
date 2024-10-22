using GameEngine.Core.Renders;
using GameEngine.Resources;
using GameEngine.Resources.Meshes;
using GameEngine.Resources.Shaders;
using GameEngine.Resources.Textures;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GameEngine.GameObjects.Components.List
{
    public class MeshRender : Component
    {
        public List<Mesh> meshes;

        public override void Start()
        {
            Name = "Mesh render";

            if(meshes == null)
                meshes = new List<Mesh>();
        }

        public override void Update(Shader shader, float deltaTime)
        {
            
        }

        public override void Draw(Shader shader)
        {
            shader.SetInt("material.diffuse", 0);
            shader.SetInt("material.specular", 1);
            shader.SetInt("gameObjectId", gameObject.Id);
            foreach (var mesh in meshes)
            {
                shader.SetInt("meshId", mesh.Id);
                mesh.Draw(PrimitiveType.Triangles, shader);
            }
            base.Draw(shader);
        }

        public void AddMesh(Mesh mesh)
        {
            if(meshes != null)
                meshes.Add(mesh);
        }

        public void AddMeshRange(List<Mesh> meshes)
        {
            if(this.meshes != null)
                this.meshes.AddRange(meshes);
        }
    }
}
