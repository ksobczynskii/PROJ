using PROJ.Configuration;

namespace PROJ;
using Microsoft.Extensions.Configuration;
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

