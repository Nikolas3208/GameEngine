using GameEngine.Core;
using GameEngine.Core.Structs;
using GameEngine.Renders;
using GameEngine.Renders.Bufers;
using GameEngine.Resources;
using GameEngine.Resources.Meshes;
using GameEngine.Resources.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GameEngine.GameObjects.Components.List
{
    public enum ProjectionType
    {
        Perspective = 0,
        Orthgrafic = 1
    }

    [Serializable]
    public class CameraRender : Component
    {
        private CubemapTexture texture;
        private Mesh mesh;
        [JsonIgnore]
        private Shader skyBoxShader;

        private Matrix4 projection = Matrix4.Identity;
        private float fov = MathHelper.PiOver2;
        private float aspect = 0;
        private float yaw = -MathHelper.PiOver2;
        private float pitch;

        public float depthNear = 0.01f;
        public float depthFar = 1000f;
        public float size = 10;


        private Vector3f front = -Vector3f.UnitZ;

        private Vector3f up = Vector3f.UnitY;

        private Vector3f right = Vector3f.UnitX;

        private Vertex[] skyboxVertices =
         {
	        //   Coordinates
	        new Vertex(new Vector3f(-1.0f, -1.0f,  1.0f)),//        7--------6
	        new Vertex(new Vector3f( 1.0f, -1.0f,  1.0f)),//       /|       /|
            new Vertex(new Vector3f(1.0f, -1.0f,  -1.0f)),//      4--------5 |
	        new Vertex(new Vector3f(-1.0f, -1.0f,  -1.0f)),//      | |      | |
	        new Vertex(new Vector3f(-1.0f, 1.0f,  1.0f)),//      | 3------|-2
	        new Vertex(new Vector3f(1.0f, 1.0f,  1.0f)),//      |/       |/
	        new Vertex(new Vector3f(1.0f, 1.0f,  -1.0f)),//      0--------1
	        new Vertex(new Vector3f(-1.0f, 1.0f,  -1.0f))
        };

        private uint[] skyboxIndices =
        {
	        // Right
	        1, 2, 6,
            6, 5, 1,
	        // Left
	        0, 4, 7,
            7, 3, 0,
	        // Top
	        4, 5, 6,
            6, 7, 4,
	        // Bottom
	        0, 3, 2,
            2, 1, 0,
	        // Back
	        0, 1, 5,
            5, 4, 0,
	        // Front
	        3, 7, 6,
            6, 2, 3
        };

        public float Aspect { get => aspect; set => aspect = value; }

        public Vector3f Front => front;
        public Vector3f Up => up;

        public Vector3f Right => right;

        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(pitch);
            set
            {
                var angle = MathHelper.Clamp(value, -89, 89);
                pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }

        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(yaw);
            set
            {
                yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }
        public float Fov
        {
            get => MathHelper.RadiansToDegrees(fov);
            set
            {
                var angle = MathHelper.Clamp(value, 3f, 179f);
                fov = MathHelper.DegreesToRadians(angle);
            }
        }

        public bool IsSkyBox = true;

        public Vector3f Position;

        public ProjectionType ProjectionType = ProjectionType.Perspective;

        public CameraRender(Vector3f position, float aspect)
        {
            Position = position;
            Aspect = aspect;
        }

        public CameraRender()
        {

        }
        public override void Start()
        {
            Name = "Camera render";

            projection = Matrix4.Identity;

            skyBoxShader = Shader.LoadFromFile(AssetManager.GetShader("skybox"));

            string[] paths = {
                AssetManager.GetTexture("right"),
                AssetManager.GetTexture("left"),
                AssetManager.GetTexture("top"),
                AssetManager.GetTexture("bottom"),
                AssetManager.GetTexture("front"),
                AssetManager.GetTexture("back")
            };

            texture = CubemapTexture.LoadFromFile(paths);

            VertexArray vertexArray = new VertexArray(new VertexBuffer(skyboxVertices), new IndexBuffer(skyboxIndices), skyBoxShader);

            mesh = new Mesh(vertexArray);

            skyBoxShader.Use();
        }

        public override void Update(float deltaTime)
        {
            switch (ProjectionType)
            {
                case ProjectionType.Perspective:
                    projection = Matrix4.CreatePerspectiveFieldOfView(fov, aspect, depthNear, depthFar);
                    break;
                case ProjectionType.Orthgrafic:
                    projection = Matrix4.CreateOrthographic(size, size, depthNear, depthFar);
                    break;
            }
        }

        private void UpdateVectors()
        {
            front.X = MathF.Cos(pitch) * MathF.Cos(yaw);
            front.Y = MathF.Sin(pitch);
            front.Z = MathF.Cos(pitch) * MathF.Sin(yaw);
            front = Vector3f.Normalize(front);



            right = Vector3f.Vector3F(Vector3.Normalize(Vector3.Cross(Vector3f.Vector3(front), Vector3.UnitY)));
            up = Vector3f.Vector3F(Vector3.Normalize(Vector3.Cross(Vector3f.Vector3(right), Vector3f.Vector3(front))));
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Vector3f.Vector3(Position), Vector3f.Vector3(Position + front), Vector3f.Vector3(up));
        }

        public Matrix4 GetProjectionMatrix()
        {
            return projection;
        }

        public override void Draw(Shader shader)
        {
            if (IsSkyBox && skyBoxShader != null)
            {
                GL.CullFace(CullFaceMode.Front);
                GL.FrontFace(FrontFaceDirection.Ccw);
                GL.DepthFunc(DepthFunction.Lequal);

                this.skyBoxShader.Use();

                this.skyBoxShader.SetMatrix4("projection", GetProjectionMatrix());
                this.skyBoxShader.SetMatrix4("view", GetViewMatrix());
                this.skyBoxShader.SetInt("skybox", 0);
                texture.Use(TextureUnit.Texture0);

                mesh.Draw(PrimitiveType.Triangles);

                GL.DepthFunc(DepthFunction.Less);

                GL.CullFace(CullFaceMode.Back);
            }

            shader.Use();

            shader.SetMatrix4("projection", GetProjectionMatrix());
            shader.SetMatrix4("view", GetViewMatrix());

            shader.SetVector3("viewPos", Position);
        }
    }
}
