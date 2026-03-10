using PROJ;

static class Program
{
    static void Main()
    {
        // Player player = new Player();
        Game game = new Game();
        game.Start();
        game.WaitForMove();
        game.End();
        // Console.WriteLine("Hello");
    }
}

