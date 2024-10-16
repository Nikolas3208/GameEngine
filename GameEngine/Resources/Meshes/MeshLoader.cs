using Assimp.Configs;
using Assimp;
using GameEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Resources.Shaders;

namespace GameEngine.Resources.Meshes
{
    public class MeshLoader
    {
        private static Scene scene;

        private static List<BaseMesh> meshs = new List<BaseMesh>();
        private static List<Vertex> vertices = new List<Vertex>();
        private static List<uint> indices = new List<uint>();

        private static OpenTK.Graphics.OpenGL4.PrimitiveType faceMode = OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles;

        private static Vertex vertex = new Vertex();

        public static List<BaseMesh> LoadMesh(string path, BaseShader shader)
        {
            meshs = new List<BaseMesh>();
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

                            vertex.Position = MathHelper.FromVector(mesh.Vertices[indics]);
                            vertex.TexCoords = MathHelper.FromVector2D(mesh.TextureCoordinateChannels[0][indics]);
                            vertex.Normal = MathHelper.FromVector(mesh.Normals[indics]);

                            vertices.Add(vertex);

                        }
                        for (int i = 0; i < face.IndexCount; i++)
                        {
                            indices.Add((uint)face.Indices[i]);
                        }

                    }

                    DefaultMesh defaultMesh = new DefaultMesh(shader, vertices.ToArray(), indices.ToArray(), mesh.Name);
                    defaultMesh.MaterialName = scene.Materials[mesh.MaterialIndex].Name;

                    meshs.Add(defaultMesh);


                    vertices = new List<Vertex>();
                    indices = new List<uint>();
                }
            }
            return meshs;
        }

    }
}
