using GameEngine.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Structs
{
    public struct Texture
    {
        public int Id;
        public TextureType Type;
        public string Path;

        public Texture()
        {
            
        }

        public Texture(int id, TextureType type)
        {
            Id = id;
            Type = type;
        }
    }
}
