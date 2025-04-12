using GameEngine.Core;
using GameEngine.Core.GameObjects;
using GameEngine.Core.GameObjects.Components;
using GameEngine.Core.Serializer;
using GameEngine.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;

namespace GameEngine.Example
{
    public class TestScene : Scene
    {
        private Light _flashLight;
        private List<Mesh> _meshes;

        private bool _firstMove = true;
        private Vector2 _lastPos;
        
        public TestScene()
        {

        }

        public override void Start()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            shader = assetManager.GetShader("base");

            _flashLight = new Light(new Vector3(1), new Vector3(1), new Vector3(0),
                0.09f, 0.032f, 1, MathF.Cos(MathHelper.DegreesToRadians(12.5f)),
                MathF.Cos(MathHelper.DegreesToRadians(17.5f)));

            AddLight(_flashLight);

            _meshes = assetManager.GetMesh("Cube");

            var meshRender = new MeshRender(_meshes.ToArray());
            var gameObject = new GameObject(this);
            gameObject.AddComponent(meshRender);
            gameObject.Position = new Vector3(0, 0, 0);

            AddGameObject(gameObject);

            game!.GetWindow().CursorState = CursorState.Grabbed;

            base.Start();
        }

        public override void Update(float deltaTime)
        {
            _flashLight.Position = camera.Position;
            _flashLight.Direction = camera.Front;

            var input = game!.GetKeyboardState();

            if (!game.GetWindow().IsFocused)
            {
                return;
            }

            if (input.IsKeyReleased(Keys.Escape))
            {
                game.GetWindow().Close();
            }

            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;

            if (input.IsKeyReleased(Keys.Q))
            {
                if (game.GetWindow().CursorState == CursorState.Normal)
                    game.GetWindow().CursorState = CursorState.Grabbed;
                else
                    game.GetWindow().CursorState = CursorState.Normal;
            }

            if(input.IsKeyReleased(Keys.F))
            {
                var light = Light.DefaultPoint;
                light.Position = camera.Position + camera.Front;

                AddLight(light);

                var meshRender = new MeshRender(_meshes.ToArray());
                var gameObject = new GameObject(this);
                gameObject.AddComponent(meshRender);
                gameObject.Position = camera.Position + camera.Front;
                gameObject.Scale = new Vector3(0.25f);
                AddGameObject(gameObject);
            }


            if (input.IsKeyDown(Keys.W))
            {
                camera.Position += camera.Front * cameraSpeed * deltaTime; // Forward
            }
            if (input.IsKeyDown(Keys.S))
            {
                camera.Position -= camera.Front * cameraSpeed * deltaTime; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                camera.Position -= camera.Right * cameraSpeed * deltaTime; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                camera.Position += camera.Right * cameraSpeed * deltaTime; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                camera.Position += camera.Up * cameraSpeed * deltaTime; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                camera.Position -= camera.Up * cameraSpeed * deltaTime; // Down
            }

            var mouse = game.GetMouseState();

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

                camera.Yaw += deltaX * sensitivity;
                camera.Pitch -= deltaY * sensitivity;
            }

            base.Update(deltaTime);
        }

        public override void Draw(float deltaTime)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            base.Draw(deltaTime);
        }

        public override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            camera.Fov -= e.OffsetY;
        }

        public override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
        }
    }
}
