// Created by LunarEclipse on 2024-6-3 9:20.

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace USEN.Games.Yamanote
{
    // A roulette data object represents a single roulette wheel.
    // It contains a list of sectors, each with a content and a weight.
    [Serializable]
    [CreateAssetMenu(fileName = "Yamanote", menuName = "Scriptable Objects/Yamanote/Yamanote")]
    public class YamanoteData : ScriptableObject
    {
        [ReadOnly]
        public string id;
        public string title;
        
        [FormerlySerializedAs("objects")] 
        [TableList(ShowIndexLabels = true, AlwaysExpanded = true, DrawScrollView = false)]
        public List<string> sectors = new();
        

        public void OnValidate()
        {
            if (string.IsNullOrEmpty(id))
                id = Guid.NewGuid().ToString();
            
            if (string.IsNullOrEmpty(name))
                title = base.name;
            
            for (int i = 0; i < sectors.Count; i++)
            {
                var sector = sectors[i];
                // sector.id = sectors.IndexOf(sector);
                // sector.color = Color.HSVToRGB(1.0f / sectors.Count * i, 0.5f, 1f);
            }
        }
    }
}