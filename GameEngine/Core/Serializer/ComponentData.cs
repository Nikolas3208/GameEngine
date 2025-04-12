namespace GameEngine.Core.Serializer
{
    public class ComponentData
    {
        public string Type { get; set; } // тип компонента
        public Dictionary<string, string> Properties { get; set; } = new();
    }
}
