using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.ResourceLoad;

namespace GameEngine.Terrain
{
    public class BaseTerrain
    {
        public int Size { get; private set; }

        private List<Vertex> vertices;

        private List<uint> indices;

        private Mesh mesh;
        private Shader Shader;

        public ArraysTerrain arraysTerrain { get; private set; } = new ArraysTerrain();

        public BaseTerrain(int size, float[,] data)
        {
            arraysTerrain.InitArrays(ref vertices, ref indices, size, data);
            Shader = InitShader(Content.SHADER_PATH + "terrainShader.vert", Content.SHADER_PATH + "multiTextureTerrainShader.frag");

            InitMesh();
            InitTexure();
        }

        public void InitTexure()
        {
            List<Texture> textures = new List<Texture>();
            textures.Add(Texture.LoadFromFile(Content.TEXTURE_PATH + "TerrainTextures\\aerial_rocks_02_diff_2k.jpg"));
            textures.Add(Texture.LoadFromFile(Content.TEXTURE_PATH + "TerrainTextures\\aerial_rocks_04_diff_2k.jpg"));
            textures.Add(Texture.LoadFromFile(Content.TEXTURE_PATH + "TerrainTextures\\forest_leaves_02_diffuse_2k.jpg"));
            textures.Add(Texture.LoadFromFile(Content.TEXTURE_PATH + "TerrainTextures\\rocky_trail_diff_2k.jpg"));
            textures.Add(Texture.LoadFromFile(Content.TEXTURE_PATH + "TerrainTextures\\blendMap.png"));

            Shader.SetInt("texture0", 0);
            Shader.SetInt("texture1", 1);
            Shader.SetInt("texture2", 2);
            Shader.SetInt("texture3", 3);
            Shader.SetInt("texture4", 4);

            for (int i = 0; i < textures.Count; i++)
            {
                textures[i].Use(TextureUnit.Texture0 + i);
            }
        }

        private Mesh InitMesh(Shader shader) => new Mesh(vertices, indices, shader);

        public Shader InitShader(string vert, string frag) => new Shader(vert, frag);

        public void InitMesh() => mesh = InitMesh(Shader);

        private void UniformUpdate(Shader shader)
        {
            Matrix4 matrix = Matrix4.Identity;
            shader.SetMatrix4("model", matrix);
        }

        public void UpdateShader(Shader shader, Camera camera)
        {
            shader.Use();

            shader.SetMatrix4("view", camera.GetViewMatrix());
            shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            shader.SetVector3("viewPos", camera.Position);

            // Here we specify to the shaders what textures they should refer to when we want to get the positions.
            shader.SetInt("material.diffuse", 0);
            shader.SetInt("material.specular", 1);
            shader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            shader.SetFloat("material.shininess", 32.0f);

            shader.SetVector3("light.position", new Vector3());
            shader.SetVector3("light.ambient", new Vector3(0.2f));
            shader.SetVector3("light.diffuse", new Vector3(0.5f));
            shader.SetVector3("light.specular", new Vector3(1.0f));
        }

        public void RenderMesh(Camera camera)
        {
            UpdateShader(Shader, camera);
            UniformUpdate(Shader);

            mesh.Draw(Shader, PrimitiveType.Triangles);
        }
    }
}
