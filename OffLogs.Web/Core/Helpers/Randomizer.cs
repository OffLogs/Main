using System;

namespace OffLogs.Web.Core.Helpers
{
    public static class Randomizer
    {
        private static Random _randomizer = new Random();
        
        public static int GetNumber(int minValue = 0)
        {
            return _randomizer.Next(minValue);
        }
        
        public static int GetNumber(int minValue, int maxValue)
        {
            return _randomizer.Next(minValue, maxValue);
        }
    }
}