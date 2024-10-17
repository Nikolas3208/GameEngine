using GameEngine.Resources;
using GameEngine.Resources.Materials;
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
        public Dictionary<string, BaseMaterial> materials;
        public List<BaseMesh> meshes;

        public override void Start()
        {
            Name = "Mesh render";

            if(meshes == null)
                meshes = new List<BaseMesh>();
            if (materials == null)
            {
                materials = new Dictionary<string, BaseMaterial>();
            }
        }

        public override void Update(BaseShader shader, float deltaTime)
        {
            
        }

        public BaseMaterial GetMaterial(string key)
        {
            if(materials.ContainsKey(key))
                return materials[key];

            return null;
        }

        public override void Draw(BaseShader shader)
        {
            base.Draw(shader);
            if (meshes != null)
                for (int i = 0; i < meshes.Count; i++)
                {
                    if (materials != null && materials.Count > i)
                        if(GetMaterial(meshes[i].MaterialName) != null)
                        GetMaterial(meshes[i].MaterialName).Draw(shader);

                    meshes[i].Draw(PrimitiveType.Triangles);
                }
        }

        public void AddMaterial(BaseMaterial material)
        {
            if(materials != null)
                materials.Add(material.Name, material);
        }
        public void AddMaterialRange(Dictionary<string, BaseMaterial> materials)
        {
            if (this.materials != null)
                foreach (var item in materials)
                {
                    this.materials.Add(item.Key, item.Value);
                }
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
