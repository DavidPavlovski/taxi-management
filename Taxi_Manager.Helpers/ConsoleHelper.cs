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

        public static string GetInput(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }
        public static int GetNumberInput(string input, int range)
        {
            if (!int.TryParse(input, out int num))
            {
                throw new Exception($"Input number must be a number");
            }
            else if (num <= 0 || num > range)
            {
                throw new Exception($"Input number must be between 1 and {range}");
            }
            return num;
        }
    }
}
