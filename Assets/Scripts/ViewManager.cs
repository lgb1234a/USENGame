﻿using System;
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

    void Start()
    {
        Application.targetFrameRate = 30;
        m_viewStack = new Stack<IViewOperater>();
        m_rootCanvas = GameObject.FindGameObjectWithTag("Modal").transform;
        var rootView = new HomeView(m_rootCanvas);
        Push(rootView);
    }

    void Update()
    {
        m_currentView.Update();
    }

    public void Push(IViewOperater view)
    {
        m_viewStack.Push(view);
        m_currentView = view;
        m_currentView.Show();
    }

    public void Pop()
    {
        m_lastPopedView = m_viewStack.Pop();
        m_currentView = m_viewStack.Peek();
        if (m_currentView != null) {
            m_currentView.Show();
        }
    }

    public void OnAndroidKeyDown(string keyName) {
        Debug.Log($"Call from android key:{keyName}");
        m_currentView.OnAndroidKeyDown(keyName);
    }
}
