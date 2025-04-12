using OpenTK.Mathematics;

namespace GameEngine.Core
{
    public class Transform
    {
        private Vector3 _position;
        private Vector3 _scale;
        private Vector3 _rotation;
        private Matrix4 _model = Matrix4.Identity;

        public Vector3 Position { get => _position; set { _position = value; UpdateTranform(); } }
        public Vector3 Scale { get => _scale; set { _scale = value; UpdateTranform(); } }
        public Vector3 Rotation { get => _rotation; set { _rotation = value; UpdateTranform(); } }

        public Transform()
        {
            _position = new Vector3(0);
            _scale = new Vector3(1);
            _rotation = new Vector3();

            UpdateTranform();
        }

        public Transform(Transform transform)
        {
            _position = transform._position;
            _scale = transform._scale;
            _rotation = transform._rotation;

            UpdateTranform();
        }

        public Transform(Vector3 position, Vector3 scale, Vector3 rotation)
        {
            _position = position;
            _scale = scale;
            _rotation = rotation;

            UpdateTranform();
        }

        private void UpdateTranform()
        {
           // _model *= Matrix4.CreateRotationX(_rotation.X);
            //_model *= Matrix4.CreateRotationY(_rotation.Y);
            //_model *= Matrix4.CreateRotationZ(_rotation.Z);
            _model *= Matrix4.CreateTranslation(_position);
            _model *= Matrix4.CreateScale(_scale);
        }

        public Matrix4 GetModelMatrix()
        {
            return _model;
        }
    }
}
