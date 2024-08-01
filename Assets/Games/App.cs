// Created by LunarEclipse on 2024-8-1 19:15.

using Luna;
using UnityEditor;
using UnityEngine;

namespace Games
{
    public static class App
    {
        // [RuntimeInitializeOnLoadMethod]
        // private static void Init()
        // {
        //     Events.ApplicationStart += OnApplicationStart;
        //     Events.ApplicationQuit += OnApplicationQuit;
        // }

        private static void OnApplicationStart(object sender)
        {
            // Debug.Log("Application started.");
            // Caching.compressionEnabled = false;
        }

        private static void OnApplicationQuit(object sender)
        {
            // Debug.Log("Application quit.");
        }
    }
}