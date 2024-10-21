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
        public List<BaseMesh> meshes;

        public override void Start()
        {
            Name = "Mesh render";

            if(meshes == null)
                meshes = new List<BaseMesh>();
        }

        public override void Update(BaseShader shader, float deltaTime)
        {
            
        }

        public override void Draw(BaseShader shader)
        {
            foreach (var mesh in meshes)
            {
                mesh.Draw(PrimitiveType.Triangles, shader);
            }
            base.Draw(shader);
        }

        public void AddMesh(BaseMesh mesh)
        {
            if(meshes != null)
                meshes.Add(mesh);
        }

        public void AddMeshRange(List<BaseMesh> meshes)
        {
            if(this.meshes != null)
                this.meshes.AddRange(meshes);
        }
    }
}
