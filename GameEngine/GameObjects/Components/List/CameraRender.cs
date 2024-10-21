using GameEngine.Bufers;
using GameEngine.Core.Structs;
using GameEngine.Resources;
using GameEngine.Resources.Meshes;
using GameEngine.Resources.Shaders;
using GameEngine.Resources.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GameEngine.GameObjects.Components.List
{
    public enum ProjectionType
    {
        Perspective = 0,
        Orthgrafic = 1
    }
    public class CameraRender : Component
    {
        private CubemapTexture texture;
        private skyboxMesh mesh;
        private BaseShader shader;

        private Matrix4 projection = Matrix4.Identity;
        private float fov = MathHelper.PiOver2;
        private float aspect = 0;
        private float yaw = -MathHelper.PiOver2;
        private float pitch;

        public float depthNear = 0.01f;
        public float depthFar = 50f;
        public float size = 10;


        private Vector3 front = -Vector3.UnitZ;

        private Vector3 up = Vector3.UnitY;

        private Vector3 right = Vector3.UnitX;

        private Vertex[] skyboxVertices =
        {
	        //   Coordinates
	        new Vertex(new Vector3(-1.0f, -1.0f,  1.0f)),//        7--------6
	        new Vertex(new Vector3( 1.0f, -1.0f,  1.0f)),//       /|       /|
            new Vertex(new Vector3(1.0f, -1.0f,  -1.0f)),//      4--------5 |
	        new Vertex(new Vector3(-1.0f, -1.0f,  -1.0f)),//      | |      | |
	        new Vertex(new Vector3(-1.0f, 1.0f,  1.0f)),//      | 3------|-2
	        new Vertex(new Vector3(1.0f, 1.0f,  1.0f)),//      |/       |/
	        new Vertex(new Vector3(1.0f, 1.0f,  -1.0f)),//      0--------1
	        new Vertex(new Vector3(-1.0f, 1.0f,  -1.0f))
        };

        float[] skyboxVerticesFloat =
        {
	        //   Coordinates
	        -1.0f, -1.0f,  1.0f,//        7--------6
	         1.0f, -1.0f,  1.0f,//       /|       /|
	         1.0f, -1.0f, -1.0f,//      4--------5 |
	        -1.0f, -1.0f, -1.0f,//      | |      | |
	        -1.0f,  1.0f,  1.0f,//      | 3------|-2
	         1.0f,  1.0f,  1.0f,//      |/       |/
	         1.0f,  1.0f, -1.0f,//      0--------1
	        -1.0f,  1.0f, -1.0f
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

        public Vector3 Front => front;

        public Vector3 Up => up;

        public Vector3 Right => right;

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

        public Vector3 Position;

        public ProjectionType ProjectionType = ProjectionType.Perspective;

        public CameraRender(Vector3 position, float aspect)
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

            shader = ShaderLoad.Load(AssetManager.GetShader("skybox"));

            mesh = new skyboxMesh(shader, skyboxVerticesFloat, skyboxIndices);

            string[] paths = { 
                AssetManager.GetTexture("right"),
                AssetManager.GetTexture("left"),
                AssetManager.GetTexture("top"),
                AssetManager.GetTexture("bottom"),
                AssetManager.GetTexture("front"),
                AssetManager.GetTexture("back")
            };

            texture = CubemapTexture.LoadFromFile(paths);
        }

        public override void Update(BaseShader shader, float deltaTime)
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
            front = Vector3.Normalize(front);

            right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(right, front));
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + front, up);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return projection;
        }

        public override void Draw(BaseShader shader)
        {
            base.Draw(shader);

            if (IsSkyBox)
            {
                GL.CullFace(CullFaceMode.Front);
                GL.FrontFace(FrontFaceDirection.Ccw);
                GL.DepthFunc(DepthFunction.Lequal);

                this.shader.Use();

                this.shader.SetMatrix4("projection", GetProjectionMatrix());
                this.shader.SetMatrix4("view", GetViewMatrix());
                this.shader.SetInt("skybox", 0);
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
