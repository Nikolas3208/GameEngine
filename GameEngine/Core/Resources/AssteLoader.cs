namespace GameEngine.Core.Resources
{
    public class AssteLoader
    {
        public static AssetManager CreateManager(string path)
        {
            var manager = new AssetManager();

            var dir = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);

            foreach (var img in dir)
            {
                string name = Path.GetFileNameWithoutExtension(img);
                manager.AddTexture(name, img);
            }

            dir = Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories);

            foreach (var img in dir)
            {
                string name = Path.GetFileNameWithoutExtension(img);
                manager.AddTexture(name, img);
            }

            dir = Directory.GetFiles(path, "*.obj", SearchOption.AllDirectories);

            foreach (var img in dir)
            {
                string name = Path.GetFileNameWithoutExtension(img);
                manager.AddMesh(name, img);
            }

            dir = Directory.GetFiles("Shaders\\", "*", SearchOption.AllDirectories);

            string shaderPath = null;
            for (int i = 0; i < dir.Length; i++)
            {
                var imgPath = dir[i].Split("\\");
                for (int j = 0; j < imgPath.Length; j++)
                {
                    if (imgPath[j].Contains('.'))
                    {
                        var name = imgPath[j].Split('.');
                        if (shaderPath == null)
                            shaderPath = dir[i];
                        else
                        {
                            manager.AddShader(name[0], dir[i], shaderPath);
                            shaderPath = null;
                        }
                    }
                }
            }


            return manager;
        }
    }
}
