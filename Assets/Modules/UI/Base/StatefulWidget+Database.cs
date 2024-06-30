// Created by LunarEclipse on 2024-6-19 0:40.

using System.Collections.Generic;
using UnityEngine;

namespace Luna.UI
{
    public partial class StatefulWidget
    {
        public static readonly Dictionary<string, Widget> All = new ();
        
        // Find all StatefulWidget in the project at startup.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            var widgets = Resources.FindObjectsOfTypeAll<Widget>();
            foreach (var widget in widgets)
            {
                All.Add(widget.name, widget);
                Debug.Log($"[Widget] Found Widget: {widget.name}");
            }
        }
    }
}