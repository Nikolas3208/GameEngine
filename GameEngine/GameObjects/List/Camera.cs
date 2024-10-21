using Assimp;
using GameEngine.GameObjects.Components.List;
using GameEngine.Resources;
using GameEngine.Resources.Shaders;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameObjects.List
{
    public enum Direction
    {

        Left, Right, Up, Down, Breack, Front
    }


    public class Camera : GameObject
    {
        private CameraRender render;
        public Camera(Vector3 position, float aspect)
        {
            render = new CameraRender(position, aspect);
            AddComponent(render);

            Name = "Main camera";
        }

        public override void Start()
        {
            base.Start();
        }

        public void CameraRotation(Vector2 delta)
        {
            render.Yaw += delta.X;
            render.Pitch -= delta.Y;
        }

        public void CameraMove(Direction dir, float deltaTime, float speed = 45)
        {
            switch (dir)
            {
                case Direction.Left:
                    render.Position -= render.Right * speed * deltaTime; //Forward 
                    break;
                case Direction.Right:
                    render.Position += render.Right * speed * deltaTime; //Forward 
                    break;
                case Direction.Up:
                    break;
                case Direction.Down:
                    break;
                case Direction.Breack:
                    render.Position -= render.Front * speed * deltaTime; //Forward 
                    break;
                case Direction.Front:
                    render.Position += render.Front * speed * deltaTime; //Forward 
                    break;
            }

            GetComponent<TransformComponet>().Transform = render.Position;
        }

        public override void Update(float deltaTime, Shader shader)
        {
            base.Update(deltaTime, shader);
        }

        public override void Draw(Shader shader)
        {
            //render.Draw(shader);
            base.Draw(shader);
        }
    }
}
