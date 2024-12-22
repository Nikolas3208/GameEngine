using GameEngine.Core;
using GameEngine.Core.Structs;
using GameEngine.GameObjects.Components;
using GameEngine.GameObjects.Components.List;
using GameEngine.Renders;
using GameEngine.Renders.Bufers;
using GameEngine.Resources;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameEngine.GameObjects.List
{
    public class Grid : GameObject
    {
        Vertex[] vertices =
        {
            new Vertex(new Vector3f(-1.0f, 0.0f, -1.0f)),
            new Vertex(new Vector3f( 1.0f, 0.0f, -1.0f)),
            new Vertex(new Vector3f( 1.0f, 0.0f,  1.0f)),
            new Vertex(new Vector3f(-1.0f, 0.0f,  1.0f))
        };

        uint[] indices = { 0, 2, 1, 2, 0, 3 };


        public Grid() { }

        public override void Start()
        {
            shader = Shader.LoadFromFile(AssetManager.GetShader("grid"));

            VertexArray vertexArray = new VertexArray(new VertexBuffer(vertices), new IndexBuffer(indices));
            vertexArray.Init(shader);

            Mesh mesh = new Mesh(vertexArray);

            MeshRender meshRender = new MeshRender();
            meshRender.AddMesh(mesh);

            if(GetComponent<MeshRender>() == null)
                AddComponent(meshRender);

            Name = "Grid";

            base.Start();
        }

        public override void Draw(Shader shader = null)
        {
            GL.Disable(EnableCap.CullFace);
            base.Draw();
            GL.Enable(EnableCap.CullFace);
        }
    }
}
