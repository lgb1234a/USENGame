using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IViewOperater
{
    void Show();
    void Hide();
    void Update();
    void OnAndroidKeyDown(string keyName);
}

public class ViewManager : MonoBehaviourSingletonTemplate<ViewManager>
{
    private IViewOperater m_lastPopedView;
    private IViewOperater m_currentView;
    private Stack<IViewOperater> m_viewStack;
    private Transform m_rootCanvas;
    private Transform m_popupCanvas;

    public Text m_keydownDebug;

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
    }

    void Update()
    {
        if (Input.anyKeyDown) {
            AudioManager.Instance.PlayKeyDownEffect();
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
        }
    }

    public void OnAndroidKeyDown(string keyName) {
        // Debug.Log($"Call from android key:{keyName}");
        m_currentView.OnAndroidKeyDown(keyName);
    }
}
