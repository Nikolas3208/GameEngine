using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.ResourceLoad
{
    public class LoadObj
    {
        public static Mesh Load(string path)
        {
            List<float> vertices = new List<float>();
            List<float> textureVertices = new List<float>();
            List<float> normals = new List<float>();
            List<uint> vertexIndices = new List<uint>();
            List<uint> textureIndices = new List<uint>();
            List<uint> normalIndices = new List<uint>();

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Unable to open \"" + path + "\", does not exist.");
            }

            using (StreamReader streamReader = new StreamReader(path))
            {
                while (!streamReader.EndOfStream)
                {
                    List<string> words = new List<string>(streamReader.ReadLine().ToLower().Split(' '));
                    words.RemoveAll(s => s == string.Empty);

                    if (words.Count == 0)
                        continue;

                    string type = words[0];
                    words.RemoveAt(0);

                    switch (type)
                    {
                        // vertex
                        case "v":
                            /*vertices.Add(new Vector3(float.Parse(words[0].Replace('.', ',')), float.Parse(words[1].Replace('.', ',')),
                                                    float.Parse(words[2].Replace('.', ','))));*/
                            vertices.Add(float.Parse(words[0].Replace('.', ',')));
                            vertices.Add(float.Parse(words[1].Replace('.', ',')));
                            vertices.Add(float.Parse(words[2].Replace('.', ',')));
                            break;

                        case "vt":
                            //textureVertices.Add(new Vector2(float.Parse(words[0].Replace('.', ',')), float.Parse(words[1].Replace('.', ','))));
                            textureVertices.Add(float.Parse(words[0].Replace('.', ',')));
                            textureVertices.Add(float.Parse(words[1].Replace('.', ',')));
                            break;

                        case "vn":
                            //normals.Add(new Vector3(float.Parse(words[0].Replace('.', ',')), float.Parse(words[1].Replace('.', ',')), float.Parse(words[2].Replace('.', ','))));
                            normals.Add(float.Parse(words[0].Replace('.', ',')));
                            normals.Add(float.Parse(words[1].Replace('.', ',')));
                            normals.Add(float.Parse(words[2].Replace('.', ',')));
                            break;

                        // face
                        case "f":
                            foreach (string w in words)
                            {
                                if (w.Length == 0)
                                    continue;

                                string[] comps = w.Split('/');

                                // subtract 1: indices start from 1, not 0
                                vertexIndices.Add(uint.Parse(comps[0]) - 1);

                                if (comps.Length > 1 && comps[1].Length != 0)
                                    textureIndices.Add(uint.Parse(comps[1]) - 1);

                                if (comps.Length > 2)
                                    normalIndices.Add(uint.Parse(comps[2]) - 1);
                            }
                            break;
                    }
                }
            }

            return new Mesh(vertices, textureVertices, normals, vertexIndices, textureIndices, normalIndices);
        }
    }
}
