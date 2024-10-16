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
        private static string[] components;

        public static string[] GetComponentNameList()
        {
            if(components == null)
            {
                components = new string[3];
                for (int i = 0; i < 3; i++)
                {
                    components[i] = ((ComponentType)i).ToString();
                }
            }

            return components;
        }
        public static Component GetComponent(ComponentType type)
        {
            Component component = null;

            switch (type)
            {
                case ComponentType.MeshRender:
                    component = new MeshRender();
                    break;
                case ComponentType.Camera:
                    component = new CameraRender();
                    break;
                case ComponentType.Light:
                    component = new Light();
                    break;
            }

            if(components != null)
                component.Start();

            return component;
        }
    }
}
