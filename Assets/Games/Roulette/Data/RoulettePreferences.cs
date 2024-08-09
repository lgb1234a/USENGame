using UnityEngine;

namespace USEN.Games.Roulette
{
    public static class RoulettePreferences
    {
        public static DisplayMode DisplayMode
        {
            get => (DisplayMode) PlayerPrefs.GetInt("Roulette.DisplayMode", 0);
            set => PlayerPrefs.SetInt("Roulette.DisplayMode", (int) value);
        }
    }
    
    public enum DisplayMode
    {
        Normal,
        Random,
    }
}
