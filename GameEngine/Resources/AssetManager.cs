using GameEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Resources
{
    public class AssetManager
    {
        public string basePath {  get; set; }

        private static Dictionary<string, string> textures;
        private static Dictionary<string, string> meshes;
        private static Dictionary<string, string> shaders;

        public AssetManager(string basePath)
        {
            textures = new Dictionary<string, string>();
            meshes = new Dictionary<string, string>();
            shaders = new Dictionary<string, string>();

            this.basePath = basePath;

            Init(basePath);
        }
        string name = "";
        public void Init(string basePath)
        {
            var dir = Directory.GetFiles(basePath, "*.png", SearchOption.AllDirectories);

            for (int i = 0; i < dir.Length; i++)
            {
                int end = dir[i].IndexOf(".");
                for (int j = end - 1; j > 0; j--)
                {
                    char[] args;
                    if (dir[i].ToArray()[j] != '\\')
                    {
                        name += dir[i].ToArray()[j];
                    }
                    else
                    {
                        args = name.ToCharArray();
                        Array.Reverse(args);
                        name = new string(args);
                        break;
                    }
                }
                if(!textures.ContainsKey(name))
                    textures.Add(name, dir[i]);
                name = "";
            }

            dir = Directory.GetFiles(basePath, "*.jpg", SearchOption.AllDirectories);

            for (int i = 0; i < dir.Length; i++)
            {
                int end = dir[i].IndexOf(".");
                for (int j = end - 1; j > 0; j--)
                {
                    char[] args;
                    if (dir[i].ToArray()[j] != '\\')
                    {
                        name += dir[i].ToArray()[j];
                    }
                    else
                    {
                        args = name.ToCharArray();
                        Array.Reverse(args);
                        name = new string(args);
                        break;
                    }
                }
                textures.Add(name, dir[i]);
                name = "";
            }

            dir = Directory.GetFiles("Shaders\\", "*", SearchOption.AllDirectories);

            for (int i = 0; i < dir.Length; i++)
            {
                int end = dir[i].IndexOf(".");
                for (int j = end - 1; j > 0; j--)
                {
                    char[] args;
                    if (dir[i].ToArray()[j] != '\\')
                    {
                        name += dir[i].ToArray()[j];
                    }
                    else
                    {
                        args = name.ToCharArray();
                        Array.Reverse(args);
                        name = new string(args);
                        break;
                    }
                }
                int start = dir[i].IndexOf(".");

                if(!shaders.ContainsKey(name))
                    shaders.Add(name, dir[i].Substring(0, start));

                name = "";
            }

            dir = Directory.GetFiles(basePath, "*.obj", SearchOption.AllDirectories);

            for (int i = 0; i < dir.Length; i++)
            {
                int end = dir[i].IndexOf(".");
                for (int j = end - 1; j > 0; j--)
                {
                    char[] args;
                    if (dir[i].ToArray()[j] != '\\')
                    {
                        name += dir[i].ToArray()[j];
                    }
                    else
                    {
                        args = name.ToCharArray();
                        Array.Reverse(args);
                        name = new string(args);
                        break;
                    }
                }
                meshes.Add(name, dir[i]);
                name = "";
            }

            dir = Directory.GetFiles(basePath, "*.fbx", SearchOption.AllDirectories);

            for (int i = 0; i < dir.Length; i++)
            {
                int end = dir[i].IndexOf(".");
                for (int j = end - 1; j > 0; j--)
                {
                    char[] args;
                    if (dir[i].ToArray()[j] != '\\')
                    {
                        name += dir[i].ToArray()[j];
                    }
                    else
                    {
                        args = name.ToCharArray();
                        Array.Reverse(args);
                        name = new string(args);
                        break;
                    }
                }
                meshes.Add(name, dir[i]);
                name = "";
            }
        }

        public static string GetMesh(string key)
        {
            if(meshes.ContainsKey(key))
                return meshes[key];

            return null;
        }

        public static string GetTexture(string key)
        {
            if (textures.ContainsKey(key))
                return textures[key];

            return null;
        }

        public static Dictionary<string, string> GetTextures()
        {
            return textures;
        }

        public static string GetShader(string key)
        {
            if (shaders.ContainsKey(key))
                return shaders[key];

            return null;
        }

    }
}
