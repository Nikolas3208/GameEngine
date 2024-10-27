using Assimp;
using GameEngine.Core;
using GameEngine.Core.Structs;
using GameEngine.GameObjects.Components.List;
using GameEngine.Resources;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameEngine.GameObjects.List
{
    public enum Direction
    {

        Left, Right, Up, Down, Breack, Front
    }

    public class Camera : GameObject
    {
        [JsonIgnore]
        private CameraRender render;
        public Camera(Vector3f position, float aspect)
        {
            AddComponent(new CameraRender(position, aspect));
            render = GetComponent<CameraRender>();
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

            GetComponent<TransformComponet>().Position = render.Position;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
