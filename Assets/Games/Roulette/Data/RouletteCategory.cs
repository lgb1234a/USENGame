// Created by LunarEclipse on 2024-7-13 6:36.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace USEN.Games.Roulette
{
    [Serializable]
    [CreateAssetMenu(fileName = "RouletteCategory", menuName = "Scriptable Objects/Roulette/Roulette Category")]
    public class RouletteCategory : ScriptableObject
    {
        public string title;
        public List<RouletteData> roulettes;
    }
}