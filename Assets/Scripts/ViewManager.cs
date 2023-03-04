using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IViewOperater
{
    void Show();
    void Hide();
    void Update();
}

public class ViewManager : MonoBehaviourSingletonTemplate<ViewManager>
{
    private IViewOperater m_lastPopedView;
    private IViewOperater m_currentView;
    private Stack<IViewOperater> m_viewStack;
    private Transform m_rootCanvas;

    public Text m_keydownDebug;
    int j = 0;

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
        OutputKeyDebugMsg();
    }

    void OutputKeyDebugMsg() {
        for (int i = 0; i < 1000; i++)
        {
            try
            {
                if (Input.GetKeyDown((KeyCode)i))
                {
                    j++;
                    m_keydownDebug.text = j +" with: "+ i.ToString();
                    Debug.Log("Working");
                    break;
                }
            }
            catch
            {
            }
        }
        // if (Input.anyKeyDown) {
        //     foreach (KeyCode CurretkeyCode in Enum.GetValues(typeof(KeyCode)))
		// 	{
		// 		if (Input.GetKeyDown(CurretkeyCode))
		// 		{
		// 			m_keydownDebug.text = $"[{CurretkeyCode.ToString()}] pressed!";
		// 		}
		// 	}
        // }
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
}
