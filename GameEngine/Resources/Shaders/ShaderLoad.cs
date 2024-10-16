using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Resources.Shaders
{
    public static class ShaderLoad
    {
        public static BaseShader Load(string path)
        {
            if (File.Exists(path + ".geom"))
            {
                return new BaseShader(path + ".vert", path + ".frag", path + ".geom");
            }
            else
            {
                return new BaseShader(path + ".vert", path + ".frag");
            }
        }
    }
}
