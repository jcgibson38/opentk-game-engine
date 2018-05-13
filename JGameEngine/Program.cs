using JGameEngine.RenderEngine;
using System;
using System.Threading;

namespace JGameEngine
{
    class Program
    {
        private static readonly double updatesPerSecond = 30.0;
        private static readonly double framesPerSecond = 60.0;
        private static JGameWindow myWindow;

        private static Thread mainThread = Thread.CurrentThread;

        static void Main(string[] args)
        {
            Console.WriteLine("Launching Game Window...");
            myWindow = new JGameWindow();
            //myWindow = new JNoiseWindow();

            Console.WriteLine("Updates per second: " + updatesPerSecond);
            Console.WriteLine("Frames per second: " + framesPerSecond);

            myWindow.Run(updatesPerSecond,framesPerSecond);
        }
    }
}
