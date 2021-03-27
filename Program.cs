using System;
using System.Threading;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Core.GetInstance().Init();

            while(Core.isRunning) { }
            Thread.Sleep(2000);
        }
    }
}
