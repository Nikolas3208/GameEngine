using GameEngine.Core;
using GameEngine.Core.GameObjects;
using GameEngine.Core.GameObjects.Components;
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
        public TestScene()
        {

        }

        public override void Statr()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            _flashLight = new Light(new Vector3(1), new Vector3(1), new Vector3(0),
                0.09f, 0.032f, 1, MathF.Cos(MathHelper.DegreesToRadians(12.5f)),
                MathF.Cos(MathHelper.DegreesToRadians(17.5f)));

            AddLight(_flashLight);

            Light light = Light.DefaultPoint;

            _meshes = assetManager.GetMesh("Cube");
            
            for(int i = 0; i< _meshes.Count; i++)
            {
                //_meshes[i].SetMaterial(Material.Default);
            }

            for (int i = 0; i < 5; i++) 
            {
                light = Light.DefaultPoint;

                float x = Random.Shared.Next(-15, 15);
                float y = Random.Shared.Next(-15, 15);
                float z = Random.Shared.Next(-15, 15);

                light.Position = new Vector3(x, y, z);
                light.Direction = new Vector3(x, y, z);

                lights.Add(light);


                var meshRender = new MeshRender(_meshes);
                var gameObject = new GameObject(this);
                gameObject.AddComponent(meshRender);
                gameObject.Position = new Vector3(x, y, z);
                gameObject.Scale = new Vector3(0.25f);
                AddGameObject(gameObject);
            }

            for (int i = 0; i < 15; i++)
            {
                float x = Random.Shared.Next(-15, 15);
                float y = Random.Shared.Next(-15, 15);
                float z = Random.Shared.Next(-15, 15);

                var meshRender = new MeshRender(assetManager.GetMesh("Cube"));
                var gameObject = new GameObject(this);
                gameObject.AddComponent(meshRender);
                gameObject.Position = new Vector3(x, y, z);
                AddGameObject(gameObject);
            }

            game.GetWindow().CursorState = CursorState.Grabbed;

            base.Statr();
        }

        private bool _firstMove = true;

        private Vector2 _lastPos;

        public override void Update(float deltaTime)
        {
            _flashLight.Position = camera.Position;
            _flashLight.Direction = camera.Front;

            UpdateLight(_flashLight.Id, _flashLight);

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
                light.Position = camera.Position - camera.Front;

                AddLight(light);

                var meshRender = new MeshRender(_meshes);
                var gameObject = new GameObject(this);
                gameObject.AddComponent(meshRender);
                gameObject.Position = light.Position;
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
