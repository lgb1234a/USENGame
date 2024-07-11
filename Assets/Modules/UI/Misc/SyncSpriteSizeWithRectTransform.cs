// Created by LunarEclipse on 2024-7-10 7:21.

using UnityEngine;

namespace Modules.UI.Misc
{
    public class SyncSpriteSizeWithRectTransform : MonoBehaviour
    {
        public RectTransform rectTransform;

        private SpriteRenderer spriteRenderer;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (rectTransform == null)
            {
                Debug.LogError("RectTransform is not assigned.");
                return;
            }

            UpdateSpriteSize();
        }

        void Update()
        {
            UpdateSpriteSize();
        }

        void UpdateSpriteSize()
        {
            Vector2 size = rectTransform.rect.size;
            Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

            Vector3 scale = transform.localScale;
            scale.x = size.x / spriteSize.x;
            scale.y = size.y / spriteSize.y;
            transform.localScale = scale;
        }
    }
}