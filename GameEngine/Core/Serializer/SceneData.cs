namespace GameEngine.Core.Serializer
{
    public class SceneData
    {
        public List<GameObjectData> GameObjects { get; set; } = new();
        public List<LightData> Lights { get; set; } = new();
    }
}
