using System;

namespace CIS580_Project
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
