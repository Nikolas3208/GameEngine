using Assimp;
using Assimp.Configs;
using GameEngine.Resources.Textures;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Resources.Materials
{
    public class MaterialLoader
    {
        private static Scene scene;
        private static Dictionary<string, BaseMaterial> materials;
        public static Dictionary<string, BaseMaterial> LoadMaterial(string path)
        {
            materials = new Dictionary<string, BaseMaterial>();

            AssimpContext importer = new AssimpContext();
            importer.SetConfig(new NormalSmoothingAngleConfig(0));

            scene = importer.ImportFile(path, PostProcessSteps.Triangulate);
            {
                for (int i = scene.Materials.Count - 1; i > -1; i--)
                {
                    Material material = scene.Materials[i];
                    DefaulMaterial defaulMaterial = new DefaulMaterial();
                    if(material.TextureDiffuse.FilePath != "" && material.TextureDiffuse.FilePath != null)
                        defaulMaterial.AddTexture(TextureType.Diffuse, TextureLoader.LoadTexture(material.TextureDiffuse.FilePath));
                    defaulMaterial.Name = material.Name;
                    //defaulMaterial.diffuse = material.ColorDiffuse.R;
                    //defaulMaterial.shininess = material.Shininess;
                    //defaulMaterial.ambient = material.ColorAmbient.R;
                    materials.Add(material.Name, defaulMaterial);
                }
            }
            return materials;
        }
    }
}
