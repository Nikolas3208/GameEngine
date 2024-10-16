namespace GameEngine.Example
{
    public class Program
    {
        private Game Game { get; set; }
        public static void Main(string[] args)
        {
            if(args.Length != 0)
            {
                if (args[0] == "--config")
                {
                    Game engin = new Game(args[1]);
                    engin.Run();
                }
                
            }
            else
            {
                Game engin = new Game();
                engin.Run();
            }
        }
    }
}
