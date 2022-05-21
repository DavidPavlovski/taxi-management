using System.Collections.Generic;
using Taxi_Manager.Domain.Entities;

namespace System
{
    public static class EntityHelper
    {
        public static void PrintEntities<T>(this List<T> entities) where T: BaseEntity
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Console.WriteLine($"{i + 1}.) {entities[i].Print()}");
            }
        }
    }
}
