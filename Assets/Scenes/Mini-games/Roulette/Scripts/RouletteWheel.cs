// Created by LunarEclipse on 2024-6-3 9:33.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace USEN.MiniGames.Roulette
{
    public class RouletteWheel : MonoBehaviour
    {
        [SerializeField]
        private RouletteSectors rouletteData;
        
        [Header("Wheel Settings")]
        public float radius = 3.2f;
        public int segmentsPerSector = 10;
        public float sectorInterval = 0.05f;
        public float angleOffset = 180f;
        
        public bool clockwise = true;
        
        [Header("Text Settings")]
        public TMP_FontAsset font;
        public float textDistanceFromCenter = 2f;
        
        [Header("Spin Settings")]
        public float spinDuration = 4f; // Duration of the spin in seconds
        public AnimationCurve spinCurve; // Animation curve to control the spin speed
        
        public event Action OnSpinStart;
        public event Action<string> OnSpinComplete;

        private bool isSpinning = false;
        private float totalAngle;
        private float startAngle;
        
        private List<RouletteSector> _sectors;
        
        public List<RouletteSector> Sectors
        {
            get => _sectors;
            set
            {
                _sectors = new List<RouletteSector>(value);
                if (value.Count > 0)
                    DrawRouletteWheel();
            }
        }

        private void Start()
        {
            Sectors = rouletteData.objects;
        }

        private void OnValidate()
        {
            Debug.Log("OnValidate");
        }

        public void SpinWheel()
        {
            if (!isSpinning)
            {
                StartCoroutine(Spin());
            }
        }

        private IEnumerator Spin()
        {
            isSpinning = true;
            
            OnSpinStart?.Invoke();
            
            float elapsedTime = 0f;
            float angle = 0f;

            // Randomly determine the target sector
            int targetSectorIndex = Random.Range(0, Sectors.Count);
            float targetAngle = (clockwise ? - totalAngle * targetSectorIndex : 360f - targetSectorIndex * totalAngle) + angleOffset;

            startAngle = transform.eulerAngles.z;
            float endAngle = 360f * 5 + targetAngle; // Spin multiple times plus target angle

            while (elapsedTime < spinDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / spinDuration;
                angle = Mathf.Lerp(startAngle, endAngle, spinCurve.Evaluate(t)) % 360;
                transform.eulerAngles = new Vector3(0, 0, angle);

                yield return null;
            }

            // Ensure the wheel stops at the exact target sector
            transform.eulerAngles = new Vector3(0, 0, endAngle % 360);
            isSpinning = false;

            // Announce the prize
            Debug.Log("Won prize: " + Sectors[targetSectorIndex].content);
            OnSpinComplete?.Invoke(GetResult(targetSectorIndex));
        }
        
        public string GetResult(int index)
        {
            return clockwise ? Sectors[^index].content : Sectors[index].content;
        }

        void DrawRouletteWheel()
        {
            transform.localRotation = Quaternion.identity;
            totalAngle = 360f / Sectors.Count;
            
            // Clear existing sectors
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < Sectors.Count; i++)
            {
                CreateSector(i, totalAngle);
            }
            
            // Rotate the wheel to align with the first sector
            transform.localRotation = Quaternion.Euler(0, 0, angleOffset);
        }

        void CreateSector(int index, float totalAngle)
        { 
            float startAngle = (index - 0.5f) * totalAngle;
            float endAngle = startAngle + totalAngle;

            if (clockwise)
            {
                startAngle = -startAngle;
                endAngle = -endAngle;
            }

            // Create GameObject for the sector
            GameObject sectorGO = new GameObject("Sector_" + index);
            sectorGO.transform.SetParent(transform);

            // Create and set up the Mesh
            MeshFilter meshFilter = sectorGO.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = sectorGO.AddComponent<MeshRenderer>();
            meshRenderer.sortingOrder = 1;

            Mesh mesh = new Mesh();
            meshFilter.mesh = mesh;

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Color> colors = new List<Color>();

            vertices.Add(Vector3.zero); // Center point

            var centerColor = Sectors[index].color;
            colors.Add(centerColor); // Center color

            for (int j = 0; j <= segmentsPerSector; j++)
            {
                float angle = Mathf.Lerp(startAngle, endAngle, (float)j  * (1 - sectorInterval) / segmentsPerSector);
                Vector3 point = new Vector3(
                    Mathf.Cos(Mathf.Deg2Rad * angle) * radius, 
                    Mathf.Sin(Mathf.Deg2Rad * angle) * radius, 
                    0);
                vertices.Add(point);
                
                var color = Sectors[index].color;
                color.a *= 0.5f;
                colors.Add(color);

                if (j > 0)
                {
                    triangles.Add(0);
                    triangles.Add(j);
                    triangles.Add(j + 1);
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.colors = colors.ToArray();

            // Use a material that supports vertex colors
            Material material = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default"));
            meshRenderer.material = material;

            // Add text
            GameObject textGO = new GameObject("Text_" + index);
            textGO.transform.SetParent(sectorGO.transform);

            TextMeshPro text = textGO.AddComponent<TextMeshPro>();
            text.text = Sectors[index].content;
            text.horizontalAlignment = HorizontalAlignmentOptions.Left;
            text.verticalAlignment = VerticalAlignmentOptions.Middle;
            text.fontSize = 2.5f;
            text.color = Color.white;
            text.outlineColor = Color.black;
            text.outlineWidth = 1f;
            text.sortingOrder = 2;
            text.font = font ? font : Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");

            float midAngle = (startAngle + endAngle) / 2f;
            Vector3 textPos = new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * midAngle) * textDistanceFromCenter, 
                Mathf.Sin(Mathf.Deg2Rad * midAngle) * textDistanceFromCenter, 
                0);
            textGO.transform.localPosition = textPos;

            // Rotate the text to align with the sector
            textGO.transform.localRotation = Quaternion.Euler(0, 0, midAngle - 180f);

            // Adjust text size to fit within the sector
            // StartCoroutine(AdjustTextSize(text, radius));
            
            // Calculate the size of the text area to fit within the sector
            float sectorAngle = Mathf.Deg2Rad * totalAngle / 2; // Half the angle of the sector in radians
            float textWidth = 2 * radius * Mathf.Tan(sectorAngle); // Width of the text box based on the arc length
            float textHeight = radius * Mathf.Sin(sectorAngle); // Height of the text box

            // Set the size of the text area
            RectTransform rectTransform = textGO.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(textWidth, textHeight);
        }
        
        private IEnumerator AdjustTextSize(TextMeshPro text, float maxWidth)
        {
            yield return null; // Wait for one frame to get the correct bounds
            text.ForceMeshUpdate();

            while (text.bounds.size.x > maxWidth)
            {
                text.fontSize -= 1;
                text.ForceMeshUpdate();
            }
        }
    }
}