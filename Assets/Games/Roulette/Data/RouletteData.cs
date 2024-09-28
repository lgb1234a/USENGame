// Created by LunarEclipse on 2024-6-3 9:20.

using System;
using System.Collections.Generic;
using Luna.Extensions;
using Luna.Extensions.Unity;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace USEN.Games.Roulette
{
    // A roulette data object represents a single roulette wheel.
    // It contains a list of sectors, each with a content and a weight.
    [JsonObject(MemberSerialization.OptIn)]
    [CreateAssetMenu(fileName = "Roulette", menuName = "Scriptable Objects/Roulette/Roulette")]
    public class RouletteData : ScriptableObject
    {
        [ReadOnly] [JsonProperty] public string id;
        [JsonProperty] public string title;
        [JsonProperty] public long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        
        [JsonProperty] 
        [TableList(ShowIndexLabels = true, AlwaysExpanded = true, DrawScrollView = false)]
        public List<RouletteSector> sectors = new();

        public RouletteData()
        {
            id = Guid.NewGuid().ToString();
        }
        
        // Copy constructor.
        public RouletteData(RouletteData other)
        {
            id = other.id;
            title = other.title;
            sectors = new();
            for (int i = 0; i < other.sectors.Count; i++)
                sectors.Add(new RouletteSector(other.sectors[i]));
        }

        public void OnValidate()
        {
            if (string.IsNullOrEmpty(id))
                id = Guid.NewGuid().ToString();
            
            if (string.IsNullOrEmpty(name))
                title = base.name;
            
            for (int i = 0; i < sectors.Count; i++)
            {
                var sector = sectors[i];
                sector.id = sectors.IndexOf(sector);
                var color = Color.HSVToRGB(Mathf.Pow((1.0f / sectors.Count * i - 0.02f).Mod(1), 1.35f), 1f, 1f);
                sector.color = color
                    .WithSaturation(0.85f * (1f - sector.color.g * 0.2f))
                    .WithBrightness(Mathf.Clamp(1.4f * (1f - sector.color.b * 0.5f) * (1f - sector.color.g * 0.3f), 0, 1))
                    .WithAlpha(0.75f * (1f - sector.color.b * 0.1f));
            }
        }
    }
}