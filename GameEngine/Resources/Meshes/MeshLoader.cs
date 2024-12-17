using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Core.Structs;
using System.Xml.Linq;
using OpenTK.Mathematics;
using Assimp;
using Assimp.Configs;
using GameEngine.Core;
using GameEngine.Renders;

namespace GameEngine.Resources.Meshes
{
    public class MeshLoader
    {
        public static Vector3f FromVector(Vector3D vec)
        {
            Vector3f v;
            v.X = vec.X;
            v.Y = vec.Y;
            v.Z = vec.Z;
            return v;
        }

        public static Vector2f FromVector2D(Vector3D vec)
        {
            Vector2f v;
            v.X = vec.X;
            v.Y = vec.Y;
            return v;
        }

        private static Scene scene;

        private static List<Renders.Mesh> meshs = new List<Renders.Mesh>();
        private static List<Vertex> vertices = new List<Vertex>();
        private static List<uint> indices = new List<uint>();

        private static OpenTK.Graphics.OpenGL4.PrimitiveType faceMode = OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles;

        private static Vertex vertex = new Vertex();

        public static List<Renders.Mesh> LoadMesh(string path, Shader shader = null)
        {
            string name = "";
            int end = path.IndexOf(".");
            for (int j = end - 1; j > 0; j--)
            {
                char[] args;
                if (path.ToArray()[j] != '\\')
                {
                    name += path.ToArray()[j];
                }
                else
                {
                    args = name.ToCharArray();
                    Array.Reverse(args);
                    name = new string(args);
                    break;
                }
            }

            meshs = new List<Renders.Mesh>();
            AssimpContext importer = new AssimpContext();
            importer.SetConfig(new NormalSmoothingAngleConfig(0));

            scene = importer.ImportFile(path, PostProcessSteps.Triangulate);
            {
                for (int iM = 0; iM < scene.Meshes.Count; iM++)
                {
                    Assimp.Mesh mesh = scene.Meshes[iM];

                    foreach (Face face in mesh.Faces)
                    {
                        switch (face.IndexCount)
                        {
                            case 1:
                                faceMode = OpenTK.Graphics.OpenGL4.PrimitiveType.Points;
                                break;
                            case 2:
                                faceMode = OpenTK.Graphics.OpenGL4.PrimitiveType.Lines;
                                break;
                            case 3:
                                faceMode = OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles;
                                break;
                        }
                        for (int i = 0; i < face.IndexCount; i++)
                        {
                            int indics = face.Indices[i];

                            vertex.Position = FromVector(mesh.Vertices[indics]);
                            vertex.TexCoords = FromVector2D(mesh.TextureCoordinateChannels[0][indics]);
                            vertex.Normal = FromVector(mesh.Normals[indics]);

                            vertices.Add(vertex);

                        }
                        for (int i = 0; i < face.IndexCount; i++)
                        {
                            indices.Add((uint)face.Indices[i]);
                        }
                    }


                    Core.Structs.Material material = new Core.Structs.Material();

                    if (scene.Materials[mesh.MaterialIndex].TextureDiffuse.FilePath != null && scene.Materials[mesh.MaterialIndex].TextureDiffuse.FilePath != "")
                        material.textures.Add(Texture.LoadFromFile(scene.Materials[mesh.MaterialIndex].TextureDiffuse.FilePath));
                    if (scene.Materials[mesh.MaterialIndex].TextureSpecular.FilePath != null && scene.Materials[mesh.MaterialIndex].TextureSpecular.FilePath != "")
                        material.textures.Add(Texture.LoadFromFile(scene.Materials[mesh.MaterialIndex].TextureSpecular.FilePath));

                    material.Id = mesh.MaterialIndex;
                    material.Name = scene.Materials[mesh.MaterialIndex].Name;

                    VertexArray vertexArray = new VertexArray(new Renders.Bufers.VertexBuffer(vertices.ToArray()), new Renders.Bufers.IndexBuffer(indices.ToArray()), shader);

                    Renders.Mesh defaultMesh = new Renders.Mesh(vertexArray, material);
                    defaultMesh.Name = name;
                    defaultMesh.Id = meshs.Count;

                    meshs.Add(defaultMesh);


                    vertices = new List<Vertex>();
                    indices = new List<uint>();
                }
            }
            return meshs;
        }

    }
}
