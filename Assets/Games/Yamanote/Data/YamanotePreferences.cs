// Created by LunarEclipse on 2024-9-9 5:33.

using UnityEngine;

namespace USEN.Games.Yamanote
{
    public static class YamanotePreferences
    {
        public static DisplayMode DisplayMode
        {
            get => (DisplayMode) PlayerPrefs.GetInt("Yamanote.DisplayMode", 0);
            set => PlayerPrefs.SetInt("Yamanote.DisplayMode", (int) value);
        }
    }
    
    public enum DisplayMode
    {
        Normal,
        Random,
    }
}