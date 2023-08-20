using Assimp;
using GameEngine.ResourceLoad;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Player
    {
        private Camera _camera;

        private float Speed = 15;
        private float Sensitivity = 0.2f;
        private bool _firstMove = true;
        private Vector2 _lastPos;

        public Vector3 _position = new Vector3(2, 1, 2);

        public Player(Camera camera)
        {
            _camera = camera;
        }

        public void KeyUpdate(KeyboardState key)
        {
            float time = (float)Program.Game.Delta;

            if (key.IsKeyDown(Keys.W))
                _position += _camera.Front * Speed * time; // Forward
            if (key.IsKeyDown(Keys.S))
                _position -= _camera.Front * Speed * time; // Backwards
            if (key.IsKeyDown(Keys.A))
                _position -= _camera.Right * Speed * time; // Left
            if (key.IsKeyDown(Keys.D))
                _position += _camera.Right * Speed * time; // Right  

            //Vector3 NewCamPos = Program.Game.Terrain.ConstarainCameraPosToTerrain(_position);

            SetPositionCam(_position);
        }

        public void MouseUpdate(MouseState mouse)
        {
            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                _camera.Yaw += deltaX * Sensitivity;
                _camera.Pitch -= deltaY * Sensitivity;
            }
        }

        public void SetPositionCam(Vector3 position) => _camera.Position = position;

        public Vector3 GetPositiontCam() => _camera.Position;

        public Vector3 GetPositionPlayer() => _position;
    }
}
