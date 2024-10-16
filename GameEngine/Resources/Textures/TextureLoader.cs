using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Resources.Textures
{
    public class TextureLoader
    {
        public static BaseTexture LoadTexture(string path)
        {
            return BaseTexture.LoadFromFile(path);
        }
    }
}
