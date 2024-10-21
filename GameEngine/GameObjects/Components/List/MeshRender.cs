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
            foreach (var mesh in meshes)
            {
                //for (var i = 0; i < mesh.GetMaterial().textures.Count; i++)
                {
                    if (shader.ContainsKey("material.diffuse"))
                        shader.SetInt("material.diffuse", 0);
                    if (shader.ContainsKey("material.specular"))
                        shader.SetInt("material.specular", 1);
                }

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
