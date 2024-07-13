// Created by LunarEclipse on 2024-7-14 7:23.

using LeTai;
using UnityEngine;

namespace Modules.UI.Misc
{
    public static class ColorExtensions
    {
        public static Color RandomColor(float brightness = 1, float saturation = 1)
        {
            return Color.HSVToRGB(Random.value, saturation, brightness);
        }
        
        public static Color WithAlpha(this Color self, float a)
        {
            return new Color(self.r, self.g, self.b, a);
        }
    }
}