using OpenTK.Mathematics;

namespace GameEngine.Graphics
{
    public struct Vertex
    {
        public Vector3 Position { get; set; }
        public Vector3 Normal {  get; set; }
        public Vector2 TexCoords {  get; set; }

        public Vertex(Vector3 position)
        {
            Position = position;
        }

        public Vertex(Vector3 position, Vector2 texCoord)
        {
            Position = position;
            TexCoords = texCoord;
        }

        public Vertex(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
        }

        public Vertex(Vector3 position, Vector2 texCoord, Vector3 normal)
        {
            Position = position;
            TexCoords = texCoord;
            Normal = normal;
        }
    }
}
