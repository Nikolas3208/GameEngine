using GameEngine.Core.Essentials;
using GameEngine.Renders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Resources
{
    public interface IAssetManager
    {
        public bool AddTexture(string texName, string texPath, bool smooth = true);
        public bool RemoveTexture(string texName);
        public TextureLoader GetTexture(string texName);
        public Dictionary<string, TextureLoader> GetTextures();

        public bool AddMaterial(string matName, Material mat);
        public bool RemoveMaterial(string matName);
        public Material GetMaterial(string matName);
        public Dictionary <string, Material> GetMaterials();

        public bool AddShader(string shaderName, string vertPath, string fragPath, string geomPath = "");
        public bool RemoveShader(string shaderName);
        public Shader GetShader(string shaderName);
        public Dictionary<string, Shader> GetShaders();

        public bool AddMesh(string meshName, string meshPath);
        public bool RemoveMesh(string meshName);
        public List<Mesh> GetMesh(string meshName);
        public Dictionary<string ,List<Mesh>> GetMeshes();
    }
}
