using System;

namespace Lab3Game
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new SuperCoolGame();
            game.Run();
        }
    }
}
