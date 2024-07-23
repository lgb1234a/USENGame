// Created by LunarEclipse on 2024-7-13 6:36.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace USEN.Games.Yamanote
{
    [Serializable]
    [CreateAssetMenu(fileName = "YamanoteCategory", menuName = "Scriptable Objects/Yamanote/Yamanote Category")]
    public class YamanoteCategory : ScriptableObject
    {
        public string title;
        public List<YamanoteData> roulettes;
    }
}