using System;
using System.Collections.Generic;
using System.Text;

namespace Taxi_Manager.Helpers
{
    public static  class EnumHelper
    {
        public static T SelectEnum<T>(string text)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Select {text}.");
                for (int i = 0; i < Enum.GetValues(typeof(T)).Length; i++)
                {
                    Console.WriteLine($"{i + 1}.){(T)Enum.ToObject(typeof(T), i + 1)}");
                }
                string enumChoice = ConsoleHelper.GetInput("Enter index number to select : ");
                int enumIndex = ConsoleHelper.GetNumberInput(enumChoice, Enum.GetValues(typeof(T)).Length);
                return (T)Enum.ToObject(typeof(T), enumIndex);
            }
        }
    }
}
