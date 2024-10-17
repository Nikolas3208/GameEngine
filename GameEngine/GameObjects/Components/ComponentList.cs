using GameEngine.GameObjects.Components.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameObjects.Components
{
    public class ComponentList
    {
        public static Component GetComponentById(int id)
        {
            switch (id)
            {
                case 0:
                    return new TransformComponet();
                case 1:
                    return new CameraRender();
                case 2:
                    return new Light();
                case 3:
                    return new MeshRender();
            }

            return null;
        }
    }
}
