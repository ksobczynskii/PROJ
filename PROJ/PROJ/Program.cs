namespace PROJ;
static class Program
{
    static void Main()
    {
        // Player player = new Player();
        Game game = new Game();
        game.Start();
        game.WaitForMove();
        game.EndGood();
        // Console.WriteLine("Hello");
    }
}

