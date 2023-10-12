using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface IViewOperater
{
    void Show();
    void Hide();
    void Update();
    void OnThemeTypeChanged();
    void OnAndroidKeyDown(string keyName);
}

public class ViewManager : MonoBehaviourSingletonTemplate<ViewManager>
{
    private IViewOperater m_lastPopedView;
    private IViewOperater m_currentView;
    private Stack<IViewOperater> m_viewStack;
    private Transform m_rootCanvas;
    private Transform m_popupCanvas;
    private List<IViewOperater> m_allViewInstances = new();

    public Text m_keydownDebug;
    GameObject m_currentEventGO;
    public Image m_bg;
    public Image m_bgDecorate;

    public IViewOperater GetCurrentView() 
    {
        return m_currentView;
    }

    public IViewOperater GetLastPopedView()
    {
        return m_lastPopedView;
    }

    public Transform GetRootTransform()
    {
        return m_rootCanvas;
    }

    public Transform GetPopupTransform()
    {
        return m_popupCanvas;
    }

    void Start()
    {
        Application.targetFrameRate = 30;
        m_viewStack = new Stack<IViewOperater>();
        m_rootCanvas = GameObject.FindGameObjectWithTag("Modal").transform;
        m_popupCanvas = GameObject.FindGameObjectWithTag("Popup").transform;
        var rootView = new HomeView(m_rootCanvas);
        Push(rootView);

        m_bg.sprite = ThemeResManager.Instance.GetThemeBgTexture();
        m_bgDecorate.sprite = ThemeResManager.Instance.GetThemeHomeBgDecorateTexture();
    }

    void Update()
    {
        if (Input.anyKeyDown) {
            AudioManager.Instance.PlayKeyDownEffect();
            if (Input.GetKeyDown(KeyCode.T))
            {
                OnAndroidKeyDown("terminal");
            }
        }
        m_currentView.Update();
        OutputKeyDebugMsg();
    }

    void OutputKeyDebugMsg() {
        if (Input.anyKeyDown) {
            foreach (KeyCode CurretkeyCode in Enum.GetValues(typeof(KeyCode)))
			{
				if (Input.GetKeyDown(CurretkeyCode))
				{
					m_keydownDebug.text = $"[{CurretkeyCode.ToString()}] pressed!";
				}
			}
        }
    }

    public void ShowDebugInfo(string text) {
        m_keydownDebug.text = text;
    }

    public void Push(IViewOperater view)
    {
        if (!m_allViewInstances.Contains(view)) {
            m_allViewInstances.Add(view);
        }
        if (m_viewStack.Count > 0)
        {
            var lastView = m_viewStack.Peek();
            if (lastView != null)
                lastView.Hide();
        }

        if (!m_viewStack.Contains(view))
            m_viewStack.Push(view);
        m_currentView = view;
        m_currentView.Show();
    }

    public void Hided(IViewOperater view) 
    {
        m_lastPopedView = m_viewStack.Pop();
        m_currentView = m_viewStack.Peek();
        if (m_currentView != null) {
            m_currentView.Show();
            if (m_currentEventGO != null)
            {
                EventSystem.current.SetSelectedGameObject(m_currentEventGO);
            }
        }
    }

    public void OnAndroidKeyDown(string keyName) {
        // Debug.Log($"Call from android key:{keyName}");
        m_currentView.OnAndroidKeyDown(keyName);

        if (keyName == "terminal")
        {
            m_currentEventGO = EventSystem.current.currentSelectedGameObject;
            Push(TerminalView.Instance);
        }
    }


    public void OnAudioFocusChanged(string eventType) {
        Debug.LogWarning(eventType);
        ShowDebugInfo(eventType);
    }

    public void SendChangeThemeTypeEvent() {
        m_bg.sprite = ThemeResManager.Instance.GetThemeBgTexture();
        m_bgDecorate.sprite = ThemeResManager.Instance.GetThemeHomeBgDecorateTexture();
        foreach (var view in m_allViewInstances)
        {
            view.OnThemeTypeChanged();
        }
    }
}
