// Created by LunarEclipse on 2024-7-10 21:18.

using UnityEngine;

namespace USEN.Games.Roulette
{
    public class ApplyScaleToChildren : MonoBehaviour
    {
        void Start()
        {
            ApplyScale();
            transform.localScale = Vector3.one;
        }
        
        public void ApplyScale()
        {
            var scale = transform.localScale;
            foreach (Transform child in transform)
            {
                child.localScale = scale;
            }
        }
    }
}