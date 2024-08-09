using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI
{
    [DisallowMultipleComponent]
    [Icon("ToggleGroup Icon")]
    [AddComponentMenu("UI/Toggle Group", 32)]
    public class ToggleGroup: UnityEngine.UI.ToggleGroup
    {
        public List<Toggle> Toggles => base.m_Toggles;
        
        public void Add(Toggle toggle)
        {
            base.RegisterToggle(toggle);
        }

        public void Remove(Toggle toggle)
        {
            base.UnregisterToggle(toggle);
        }
        
        public void ToggleOn(int index)
        {
            if (index < 0 || index >= Toggles.Count) return;
            Toggles[index].isOn = true;
        }
    }
}