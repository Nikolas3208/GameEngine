using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Resources.Shaders
{
    public static class ShaderLoad
    {
        public static Shader Load(string path)
        {
            if (File.Exists(path + ".geom"))
            {
                return new Shader(path + ".vert", path + ".frag", path + ".geom");
            }
            else
            {
                return new Shader(path + ".vert", path + ".frag");
            }
        }
    }
}
