using System;

namespace Game.Utils
{
    internal static class NumberUtils
    {
        public static int TryAsInt(String value, int defaultValue)
        {
            int intVal;
            if (!int.TryParse(value, out intVal))
                return defaultValue;
            else return intVal;
        }

        private static readonly Random random = new Random();
        public static int randomInt(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}