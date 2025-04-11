using GameEngine.Core.Resources.Loaders.MeshLoaders;
using GameEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Resources
{
    public class AssetManager : IAssetManager
    {
        private Dictionary<string, Texture> _textures = [];
        private Dictionary<string, Material> _materials = [];
        private Dictionary<string, Shader> _shaders = [];
        private Dictionary<string, List<Mesh>> _meshs = [];
        public bool AddMaterial(string matName, Material mat)
        {
            if(_materials.ContainsKey(matName))
                return false;

            _materials.Add(matName, mat);
            return true;
        }

        public bool AddMesh(string meshName, string meshPath)
        {
            if (_meshs.ContainsKey(meshName))
            {
                return false;
            }

            var mesh = MeshLoader.LoadMesh(meshPath);
            _meshs.Add(meshName, mesh);
            return true;
        }

        public bool AddShader(string shaderName, string vertPath, string fragPath, string geomPath = "")
        {
            if(_shaders.ContainsKey(shaderName))
                return false;

            _shaders.Add(shaderName, new Shader(vertPath, fragPath, geomPath));
            return true;
        }

        public bool AddTexture(string texName, string texPath, bool smooth = true)
        {
            if (_textures.ContainsKey(texName))
                return false;

            var tex = Texture.LoadFromFile(texPath, smooth);
            if (tex == null)
                return false;

            _textures.Add(texName, tex);
            return true;
        }

        public Material GetMaterial(string matName)
        {
            if(_materials.ContainsKey(matName))
            {
                return _materials[matName];
            }

            throw new Exception("This material name is not contains on map");
        }

        public Dictionary<string, Material> GetMaterials()
        {
            return _materials;
        }

        public List<Mesh> GetMesh(string meshName)
        {
            if(_meshs.ContainsKey(meshName))
            {
                return _meshs[meshName];
            }

            return null;
            throw new Exception("This mesh name is not contains on map");
        }

        public Dictionary<string, List<Mesh>> GetMeshes()
        {
            return _meshs;
        }

        public Shader GetShader(string shaderName)
        {
            if(_shaders.ContainsKey(shaderName))
            {
                return _shaders[shaderName];
            }

            throw new Exception("This shader name is not contains on map");
        }

        public Dictionary<string, Shader> GetShaders()
        {
            return _shaders;
        }

        public Texture GetTexture(string texName)
        {
            if(_textures.ContainsKey(texName))
            {
                return _textures.First(t => t.Key == texName).Value;
            }

            throw new Exception("This texture name is not contains on map");
        }

        public Dictionary<string, Texture> GetTextures()
        {
            return _textures;
        }

        public bool RemoveMaterial(string matName)
        {
            if( _materials.ContainsKey(matName))
            {
                return _materials.Remove(matName);
            }

            return false;
        }

        public bool RemoveMesh(string meshName)
        {
            if(_meshs.ContainsKey(meshName))
            {
                return _meshs.Remove(meshName);
            }

            return false;
        }

        public bool RemoveShader(string shaderName)
        {
            if (_shaders.ContainsKey(shaderName))
            {
                return _shaders.Remove(shaderName);
            }

            return false;
        }

        public bool RemoveTexture(string texName)
        {
            if (_textures.ContainsKey(texName))
            {
                return _textures.Remove(texName);
            }

            return false;
        }
    }
}
