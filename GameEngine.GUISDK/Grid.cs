using GameEngine.Core.Renders.Bufers;
using GameEngine.Core.Structs;
using GameEngine.GameObjects.Components.List;
using GameEngine.Resources.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.LevelEditor
{
    public class Grid
    {
        Shader shader;
        BaseBuffer buffer;

        Vertex[] vertices =
        {
            new Vertex(new Vector3(-1.0f, 0.0f, -1.0f)),
            new Vertex(new Vector3( 1.0f, 0.0f, -1.0f)),
            new Vertex(new Vector3( 1.0f, 0.0f,  1.0f)),
            new Vertex(new Vector3(-1.0f, 0.0f,  1.0f))
        };

        uint[] indices = { 0, 2, 1, 2, 0, 3 };

        public Grid(string path)
        {
            shader = ShaderLoad.Load(path);
            buffer = new EBOBuffer();
            buffer.Init(shader, vertices, indices);
        }

        public void Draw(CameraRender camera)
        {
            GL.Disable(EnableCap.CullFace);

            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            Matrix4 VP = camera.GetViewMatrix() * camera.GetProjectionMatrix();
            shader.Use();
            shader.SetMatrix4("view", camera.GetViewMatrix());
            shader.SetMatrix4("projection", camera.GetProjectionMatrix());
            shader.SetMatrix4("model", Matrix4.Identity);
            shader.SetVector3("gCameraWorldPos", camera.Position);

            buffer.Draw(PrimitiveType.Triangles);

            GL.Enable(EnableCap.CullFace);

        }
    }
}
