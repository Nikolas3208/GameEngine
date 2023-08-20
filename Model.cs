using GameEngine.ResourceLoad;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using Assimp.Configs;
using Assimp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Model
    {
        private Scene scene;
        protected Shader shader;
        protected List<Texture> textures =new List<Texture>();

        private Vector3 position = new Vector3(10, 20, 10);
        private Vector3 rotation;// = new Vector3(0,0,00);
        private float scale = 1;


        protected OpenTK.Graphics.OpenGL4.PrimitiveType faceMode = OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles;

        protected List<Mesh> meshs = new List<Mesh>();
        private List<Vertex> vertices = new List<Vertex>();
        private List<uint> indices = new List<uint>();

        protected Vertex vertex = new Vertex();
        public Model(string path, Shader shader)
        {
            this.shader = shader;

            textures.Add(Texture.LoadFromFile(@"C:\Users\gaste\source\repos\Nikolas3208\GameEngine\Content\Textures\aerial_rocks_04_diff_2k.jpg"));
            textures.Add(Texture.LoadFromFile(@"C:\Users\gaste\source\repos\Nikolas3208\GameEngine\Content\Textures\aerial_rocks_04_disp_2k.png"));
            textures.Add(Texture.LoadFromFile(@"C:\Users\gaste\source\repos\Nikolas3208\GameEngine\Content\Textures\Stone.jpg"));

            LoadModel(path);
        }

        private void LoadModel(string path)
        {
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
                    AddMesh(new Mesh(vertices, indices, this.shader));

                    vertices = new List<Vertex>();
                    indices = new List<uint>();
                }
            }
        }

        protected void AddMesh(Mesh mesh) => meshs.Add(mesh);

        private Vector3 FromVector(Vector3D vec)
        {
            Vector3 v;
            v.X = vec.X;
            v.Y = vec.Y;
            v.Z = vec.Z;
            return v;
        }

        private Vector2 FromVector2D(Vector3D vec)
        {
            Vector2 v;
            v.X = vec.X;
            v.Y = vec.Y;
            return v;
        }

        public void SetPosition(Vector3 position) => this.position = position;

        private void UniformUpdate(Shader shader)
        {
            Matrix4 matrix = Matrix4.Identity;
            matrix = matrix * Matrix4.CreateTranslation(position);
            matrix = matrix * Matrix4.CreateScale(scale);
            matrix = matrix * Matrix4.CreateRotationX(rotation.X);
            matrix = matrix * Matrix4.CreateRotationY(rotation.Y);
            matrix = matrix * Matrix4.CreateRotationZ(rotation.Z);

            shader.SetMatrix4("model", matrix);
        }

        public void UpdateShader(Shader shader, Vector3 lightPos)
        {
            // Here we specify to the shaders what textures they should refer to when we want to get the positions.
            shader.SetInt("material.diffuse", 0);
            shader.SetInt("material.specular", 1);
            shader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            shader.SetFloat("material.shininess", 32.0f);

            shader.SetVector3("light.position", lightPos);
            shader.SetVector3("light.ambient", new Vector3(0.2f));
            shader.SetVector3("light.diffuse", new Vector3(0.5f));
            shader.SetVector3("light.specular", new Vector3(1.0f));
        }

        public void Render(Shader shader, Vector3 lightPos)
        {
            UniformUpdate(this.shader);
            UpdateShader(this.shader, lightPos);

            for (int i = 0; i < meshs.Count; i++)
            {
                textures[0].Use(TextureUnit.Texture6);
                textures[1].Use(TextureUnit.Texture7);


                meshs[i].Draw(this.shader, faceMode);
            }

        }
    }
}
