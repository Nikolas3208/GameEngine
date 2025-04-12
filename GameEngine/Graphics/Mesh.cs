using OpenTK.Graphics.OpenGL4;

namespace GameEngine.Graphics
{
    public class Mesh
    {
        private VertexArray? _vertexArray;
        private Material? _material;

        public Guid Id { get; }
        public string Name { get; } = string.Empty;

        public Mesh(string name = "mesh")
        {
            Name = name;

            Id = Guid.NewGuid();
        }

        public Mesh(Vertex[] vertices, uint[] indices, string name = "mesh")
        {
            _vertexArray = new VertexArray(vertices, indices);
            Name = name;

            Id = Guid.NewGuid();
        }

        public Mesh(Vertex[] vertices, uint[] indices, Material material, string name = "mesh")
        {
            _vertexArray = new VertexArray(vertices, indices);
            _material = material;
            Name = name;

            Id = Guid.NewGuid();
        }

        public Mesh(VertexArray vertexArray, Material material, string name = "mesh")
        {
            _vertexArray = vertexArray;
            _material = material;
            Name = name;

            Id = Guid.NewGuid();
        }

        public void Init(Shader shader)
        {
            _vertexArray.Init(shader);
        }

        public void DrawElements(PrimitiveType type = PrimitiveType.Triangles)
        {
            _vertexArray.DrawElements(type);
        }

        public void DrawElements(Shader shader, PrimitiveType type = PrimitiveType.Triangles)
        {
            _material.Draw(shader);

            _vertexArray.DrawElements(type);
        }

        public void DrawArrays(PrimitiveType type = PrimitiveType.Triangles)
        {
            _vertexArray.DrawArrays(type);
        }

        public void DrawArrays(Shader shader, PrimitiveType type = PrimitiveType.Triangles)
        {
            _material.Draw(shader);

            _vertexArray.DrawArrays(type);
        }

        public void SetMaterial(Material material)
        {
            _material = material;
        }

        public Material GetMaterial()
        {
            return _material;
        }

        public uint GetVerticesCount()
        {
            if( _vertexArray == null)
                return 0;

            if(_vertexArray.GetVertexBuffer() == null)
                return 0;

            return (uint)_vertexArray.GetVertexBuffer().Count;
        }

        public Vertex[]? GetVertices()
        {
            if (_vertexArray == null)
                return null;

            if (_vertexArray.GetVertexBuffer() == null)
                return null;

            return _vertexArray.GetVertexBuffer().GetVertices();
        }

        public int GetIndicesCount()
        {
            if (_vertexArray == null)
                return 0;

            if (_vertexArray.GetIndexBuffer() == null)
                return 0;

            return _vertexArray.GetIndexBuffer()!.Count;
        }

        public uint[]? GetIndices()
        {
            if (_vertexArray == null)
                return null;

            if (_vertexArray.GetIndexBuffer() == null)
                return null;

            return _vertexArray.GetIndexBuffer()!.GetIndices();
        }
    }
}
