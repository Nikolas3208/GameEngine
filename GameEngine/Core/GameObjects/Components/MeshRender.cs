using GameEngine.Graphics;

namespace GameEngine.Core.GameObjects.Components
{
    public class MeshRender : Component
    {
        private List<Mesh>? _meshes;

        public MeshRender()
        {
            _meshes = new List<Mesh>();
        }

        public MeshRender(List<Mesh> meshes)
        {
            _meshes = meshes;
        }

        public override void Start()
        {
            foreach(var mesh in _meshes!)
            {
                mesh.Init(gameObject!.GetShader());
            }
        }

        public override void Update(float deltaTime)
        {

        }

        public override void Draw(float deltaTime)
        {
            foreach(var mesh in _meshes!)
            {
                mesh.DrawElements(gameObject!.GetShader());
            }
        }

        public void AddMesh(Mesh mesh)
        {
            if (_meshes == null)
                throw new Exception("Meshes is null.");

            _meshes.Add(mesh);
        }

        public void AddMeshRange(List<Mesh> meshes)
        {
            if (_meshes == null)
                throw new Exception("Meshes is null.");

            _meshes.AddRange(meshes);
        }

        public Mesh? GetMesh(Guid id)
        {
            if (_meshes == null)
                throw new Exception("Meshes is null.");

            return _meshes.FirstOrDefault(m => m.Id == id);
        }

        public bool RemoveMesh(Mesh mesh)
        {
            if (_meshes == null)
                throw new Exception("Meshes is null.");

            return _meshes.Remove(mesh);
        }
    }
}
