using GameEngine.Core.Resources.Loaders.MeshLoaders;
using GameEngine.Core.Serializer;
using GameEngine.Graphics;

namespace GameEngine.Core.GameObjects.Components
{
    public class MeshRender : Component
    {
        private List<Mesh>? _meshes;
        public string MeshName { get; set; } = string.Empty;

        public MeshRender()
        {
            _meshes = new List<Mesh>();
        }

        public MeshRender(params Mesh[] meshes)
        {
            _meshes = meshes.ToList();

            MeshName = _meshes[0].Name;
        }

        public override void Start()
        {
            if (_meshes == null)
                return;
            if (_meshes.Count == 0 && MeshName != string.Empty)
            {
                _meshes = new List<Mesh>();
                _meshes.Add(MeshSerializer.LoadFromFile($"Assets\\{MeshName}.mesh"));
            }

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

        public override ComponentData Serialize()
        {
            return new ComponentData
            {
                Type = typeof(MeshRender).AssemblyQualifiedName!,
                Properties = new Dictionary<string, string>
                {
                    { nameof(MeshName), MeshName }
                }
            };
        }

        public override Component Deserialize(ComponentData data)
        {
            Type type = typeof(MeshRender);

            foreach (var kv in data.Properties)
            {
                switch (kv.Key)
                {
                    case nameof(MeshName):
                        MeshName = kv.Value;
                        break;
                }
            }

            return this;
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

        public List<Mesh>? GetAllMesh()
        {
            return _meshes;
        }
    }
}
