// Created by LunarEclipse on 2024-7-13 6:36.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace USEN.Games.Roulette
{
    [Serializable]
    [CreateAssetMenu(fileName = "RouletteDatabase", menuName = "Scriptable Objects/Roulette/Roulette Database")]
    public class RouletteDataset : ScriptableObject
    {
        public Version version;
        public List<RouletteCategory> categories = new();
    }
}