// Created by LunarEclipse on 2024-6-3 9:20.

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace USEN.MiniGames.Roulette
{
    [CreateAssetMenu(fileName = "RouletteData", menuName = "Scriptable Objects/Roulette Data")]
    public class RouletteSectors : ScriptableObject
    {
        [TableList(ShowIndexLabels = true, AlwaysExpanded = true, DrawScrollView = false)]
        public List<RouletteSector> objects = new();

        public void OnValidate()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                var sector = objects[i];
                
                // Debug.Log(sector.content);
                // Debug.Log(sector.color);
                
                sector.id = objects.IndexOf(sector);
                sector.color = Color.HSVToRGB(1.0f / objects.Count * i, 0.5f, 1f);
            }
        }
    }
}