using System;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Core.GetInstance().Init();

            while(Console.ReadKey().Key != ConsoleKey.Escape) { }

            Core.GetInstance().Shutdown();
        }
    }
}
