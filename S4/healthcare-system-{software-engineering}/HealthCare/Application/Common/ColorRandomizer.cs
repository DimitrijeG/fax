using System;
using System.Collections.Generic;

namespace HealthCare.Application.Common
{
    public class ColorRandomizer
    {
        private static List<string> colors = new List<string>
        {
            "#5865F2",
            "#EB459E",
            "#FEE75C",
            "#57F287",
            "#FFFFFF"
        };

        private static Random random = new Random();

        public static string GetRandomColor()
        {
            int index = random.Next(colors.Count);
            return colors[index];
        }
    }
}