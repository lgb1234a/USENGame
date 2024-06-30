// Created by LunarEclipse on 2024-6-18 22:17.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Luna.UI.Navigation
{
    public class Navigator : MonoBehaviour
    {
        // Singleton instance for easy access
        public static Navigator Instance;
        
        public Canvas canvas; // Reference to the Canvas
        public GameObject rootWidget; // The root widget of the game
        
        public List<GameObject> widgets = new(); // List of all widgets in the game
        
        public bool escToPop = true; // Pop the top widget when the escape key is pressed
        
        private readonly Dictionary<Type, GameObject> _widgetDictionary = new();
        private readonly Stack<GameObject> _widgetStack = new();
        
        private bool isDontDestroyOnLoad = false;
        
        // Load the Navigator instance on startup if it doesn't exist
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            if (Instance == null)
            {
                GameObject navigator = new GameObject("UI Navigator");
                Instance = navigator.AddComponent<Navigator>();
                DontDestroyOnLoad(navigator);
                Instance.isDontDestroyOnLoad = true;
            }
        }

        private void Awake()
        {
            Instance = this;
            // if (Instance == null)
            //     Instance = this;
            // else Destroy(gameObject); 
        }
        
        private void Start()
        {
            if (canvas == null)
            {
                // Create a new canvas if none is found
                if (!(canvas = FindObjectOfType<Canvas>()))
                {
                    GameObject canvasObject = new GameObject("Canvas");
                    canvas = canvasObject.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvasObject.AddComponent<CanvasScaler>();
                    canvasObject.AddComponent<GraphicRaycaster>();
                }
            }
            
            // Preload all widgets in the game
            if (widgets.Count == 0)
            {
                // Load stateful widgets scriptable object
                Widgets widgets = Resources.Load<Widgets>("Widgets.g");
                if (widgets != null)
                {
                    this.widgets = widgets.Prefabs;
                    foreach (var widget in this.widgets)
                    {
                        Debug.Log($"[Navigator] Found widget: {widget.name}");
                        _widgetDictionary.TryAdd(widget.GetComponent<Widget>().GetType(), widget);
                    }
                }
            }
            
            // Load the rootWidget
            if (rootWidget != null)
            {
                if (rootWidget.activeInHierarchy == false)
                {
                    Push(rootWidget);
                }
                else
                {
                    _widgetStack.Push(rootWidget);
                }
            } 
            
        }

        private void Update()
        {
            if (escToPop && Input.GetKeyDown(KeyCode.Escape))
            {
                _Pop();
            }
        }
        
        public static void Push(GameObject widgetPrefab)
        {
            Instance._Push(widgetPrefab);
        }
        
        public static void Push<T>() where T : Widget
        {
            Instance._Push<T>();
        }
        
        public static void Pop()
        {
            Instance._Pop();
        }

        protected void _Push(GameObject widgetPrefab)
        {
            if (_widgetStack.Count > 0)
            {
                _widgetStack.Peek().SetActive(false);
            }

            GameObject newWidget = Instantiate(widgetPrefab, canvas.transform);
            _widgetStack.Push(newWidget);
        }
        
        protected void _Push<T>() where T : Widget
        {
            if (_widgetDictionary.TryGetValue(typeof(T), out GameObject widgetPrefab))
            {
                Push(widgetPrefab);
            }
            else
            {
                Debug.LogError($"[Navigator] Widget of type {typeof(T)} not found.");
            }
        }

        protected void _Pop()
        {
            if (_widgetStack.Count > 1)
            {
                GameObject topWidget = _widgetStack.Pop();
                Destroy(topWidget);

                if (_widgetStack.Count > 0)
                {
                    _widgetStack.Peek().SetActive(true);
                }
            }
        }

        public void PopToRoot()
        {
            while (_widgetStack.Count > 1)
            {
                Destroy(_widgetStack.Pop());
            }

            if (_widgetStack.Count > 0)
            {
                _widgetStack.Peek().SetActive(true);
            }
        }
    }

}