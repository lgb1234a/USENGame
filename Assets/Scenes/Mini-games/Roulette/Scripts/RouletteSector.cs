// Created by LunarEclipse on 2024-6-3 9:18.

using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace USEN.MiniGames.Roulette
{
    [Serializable]
    public partial class RouletteSector
    {
        [TableColumnWidth(150, Resizable = true)]
        [HideInInspector] public int id;
        [VerticalGroup("Data"), LabelWidth(50)] public string content;
        [VerticalGroup("Data"), LabelWidth(50)] public float weight = 1;
        [VerticalGroup("Data"), LabelWidth(80)] public Color color;

        public RouletteSector(int id, string content, float weight, Color color)
        {
            this.id = id;
            this.content = content;
            this.weight = weight;
            this.color = color;
        }
        
        public RouletteSector(RouletteSector sector)
        {
            id = sector.id;
            content = sector.content;
            weight = sector.weight;
            color = sector.color;
        }
    }
}