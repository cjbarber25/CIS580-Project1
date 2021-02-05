using System;

namespace CIS580_Project1
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new SlimeGame())
                game.Run();
        }
    }
}
