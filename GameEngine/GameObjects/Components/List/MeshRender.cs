using GameEngine.Core;
using GameEngine.Core.Essentials;
using GameEngine.Renders;
using GameEngine.Resources;
using GameEngine.Resources.Meshes;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GameEngine.GameObjects.Components.List
{
    public class MeshRender : Component
    {
        public string meshPath = string.Empty;
        [JsonIgnore]
        public List<Mesh> meshes;
        [JsonInclude]
        public string MeshName = "Mesh";

        public MeshRender()
        {
            Name = "Mesh render";

            if (meshes == null)
                meshes = new List<Mesh>();
        }

        public override void Start()
        {
            if(meshes == null || (meshes != null && meshes.Count == 0))
            {
                meshes = new List<Mesh>();
                if(meshPath != string.Empty)
                {
                    AddMeshRange(MeshLoader.LoadMesh(meshPath));
                }
            }
            foreach (var mesh in meshes)
            {
                mesh.InitBuffers();
            }
        }

        public override void Update(float deltaTime)
        {
            
        }

        public override void Draw(Shader shader)
        {
            shader.SetInt("material.diffuse", 0);
            shader.SetInt("material.specular", 1);
            foreach (var mesh in meshes)
            {
                shader.SetInt("meshId", mesh.Id);
                mesh.Draw(PrimitiveType.Triangles, shader);
            }
        }

        public void AddMesh(Mesh mesh)
        {
            if (meshes != null)
            {
                foreach (var mesh1 in meshes)
                {
                    if (mesh.Name == mesh1.Name)
                        return;
                }
                meshes.Add(mesh);
            }
        }

        public void AddMeshRange(List<Mesh> meshes)
        {
            if (this.meshes != null)
            {
                this.meshes.AddRange(meshes);
                MeshName = meshes[0].Name;
            }
        }
    }
}
