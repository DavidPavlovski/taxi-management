using System;

namespace Taxi_Manager.Helpers
{
    public static class ConsoleHelper
    {
        public static void TextColor(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
