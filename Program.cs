using System;

namespace RubyDotNET
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Internal.Initialize();
            Internal.DoStuff();

            Console.ReadKey();
        }
    }
}
