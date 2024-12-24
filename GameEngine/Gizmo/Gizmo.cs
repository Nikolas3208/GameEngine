using GameEngine.Core.Essentials;
using GameEngine.Renders;
using GameEngine.Resources;
using GameEngine.Resources.Meshes;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Gizmo
{
    public enum GizmoType
    {
        Translation,
        Rotation,
        Scale
    }

    public class Gizmo
    {
        private float[] verticesTranslation =
        {
            //Vertices        //Color
            0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
            1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f,
            0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f
        };

        private Matrix4 model = Matrix4.Identity;
        private Matrix4 scale = Matrix4.Identity;
        private Matrix4 rotation = Matrix4.Identity;
        private Vector3 position;

        private Mesh meshX;
        private Mesh meshY;
        private Mesh meshZ;

        public GizmoType type;
        public Vector3 Position { get => position; set { position = value; model = Matrix4.CreateTranslation(value); } }

        public void Init(Shader shader, GizmoType type)
        {
            meshX = MeshLoader.LoadMesh(AssetManager.GetMesh("axesX")).ToArray()[0];
            meshY = MeshLoader.LoadMesh(AssetManager.GetMesh("axesY")).ToArray()[0];
            meshZ = MeshLoader.LoadMesh(AssetManager.GetMesh("axesZ")).ToArray()[0];
            //material = MaterialLoader.LoadMaterial(AssetManager.GetMesh("axesX")).Values.ToArray()[0];
        }

        public void Draw(Shader shader)
        {
            GL.Disable(EnableCap.DepthTest);

            shader.Use();
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("scale", scale);
            shader.SetMatrix4("rotation", rotation);
            shader.SetMatrix4("scale", Matrix4.CreateScale(2f));

            //material.Color = new Vector3(1, 0, 0);
            //material.Draw(shader);
            meshX.Draw(PrimitiveType.Triangles, shader);
            shader.SetInt("gameObjectId", -2);

            //material.Color = new Vector3(0, 1, 0);
            //material.Draw(shader);
            meshY.Draw(PrimitiveType.Triangles, shader);
            shader.SetInt("gameObjectId", -3);

            //material.Color = new Vector3(0, 0, 1);
            //material.Draw(shader);
            meshZ.Draw(PrimitiveType.Triangles, shader);
            shader.SetInt("gameObjectId", -4);


            GL.Enable(EnableCap.DepthTest);
        }
    }
}
